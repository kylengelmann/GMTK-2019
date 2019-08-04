using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{

    public Image storage;
    public Image controlIcon;
    public Sprite controllerIcon;
    public Sprite keyboardIcon;

    private void Start()
    {
        InputManager.OnControllerModeChange += controllerModeChange;
        controllerModeChange(InputManager.CurrentControllerMode);
    }

    private void OnDestroy()
    {
        InputManager.OnControllerModeChange -= controllerModeChange;
    }

    void controllerModeChange(ControllerMode mode)
    {
        if(mode == ControllerMode.Controller)
        {
            controlIcon.sprite = controllerIcon;
        }
        else
        {
            controlIcon.sprite = keyboardIcon;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.HasStartedLevel)
        {
            if(GameManager.Player.storedWeapon)
            {
                storage.sprite = GameManager.GetWeaponSprite(GameManager.Player.storedWeapon.pooledObjectType);
                storage.color = Color.white;
            }
            else
            {
                storage.color = Color.clear;
            }
        }
    }
}
