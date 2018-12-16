using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public float speed;
    public float tilt;
    
    public float    targetRadius = 2.0f;
    
    public GameObject cursor;

    public Vector2 direction;
    public Vector2 target;

    public LifeCounter lifeCounter;

    private Rigidbody rigidBody;
    private Renderer renderer;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (lifeCounter.IsInvulnerable && ( Time.frameCount % WorldConstants.Instance.PlayerFlickerFrequency ) == 0)
        {
            renderer.enabled = !renderer.enabled;
        } else
        {
            renderer.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (lifeCounter.HasNoLifeLeft)
        {
            // TODO How should the player be removed?
            Vector3 movementTest = new Vector3(1, 0, 0);
            rigidBody.velocity = movementTest * speed * 32.0f;
            return;
        }

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
