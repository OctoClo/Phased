using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HealthPoints = 2;
    public List<AudioClip> ExplosionSounds;

    Rigidbody rigidBody;
    bool scrollTest = false;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

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
            var spaceshipInstance = other.GetComponent<Spaceship>();

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
            // Play animation of hurt
        }
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
