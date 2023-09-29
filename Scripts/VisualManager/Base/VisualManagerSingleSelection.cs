using System;
using UnityEngine;

namespace InteractiveSystem
{
    /// <summary>
    /// 可视化管理器, 但它只允许单个可视化对象被选择
    /// </summary>
    public abstract class VisualManagerSingleSelection<T> : VisualManager<T>
        where T : class, IVisible
    {
        protected T Selected { get; private set; }

        protected override void Update()
        {
            DirtList.Clear();
            foreach (var (visible, state) in VisualStates)
            {
                var buf = visible.Enable ? UpdateVisualState(visible) : VisualState.Invisible;
                if (buf == state) continue;

                //! 易错代码
                // 只允许一个对象被选择
                if (buf == VisualState.Selected && visible != Selected)
                {
                    if (Selected != null && DirtList.ContainsKey(Selected))
                        DirtList[Selected] = VisualState.Selectable;
                    Selected = visible;
                }

                if (Selected == visible && buf != VisualState.Selected)
                {
                    Selected = null;
                }

                // 无论之前有没有被唯一选择更新, 都要更新其新计算出来的状态
                DirtList[visible] = buf;
            }

            // 统一刷新
            foreach (var (visible, target) in DirtList)
            {
                var oldState = VisualStates[visible];
                VisualStates[visible] = target;
                if(oldState == VisualState.Invisible)
                    visible.VisibleInit();
                switch (target)
                {
                    case VisualState.Invisible:
                        visible.OnInvisible(oldState);
                        break;
                    case VisualState.Visible:
                        visible.OnVisible(oldState);
                        break;
                    case VisualState.Selectable:
                        visible.OnSelectable(oldState);
                        break;
                    case VisualState.Selected:
                        visible.OnSelected(oldState);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        #if UNITY_EDITOR
        protected override void OnGUI()
        {
            if(!showDebugGUI) return;
            GUILayout.Label("Select Object:" + Selected);
            base.OnGUI();
        }
#endif
    }
}