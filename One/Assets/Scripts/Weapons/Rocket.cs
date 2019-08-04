using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : BasicProjectile
{

    public float boomRadius = 2f;
    public LayerMask boomLM;

    public PooledObjectType explosion;
    public AudioClip boomSFX;

    protected override void HandleHit(Collider other)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, boomRadius, boomLM);
        foreach(Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.Damage();
            }
        }
        GameObject boom = ObjectPoolManager.GetPooledObject(explosion);
        boom.transform.position = transform.position;
        boom.SetActive(true);

        GameObject soundGO = ObjectPoolManager.GetPooledObject(PooledObjectType.AudioSource);
        soundGO.transform.position = transform.position;
        PooledAudioSource sound = soundGO.GetComponent<PooledAudioSource>();
        soundGO.SetActive(true);
        sound.PlaySound(boomSFX);
        pooledObjectHandler.ForceReturn();

    }

    public override void Fire(Vector3 direction)
    {
        base.Fire(direction);
        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1f, 1f);
    }
}
