using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, -1);
        Vector3 updatedVelocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.ObstacleSpeedMultiplier);

        rigidBody.velocity = updatedVelocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Spaceship spaceshipInstance = other.GetComponent<Spaceship>();
            spaceshipInstance.RemoveLife();
        }
    }
}
