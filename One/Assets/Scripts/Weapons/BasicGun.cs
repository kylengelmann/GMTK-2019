using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGun : WeaponBase
{
    public PooledObjectType bullet;
    public Transform FirePoint;
    public AudioClip pew;

    protected override void DoShoot()
    {
        ShootProjectile(FirePoint.position, FirePoint.forward);
        Pew();
        Break();
    }

    protected void Pew()
    {
        GameObject audio = ObjectPoolManager.GetPooledObject(PooledObjectType.AudioSource);
        audio.transform.position = transform.position;
        audio.SetActive(true);
        audio.GetComponent<PooledAudioSource>().PlaySound(pew);
    }

    protected virtual void ShootProjectile(Vector3 position, Vector3 direction)
    {
        GameObject projectile = ObjectPoolManager.GetPooledObject(bullet);
        projectile.transform.position = position;
        projectile.SetActive(true);
        projectile.GetComponent<BasicProjectile>().Fire(direction);
    }

}
