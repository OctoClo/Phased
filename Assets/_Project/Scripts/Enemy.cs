using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody rigidBody;

    // PROTOTYPE TEST
    bool scrollTest = false;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        scrollTest = Random.Range(0, 2) >= 1;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, -1);
        Vector3 updatedVelocity = movement * ( WorldConstants.WorldScrollSpeed * WorldConstants.WorldScrollSpeed * WorldConstants.ObstacleSpeedMultiplier);

        if (scrollTest)
        {
            updatedVelocity.x = Mathf.Sin( Time.time * WorldConstants.MovingObstacleLateralSpeed) 
                                * WorldConstants.MovingObstacleLateralWidth;
        }

        rigidBody.velocity = updatedVelocity;
    }

    void OnTriggerEnter( Collider other )
    {
        if ( other.CompareTag( "Player" ) )
        {
            var spaceshipInstance = other.GetComponent<Spaceship>();

            // PROTO TEST Remove life
            // TODO Implement life system
            spaceshipInstance.Life -= WorldConstants.ObstacleCollisionDamage;
        }
    }
}
