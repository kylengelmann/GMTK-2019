using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjectHandler : MonoBehaviour
{
    public enum ReturnAction
    {
        Deactivate,
        Lifetime,
    }

    public ReturnAction returnAction;

    [SerializeField]
    PooledObjectType pooledObjectType;


    public float MaxLifetime;
    public TimeChannel timeChannel;


    float timeAlive;

    bool hasReturned = true;

    private void OnDisable()
    {
        if(returnAction != ReturnAction.Deactivate) return;
        if(!gameObject.activeSelf)
        {
            ObjectPoolManager.ReturnPooledObject(pooledObjectType, gameObject);
        }
    }

    private void OnEnable()
    {
        if(!hasReturned) return;
        if(GameManager.HasStartedLevel)
        {
            Init();
        }
    }

    void Init()
    {
        hasReturned = false;
        timeAlive = 0f;
    }

    private void Update()
    {
        timeAlive += TimeManager.GetTimeDelta(timeChannel);
        if(returnAction == ReturnAction.Lifetime && timeAlive > MaxLifetime)
        {
            ReturnToPool();
        }
    }

    public void ForceReturn()
    {
        ReturnToPool();
    }

    void ReturnToPool()
    {
        if(hasReturned) return;
        hasReturned = true;
        ObjectPoolManager.ReturnPooledObject(pooledObjectType, gameObject);
    }
}
