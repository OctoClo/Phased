using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public float speed;
    public float tilt;
    public int playerNumber;

    public PlayerInputManager inputManager;

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var direction = inputManager.direction[playerNumber];

        float moveHorizontal = direction.x;
        float moveVertical = direction.y;

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rigidBody.velocity = movement * speed;

        rigidBody.rotation = Quaternion.Euler(rigidBody.velocity.z * tilt, 0, rigidBody.velocity.x * -tilt);
    }
}
