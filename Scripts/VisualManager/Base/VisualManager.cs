using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveSystem
{
    /// <summary>
    /// 可视化管理器, 它用于向所有实现了可视化接口的对象提供它们的可视化状态
    /// </summary>
    public abstract class VisualManager<T, TU> : Singleton<VisualManager<T, TU>>
        where T : IVisible<TU>
        where TU : struct, Enum
    {
        protected Dictionary<T, TU> VisualStates { get; private set; }
        protected Dictionary<T, TU> DirtList { get; private set; }

        protected readonly TU[] stateValues = (TU[])Enum.GetValues(typeof(TU));

        public void Register(T visible)
        {
            if (VisualStates.ContainsKey(visible))
            {
                Debug.LogError("存在重复注册!");
                return;
            }

            VisualStates.Add(visible, stateValues[0]);
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

        public TU GetState(T visible)
        {
            if (!VisualStates.ContainsKey(visible))
                return stateValues[0];
            return VisualStates[visible];
        }

        protected abstract TU UpdateVisualState(T visible);

        protected virtual void InitField()
        {
        }

        protected void UpdateState()
        {
            // 统一刷新
            foreach (var (visible, target) in DirtList)
            {
                var oldState = VisualStates[visible];
                VisualStates[visible] = target;
                if(oldState.Equals(stateValues[0]))
                    visible.VisibleInit();
                visible.UpdateVisibleState(oldState, target);
            }
            DirtList.Clear();
        }
        
        private void Awake()
        {
            if (CreateInstance(this)) return;
            VisualStates = new Dictionary<T, TU>();
            DirtList = new Dictionary<T, TU>();
            InitField();
        }


        protected virtual void Update()
        {
            foreach (var (visible, state) in VisualStates)
            {
                var buf = visible.Enable ? UpdateVisualState(visible) : stateValues[0];
                if (buf.Equals(state)) continue;
                DirtList.Add(visible, buf);
            }

            UpdateState();
        }
        
        #if UNITY_EDITOR
        [Header("Editor Property")]
        public bool showDebugGUI;
        
        protected virtual void OnGUI()
        {
            if (!showDebugGUI) return;
            // 打印所有可见对象的状态
            foreach (var (visible, state) in VisualStates)
            {
                if (visible is Component com)
                {
                    GUILayout.Label(com.gameObject.name + ":" + state);
                }
            }
        }
        #endif
    }
}