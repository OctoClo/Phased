using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawnable"))
        {
            Spawnable spawnable = other.GetComponent<Spawnable>();
            spawnable.Spawn();
        }
    }
}
