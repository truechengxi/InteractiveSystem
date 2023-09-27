using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveSystem
{
    public interface ISimpleVisible : IVisible
    {
        public Vector3 Position { get; }
    }
}