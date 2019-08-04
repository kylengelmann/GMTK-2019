using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{

    public int uses = -1;
    public PooledObjectType weaponType = PooledObjectType.Pistol;

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if(player)
        {
            if(player.PickupWeapon(weaponType))
            {
                if(uses > 0)
                {
                    --uses;
                    if(uses == 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
