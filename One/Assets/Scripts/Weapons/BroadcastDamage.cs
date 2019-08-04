using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastDamage : MonoBehaviour, IDamageable
{
    IDamageable parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.GetComponent<IDamageable>();
    }

    public void Damage(int amount = 1)
    {
        parent.Damage(amount);
    }
}
