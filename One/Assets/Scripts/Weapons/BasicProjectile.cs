using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(int damage = 1);
}

public class BasicProjectile : MonoBehaviour
{   
    [SerializeField]
    float speed = 10f;

    

    protected Vector3 direction;

    const float timeTransition = .5f;

    float timeAliveAbsolute = 0f;

    protected PooledObjectHandler pooledObjectHandler;

    private void Awake()
    {
        pooledObjectHandler = GetComponent<PooledObjectHandler>();
    }

    protected virtual void HandleHit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.Damage();
        }
        pooledObjectHandler.ForceReturn();
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other);
    }

    public virtual void Fire(Vector3 direction)
    {
        this.direction = direction.normalized;
        timeAliveAbsolute = 0f;
    }

    protected float dt;

    protected virtual void Update()
    {
        timeAliveAbsolute += TimeManager.GetTimeDelta(TimeChannel.Absolute);
        dt = Mathf.Lerp(TimeManager.GetTimeDelta(TimeChannel.Player), TimeManager.GetTimeDelta(TimeChannel.World), timeAliveAbsolute/timeTransition);
        transform.position += direction*speed*dt;
    }
}
