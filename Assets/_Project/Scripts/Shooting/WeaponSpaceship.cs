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

    protected override void Start()
    {
        playerNumber = Spaceship.PlayerNumber;
        base.Start();
    }

    protected override bool IsReadytoFire()
    {
        return (IsFiring && lastFire >= FireInterval);
    }
}
