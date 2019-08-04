using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : BasicGun
{
    public int numShots = 5;
    public float spread = 60f;
    protected override void DoShoot()
    {
        Vector3 dir = FirePoint.transform.forward;
        Quaternion dirRot = Quaternion.AngleAxis(-spread/2f, Vector3.up);
        float angleStep = spread / (((float)numShots - 1f));
        Quaternion stepRot = Quaternion.AngleAxis(angleStep, Vector3.up);
        for(int i = 0; i < numShots; ++i)
        {
            ShootProjectile(FirePoint.position, dirRot * dir);
            dirRot = stepRot * dirRot;
        }
        Pew();
        Break();
    }

}
