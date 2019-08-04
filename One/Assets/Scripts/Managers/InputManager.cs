using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerInputType
{
    MoveX = 0,
    MoveY,
    AimX,
    AimY,
    Shoot,
    Switch,

    NumInputTypes
}

public enum ControllerMode
{
    Keyboard = 0,
    Controller
}


public class InputManager : MonoBehaviour
{
    static Dictionary<PlayerInputType, float> inputValues = new Dictionary<PlayerInputType, float>();
    static Dictionary<PlayerInputType, bool> wasDown = new Dictionary<PlayerInputType, bool>();

    public float deadZone = .25f;

    public static ControllerMode CurrentControllerMode {get; private set;}
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < (int)PlayerInputType.NumInputTypes; ++i)
        {
            inputValues.Add((PlayerInputType)i, 0f);
            wasDown.Add((PlayerInputType)i, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < (int)PlayerInputType.NumInputTypes; ++i)
        {
            wasDown[(PlayerInputType)i] = inputValues[(PlayerInputType)i] > 0f;
        }

        ControllerMode prevMode = CurrentControllerMode;

        Vector2 tempAxis = new Vector2(Input.GetAxisRaw("MoveX"), Input.GetAxisRaw("MoveY"));
        if(tempAxis.sqrMagnitude < deadZone*deadZone)
        {
            tempAxis = Vector2.zero;
        }
        else
        {
            CurrentControllerMode = ControllerMode.Controller;
        }
        inputValues[PlayerInputType.MoveX] = tempAxis.x;
        inputValues[PlayerInputType.MoveY] = tempAxis.y;

        tempAxis = new Vector2(Input.GetAxisRaw("AimX"), Input.GetAxisRaw("AimY"));
        if (tempAxis.sqrMagnitude < deadZone * deadZone)
        {
            tempAxis = Vector2.zero;
        }
        else
        {
            CurrentControllerMode = ControllerMode.Controller;
        }
        inputValues[PlayerInputType.AimX] = tempAxis.x;
        inputValues[PlayerInputType.AimY] = tempAxis.y;

        float shoot = Input.GetAxisRaw("Shoot");
        inputValues[PlayerInputType.Shoot] = shoot;
        if(shoot > 0f)
        {
            CurrentControllerMode = ControllerMode.Controller;
        }

        float Switch = Input.GetKey(KeyCode.Joystick8Button5) ? 1f : -1f;
        if(Switch > 0f)
        {
            CurrentControllerMode = ControllerMode.Controller;
        }
        inputValues[PlayerInputType.Switch] = Switch;


        if(Input.GetKey(KeyCode.A))
        {
            tempAxis.x = -1f;
            CurrentControllerMode = ControllerMode.Keyboard;
        }
        if(Input.GetKey(KeyCode.D))
        {
            tempAxis.x += 1f;
            CurrentControllerMode = ControllerMode.Keyboard;
        }

        if (Input.GetKey(KeyCode.S))
        {
            tempAxis.y = -1f;
            CurrentControllerMode = ControllerMode.Keyboard;
        }
        if (Input.GetKey(KeyCode.W))
        {
            tempAxis.y += 1f;
            CurrentControllerMode = ControllerMode.Keyboard;
        }

        if(Input.GetKey(KeyCode.Mouse0))
        {
            shoot = 1f;
            CurrentControllerMode = ControllerMode.Keyboard;
        }
        else
        {
            shoot = -1f;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Switch = 1f;
            CurrentControllerMode = ControllerMode.Keyboard;
        }
        else
        {
            Switch = -1f;
        }

        if ((Input.mousePosition - mousePos).sqrMagnitude > .5f)
        {
            mousePos = Input.mousePosition;
            CurrentControllerMode = ControllerMode.Keyboard;
        }

        if(CurrentControllerMode == ControllerMode.Keyboard)
        {
            tempAxis.Normalize();
            inputValues[PlayerInputType.MoveX] = tempAxis.x;
            inputValues[PlayerInputType.MoveY] = tempAxis.y;

            tempAxis = getMouseAim();
            inputValues[PlayerInputType.AimX] = tempAxis.x;
            inputValues[PlayerInputType.AimY] = tempAxis.y;


            inputValues[PlayerInputType.Shoot] = shoot;
            inputValues[PlayerInputType.Switch] = Switch;
        }

        if(CurrentControllerMode != prevMode && OnControllerModeChange != null)
        {
            OnControllerModeChange(CurrentControllerMode);
        }
    }

    public static UnityAction<ControllerMode> OnControllerModeChange;


    Vector3 mousePos;

    Vector2 getMouseAim()
    {
        if(!GameManager.HasStartedLevel) return Vector2.zero;

        Vector3 playerPosScreen = GameManager.mainCamera.WorldToScreenPoint(GameManager.Player.transform.position);
        Vector2 aimAxis = mousePos - playerPosScreen;
        return aimAxis.normalized;
    }

    public static float GetAxis(PlayerInputType inputType)
    {
        return inputValues[inputType];
    }

    public static bool GetButton(PlayerInputType inputType)
    {
        return inputValues[inputType] > 0f;
    }

    public static bool GetButtonDown(PlayerInputType inputType)
    {
        return !wasDown[inputType] && GetButton(inputType);
    }

    public static bool GetButtonUp(PlayerInputType inputType)
    {
        return wasDown[inputType] && GetButton(inputType);
    }
}
