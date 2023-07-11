using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FFramework
{
    public class UIMgr<T> : BaseMgr<UIPanel>, ISingletonMono<T> where T : UIMgr<T>
    {
        #region
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
        #endregion

        protected virtual void Start()
        {
            UIPanel[] uIPanels = GetComponentsInChildren<UIPanel>();

            //面板名与脚本名一致
            foreach (var panel in uIPanels)
            {
                string name = panel.name;
                print(name);

                Items.Add(name, panel);
            }

        }

        public Ty GetPanel<Ty>() where Ty : UIPanel
        {
            string name = typeof(Ty).Name;
            if (Items.ContainsKey(name))
            {
                return Items[name] as Ty;
            }
            return null;
        }
    }
}
