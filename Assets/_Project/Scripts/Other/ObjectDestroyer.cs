using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        EnemySphere enemy = other.GetComponent<EnemySphere>();

        if (enemy)
        {
            Destroy(enemy.gameObject);
        }
        else
        {
            Bullet bullet = other.GetComponent<Bullet>();

            if (bullet)
            {
                Destroy(bullet.gameObject);
            }
            else
            {
                Obstacle obstacle = other.GetComponent<Obstacle>();

                if (obstacle)
                {
                    Destroy(obstacle.gameObject);
                }
            }
        }
    }
}
