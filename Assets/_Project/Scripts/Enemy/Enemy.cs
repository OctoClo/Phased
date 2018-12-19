using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HealthPoints = 2;
    public List<AudioClip> ExplosionSounds;

    Rigidbody rigidBody;
    Renderer enemyRenderer;

    bool scrollTest = false;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        enemyRenderer = GetComponent<Renderer>();

        scrollTest = Random.Range(0, 2) >= 1;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, -1);
        Vector3 updatedVelocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.ObstacleSpeedMultiplier);

        if (scrollTest)
        {
            updatedVelocity.x = Mathf.Sin(Time.time * WorldConstants.Instance.MovingObstacleLateralSpeed)
                                * WorldConstants.Instance.MovingObstacleLateralWidth;
        }

        rigidBody.velocity = updatedVelocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Spaceship spaceshipInstance = other.GetComponent<Spaceship>();

            if (!spaceshipInstance.LifeCounter.IsInvulnerable)
            {
                spaceshipInstance.LifeCounter.RemoveLife(spaceshipInstance);
                spaceshipInstance.PlayImpactSFX();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        HealthPoints -= damage;

        if (HealthPoints <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Blink());
        }
    }

   IEnumerator Blink()
    {
        enemyRenderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.enabled = true;
    }

    void Die()
    {
        Destroy(gameObject, PlayExplosionFX());
    }

    float PlayExplosionFX()
    {
        var audioSource = GetComponent<AudioSource>();
        var idx = Random.Range(0, ExplosionSounds.Count);
        audioSource.PlayOneShot(ExplosionSounds[idx]);

        GetComponent<Renderer>().enabled = false;

        return ExplosionSounds[idx].length;
    }
}
