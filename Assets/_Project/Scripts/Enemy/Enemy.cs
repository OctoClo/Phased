using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody rigidBody;
    bool scrollTest = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        scrollTest = Random.Range(0, 2) >= 1;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, -1);
        Vector3 updatedVelocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.ObstacleSpeedMultiplier);

        if (scrollTest)
        {
            updatedVelocity.x = Mathf.Sin(Time.time * WorldConstants.Instance.MovingObstacleLateralSpeed)
                                * WorldConstants.Instance.MovingObstacleLateralWidth;
        }

        rigidBody.velocity = updatedVelocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var spaceshipInstance = other.GetComponent<Spaceship>();

            if (!spaceshipInstance.lifeCounter.IsInvulnerable)
            {
                spaceshipInstance.lifeCounter.RemoveLife(spaceshipInstance);
            }
        }
    }
}
