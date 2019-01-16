using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [Header("Parameters")]
    public int PlayerNumber = 0;
    public float Speed = 13.0f;
    public float Tilt = 0.6f;
    
    [Header("Weapon")]
    public float TargetRadius = 2.0f;
    public List<GameObject> Weapons;

    [Header("FX")]
    public GameObject LoseLifeFX;
    public GameObject DeathFX;

    [Header("Components")]
    public GameObject Cursor;
    public Rigidbody RigidBodyTilt;

    [Header("Misc")]
    public List<AudioClip> ImpactSounds;

    int soundIndex;

    [HideInInspector]
    public Vector2 Direction;
    [HideInInspector]
    public Vector2 Target;
    [HideInInspector]
    public bool IsFiring;

    Rigidbody rigidBody;
    Renderer spaceshipRenderer;
    Color emissiveColor;
    float previousEmissiveIntensity = 1.0f;
    float emissiveIntensity = 1.0f;
    float emissiveIntensityInterpolator = 1.0f;

    GameObject weaponGO;
    WeaponSpaceship weapon;
    
    Vector2 previousTarget;
    float previousAngle;
    float angleOffset = -90.0f;

    bool gameActive = false;
    bool phasedWeapon;
    Vector3 initialPosition;
    GameObject deathFXGO;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        spaceshipRenderer = GetComponentInChildren<Renderer>();
        soundIndex = 0;
        initialPosition = transform.position;
        emissiveColor = spaceshipRenderer.material.GetColor("_EmissionColor");
    }

    public void Initialize()
    {
        spaceshipRenderer.enabled = true;
        transform.position = initialPosition;
        phasedWeapon = false;
        gameActive = true;
        SetWeapon(EStatePhase.NO_PHASE);
    }

    void Update()
    {
        if (gameActive)
        {
            if (LifeCounter.Instance.DamageSource == this && LifeCounter.Instance.IsInvulnerable && (Time.frameCount % WorldConstants.Instance.PlayerFlickerFrequency) == 0)
            {
                spaceshipRenderer.enabled = !spaceshipRenderer.enabled;
            }
            else
            {
                spaceshipRenderer.enabled = true;
            }

            weapon.IsFiring = IsFiring;

            spaceshipRenderer.material.SetColor("_EmissionColor", emissiveColor * Mathf.Lerp(previousEmissiveIntensity, emissiveIntensity, emissiveIntensityInterpolator));

            emissiveIntensityInterpolator = Mathf.Min(1.0f, emissiveIntensityInterpolator + (2f * Time.deltaTime));
        }
    }

    void FixedUpdate()
    {
        if (gameActive)
        {
            float moveHorizontal = Direction.x;
            float moveVertical = Direction.y;

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            rigidBody.velocity = movement * Speed;
            RigidBodyTilt.transform.position = rigidBody.transform.position;

            RigidBodyTilt.rotation = Quaternion.Euler(rigidBody.velocity.z * Tilt, 0, rigidBody.velocity.x * -Tilt);

            float angle = 0.0f;

            if (!Target.Equals(Vector2.zero))
            {
                angle = (Mathf.Atan2(Target.y * TargetRadius, Target.x * TargetRadius) * Mathf.Rad2Deg) + angleOffset;
            }

            Cursor.transform.RotateAround(transform.position, Vector3.up, previousAngle - angle);

            previousAngle = angle;
        }
    }    

    public void SetWeapon(EStatePhase state)
    {
        if (weaponGO)
            Destroy(weaponGO);

        weaponGO = Instantiate(Weapons[(int)state], Cursor.transform);
        weapon = weaponGO.GetComponent<WeaponSpaceship>();
        weapon.Spaceship = this;
        weapon.Cursor = Cursor;
        weapon.SetPhased(phasedWeapon);
    }

    public void SetPhased(bool phased)
    {
        phasedWeapon = phased;
        weapon.SetPhased(phasedWeapon);
    }

    public void RemoveLife()
    {
        if (gameActive && !LifeCounter.Instance.IsInvulnerable)
        {
            LifeCounter.Instance.RemoveLife(this);
            StartCoroutine(OutputManager.VibrateAll(WorldConstants.Instance.PlayerHitVibrationDuration));
            PlayImpactSFX();
        }
    }

    void PlayImpactSFX()
    {
        var audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(ImpactSounds[soundIndex]);

        soundIndex++;
        if (soundIndex >= ImpactSounds.Count) soundIndex = 0;
    }

    public void PlayLoseLifeVFX()
    {
        GameObject loseLifeFXGO = Instantiate(LoseLifeFX, transform);
        loseLifeFXGO.transform.position = transform.position;
        loseLifeFXGO.transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);

        ParticleSystem[] loseLifeFXs = loseLifeFXGO.transform.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem fx in loseLifeFXs)
        {
            fx.Play();
        }
    }

    public void WaitUntilDeath()
    {
        gameActive = false;
        rigidBody.velocity = Vector3.zero;
        RigidBodyTilt.rotation = Quaternion.identity;
    }

    public void PlayDeathVFX()
    {
        deathFXGO = Instantiate(DeathFX, transform);
        deathFXGO.transform.position = transform.position;
        deathFXGO.transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);

        ParticleSystem[] deathFXs = deathFXGO.transform.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem fx in deathFXs)
        {
            fx.Play();
        }
    }

    public bool HasDeathFXFinished()
    {
        return (deathFXGO == null);
    }

    public void SetGlowIntensity(float value, bool instantly)
    {
        previousEmissiveIntensity = emissiveIntensity;
        emissiveIntensity = value;
        if(!instantly) emissiveIntensityInterpolator = 0;
    }
}
