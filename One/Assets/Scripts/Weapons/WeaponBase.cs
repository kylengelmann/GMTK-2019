using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponBase : MonoBehaviour
{
    bool hasFired;

    protected void Break()
    {
        
        //if(OnBreak != null)
        //{
        //    OnBreak();
        //}
    }

    //public UnityAction OnBreak;

    public void Shoot()
    {
        if(!hasFired) DoShoot();
        hasFired = true;
    }

    protected abstract void DoShoot();

}
