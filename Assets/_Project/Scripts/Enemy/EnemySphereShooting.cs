using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EShootMode { NEAREST, SPACESHIP, RANDOM, BOTH };
public enum EShootTarget { SPACESHIP1, SPACESHIP2, RANDOM };

public class EnemySphereShooting : EnemySphere
{
    public EShootMode ShootMode = EShootMode.NEAREST;
    public EShootTarget ShootTarget = EShootTarget.SPACESHIP1;

    public GameObject Weapon;
    public GameObject Target;
    public GameObject SecondTarget;
    public GameObject Cursor;
    public GameObject SecondCursor;

    GameObject[] spaceships;
    GameObject weaponGO;
    WeaponEnemy weapon;

    float distance, minDistance;

    void Awake()
    {
        CheckIfWaitUntilDeath();
    }

    protected override void Start()
    {
        spaceships = GameObject.FindGameObjectsWithTag("Player");

        CreateWeapon();
        InitializeShoot();

        base.Start();
    }

    void CreateWeapon()
    {
        weaponGO = Instantiate(Weapon, gameObject.transform);
        weapon = weaponGO.GetComponent<WeaponEnemy>();
        weapon.Cursor = Cursor;
        if (ShootMode == EShootMode.BOTH)
        {
            weapon.SecondCursor = SecondCursor;
        }
    }

    void InitializeShoot()
    {
        if (ShootMode == EShootMode.RANDOM)
        {
            ShootMode = (EShootMode)Random.Range(0, 2);
            Debug.Log("Shoot mode: " + ShootMode);
        }
        else if (ShootMode == EShootMode.BOTH)
        {
            Target = GameObject.Find("Spaceship1");
            SecondTarget = GameObject.Find("Spaceship2");
        }

        if (ShootMode == EShootMode.SPACESHIP)
        {
            switch (ShootTarget)
            {
                case EShootTarget.SPACESHIP1:
                    Target = GameObject.Find("Spaceship1");
                    break;

                case EShootTarget.SPACESHIP2:
                    Target = GameObject.Find("Spaceship2");
                    break;

                case EShootTarget.RANDOM:
                    Target = GameObject.Find("Spaceship" + Random.Range(1, 3));
                    Debug.Log("Shoot target: " + Target.name);
                    break;
            }
        }
    }

    void CheckIfWaitUntilDeath()
    {
        if (ShootMode == EShootMode.BOTH || Pattern == eBehaviour.LINEAR_SWIPE)
        {
            WaitUntilDeath = true;
        }
    }

    void Update()
    {
        if (ShootMode == EShootMode.NEAREST)
        {
            minDistance = 9999f;

            foreach (GameObject spaceship in spaceships)
            {
                distance = Vector3.Distance(gameObject.transform.position, spaceship.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    Target = spaceship;
                }
            }
        }

        Cursor.transform.LookAt(Target.transform);

        if (ShootMode == EShootMode.BOTH)
        {
            SecondCursor.transform.LookAt(SecondTarget.transform);
        }
    }
}
