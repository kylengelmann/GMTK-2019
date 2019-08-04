using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGun : WeaponBase
{
    public PooledObjectType bullet;
    public Transform FirePoint;

    protected override void DoShoot()
    {
        ShootProjectile(FirePoint.position, FirePoint.forward);
        Break();
    }

    protected virtual void ShootProjectile(Vector3 position, Vector3 direction)
    {
        GameObject projectile = ObjectPoolManager.GetPooledObject(bullet);
        projectile.transform.position = position;
        projectile.SetActive(true);
        projectile.GetComponent<BasicProjectile>().Fire(direction);
    }

}
