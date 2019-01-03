using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpaceship : Bullet
{
    public int Damage = 1;

    void OnTriggerEnter(Collider other)
    {
        EnemySphere enemy = other.GetComponent<EnemySphere>();

        if (enemy)
        {
            enemy.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
