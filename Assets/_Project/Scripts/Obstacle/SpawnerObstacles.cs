using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObstacles : MonoBehaviour
{
    public GameObject prefab;
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

        if (lastSpawn >= spawnInterval)
        {
            GameObject spawn = Instantiate(prefab, new Vector3(Random.Range(spawnXMin, spawnXMax), spawnY, spawnZ), Quaternion.identity);
            Obstacle obstacle = spawn.GetComponent<Obstacle>();

            spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            lastSpawn = 0;
        }
    }
}
