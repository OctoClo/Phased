using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [HideInInspector]
    public Vector3 movement = new Vector3(0, 0, -1);

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 updatedVelocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.ObstacleMultiplier);

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
