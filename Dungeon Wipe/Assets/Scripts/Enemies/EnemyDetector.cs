using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private bool stationaryEnemy;

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
            StationaryEnemy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            StationaryEnemy = false;
        }
    }
}
