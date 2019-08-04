using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    struct spawnInfo
    {
        public PooledObjectType enemyType;
        public float probability;
    }

    [SerializeField]
    spawnInfo[] spawns;

    public AnimationCurve chanceOfSpawning;
    public float timeFollowCurve = 300f;

    // Start is called before the first frame update
    void Start()
    {
        float sum = 0f;
        foreach(spawnInfo info in spawns)
        {
            sum += info.probability;
        }

        for(int i = 0; i < spawns.Length; ++i)
        {
            spawns[i].probability /= sum;
        }
        GameManager.OnLevelStart += OnStart;
    }

    void OnStart()
    {
        StartCoroutine(spawnLoop());
    }

    const float waitTime = 1f;

    float probOfSpawning;

    float timeAlive;

    IEnumerator spawnLoop()
    {
        float timeWaiting = Random.value * waitTime;
        while(true)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            if ((viewPos.x > -.05f && viewPos.x < 1.05f) && (viewPos.y > -.05f && viewPos.y < 1.05f))
            {
                yield return null;
                continue;
            }
            timeWaiting += TimeManager.GetTimeDelta(TimeChannel.World);
            timeAlive += TimeManager.GetTimeDelta(TimeChannel.World);
            if(timeWaiting < waitTime)
            {
                yield return null;
                continue;
            }

            float probOfSpawn = chanceOfSpawning.Evaluate(Mathf.Clamp01(timeAlive/timeFollowCurve));

            float rand = Random.value;
            if(rand < probOfSpawn)
            {
                Spawn();
            }


            yield return null;
        }
    }

    void Spawn()
    {
        float sum = 0;
        float rand = Random.value;
        PooledObjectType spawning = spawns[spawns.Length - 1].enemyType;
        foreach(spawnInfo info in spawns)
        {
            sum += info.probability;
            if(sum < rand)
            {
                spawning = info.enemyType;
                break;
            }
        }

        GameObject enemy = ObjectPoolManager.GetPooledObject(spawning);
        enemy.transform.position = transform.position;
        enemy.SetActive(true);

    }

}
