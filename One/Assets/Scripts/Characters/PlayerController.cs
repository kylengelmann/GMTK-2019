using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerControlType
{
    None = 0,
    Move = 1,
    Aim = 2,
    Shoot = 4,
}

public class PlayerController : MonoBehaviour
{
    PlayerControlType disabledControlTypes = PlayerControlType.None;
    public PlayerCharacter Character {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        Character = GetComponent<PlayerCharacter>();
    }

    public void SetControlTypeEnabled(PlayerControlType type, bool enabled)
    {
        if(enabled)
        {
            disabledControlTypes &= ~type;
        }
        else
        {
            disabledControlTypes |= type;
        }
    }

    public bool GetControleTypeEnabled(PlayerControlType type)
    {
        return (disabledControlTypes & type) == 0;
    }


    void Update()
    {
        if(!GameManager.HasStartedLevel) return;
        if(GetControleTypeEnabled(PlayerControlType.Move)) {
            Character.SetMoveInput(new Vector2(InputManager.GetAxis(PlayerInputType.MoveX), InputManager.GetAxis(PlayerInputType.MoveY)));
        }
        else
        {
            Character.SetMoveInput(Vector2.zero);
        }
        if(GetControleTypeEnabled(PlayerControlType.Aim))
        {
            Character.SetAimDirection(new Vector2(InputManager.GetAxis(PlayerInputType.AimX), InputManager.GetAxis(PlayerInputType.AimY)));
        }
        else
        {
            Character.SetAimDirection(Vector2.zero);
        }
    }
}
