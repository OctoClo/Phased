using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
