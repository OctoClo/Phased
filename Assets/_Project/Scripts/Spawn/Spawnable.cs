using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public GameObject Child;

    Vector3 movement = new Vector3(0, 0, -1);
    Rigidbody rigidBody;
    GameObject spawnsFolder;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        
        rigidBody.velocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.SpawnableSpeedMultiplier);

        spawnsFolder = GameObject.Find("Spawns");
    }

    public void Spawn()
    {
        Child.transform.SetParent(spawnsFolder.transform);
        Child.SetActive(true);
        Destroy(gameObject);
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
