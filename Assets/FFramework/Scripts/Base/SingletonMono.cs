using MergeHooks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public class SingletonMono<T> : MonoBehaviour, ISingletonMono<T> where T : MonoBehaviour
    {
        static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    //设置对象的名字为脚本名
                    obj.name = typeof(T).ToString();
                    //让这个单例模式对象 过场景 不移除
                    //因为 单例模式对象 往往 是存在整个程序生命周期中的
                    //DontDestroyOnLoad(obj);
                    instance = obj.AddComponent<T>();
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                throw new Exception("Try to create multiple singleton!!! The new one has been destoryed");
            }
            instance = this as T;
            //DontDestroyOnLoad(this);
        }
    }
}
