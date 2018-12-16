using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Speed = 0;

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (Speed == 0)
        {
            Speed = Random.Range(6, 13);
        }

        Vector3 movement = new Vector3(0, 0, -1);
        rigidBody.velocity = movement * Speed;
    }
}
