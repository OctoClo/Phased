using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBrick : MonoBehaviour
{
    public int SpawnableSpeed = 5;

    public GameObject Plane;
    public GameObject SpawnableFolder;

    [HideInInspector]
    public bool HasSpawnedEverything = false;

    private void Start()
    {
        List<GameObject> spawnables = new List<GameObject>();
        Spawnable spawnable;

        for (int childIndex = 0; childIndex < SpawnableFolder.transform.childCount; childIndex++)
        {
            spawnable = SpawnableFolder.transform.GetChild(childIndex).GetComponent<Spawnable>();

            if (spawnable)
            {
                spawnable.Speed = SpawnableSpeed;
            }
        }
    }

    private void Update()
    {
        if (!HasSpawnedEverything && SpawnableFolder.transform.childCount == 0)
        {
            HasSpawnedEverything = true;
        }
    }
}
