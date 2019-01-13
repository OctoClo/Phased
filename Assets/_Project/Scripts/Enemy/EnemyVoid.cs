using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVoid : EnemySphere
{
    protected override void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Vector3 updatedVelocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.EnemyVoidSpeedMultiplier);
        rigidBody.velocity = updatedVelocity;

        Renderer renderer = GetComponent<Renderer>();
        renderer.enabled = false;
    }

    protected override void HandleMovement()
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
