using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponBase : MonoBehaviour
{
    public PooledObjectType pooledObjectType;


    protected void Break()
    {
        GameObject pop = ObjectPoolManager.GetPooledObject(PooledObjectType.Pop);

        pop.transform.position = transform.position;
        pop.SetActive(true);

        ObjectPoolManager.ReturnPooledObject(pooledObjectType, gameObject);

    }

    //public UnityAction OnBreak;

    public void Shoot()
    {
        DoShoot();
    }

    protected abstract void DoShoot();

}
