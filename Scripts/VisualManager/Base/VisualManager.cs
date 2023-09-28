using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveSystem
{
    public enum VisualState
    {
        Invisible,
        Visible,
        Selectable,
        Selected
    }

    /// <summary>
    /// 可视化管理器, 它用于向所有实现了可视化接口的对象提供它们的可视化状态
    /// </summary>
    public abstract class VisualManager<T> : Singleton<VisualManager<T>>
        where T : IVisible
    {
        protected Dictionary<T, VisualState> VisualStates { get; private set; }
        protected Dictionary<T, VisualState> DirtList { get; private set; }

        public void Register(T visible)
        {
            if (VisualStates.ContainsKey(visible))
            {
                Debug.LogError("存在重复注册!");
                return;
            }

            VisualStates.Add(visible, VisualState.Invisible);
        }

        public void Unregister(T visible)
        {
            if (!VisualStates.ContainsKey(visible))
            {
                Debug.LogError("注销不存在的注册");
                return;
            }

            VisualStates.Remove(visible);
        }

        public VisualState GetState(T visible)
        {
            if (!VisualStates.ContainsKey(visible))
                return VisualState.Invisible;
            return VisualStates[visible];
        }

        protected abstract VisualState UpdateVisualState(T visible);

        protected virtual void InitField()
        {
        }

        private void Awake()
        {
            if (CreateInstance(this)) return;
            VisualStates = new Dictionary<T, VisualState>();
            DirtList = new Dictionary<T, VisualState>();
            InitField();
        }


        protected virtual void Update()
        {
            DirtList.Clear();
            foreach (var (visible, state) in VisualStates)
            {
                var buf = visible.Enable ? UpdateVisualState(visible) : VisualState.Invisible;
                if (buf == state) continue;
                DirtList.Add(visible, buf);
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
    }
}