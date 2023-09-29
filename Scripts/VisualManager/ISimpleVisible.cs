using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveSystem
{
    public interface ISimpleVisible : IVisible<VisualState>
    {
        public Vector3 Position { get; }

        void IVisible<VisualState>.UpdateVisibleState(VisualState oldState, VisualState targetState)
        {
            switch (targetState)
            {
                case VisualState.Invisible:
                    this.OnInvisible(oldState);
                    break;
                case VisualState.Visible:
                    this.OnVisible(oldState);
                    break;
                case VisualState.Selectable:
                    this.OnSelectable(oldState);
                    break;
                case VisualState.Selected:
                    this.OnSelected(oldState);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void OnSelected(VisualState oldState){}

        protected void OnSelectable(VisualState oldState){}

        protected void OnVisible(VisualState oldState){}

        protected void OnInvisible(VisualState oldState){}
    }
}