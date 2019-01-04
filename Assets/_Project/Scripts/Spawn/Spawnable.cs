using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public GameObject Child;

    [HideInInspector]
    public int Speed = 0;

    bool hasStarted = false;
    Rigidbody rigidBody;
    GameObject spawnsFolder;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        spawnsFolder = GameObject.Find("Spawns");
    }

    private void Update()
    {
        if (Speed != 0 && !hasStarted)
        {
            Vector3 movement = new Vector3(0, 0, -1);
            rigidBody.velocity = movement * Speed;

            hasStarted = true;
        }
    }

    public void Spawn()
    {
        Child.transform.SetParent(spawnsFolder.transform);
        Child.SetActive(true);
        Destroy(gameObject);
    }
}
