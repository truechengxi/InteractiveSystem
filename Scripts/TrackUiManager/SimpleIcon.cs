using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace InteractiveSystem
{
    public class SimpleIcon : MonoBehaviour, ITrackIcon<ISimpleVisible>, ITrackIconPoolCreater
    {
        private ISimpleVisible _source;
        private Camera _camera;
        
        private void Track()
        {
            transform.position = _camera.WorldToScreenPoint(_source.Position);
        }

        private void Update()
        {
            Track();
        }
        

        public void Init(ISimpleVisible source)
        {
            _source = source;
            _camera = Camera.main;
            Track();
        }

        public void Release()
        {
            TrackIconManager.Instance.Release(this);
        }

        public IObjectPool<Component> GetPool(Transform parent)
        => new ObjectPool<Component>(() => Instantiate(this, parent),
            c=>c.gameObject.SetActive(true),
            c=>c.gameObject.SetActive(false),
            c=>Destroy(c.gameObject));
    }
}
