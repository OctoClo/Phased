using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Speed = 15;

    public float maxLifeTime = 1.6f;
    float currentLifeTime = 0f;
    
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();   
        rigidBody.velocity = transform.up * WorldConstants.Instance.WorldScrollSpeed * Speed;
    }

    private void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
