using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikazeBehaviour : MonoBehaviour
{
    GameObject target = null;

    void Start()
    {
        // 1..2 range
        var targetIndex = Random.Range(0, 2);

        target = SpaceshipsManager.Instance.Spaceships[targetIndex];
    }

    void Update()
    {
        transform.Translate( ( target.transform.position - transform.position ) * Time.deltaTime );
    }
}
