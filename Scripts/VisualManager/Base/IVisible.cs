using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveSystem
{
    public interface IVisible<T>
    where T : Enum
    {
        bool Enable { get; }

        public void VisibleInit()
        {
            
        }

        public void UpdateVisibleState(T oldState, T targetState)
        {
            
        }
    }
}