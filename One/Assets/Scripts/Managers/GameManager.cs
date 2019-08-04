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

    public bool isInLevel = true;

    [System.Serializable]
    struct weaponSpriteInfo
    {
        public PooledObjectType weapon;
        public Sprite sprite;
    }
    [SerializeField]
    weaponSpriteInfo[] WeaponSprites;

    static Dictionary<PooledObjectType, Sprite> weaponSprites = new Dictionary<PooledObjectType, Sprite>();

    public static Sprite GetWeaponSprite(PooledObjectType weapon)
    {
        return weaponSprites[weapon];
    }


    private void Awake()
    {
        if(Instance) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach(weaponSpriteInfo weapon in WeaponSprites)
        {
            weaponSprites.Add(weapon.weapon, weapon.sprite);
        }
    }

    private void Start()
    {
        if(isInLevel)
        {
            StartCoroutine(ReadyGameplay());
        }
    }

    IEnumerator ReadyGameplay()
    {
        yield return ObjectPoolManager.PreinstantiateObjects();
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
