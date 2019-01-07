using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikazeBehaviour : MonoBehaviour
{
    GameObject target = null;

    // Start is called before the first frame update
    void Start()
    {
        // 1..2 range
        var targetIndex = Random.Range(1, 3);

        target = GameObject.Find("Spaceship" + targetIndex);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate( ( target.transform.position - transform.position ) * Time.deltaTime );
    }
}
