using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum PooledObjectType
{
    PlayerBullet,
    EnemyBullet,
    Pistol,
    Pop,
    GunDum,
    WeaponPickup,
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

    GameObject poolParent = null;

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
        poolParent = new GameObject("ObjectPool");
        foreach(PooledObjectType type in numPreinstantiate.Keys)
        {
            GameObject prefab = prefabs[type];
            for(int i = 0; i < numPreinstantiate[type]; ++i) {
                GameObject preInstantiated = Instantiate(prefab, poolParent.transform);
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
            pooledObject.transform.parent = null;
            return pooledObject;
        }

        pooledObject = Instantiate(prefabs[type]);
        pooledObject.SetActive(false);

        pooledObject.transform.parent = null;
        return pooledObject;
    }

    public static void ReturnPooledObject(PooledObjectType type, GameObject returningObject)
    {
        returningObject.transform.parent = instance.poolParent.transform;
        returningObject.SetActive(false);
        objectPools[type].Add(returningObject);
    }


}
