using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float FireInterval = 0.3f;

    [HideInInspector]
    public Spaceship Spaceship;
    [HideInInspector]
    public bool IsFiring;

    int playerNumber;
    float lastFire;
    GameObject cursor;

    void Start()
    {
        playerNumber = Spaceship.PlayerNumber;
        cursor = Spaceship.Cursor;
        lastFire = 9999;
    }

    void Update()
    {
        lastFire += Time.deltaTime;
        if (IsFiring && lastFire >= FireInterval)
        {
            Instantiate(BulletPrefab, cursor.transform.position, BulletPrefab.transform.rotation);
            lastFire = 0;
        }
    }
}
