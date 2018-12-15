using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public float speed;
    public float tilt;

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("L_XAxis_1");
        float moveVertical = -Input.GetAxis("L_YAxis_1");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rigidBody.velocity = movement * speed;

        rigidBody.rotation = Quaternion.Euler(rigidBody.velocity.z * tilt, 0, rigidBody.velocity.x * -tilt);
    }
}
