using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBehaviour
{
    LINEAR, // Pattern 1
    LINEAR_SWIPE, // Pattern 2
    SIN_PATH, // Pattern 3
	REVERSED_SIN_PATH,
    KAMIKAZE,
    TANK
};

public class EnemySphere : MonoBehaviour
{
    public int HealthPoints = 2;
    public uint KillReward = 10;
    public bool WaitUntilDeath = false;

    public eBehaviour Pattern = eBehaviour.LINEAR;

    public List<AudioClip> ExplosionSounds;

    [HideInInspector]
    public Vector3 movement = new Vector3(0, 0, -1);

    protected Rigidbody rigidBody;
    Renderer[] enemyRenderers;

    bool firstRebound = true;

    bool marked = false;
    float timeMarked = 0f;
    GameObject weaponMark;

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        enemyRenderers = transform.GetComponentsInChildren<Renderer>();

        if (Pattern == eBehaviour.KAMIKAZE)
        {
            gameObject.AddComponent<EnemyKamikazeBehaviour>();
        }
    }

    protected virtual void FixedUpdate()
    {
        CheckIfMarked();
        HandleMovement();
    }

    protected void CheckIfMarked()
    {
        if (marked)
        {
            timeMarked += Time.fixedDeltaTime;

            if (timeMarked > WorldConstants.Instance.MarkBulletTime)
            {
                marked = false;
                timeMarked = 0f;
                Debug.Log("Not marked anymore");
            }
        }
    }

    protected virtual void HandleMovement()
    {
        Vector3 updatedVelocity = movement * WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.EnemyMultiplier;

        if (Pattern == eBehaviour.SIN_PATH
			|| Pattern == eBehaviour.REVERSED_SIN_PATH)
        {
			var dirSign = ( Pattern == eBehaviour.SIN_PATH ) ? 1 : -1;
            updatedVelocity.x = Mathf.Sin(Time.time * WorldConstants.Instance.EnemySinusPatternLateralMultiplier * dirSign) * WorldConstants.Instance.EnemySinusPatternLateralWidth;
        }

        rigidBody.velocity = updatedVelocity;
    }

    protected virtual void OnTriggerEnter(Collider other)
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

    public virtual void TakeDamage(int damage, GameObject weaponFrom, bool phased)
    {
        if (phased && !marked)
        {
            marked = true;
            timeMarked = 0f;
            weaponMark = weaponFrom;
        }

        if (marked && weaponFrom != weaponMark)
        {
            HealthPoints -= damage * WorldConstants.Instance.MarkBulletDamageMultiplicator;
            Debug.Log("Much damage from phase!");
        }
        else
        {
            HealthPoints -= damage;
        }

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
        foreach (Renderer renderer in enemyRenderers)
        {
            renderer.enabled = false;
        }
        
        yield return new WaitForSeconds(0.1f);

        foreach (Renderer renderer in enemyRenderers)
        {
            renderer.enabled = true;
        }
    }    

    float PlayExplosionFX()
    {
        var audioSource = GetComponent<AudioSource>();
        var idx = Random.Range(0, ExplosionSounds.Count);
        audioSource.PlayOneShot(ExplosionSounds[idx]);

        return ExplosionSounds[idx].length;
    }
}
