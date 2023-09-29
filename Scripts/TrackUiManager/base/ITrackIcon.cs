using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace InteractiveSystem
{
    public interface ITrackIcon<T>
    {
        void Init(T source);
        void Release();
    }

    public interface ITrackIconPoolCreater
    {
        IObjectPool<Component> GetPool(Transform parent);
    }
}
