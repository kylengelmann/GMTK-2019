using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IDamageable
{

    public GameObject Reticle;
    public PooledObjectType DefaultWeapon;
    WeaponBase currentWeapon;
    WeaponBase storedWeapon;

    public float weaponDist = 1f;

    public void Damage()
    {
        Debug.Log("OW");
    }

    public void Init()
    {
        PickupWeapon(DefaultWeapon);
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
            Reticle.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.HasStartedLevel) return;
        float dt = TimeManager.GetTimeDelta(TimeChannel.Player);
        movementComponent.UpdatePosition(moveInput, dt);
        if(aimDir.sqrMagnitude < .04f || !currentWeapon)
        {
            aimDir = moveInput;
        }
        if(aimDir.sqrMagnitude > .04f)
        {
            transform.localScale = new Vector3(Mathf.Sign(aimDir.x), 1f, 1f);
            if(currentWeapon) {

                Vector3 aimDir3D = new Vector3(aimDir.x, 0f, aimDir.y);
                currentWeapon.transform.rotation = Quaternion.AngleAxis(-90, Vector3.up) * Quaternion.LookRotation(aimDir3D * Mathf.Sign(aimDir.x));
                currentWeapon.transform.position = transform.position + aimDir3D*weaponDist;

                Reticle.transform.rotation = currentWeapon.transform.rotation;
                Reticle.transform.position = transform.position + aimDir3D * (weaponDist + 1f);
            }
        }

    }

    public bool PickupWeapon(PooledObjectType weaponType)
    {
        bool pickedUp = !currentWeapon;
        if(pickedUp)
        {
            Reticle.SetActive(true);
            currentWeapon = ObjectPoolManager.GetPooledObject(weaponType).GetComponent<WeaponBase>();
            currentWeapon.transform.position = transform.position + Vector3.right * weaponDist;
            currentWeapon.transform.parent = transform;
            currentWeapon.transform.localScale = Vector3.one;
            currentWeapon.gameObject.SetActive(true);
        }
        return pickedUp;
    }
}
