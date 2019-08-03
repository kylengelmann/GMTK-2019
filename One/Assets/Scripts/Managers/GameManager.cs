using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public static PlayerCharacter Player {get; private set;}
    public PlayerCharacter PlayerPrefab;
    public static bool HasStartedLevel {get; private set;}
    public static Camera mainCamera;

    private void Awake()
    {
        if(Instance) {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Init()
    {
        StartLevel();
    }

    void StartLevel()
    {
        mainCamera = Camera.main;

        GameObject playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");

        Player = Instantiate(PlayerPrefab);
        Player.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
        Player.Init();

        if(OnLevelStart != null) {
            OnLevelStart();
            OnLevelStart = null;
        }
        HasStartedLevel = true;
    }

    public static UnityAction OnLevelStart;

}
