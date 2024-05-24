using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnCollectibles", menuName = "SpawnCollectibles")]
/// <summary>
/// Represents the properties for spawning collectibles in the game.
/// </summary>
public class SpawnCollectibles : ScriptableObject
{
    [SerializeField] private float spawnInterval; // Minimum time between spawns
    [SerializeField] private float spawnTimeVariation; // Variation in time between spawns
    [SerializeField] private List<GameObject> prefabs; // List of collectible prefabs
    [SerializeField] private List<GameObject> healthPrefabs; // List of collectible prefabs
    [SerializeField] private bool onlyHealth;

    /// <summary>
    /// Gets or sets the minimum time between spawns.
    /// </summary>
    public float SpawnInterval
    {
        get { return this.spawnInterval; }
        set { this.spawnInterval = value; }
    }

    /// <summary>
    /// Gets or sets the variation in time between spawns.
    /// </summary>
    public float SpawnTimeVariation
    {
        get { return this.spawnTimeVariation; }
        set { this.spawnTimeVariation = value; }
    }

    /// <summary>
    /// Gets or sets the variable of onlyHealth.
    /// </summary>
    public bool OnlyHealth
    {
        get { return this.onlyHealth; }
        set { this.onlyHealth = value; }
    }

    /// <summary>
    /// Gets or sets the list of collectible prefabs.
    /// </summary>
    public List<GameObject> Prefabs
    {
        get { return this.prefabs; }
        set { this.prefabs = value; }
    }

    /// <summary>
    /// Gets or sets the list of health collectible prefabs.
    /// </summary>
    public List<GameObject> HealthPrefabs
    {
        get { return this.healthPrefabs; }
        set { this.healthPrefabs = value; }
    }
}
