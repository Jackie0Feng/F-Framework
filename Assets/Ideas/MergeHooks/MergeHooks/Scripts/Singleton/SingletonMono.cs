using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeHooks
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;
        protected virtual void Awake()
        {
            if (Instance != null)
                throw new Exception("尝试创建多个单例异常");
            Instance = this as T;
        }
    }
}
