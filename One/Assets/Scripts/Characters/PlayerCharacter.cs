using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    public PooledObjectType DefaultWeapon;
    WeaponBase currentWeapon;
    WeaponBase storedWeapon;

    public float weaponDist = 1f;

    public void Init()
    {
        currentWeapon = ObjectPoolManager.GetPooledObject(DefaultWeapon).GetComponent<WeaponBase>();
        currentWeapon.transform.position = transform.position + Vector3.right*weaponDist;
        currentWeapon.gameObject.SetActive(true);
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
        if(currentWeapon)
        {
            currentWeapon.Shoot();
            currentWeapon.transform.parent = null;
            currentWeapon = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.HasStartedLevel) return;
        float dt = TimeManager.GetTimeDelta(TimeChannel.Player);
        movementComponent.UpdatePosition(moveInput, dt);
        if(aimDir.sqrMagnitude < .04f)
        {
            aimDir = moveInput;
        }
        if(aimDir.sqrMagnitude > .04f && currentWeapon)
        {
            Vector3 aimDir3D = new Vector3(aimDir.x, 0f, aimDir.y);
            currentWeapon.transform.rotation = Quaternion.AngleAxis(-90, Vector3.up) * Quaternion.LookRotation(aimDir3D);
            currentWeapon.transform.position = transform.position + aimDir3D*weaponDist;
        }

    }
}
