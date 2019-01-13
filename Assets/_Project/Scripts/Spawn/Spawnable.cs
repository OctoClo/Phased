using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public GameObject Child;

    Vector3 movement = new Vector3(0, 0, -1);
    Rigidbody rigidBody;
    GameObject spawnsFolder;

    bool waitUntilDeath = false;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        
        rigidBody.velocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.SpawnableMultiplier);

        spawnsFolder = FolderManager.Instance.SpawnFolder;
    }

    public void Spawn()
    {
        Child.transform.SetParent(spawnsFolder.transform);
        Child.SetActive(true);

        EnemySphere enemy = Child.GetComponent<EnemySphere>();
        if (enemy)
        {
            waitUntilDeath = enemy.WaitUntilDeath;
            rigidBody.velocity = Vector3.zero;

            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }

        if (!waitUntilDeath)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (waitUntilDeath && Child == null)
        {
            Destroy(gameObject);
        }
    }

    public void ReverseMovement()
    {
        movement.z = -movement.z;

        EnemySphere enemy = Child.GetComponent<EnemySphere>();
        if (enemy)
        {
            enemy.movement.z = -enemy.movement.z;
            enemy.transform.Rotate(0, 180, 0);
        }
        else
        {
            Obstacle obstacle = Child.GetComponent<Obstacle>();
            if (obstacle)
            {
                obstacle.movement.z = -obstacle.movement.z;
            }
        }
    }
}
