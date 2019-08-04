using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{

    public int uses = -1;
    public PooledObjectType weaponType = PooledObjectType.Pistol;

    public SpriteRenderer sprite;
    public float respawnTime = 5f;
    bool isActive = true;

    IEnumerator OnUse()
    {
        isActive = false;
        float time = 0;
        sprite.enabled = false;
        while (time < respawnTime)
        {
            time += TimeManager.GetTimeDelta(TimeChannel.Player);
            yield return null;
        }
        sprite.enabled = true;
        isActive = true;
        if(curOther)
        OnTriggerEnter(curOther);
    }

    Collider curOther;

    private void OnTriggerExit(Collider other)
    {
        if(other == curOther) curOther = null;
    }

    private void OnTriggerEnter(Collider other)
    {

        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if(!isActive)
        {
            curOther = other;
            return;
        }
        if(player)
        {
            if(player.PickupWeapon(weaponType))
            {
                StartCoroutine(OnUse());
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
