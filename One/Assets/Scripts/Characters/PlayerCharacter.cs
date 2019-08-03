﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    public void Init()
    {
        
    }

    PlayerMovementComponent movementComponent;

    // Start is called before the first frame update
    void Start()
    {
        movementComponent = GetComponent<PlayerMovementComponent>();   
    }

    Vector2 moveInput = Vector2.zero;

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    Vector2 aimDir = Vector2.zero;
    public void SetAimDirection(Vector2 input)
    {
        aimDir = input;
    }

    public void Shoot()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.HasStartedLevel) return;
        float dt = Time.deltaTime;
        movementComponent.UpdatePosition(moveInput, dt);
        if(aimDir.sqrMagnitude < .04f)
        {
            aimDir = moveInput;
        }
        if(aimDir.sqrMagnitude >= .04f) {

            transform.rotation = Quaternion.LookRotation(new Vector3(aimDir.x, 0f, aimDir.y), Vector3.up);
            Debug.Log(transform.forward);
        }
    }
}