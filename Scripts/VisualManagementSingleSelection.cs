using System;

namespace InteractiveSystem
{
    /// <summary>
    /// 可视化管理器, 但它只允许单个可视化对象被选择
    /// </summary>
    public abstract class VisualManagementSingleSelection<T> : VisualManagement<T>
        where T : IVisible
    {
        protected T Selected { get; private set; }

        protected override void Update()
        {
            DirtList.Clear();
            foreach (var visible in VisualStates.Keys)
            {
                var state = VisualStates[visible];
                var buf = visible.Enable ? UpdateVisualState(visible) : VisualState.Invisible;
                if (buf == state) continue;

                //! 易错代码
                // 只允许一个对象被选择
                if (buf == VisualState.Selected && Selected != null && DirtList.ContainsKey(Selected))
                    DirtList[visible] = VisualState.Selectable;

                Selected = visible;

                // 无论之前有没有被唯一选择更新, 都要更新其新计算出来的状态
                DirtList[visible] = buf;
            }

            // 统一刷新
            foreach (var (visible, target) in DirtList)
                switch (target)
                {
                    case VisualState.Invisible:
                        visible.OnInvisible(VisualStates[visible]);
                        break;
                    case VisualState.Visible:
                        visible.OnVisible(VisualStates[visible]);
                        break;
                    case VisualState.Selectable:
                        visible.OnSelectable(VisualStates[visible]);
                        break;
                    case VisualState.Selected:
                        visible.OnSelected(VisualStates[visible]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }
    }
}