using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace InteractiveSystem
{
    public abstract class TrackIconManager<T, TU> : Singleton<TrackIconManager<T, TU>>
    where T : Component, ITrackIcon<TU>
    where TU : Component
    {
        private IObjectPool<T> _pool;
        
        public Transform poolParent;
        public T prefab;
        
        private void Awake()
        {
            if(CreateInstance(this)) return;
            _pool = new ObjectPool<T>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
        }

        protected virtual void ActionOnDestroy(T obj)
        {
            Destroy(obj.gameObject);
        }

        protected virtual void ActionOnRelease(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        protected virtual void ActionOnGet(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        protected virtual T CreateFunc()
        {
            return Instantiate(prefab, poolParent);
        }
        
        public T GetIcon(TU source)
        {
            var buf = _pool.Get();
            buf.Init(source);
            return buf;
        }

        public void ReleaseWithPool(T icon)
        {
            _pool.Release(icon);
        }
    }
}
