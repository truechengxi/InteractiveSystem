using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveSystem
{
    public interface ITrackIcon<T>
    {
        void Init(T source);
        void Release();
    }
}
