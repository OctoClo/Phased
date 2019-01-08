using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public GameObject Child;
    
    Rigidbody rigidBody;
    GameObject spawnsFolder;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Vector3 movement = new Vector3(0, 0, -1);
        rigidBody.velocity = movement * (WorldConstants.Instance.WorldScrollSpeed * WorldConstants.Instance.SpawnableSpeedMultiplier);

        spawnsFolder = GameObject.Find("Spawns");
    }

    public void Spawn()
    {
        Child.transform.SetParent(spawnsFolder.transform);
        Child.SetActive(true);
        Destroy(gameObject);
    }
}
