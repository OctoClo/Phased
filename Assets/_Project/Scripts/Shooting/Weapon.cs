using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float FireInterval = 0.3f;

    public string SoundEventName = "joueur"; // joueur, ennemi, elites ou boss

    [HideInInspector]
    public GameObject Cursor;

    protected float lastFire;
    protected GameObject bullet;

    protected GameObject bulletsFolder;

    protected virtual void Start()
    {
        lastFire = float.MaxValue;

        bulletsFolder = FolderManager.Instance.BulletsFolder;
    }

    protected virtual void Update()
    {
        lastFire += Time.deltaTime;

        if (IsReadytoFire())
        {
            lastFire = 0;
            
            Fire();            
        }
    }

    protected virtual bool IsReadytoFire()
    {
        return (lastFire >= FireInterval);
    }

    protected virtual void Fire()
    {
        AkSoundEngine.PostEvent("fire_" + SoundEventName, gameObject);

        bullet = Instantiate(BulletPrefab, Cursor.transform.position, Quaternion.identity);
        bullet.transform.rotation = Quaternion.Euler(90, Cursor.transform.rotation.eulerAngles.y, 0);
        bullet.transform.SetParent(bulletsFolder.transform);
    }
}
