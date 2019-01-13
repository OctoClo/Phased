using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Speed = 15;
    
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();   
        rigidBody.velocity = transform.up * WorldConstants.Instance.WorldScrollSpeed * Speed;
    }    
}
