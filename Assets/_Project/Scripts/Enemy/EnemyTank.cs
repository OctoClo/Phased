using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ETankPatternStep { MOVING, RESTING, SHOOTING }

public class EnemyTank : EnemySphereShooting
{
    public int RestingTime = 10;
    public int ShootingTime = 5;
    public bool GoingRightFirst = true;

    ETankPatternStep patternStep = ETankPatternStep.MOVING;
    float timeSpentInStep = 0f;
    Vector3 lateralMovement;

    protected override void Start()
    {
        base.Start();

        if (GoingRightFirst)
        {
            lateralMovement = new Vector3(1, 0, 0);
        }
        else
        {
            lateralMovement = new Vector3(-1, 0, 0);
        }

        HandleNewPatternStep();
    }

    protected override void CreateWeapon()
    {
        base.CreateWeapon();

        if (ShootMode == EShootMode.BOTH)
        {
            weapon.SecondCursor = SecondCursor;
        }
    }

    protected override void InitializeShoot()
    {
        Target = SpaceshipsManager.Instance.Spaceships[0];
        SecondTarget = SpaceshipsManager.Instance.Spaceships[1];
    }

    protected override void Update()
    {
        Cursor.transform.LookAt(Target.transform);
        SecondCursor.transform.LookAt(SecondTarget.transform);
    }

    protected override void FixedUpdate()
    {
        CheckIfMarked();

        if (patternStep != ETankPatternStep.MOVING)
        {
            timeSpentInStep += Time.fixedDeltaTime;

            if (patternStep == ETankPatternStep.RESTING && timeSpentInStep >= RestingTime)
            {
                timeSpentInStep = 0;
                patternStep = ETankPatternStep.SHOOTING;
                HandleNewPatternStep();
            }
            else if (patternStep == ETankPatternStep.SHOOTING && timeSpentInStep >= ShootingTime)
            {
                timeSpentInStep = 0;
                patternStep = ETankPatternStep.RESTING;
                HandleNewPatternStep();
            }
        }
    }

    void HandleNewPatternStep()
    {
        switch (patternStep)
        {
            case ETankPatternStep.MOVING:
                rigidBody.velocity = movement * WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.TankMultiplier;
                weaponGO.SetActive(false);
                break;

            case ETankPatternStep.RESTING:
                rigidBody.velocity = lateralMovement * WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.TankLateralMultiplier;
                weaponGO.SetActive(false);
                break;

            case ETankPatternStep.SHOOTING:
                rigidBody.velocity = Vector3.zero;
                weaponGO.SetActive(true);
                break;
        }
    }

    public void BeginAttack()
    {
        patternStep = ETankPatternStep.RESTING;
        HandleNewPatternStep();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("TankBounds"))
        {
            lateralMovement.x = -lateralMovement.x;
            rigidBody.velocity = lateralMovement * WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.TankLateralMultiplier; ;
        }
    }
}
