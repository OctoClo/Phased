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
    protected GameObject weaponGO;
    protected WeaponEnemy weapon;

    float distance, minDistance;
    bool shootingActive = true;

    void Awake()
    {
        CheckIfWaitUntilDeath();
    }

    protected override void Start()
    {
        base.Start();

        EventManager.Instance.AddListener<GameAlmostOverEvent>(OnGameAlmostOverEvent);
        spaceships = GameObject.FindGameObjectsWithTag("Player");

        CreateWeapon();
        InitializeShoot();
    }

    protected virtual void CreateWeapon()
    {
        weaponGO = Instantiate(Weapon, gameObject.transform);
        weapon = weaponGO.GetComponent<WeaponEnemy>();
        weapon.Cursor = Cursor;
        weaponGO.SetActive(false);
    }

    protected virtual void InitializeShoot()
    {
        if (ShootMode == EShootMode.RANDOM)
        {
            ShootMode = (EShootMode)Random.Range(0, 2);
            Debug.Log("Shoot mode: " + ShootMode);
        }

        if (ShootMode == EShootMode.SPACESHIP)
        {
            switch (ShootTarget)
            {
                case EShootTarget.SPACESHIP1:
                    Target = SpaceshipsManager.Instance.Spaceships[0];
                    break;

                case EShootTarget.SPACESHIP2:
                    Target = SpaceshipsManager.Instance.Spaceships[1];
                    break;

                case EShootTarget.RANDOM:
                    Target = SpaceshipsManager.Instance.Spaceships[Random.Range(0, 2)];
                    Debug.Log("Shoot target: " + Target.name);
                    break;
            }
        }
    }

    void CheckIfWaitUntilDeath()
    {
        if (Pattern == eBehaviour.LINEAR_SWIPE)
        {
            WaitUntilDeath = true;
        }
    }

    protected virtual void Update()
    {
        if (shootingActive)
        {
            if (ShootMode == EShootMode.NEAREST)
            {
                minDistance = float.MaxValue;

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
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("ShootLine"))
        {
            if (weaponGO)
            {
                weaponGO.SetActive(!reverseMove);
            }
        }

        if (other.CompareTag("StopShootLine"))
        {
            if (weaponGO)
            {
                weaponGO.SetActive(reverseMove);
            }
        }
    }

    void OnGameAlmostOverEvent(GameAlmostOverEvent e)
    {
        if (this != null)
        {
            Debug.Log("Le Gondor répondra présent !");
            Destroy(weaponGO);
            shootingActive = false;
        }
        else
        {
            Debug.Log("I'm already dead man");
        }
    }
}
