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

    int soundIndex;
    int playerNumber;
    float lastFire;
    GameObject cursor;
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

            Instantiate(BulletPrefab, cursor.transform.position, BulletPrefab.transform.rotation);
            lastFire = 0;
        }
    }
}
