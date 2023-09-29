using System;
using UnityEngine;

namespace InteractiveSystem
{
    /// <summary>
    /// 可视化管理器, 但它只允许单个可视化对象被选择
    /// </summary>
    public abstract class VisualManagerSingleSelection<T, TU> : VisualManager<T, TU>
        where T : class, IVisible<TU>
        where TU : struct, Enum

    {
    protected T Selected { get; private set; }

    protected override void Update()
    {
        foreach (var (visible, state) in VisualStates)
        {
            var buf = visible.Enable ? UpdateVisualState(visible) : stateValues[0];
            if (buf.Equals(state)) continue;

            //! 易错代码
            // 只允许一个对象被选择
            if (buf.Equals(stateValues[^1]) && visible != Selected)
            {
                if (Selected != null && DirtList.ContainsKey(Selected))
                    DirtList[Selected] = stateValues[^2];
                Selected = visible;
            }

            if (Selected == visible && !buf.Equals(stateValues[^1]))
            {
                Selected = null;
            }

            // 无论之前有没有被唯一选择更新, 都要更新其新计算出来的状态
            DirtList[visible] = buf;
        }

        UpdateState();
    }

#if UNITY_EDITOR
    protected override void OnGUI()
    {
        if (!showDebugGUI) return;
        GUILayout.Label("Select Object:" + Selected);
        base.OnGUI();
    }
#endif
    }
}