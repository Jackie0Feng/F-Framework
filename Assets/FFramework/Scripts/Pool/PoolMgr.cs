using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
//using UnityEngine.Rendering;

namespace FFramework
{
    public class PoolMgr : SingletonMono<PoolMgr>
    {
        Dictionary<string, IObjectPool<GameObject>> poolDic = new Dictionary<string, IObjectPool<GameObject>>();

        public Dictionary<string, IObjectPool<GameObject>> PoolDic { get => poolDic; set => poolDic = value; }

        public GameObject GetGameObject(GameObject prefab, bool collectionCheck = true, int defaultCapacity = 20, int maxSize = 100)
        {
            if (poolDic.ContainsKey(prefab.name))
            {
                return poolDic[prefab.name].Get();
            }
            else
            {
                GameObject poolParent = new GameObject(prefab.name);
                poolParent.transform.parent = this.transform;

                IObjectPool<GameObject> objectPool = new ObjectPool<GameObject>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                        collectionCheck, defaultCapacity, maxSize);

                GameObject CreateProjectile()
                {
                    GameObject prefabInstance = Instantiate(prefab);
                    prefabInstance.name = prefab.name;
                    prefabInstance.transform.parent = poolParent.transform;

                    return prefabInstance;
                }
                // invoked when returning an item to the object pool
                void OnReleaseToPool(GameObject pooledObject)
                {
                    print(pooledObject.name + " Released !!!");
                    pooledObject.SetActive(false);
                }
                // invoked when retrieving the next item from the object pool
                void OnGetFromPool(GameObject pooledObject)
                {
                    print(pooledObject.name + " Get !!!");
                    pooledObject.SetActive(true);
                }
                // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
                void OnDestroyPooledObject(GameObject pooledObject)
                {
                    Destroy(pooledObject);
                }

                poolDic.Add(prefab.name, objectPool);
                return objectPool.Get();
            }
        }

        public void ReleaseToPool(GameObject pooledObject)
        {
            if (poolDic.ContainsKey(pooledObject.name))
            {

                poolDic[pooledObject.name].Release(pooledObject);
            }
            else
            {
                Debug.LogError("not found pool !!!");
            }
        }

    }
}
