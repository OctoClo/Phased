using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemies : MonoBehaviour
{
    public GameObject prefabEnemy;
    public float spawnIntervalMin, spawnIntervalMax;
    public float spawnXMin, spawnXMax;
    public float spawnY, spawnZ;

    float spawnInterval;
    float lastSpawn;

    void Start()
    {
        spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    void Update()
    {
        lastSpawn += Time.deltaTime;

        if (lastSpawn > spawnInterval)
        {
            GameObject spawn = Instantiate(prefabEnemy, new Vector3(Random.Range(spawnXMin, spawnXMax), spawnY, spawnZ), Quaternion.identity);
            Enemy enemy = spawn.GetComponent<Enemy>();

            spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            lastSpawn = 0;
        }
    }
}
