using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceOrigin.ObjectPool
{
    [System.Serializable]
    public class PoolInitializeObject
    {
        public GameObject m_poolObjectPrefab;
        public int m_poolMaxSize;
    }

    public class PoolManager : MonoBehaviour
    {
        public PoolInitializeObject[] objectsTobePooled;

        Dictionary<string, ObjectPool> m_objectsPools;
        static PoolManager ms_sharedInstance;

        public void Awake()
        {
            ms_sharedInstance = GetComponent<PoolManager>();
            this.m_objectsPools = new Dictionary<string, ObjectPool>();

            for (int i = 0; i < objectsTobePooled.Length; i++)
            {
                PoolInitializeObject objectTobeInitialized = objectsTobePooled[i];
                CreatePool(objectTobeInitialized.m_poolObjectPrefab, objectTobeInitialized.m_poolMaxSize);
            }
        }

        public static PoolManager Instance
        {
            get
            {
                return ms_sharedInstance;
            }
        }

        public void CreatePool(GameObject obj, int maxSize)
        {
            ObjectPool objPool = new ObjectPool();

            if (objPool.CreateObjectPool(obj, maxSize))
            {
                m_objectsPools[obj.name] = objPool;
            }
        }

        public GameObject GetObjectFromPool(string objectName)
        {
            ObjectPool specificObjectPool = m_objectsPools[objectName];
            return specificObjectPool.GetObject();
        }

        public void PutObjectBacktoPool(GameObject go)
        {
            ObjectPool specificObjectPool = m_objectsPools[go.name];
            specificObjectPool.PutObject(go);
        }
    }
}
