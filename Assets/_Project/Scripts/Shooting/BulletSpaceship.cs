using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpaceship : Bullet
{
    public int Damage = 1;
    public GameObject Impact_FX;

    [HideInInspector]
    public GameObject WeaponFrom;

    bool phasedBullet = false;

    public void SetPhased(bool phased)
    {
        phasedBullet = phased;
    }

    void OnTriggerEnter(Collider other)
    {
        EnemySphere enemy = other.GetComponent<EnemySphere>();

        if (enemy)
        {
            bool damageTaken;
            enemy.TakeDamage(Damage, WeaponFrom, phasedBullet, out damageTaken);

            if (damageTaken)
            {
                PlayImpactVFX();
            }

            Destroy(gameObject);
        }
    }

    void PlayImpactVFX()
    {
        GameObject impactFXGO = Instantiate(Impact_FX);
        impactFXGO.transform.position = transform.position;
        impactFXGO.transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);
        impactFXGO.transform.SetParent(FolderManager.Instance.VFXFolder.transform);

        ParticleSystem impactFX = impactFXGO.GetComponent<ParticleSystem>();
        impactFX.Play();
    }
}
