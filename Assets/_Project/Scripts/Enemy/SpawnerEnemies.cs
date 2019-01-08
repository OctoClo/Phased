using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemies : MonoBehaviour
{
    public GameObject PrefabEnemy;

    public EShootMode ShootMode = EShootMode.NEAREST;
    public EShootTarget ShootTarget = EShootTarget.RANDOM;
    public eBehaviour Pattern = eBehaviour.LINEAR;

    public float SpawnIntervalMin = 2f;
    public float SpawnIntervalMax = 4f;
    public float SpawnXMin = -20f;
    public float SpawnXMax = 20f;
    public float SpawnY = 1f;
    public float SpawnZ = 80f;

    float spawnInterval;
    float lastSpawn;

    void Start()
    {
        UnityEngine.Random.InitState(42);
        spawnInterval = UnityEngine.Random.Range(SpawnIntervalMin, SpawnIntervalMax);
    }

    void Update()
    {
        lastSpawn += Time.deltaTime;

        if (lastSpawn >= spawnInterval)
        {
            GameObject spawn = Instantiate(PrefabEnemy, new Vector3(UnityEngine.Random.Range(SpawnXMin, SpawnXMax), SpawnY, SpawnZ), Quaternion.identity);
            EnemySphere enemy = spawn.GetComponent<EnemySphere>();
            enemy.Pattern = Pattern;

            EnemySphereShooting enemyShooting = spawn.GetComponent<EnemySphereShooting>();
            if (enemyShooting)
            {
                enemyShooting.ShootMode = ShootMode;
                enemyShooting.ShootTarget = ShootTarget;
            }

            spawnInterval = UnityEngine.Random.Range(SpawnIntervalMin, SpawnIntervalMax);
            lastSpawn = 0;
        }
    }
}
