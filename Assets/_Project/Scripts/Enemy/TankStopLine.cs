using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStopLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EnemyTank enemyTank = other.GetComponent<EnemyTank>();

        if (enemyTank)
        {
            enemyTank.BeginAttack();
        }
    }
}
