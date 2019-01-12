using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();

        if (bullet)
        {
            Destroy(bullet.gameObject);
        }
        else
        {
            EnemySphere enemy = other.GetComponent<EnemySphere>();

            if (enemy && enemy.gameObject.activeSelf && enemy.Pattern != eBehaviour.LINEAR_SWIPE)
            {
                Destroy(enemy.gameObject);
            }
            else
            {
                Obstacle obstacle = other.GetComponent<Obstacle>();

                if (obstacle && obstacle.gameObject.activeSelf)
                {
                    Destroy(obstacle.gameObject);
                }
            }
        }
    }
}
