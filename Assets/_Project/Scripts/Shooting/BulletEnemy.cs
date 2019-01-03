using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : Bullet
{
    void OnTriggerEnter(Collider other)
    {
        Spaceship spaceship = other.GetComponent<Spaceship>();

        if (spaceship)
        {
            spaceship.RemoveLife();
            Destroy(gameObject);
        }
        else
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();

            if (obstacle)
            {
                Destroy(gameObject);
            }
        }
    }
}
