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

    Dictionary<PooledObjectType, GameObject> prefabs = new Dictionary<PooledObjectType, GameObject>();
    Dictionary<PooledObjectType, List<GameObject>> objectPools = new Dictionary<PooledObjectType, List<GameObject>>();

    private void Awake()
    {
        foreach(PooledObjectInfo info in PooledObjects)
        {
            prefabs.Add(info.ObjectType, info.prefab);
            objectPools.Add(info.ObjectType, new List<GameObject>());
        }
    }

    public void ClearObjectPools()
    {
        foreach (PooledObjectType type in objectPools.Keys)
        {
            objectPools[type] = new List<GameObject>();
        }
    }

    public IEnumerator PreinstantiateObjectsRoutine()
    {
        foreach(PooledObjectInfo info in PooledObjects)
        {
            for(int i = 0; i < info.NumToPreinstantiate; ++i) {
                GameObject preInstantiated = Instantiate(info.prefab, transform);
                preInstantiated.SetActive(false);
                objectPools[info.ObjectType].Add(preInstantiated);
            }
            yield return null;
        }
    }


}
