using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public PooledObjectType weaponType;
    public SpriteRenderer spriteRenderer;
    PooledObjectHandler pooledObjectHandler;

    private void Awake()
    {
        pooledObjectHandler = GetComponent<PooledObjectHandler>();
    }

    public void Init()
    {
        spriteRenderer.sprite = GameManager.GetWeaponSprite(weaponType);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if(player)
        {
            bool wasPickedUp = player.PickupWeapon(weaponType);
            if(wasPickedUp)
            {
                pooledObjectHandler.ForceReturn();
            }
        }
    }
}
