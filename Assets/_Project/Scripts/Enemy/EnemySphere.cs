using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySphere : MonoBehaviour
{
    public int HealthPoints = 2;
    public bool PatternSinus = false;
    public bool PatternSwipe = false;
    public List<AudioClip> ExplosionSounds;

    Rigidbody rigidBody;
    Renderer enemyRenderer;
    Vector3 movement;

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        enemyRenderer = GetComponent<Renderer>();

        movement = new Vector3(0, 0, -1);
    }

    void FixedUpdate()
    {
        Vector3 updatedVelocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.EnemySpeedMultiplier);

        if (PatternSinus)
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
            spaceshipInstance.RemoveLife();
        }

        if (other.CompareTag("WorldBounds"))
        {
            if (PatternSwipe)
            {
                movement.z = -movement.z;
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

    void Die()
    {
        PlayExplosionFX();
        Destroy(gameObject);
    }

   IEnumerator Blink()
    {
        enemyRenderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.enabled = true;
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
