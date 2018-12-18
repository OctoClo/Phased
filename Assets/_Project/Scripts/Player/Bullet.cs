using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage = 100;
    public int Speed = 15;
    public float TimeBeforeDestroy = 1.5f;

    float currentTime;
    Rigidbody rigidBody;

    void Start()
    {
        currentTime = 0;
        rigidBody = GetComponent<Rigidbody>();
        
        rigidBody.velocity = transform.up * Speed;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= TimeBeforeDestroy)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy)
        {
            enemy.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
