using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpaceship : Bullet
{
    public int Damage = 1;

    [HideInInspector]
    public GameObject WeaponFrom;

    bool phasedBullet = false;

    public void SetPhased(bool phased)
    {
        phasedBullet = phased;
    }

    void OnTriggerEnter(Collider other)
    {
        EnemySphere enemy = other.GetComponent<EnemySphere>();

        if (enemy)
        {
            enemy.TakeDamage(Damage, WeaponFrom, phasedBullet);
            Destroy(gameObject);
        }
    }
}
