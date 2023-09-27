using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveSystem
{
    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected bool CreateInstance(T instance)
        {
            if (Instance != null)
            {
                Debug.LogWarning($"类型{typeof(T)}存在多个实例!", instance);
                return true;
            }

            Instance = instance;
            return false;
        }
    }
}