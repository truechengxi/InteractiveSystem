using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveSystem
{
    public interface IVisible
    {
#if UNITY_EDITOR
        string Name { get; }
#endif

        bool Enable { get; }

        public void VisibleInit()
        {
            
        }
        
        public void OnInvisible(VisualState oldState)
        {
        }

        public void OnVisible(VisualState oldState)
        {
        }

        public void OnSelectable(VisualState oldState)
        {
        }

        public void OnSelected(VisualState oldState)
        {
        }
    }
}