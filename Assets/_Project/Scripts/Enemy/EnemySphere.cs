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
    public Vector3 PopupScoreOffset = new Vector3(0, 3, 0);
    public bool WaitUntilDeath = false;

    public float MoveSpeed = 10f;

    public eBehaviour Pattern = eBehaviour.LINEAR;

    public GameObject DeathFX;
    [HideInInspector]
    public Vector3 movement = new Vector3(0, 0, -1);
    
    public string SoundEventName = "ennemi"; // ennemi, elites ou boss
    
    protected Rigidbody rigidBody;
    Renderer[] enemyRenderers;
    List<Color> matColors = new List<Color>();
    public Color hitColor;
    public float hitIntensity = 10.0f;

    bool firstRebound = true;

    bool marked = false;
    float timeMarked = 0f;
    GameObject weaponMark;
    protected bool reverseMove = false;

    protected GameObject deathFXGO;

    ScreenPause screenPause;

    protected virtual void Start()
    {
        screenPause = GameObject.Find("Scripts/ScreenPause").GetComponent<ScreenPause>();

        rigidBody = GetComponent<Rigidbody>();
        enemyRenderers = transform.GetComponentsInChildren<Renderer>();

        if (Pattern == eBehaviour.KAMIKAZE)
        {
            gameObject.AddComponent<EnemyKamikazeBehaviour>();
        }

        foreach (Renderer renderer in enemyRenderers)
        {
            if(renderer.material.IsKeywordEnabled("_EMISSION"))
            {
                matColors.Add(renderer.material.GetColor("_EmissionColor"));
            }
            
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
            }
        }
    }

    protected virtual void HandleMovement()
    {
        Vector3 updatedVelocity = movement * WorldConstants.Instance.WorldScrollSpeed * MoveSpeed;

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

    public virtual void TakeDamage(int damage, GameObject weaponFrom, bool phased, out bool damageTaken)
    {
        damageTaken = true;

        if (phased && !marked)
        {
            marked = true;
            timeMarked = 0f;
            weaponMark = weaponFrom;
        }

        if (marked && weaponFrom != weaponMark)
        {
            HealthPoints -= damage * WorldConstants.Instance.MarkBulletDamageMultiplicator;
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
            AkSoundEngine.PostEvent("impact_" + SoundEventName, gameObject);
            StartCoroutine(Blink());
        }
    }

    void Die()
    {
        //screenPause.Pause(0.1f);
        PlayExplosionSFX();
        PlayDeathVFX();
        GameScore.AddToScore(KillReward, transform.position + PopupScoreOffset);
        Destroy(gameObject);
    }

    IEnumerator Blink()
    {
        foreach (Renderer renderer in enemyRenderers)
        {
            if (renderer.material.IsKeywordEnabled("_EMISSION")) renderer.material.SetColor("_EmissionColor", hitColor * hitIntensity);
        }
        
        yield return new WaitForSecondsRealtime(0.1f);

        int matIndex = 0;

        for (int i = 0; i < enemyRenderers.Length; i++)
        {
            if (enemyRenderers[i].material.IsKeywordEnabled("_EMISSION")) 
            {
                enemyRenderers[i].material.SetColor("_EmissionColor", matColors[matIndex]);
                matIndex++;
            }
        }
    }

    void PlayExplosionSFX()
    {
        AkSoundEngine.PostEvent("explosion_" + SoundEventName, gameObject);
	}
	
    public virtual void PlayDeathVFX()
	{
        deathFXGO = Instantiate(DeathFX, transform);
        deathFXGO.transform.position = transform.position + new Vector3(0, 2, 0);
        deathFXGO.transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);
        deathFXGO.transform.SetParent(FolderManager.Instance.VFXFolder.transform);

        ParticleSystem[] deathFXs = deathFXGO.transform.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem fx in deathFXs)
        {
            fx.Play();
        }
    }

    public void ReverseMovement()
    {
        reverseMove = true;
        movement.z = -movement.z;
        transform.Rotate(0, 180, 0);
    }
}
