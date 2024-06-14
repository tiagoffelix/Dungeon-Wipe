using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public bool stationaryEnemy;

    /// <summary>
    /// Gets or sets a value indicating whether there is an enemy in the trigger area.
    /// </summary>
    public bool StationaryEnemy
    {
        get { return stationaryEnemy; }
        set { stationaryEnemy = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.Health > 0)
                {
                    stationaryEnemy = true;
                    enemy.OnDeath += HandleEnemyDeath;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && enemy.Health > 0)
            {
                stationaryEnemy = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnDeath -= HandleEnemyDeath;
                stationaryEnemy = false;
            }
        }
    }

    private void HandleEnemyDeath()
    {
        stationaryEnemy = false;
    }
}
