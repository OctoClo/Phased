using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int speed;
    public float timeBeforeDestroy;

    float currentTime;
    Rigidbody rigidBody;

    void Start()
    {
        currentTime = 0;
        rigidBody = GetComponent<Rigidbody>();

        Vector3 movement = new Vector3(0, 0, 1);
        rigidBody.velocity = movement * speed;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeBeforeDestroy)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
