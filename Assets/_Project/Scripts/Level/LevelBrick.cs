﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBrick : MonoBehaviour
{
    public bool SpawnFromBack = false;

    public GameObject Arrow;
    public GameObject Plane;
    public GameObject SpawnableFolder;

    [HideInInspector]
    public bool HasSpawnedEverything = false;

    Vector3 reverseRotation = new Vector3(0, -180, 0);

    private void Start()
    {
        Arrow.SetActive(false);
    }

    private void Update()
    {
        if (!HasSpawnedEverything && SpawnableFolder.transform.childCount == 0)
        {
            HasSpawnedEverything = true;
        }
    }

    public void ReverseSpawnables()
    {
        for (int childIndex = 0; childIndex < SpawnableFolder.transform.childCount; childIndex++)
        {
            SpawnableFolder.transform.GetChild(childIndex).GetComponent<Spawnable>().ReverseMovement();
        }
    }

    public void WaitUntilBrickEnd()
    {
        for (int childIndex = 0; childIndex < SpawnableFolder.transform.childCount; childIndex++)
        {
            SpawnableFolder.transform.GetChild(childIndex).GetComponent<Spawnable>().LastLevel = true;
        }

        Plane.SetActive(false);
    }
}
