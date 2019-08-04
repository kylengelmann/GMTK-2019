using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{   
    [SerializeField]
    float speed = 10f;

    Vector3 direction;

    const float timeTransition = .5f;

    float timeAliveAbsolute = 0f;

    protected virtual void HandleHit(Collider other)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other);
    }

    public virtual void Fire(Vector3 direction)
    {
        this.direction = direction;
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
