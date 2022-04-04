using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] monsterPrefab;
    public Color blue;
    public Color purple;
    public Color red;
    public SpawnPoint[] spawners;


    private float waveStartTime;
    private int spawnedThisWave;

    public int currentWave { get; private set; }    //contains the wave currently active or the number of waves cleared
    public bool waveFinished { get; private set; }
    private Transform gridAnchor;
    private RTSController RTSC;

    // Start is called before the first frame update
    void Start()
    {
        RTSC = GetComponentInParent<RTSController>();
        gridAnchor = GameObject.Find("Grid Anchor").transform;
        waveFinished = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.M))
        {
            int rand = Random.Range(0, spawners.Length);
            MonsterBase monster = Instantiate(monsterPrefab[0], gridAnchor).GetComponent<MonsterBase>();
            monster.SetSpawn(spawners[rand]);
        }
        */

        if (Input.GetKeyDown(KeyCode.Q))
            StartNextWave();

        if (waveFinished)
            return;

        float waveTime = Time.time - waveStartTime;

        if (currentWave >= 1 && currentWave <= 3)
        {

            if (waveTime >= (currentWave < 3 ? 2 : 1) * spawnedThisWave)
            {
                //int rand = Random.Range(0, spawners.Length);
                int spawner;
                if (currentWave == 1)
                    spawner = 0;
                else if (currentWave == 2)
                    spawner = 1;

                else spawner = Random.Range(0, spawners.Length);

                SpawnMonster(RTSC, spawner, blue, 2, 30, 10, 1, 50, MonsterMove.path, MonsterAttack.attackAll);

                if (spawnedThisWave >= (currentWave < 3 ? 10 : 20))
                    waveFinished = true;
            }
        }
        else if (currentWave == 4)
        {
            if (waveTime >= 2 * spawnedThisWave)
            {
                SpawnMonster(RTSC, Random.Range(0, spawners.Length), purple, 2, 30, 10, 1, 50, MonsterMove.moveToNearestTower, MonsterAttack.attackTowers);

                if (spawnedThisWave >= 10)
                    waveFinished = true;
            }
        }
        else if (currentWave == 5)
        {
            if (waveTime >= 2 * spawnedThisWave)
            {
                SpawnMonster(RTSC, Random.Range(0, spawners.Length), red, 2, 30, 10, 1, 50, MonsterMove.moveToNearestPlayer, MonsterAttack.attackPlayers);

                if (spawnedThisWave >= 10)
                    waveFinished = true;
            }
        }

        else if (currentWave >= 6)
        {
            if (waveTime >= (3f / currentWave) * spawnedThisWave)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < .7f)
                    SpawnMonster(RTSC, Random.Range(0, spawners.Length), blue, 2 + .2f * currentWave, 5 * currentWave, 10, 1, 50, MonsterMove.path, MonsterAttack.attackAll);
                else if (rand < .8f)
                    SpawnMonster(RTSC, Random.Range(0, spawners.Length), purple, 2 + .2f * currentWave, 5 * currentWave, 10, 1, 50, MonsterMove.moveToNearestTower, MonsterAttack.attackTowers);
                else if (rand < .9f)
                    SpawnMonster(RTSC, Random.Range(0, spawners.Length), red, 2 + .2f * currentWave, 5 * currentWave, 10, 1, 50, MonsterMove.moveToNearestPlayer, MonsterAttack.attackPlayers);
                else
                    SpawnMonster(RTSC, Random.Range(0, spawners.Length), blue, 2 + .2f * currentWave, 5 * currentWave, 10, 1, 50, MonsterMove.wander, MonsterAttack.attackAll);

                if (spawnedThisWave >= 5 * currentWave)
                    waveFinished = true;
            }
        }
        
    }

    public void StartNextWave(float delay = 0)
    {
        waveStartTime = Time.time + delay;
        waveFinished = false;
        currentWave++;
        spawnedThisWave = 0;
    }

    public void SpawnMonster(RTSController rtsc, int spawner, Color color, float moveSpeed, int HP, int attack, float cooldown, int bones, MonsterMove moveType, MonsterAttack AttackType)
    {
        MonsterBase monster = Instantiate(monsterPrefab[0], gridAnchor).GetComponent<MonsterBase>();
        monster.SetSpawn(rtsc, spawners[spawner]);
        monster.SetStats(color, moveSpeed, HP, attack, cooldown, bones, moveType, AttackType);
        spawnedThisWave++;
    }
}