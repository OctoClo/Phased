using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpaceship : Bullet
{
    public int Damage = 1;
    public GameObject Impact_FX;
    
    void OnTriggerEnter(Collider other)
    {
        EnemySphere enemy = other.GetComponent<EnemySphere>();

        if (enemy)
        {

            GameObject impactFXGO = Instantiate(Impact_FX);
            impactFXGO.transform.position = transform.position;
            impactFXGO.transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);

            ParticleSystem impactFX = impactFXGO.GetComponent<ParticleSystem>();
            impactFX.Play();

            enemy.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
