using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum PooledObjectType
{
    PlayerBullet,
    EnemyBullet,

}

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    struct PooledObjectInfo
    {
        public PooledObjectType ObjectType;
        public GameObject prefab;
        public int NumToPreinstantiate;
    }

    [SerializeField]
    PooledObjectInfo[] PooledObjects;

    static Dictionary<PooledObjectType, GameObject> prefabs = new Dictionary<PooledObjectType, GameObject>();
    static Dictionary<PooledObjectType, List<GameObject>> objectPools = new Dictionary<PooledObjectType, List<GameObject>>();
    static Dictionary<PooledObjectType, int> numPreinstantiate = new Dictionary<PooledObjectType, int>();

    static ObjectPoolManager instance;

    private void Awake()
    {
        if(instance)
        {
           return;
        }
        foreach(PooledObjectInfo info in PooledObjects)
        {
            prefabs.Add(info.ObjectType, info.prefab);
            objectPools.Add(info.ObjectType, new List<GameObject>());
            numPreinstantiate.Add(info.ObjectType, info.NumToPreinstantiate);
        }
        instance = this;
    }

    public static void ClearObjectPools()
    {
        foreach (PooledObjectType type in objectPools.Keys)
        {
            objectPools[type] = new List<GameObject>();
        }
    }

    public static Coroutine PreinstantiateObjects()
    {
        return instance.StartCoroutine(instance.PreinstantiateObjectsRoutine());
    }

    IEnumerator PreinstantiateObjectsRoutine()
    {
        foreach(PooledObjectType type in numPreinstantiate.Keys)
        {
            GameObject prefab = prefabs[type];
            for(int i = 0; i < numPreinstantiate[type]; ++i) {
                GameObject preInstantiated = Instantiate(prefab, instance.transform);
                preInstantiated.SetActive(false);
                objectPools[type].Add(preInstantiated);
            }
            yield return null;
        }
    }

    public static GameObject GetPooledObject(PooledObjectType type)
    {
        List<GameObject> pool = objectPools[type];
        GameObject pooledObject = null;
        if (pool.Count > 0)
        {
            pooledObject = pool[0];
            pool.RemoveAt(0);
            return pooledObject;
        }

        pooledObject = Instantiate(prefabs[type]);
        pooledObject.SetActive(false);

        return pooledObject;
    }

    public static void ReturnPooledObject(PooledObjectType type, GameObject returningObject)
    {

        returningObject.SetActive(false);
        returningObject.transform.SetParent(instance.transform);
        objectPools[type].Add(returningObject);
    }


}
