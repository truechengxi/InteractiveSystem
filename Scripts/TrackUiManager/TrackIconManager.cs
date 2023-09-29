using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace InteractiveSystem
{
    public class TrackIconManager : Singleton<TrackIconManager>
    {
        private Dictionary<Component, IObjectPool<Component>> _pools;
        private Dictionary<Component, IObjectPool<Component>> _objs;
        
        public Transform parent;

        private void Awake()
        {
            if(CreateInstance(this))
                return;
            _pools = new Dictionary<Component, IObjectPool<Component>>();
            _objs = new Dictionary<Component, IObjectPool<Component>>();
        }

        public T Get<T, TU>(T prefab, TU source) where T : Component, ITrackIcon<TU>
        {
            T result = null;
            // 若含有对象池创建器就尝试使用对象池
            if (prefab is ITrackIconPoolCreater pc)
            {
                if (!_pools.ContainsKey(prefab))
                    _pools[prefab] = pc.GetPool(parent);
                var p = _pools[prefab];
                result = p.Get() as T;
                if (result != null)
                {
                    _objs.Add(result, p);
                    result.Init(source);
                }
                return result;
            }

            result = Instantiate(prefab);
            return result;
        }

        public void Release(Component obj)
        {
            if (_objs.ContainsKey(obj))
            {
                _objs[obj].Release(obj);
                _objs.Remove(obj);
                return;
            }
            
            Destroy(obj);
        }
    }
}