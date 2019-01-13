using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVoid : EnemySphere
{
    void Awake()
    {
        WaitUntilDeath = true;
    }

    protected override void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Vector3 updatedVelocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.EnemyVoidSpeedMultiplier);
        rigidBody.velocity = updatedVelocity;
    }

    protected override void FixedUpdate()
    {
        // Do nothing
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // Do nothing
    }

    public override void TakeDamage(int damage, GameObject weaponFrom, bool phased)
    {
        // Do nothing
    }
}
