using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

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

            Obstacle obstacle = other.GetComponent<Obstacle>();

            if (obstacle)
            {
                Destroy(obstacle.gameObject);
            }
        }
    }
}
