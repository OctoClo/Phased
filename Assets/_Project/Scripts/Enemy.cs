using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed = 0;

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (speed == 0)
        {
            speed = Random.Range(6, 13);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, -1);
        rigidBody.velocity = movement * speed;
    }
}
