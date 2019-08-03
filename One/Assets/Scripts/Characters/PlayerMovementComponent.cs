using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementComponent : MonoBehaviour
{
    CharacterController charControl;

    public float MaxSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        charControl = GetComponent<CharacterController>();
    }

    float friction;

    public void UpdatePosition(Vector2 direction, float dt)
    {
        Vector3 ds = Vector3.zero;
        ds.x = direction.x;
        ds.z = direction.y;

        ds *= MaxSpeed;

        ds += Physics.gravity*dt;
        ds *= dt;
        charControl.Move(ds);
    }
}
