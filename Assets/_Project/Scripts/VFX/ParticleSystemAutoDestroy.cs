using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem[] ps;

    public void Start()
    {
        ps = transform.GetComponentsInChildren<ParticleSystem>();
    }

    public void Update()
    {
        if (!ps.Any(x => x.IsAlive()))
        {
            Destroy(gameObject);
        }
    }
}