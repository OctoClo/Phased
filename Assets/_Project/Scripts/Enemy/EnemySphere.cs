using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBehaviour
{
    LINEAR, // Pattern 1
    LINEAR_SWIPE, // Pattern 2
    SIN_PATH, // Pattern 3
    KAMIKAZE,
};

public class EnemySphere : MonoBehaviour
{
    public int HealthPoints = 2;
    public uint KillReward = 10;

    public eBehaviour Pattern = eBehaviour.LINEAR;
    public List<AudioClip> ExplosionSounds;

    Rigidbody rigidBody;
    Renderer enemyRenderer;
    Vector3 movement;

    bool firstRebound = true;

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        enemyRenderer = GetComponent<Renderer>();

        movement = new Vector3(0, 0, -1);

        if (Pattern == eBehaviour.KAMIKAZE)
        {
            gameObject.AddComponent<EnemyKamikazeBehaviour>();
        }
    }

    void FixedUpdate()
    {
        Vector3 updatedVelocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.EnemySpeedMultiplier);

        if (Pattern == eBehaviour.SIN_PATH)
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
            if (Pattern == eBehaviour.LINEAR_SWIPE)
            {
                if (firstRebound)
                {
                    firstRebound = false;
                }
                else
                {
                    movement.z = -movement.z;
                }
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
        GameScore.AddToScore(KillReward);
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
