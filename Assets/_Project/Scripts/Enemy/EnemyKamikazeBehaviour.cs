using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikazeBehaviour : MonoBehaviour
{
    GameObject target = null;

    void Start()
    {
        // 1..2 range
        var targetIndex = Random.Range(1, 3);

        target = GameObject.Find("Spaceship" + targetIndex);
    }

    void Update()
    {
        transform.Translate( ( target.transform.position - transform.position ) * Time.deltaTime );
    }
}
