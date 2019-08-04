﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class BasicEnemy : MonoBehaviour, IDamageable
{
    public PooledObjectType enemyType;

    public float speed;

    
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }



    public virtual void Damage()
    {
        Die();
    }

    protected virtual void Die()
    {
        GameObject pickup = ObjectPoolManager.GetPooledObject(PooledObjectType.WeaponPickup);
        pickup.transform.position = transform.position;
        pickup.SetActive(true);
        WeaponPickup weaponPickup = pickup.GetComponent<WeaponPickup>();
        weaponPickup.weaponType = PooledObjectType.Pistol;
        weaponPickup.Init();
        ObjectPoolManager.ReturnPooledObject(enemyType, gameObject);
    }
    

    float timeSinceDestSet;


    private void Update()
    {
        if(!GameManager.HasStartedLevel) return;
        float timeScale = TimeManager.GetTimeScale(TimeChannel.World);
        Vector3 targetPos = GameManager.Player.transform.position;

        agent.speed = speed*timeScale;
        agent.SetDestination(targetPos);

        transform.localScale = new Vector3(Mathf.Sign(targetPos.x - transform.position.x), 1f, 1f);
    }


    protected virtual void Shoot() { }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.Damage();
        }
    }


}
