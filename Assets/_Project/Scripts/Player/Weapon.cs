using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float FireInterval = 0.3f;
    public List<AudioClip> FireSounds;

    [HideInInspector]
    public Spaceship Spaceship;
    [HideInInspector]
    public bool IsFiring;

    int playerNumber;
    float lastFire;

    GameObject cursor;
    GameObject bullet;

    int soundIndex;
    AudioSource audioSource;

    void Start()
    {
        playerNumber = Spaceship.PlayerNumber;
        cursor = Spaceship.Cursor;
        lastFire = float.MaxValue;
        soundIndex = 0;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        lastFire += Time.deltaTime;

        if (IsFiring && lastFire >= FireInterval)
        {
            audioSource.PlayOneShot(FireSounds[soundIndex], 0.15f);

            soundIndex++;
            if (soundIndex >= FireSounds.Count) soundIndex = 0;
            
            bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(90, cursor.transform.rotation.eulerAngles.y, 0);

            lastFire = 0;
        }
    }
}
