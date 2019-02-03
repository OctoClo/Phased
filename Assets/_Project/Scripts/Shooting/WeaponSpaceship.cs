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
    Renderer cursorRenderer;
    Color cursorColor;
    Color flashColor;

    ScreenShake screenshake;

    protected override void Start()
    {
        screenshake = Camera.main.GetComponent<ScreenShake>();
        playerNumber = Spaceship.PlayerNumber;
        cursorRenderer = Cursor.GetComponent<Renderer>();
        cursorColor = cursorRenderer.material.GetColor("_EmissionColor");
        flashColor = cursorColor * 2;
        base.Start();
    }

    private void OnDisable()
    {
        cursorRenderer.material.SetColor("_EmissionColor", cursorColor);
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

        screenshake.Shake(0.05f, 0.12f);

        myBullet = bullet.GetComponent<BulletSpaceship>();
        myBullet.SetPhased(bulletPhased);
        myBullet.WeaponFrom = gameObject;

        StartCoroutine(MuzzleFlash());
    }

    IEnumerator MuzzleFlash()
    {
        cursorRenderer.material.SetColor("_EmissionColor", flashColor);
        yield return new WaitForSecondsRealtime(0.1f);
        cursorRenderer.material.SetColor("_EmissionColor", cursorColor);
    }
}
