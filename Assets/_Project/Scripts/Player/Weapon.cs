using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject prefabBullet;
    public float fireInterval;

    float lastFire;
    int playerNumber;
    GameObject viewfinder;

    [HideInInspector]
    public Spaceship Spaceship;
    PlayerInputManager inputManager;

    void Start()
    {
        playerNumber = Spaceship.PlayerNumber;
        inputManager = Spaceship.InputManager;

        viewfinder = Spaceship.gameObject.transform.Find("Viewfinder").gameObject;
        lastFire = 9999;
    }

    void Update()
    {
        lastFire += Time.deltaTime;
        if (inputManager.Fire[playerNumber] > 0.0f && lastFire >= fireInterval)
        {
            Instantiate(prefabBullet, viewfinder.transform.position, prefabBullet.transform.rotation);
            lastFire = 0;
        }
    }
}
