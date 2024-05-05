using System;
using UnityEngine;

/// <summary>
/// ScriptableObject representing the spawn rate of enemies.
/// </summary>
[CreateAssetMenu(fileName = "NewEnemySpawnRate", menuName = "EnemySpawnRate")]
public class EnemySpawnRate : ScriptableObject
{
    /// <summary>
    /// The time between each enemy spawn.
    /// </summary>
    [SerializeField] private int timeBetweenSpawns;

    /// <summary>
    /// The variation allowed in spawn time.
    /// </summary>
    [SerializeField] private int spawnTimeVariation;

    /// <summary>
    /// Array to hold the enemy prefabs.
    /// </summary>
    [SerializeField] private GameObject[] enemyPrefabs;

    /// <summary>
    /// Gets or sets the time between enemy spawns.
    /// </summary>
    public int TimeBetweenSpawns
    {
        get { return this.timeBetweenSpawns; }
        set { this.timeBetweenSpawns = value; }
    }

    /// <summary>
    /// Gets or sets the variation in spawn time.
    /// </summary>
    public int SpawnTimeVariation
    {
        get { return this.spawnTimeVariation; }
        set { this.spawnTimeVariation = value; }
    }

    /// <summary>
    /// Gets the array of enemy prefabs.
    /// </summary>
    public GameObject[] EnemyPrefabs
    {
        get { return this.enemyPrefabs; }
    }
}
