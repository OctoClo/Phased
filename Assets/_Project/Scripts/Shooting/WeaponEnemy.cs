using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnemy : Weapon
{
    [HideInInspector]
    public GameObject SecondCursor;

    protected override void Fire()
    {
        base.Fire();

        // Fire second bullet for other spaceship if needed
        if (SecondCursor)
        {
            bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(90, SecondCursor.transform.rotation.eulerAngles.y, 0);
            bullet.transform.SetParent(bulletsFolder.transform);
        }
    }
}
