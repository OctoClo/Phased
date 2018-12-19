using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public int PlayerNumber = 0;

    public float Speed = 13.0f;
    public float Tilt = 0.6f;
    
    public GameObject WeaponPrefab;

    public float TargetRadius = 2.0f;

    public GameObject Cursor;
    public Rigidbody RigidBodyTilt;

    public LifeCounter LifeCounter;
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

    GameObject weaponGO;
    Weapon weapon;

    Vector3 cursorScale;
    Vector2 previousTarget;
    float previousAngle = 0.0f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        spaceshipRenderer = GetComponentInChildren<Renderer>();

        cursorScale = Cursor.transform.localScale;
        soundIndex = 0;

        SetWeapon(WeaponPrefab);
    }

    void Update()
    {
        if (LifeCounter.DamageSource == this && LifeCounter.IsInvulnerable && (Time.frameCount % WorldConstants.Instance.PlayerFlickerFrequency) == 0)
        {
            spaceshipRenderer.enabled = !spaceshipRenderer.enabled;
        }
        else
        {
            spaceshipRenderer.enabled = true;
        }

        weapon.IsFiring = IsFiring;
    }

    void FixedUpdate()
    {
        if (LifeCounter.HasNoLifeLeft)
        {
            // TODO How should the player be removed?
            Vector3 movementTest = new Vector3(1, 0, 0);
            rigidBody.velocity = movementTest * Speed * 32.0f;
            return;
        }

        float moveHorizontal = Direction.x;
        float moveVertical = Direction.y;

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rigidBody.velocity = movement * Speed;
        RigidBodyTilt.transform.position = rigidBody.transform.position;

        RigidBodyTilt.rotation = Quaternion.Euler(rigidBody.velocity.z * Tilt, 0, rigidBody.velocity.x * -Tilt);

		float angle = Mathf.Atan2(Target.y * TargetRadius, Target.x * TargetRadius) * Mathf.Rad2Deg;
		Cursor.transform.RotateAround(transform.position, Vector3.up, previousAngle - angle);

		previousAngle = angle;
    }

    public void PlayImpactSFX()
    {
        var audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(ImpactSounds[soundIndex]);

        soundIndex++;
        if (soundIndex >= ImpactSounds.Count) soundIndex = 0;
    }

    public void SetWeapon(GameObject newWeapon)
    {
        WeaponPrefab = newWeapon;

        if (weaponGO)
            Destroy(weaponGO);

        weaponGO = Instantiate(WeaponPrefab, Cursor.transform);
        weapon = weaponGO.GetComponent<Weapon>();
        weapon.Spaceship = this;
    }
}
