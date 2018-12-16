using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public float speed;
    public float tilt;

    // DEBUG TEST
    public float Life = 100.0f;

    public float targetRadius = 2.0f;

    public int playerNumber;

    public PlayerInputManager inputManager;
    public GameObject cursor;

    Rigidbody rigidBody;

    public Vector2 direction;
    public Vector2 target;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = direction.x;
        float moveVertical = direction.y;

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rigidBody.velocity = movement * speed;

        rigidBody.rotation = Quaternion.Euler(rigidBody.velocity.z * tilt, 0, rigidBody.velocity.x * -tilt);
        
        var targetPosition = transform.position;
        targetPosition.x += target.x * targetRadius;
        targetPosition.z += target.y * targetRadius;

        if ( target.magnitude != 0.0f )
            cursor.transform.position = targetPosition;
        
        float angle = Mathf.Atan2(targetPosition.z - transform.position.z, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;

        cursor.transform.rotation = Quaternion.Euler(-90, 0.0f, angle * -1);
    }
}
