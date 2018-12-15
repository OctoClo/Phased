using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed;

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (speed == 0)
        {
            speed = Random.Range(5, 13);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, -1);
        rigidBody.velocity = movement * speed;
    }
}
