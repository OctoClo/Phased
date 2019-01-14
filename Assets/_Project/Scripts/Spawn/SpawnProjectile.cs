using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();

    private GameObject effectToSpawn;

    void Start(){
        effectToSpawn = vfx[0];
    }

    void Update()
    {
        
            SpawnVFX();
        
    }
    void SpawnVFX()
    {
        GameObject vfx;

        if (firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);

        }
        else
        {
            Debug.Log("No Fire Point");
        }
    }
    
        
    }

