using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float FireInterval = 0.3f;    

    public List<AudioClip> FireSounds;

    [HideInInspector]
    public GameObject Cursor;

    protected float lastFire;
    protected GameObject bullet;

    protected int soundIndex;
    protected AudioSource audioSource;

    GameObject bulletsFolder;

    protected virtual void Start()
    {
        lastFire = float.MaxValue;
        soundIndex = 0;
        audioSource = GetComponent<AudioSource>();

        bulletsFolder = GameObject.Find("Bullets");
    }

    protected virtual void Update()
    {
        lastFire += Time.deltaTime;

        if (IsReadytoFire())
        {
            audioSource.PlayOneShot(FireSounds[soundIndex], 0.15f);

            soundIndex++;
            if (soundIndex >= FireSounds.Count) soundIndex = 0;
            
            bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(90, Cursor.transform.rotation.eulerAngles.y, 0);
            bullet.transform.SetParent(bulletsFolder.transform);

            lastFire = 0;
        }
    }

    protected virtual bool IsReadytoFire()
    {
        return (lastFire >= FireInterval);
    }
}
