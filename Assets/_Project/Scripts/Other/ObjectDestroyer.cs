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

            if (enemy && !enemy.PatternSwipe)
            {
                Destroy(enemy.gameObject);
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
