using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class BasicEnemy : MonoBehaviour, IDamageable
{
    public PooledObjectType enemyType;

    [System.Serializable]
    struct weaponDropInfo
    {
        public PooledObjectType weaponType;
        public float probability;
    }

    [SerializeField]
    weaponDropInfo[] weaponDrops;

    public float numDrops = 2;

    public float speed;

    public PooledObjectType bullets = PooledObjectType.None;
    public float shootCooldown;

    public AudioClip dieSound;
    
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        float sum = 0;
        foreach(weaponDropInfo info in weaponDrops)
        {
            sum += info.probability;
        }
        for(int i = 0; i < weaponDrops.Length; ++i)
        {
            weaponDrops[i].probability /= sum;
        }
    }

    protected void DieSound()
    {
        GameObject audio = ObjectPoolManager.GetPooledObject(PooledObjectType.AudioSource);
        audio.transform.position = transform.position;
        audio.SetActive(true);
        audio.GetComponent<PooledAudioSource>().PlaySound(dieSound);
    }

  public virtual void Damage()
    {
        DieSound();
        Die();
    }

    protected virtual void Die()
    {
        for(int i = 0; i < numDrops; ++i) {
            GameObject pickup = ObjectPoolManager.GetPooledObject(PooledObjectType.WeaponPickup);
            pickup.transform.position = transform.position + new Vector3((Random.value - .5f)*2f, 0f, (Random.value - .5f)*2f);
            pickup.SetActive(true);
            WeaponPickup weaponPickup = pickup.GetComponent<WeaponPickup>();
            weaponPickup.weaponType = PickWeaponDrop();
            weaponPickup.Init();
        }
        GameObject splosion = ObjectPoolManager.GetPooledObject(PooledObjectType.DeathExplosion);
        splosion.transform.position = transform.position;
        splosion.SetActive(true);
        ObjectPoolManager.ReturnPooledObject(enemyType, gameObject);
    }

    PooledObjectType PickWeaponDrop()
    {
        float rand = Random.value;
        float curSum = 0;
        foreach(weaponDropInfo info in weaponDrops)
        {
            curSum += info.probability;
            if(rand < curSum)
            {
                return info.weaponType;
            }
        }
        return weaponDrops[weaponDrops.Length-1].weaponType;
    }
    

    float timeSinceDestSet;

    float timeSinceLastShoot;


    private void Update()
    {
        if(!GameManager.HasStartedLevel) return;
        float timeScale = TimeManager.GetTimeScale(TimeChannel.World);
        Vector3 targetPos = GameManager.Player.transform.position;

        agent.speed = speed*timeScale;
        agent.SetDestination(targetPos);

        transform.localScale = new Vector3(Mathf.Sign(targetPos.x - transform.position.x), 1f, 1f);

        timeSinceLastShoot += TimeManager.GetTimeDelta(TimeChannel.World);
        if(timeSinceLastShoot > shootCooldown && shootCooldown > 0.1f)
        {
            Shoot();
            timeSinceLastShoot = 0f;
        }
    }


    protected virtual void Shoot() 
    {
        if(bullets == PooledObjectType.None) return;
        
        GameObject bulletGO = ObjectPoolManager.GetPooledObject(bullets);
        bulletGO.transform.position = transform.position;
        BasicProjectile bullet = bulletGO.GetComponent<BasicProjectile>();
        bullet.Fire(GameManager.Player.transform.position - transform.position);
        bulletGO.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.Damage();
        }
    }


}
