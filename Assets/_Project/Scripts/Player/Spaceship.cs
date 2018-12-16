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

    public LifeCounter lifeCounter;

    public PlayerInputManager InputManager;

    [HideInInspector]
    public Vector2 Direction;
    [HideInInspector]
    public Vector2 Target;
    [HideInInspector]
    public bool IsFiring;

    Rigidbody rigidBody;
    Renderer spaceshipRenderer;
    Weapon weapon;

    Vector3 cursorScale;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        spaceshipRenderer = GetComponent<Renderer>();

        GameObject weaponGO = Instantiate(WeaponPrefab, transform);
        weapon = weaponGO.GetComponent<Weapon>();
        weapon.Spaceship = this;

        cursorScale = Cursor.transform.localScale;
    }

    void Update()
    {
        if (lifeCounter.DamageSource == this && lifeCounter.IsInvulnerable && (Time.frameCount % WorldConstants.Instance.PlayerFlickerFrequency) == 0)
        {
            spaceshipRenderer.enabled = !spaceshipRenderer.enabled;
        }
        else
        {
            spaceshipRenderer.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (lifeCounter.HasNoLifeLeft)
        {
            // TODO How should the player be removed?
            Vector3 movementTest = new Vector3(1, 0, 0);
            rigidBody.velocity = movementTest * Speed * 32.0f;
            return;
        }

        weapon.IsFiring = IsFiring;

        float moveHorizontal = Direction.x;
        float moveVertical = Direction.y;

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rigidBody.velocity = movement * Speed;

        rigidBody.rotation = Quaternion.Euler(rigidBody.velocity.z * Tilt, 0, rigidBody.velocity.x * -Tilt);

        Vector3 targetPosition = transform.position;
        targetPosition.x += Target.x * TargetRadius;
        targetPosition.z += Target.y * TargetRadius;

        if ( Target.magnitude != 0.0f )
            Cursor.transform.position = targetPosition;

        float angle = Mathf.Atan2(targetPosition.z - transform.position.z, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;

        Cursor.transform.rotation = Quaternion.Euler(-90, 0.0f, angle * -1);
        Cursor.transform.localScale = cursorScale;
    }
}
