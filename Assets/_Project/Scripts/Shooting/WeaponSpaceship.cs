using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpaceship : Weapon
{
    [HideInInspector]
    public Spaceship Spaceship;
    [HideInInspector]
    public bool IsFiring;

    int playerNumber;
    bool bulletPhased = false;
    BulletSpaceship myBullet;

    protected override void Start()
    {
        playerNumber = Spaceship.PlayerNumber;
        base.Start();
    }

    protected override bool IsReadytoFire()
    {
        return (IsFiring && lastFire >= FireInterval);
    }

    public void SetPhased(bool phased)
    {
        bulletPhased = phased;
    }

    protected override void Fire()
    {
        base.Fire();

        myBullet = bullet.GetComponent<BulletSpaceship>();
        myBullet.SetPhased(bulletPhased);
        myBullet.WeaponFrom = gameObject;
    }
}
