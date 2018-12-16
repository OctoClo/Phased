using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, -1);
        rigidBody.velocity = movement * ( WorldConstants.WorldScrollSpeed * WorldConstants.WorldScrollSpeed * WorldConstants.ObstacleSpeedMultiplier);
    }

    void OnTriggerEnter( Collider other )
    {
        if ( other.CompareTag( "Player" ) )
        {
            var spaceshipInstance = other.GetComponent<Spaceship>();

            // Remove life
            spaceshipInstance.Life -= WorldConstants.ObstacleCollisionDamage;
        }
    }
}
