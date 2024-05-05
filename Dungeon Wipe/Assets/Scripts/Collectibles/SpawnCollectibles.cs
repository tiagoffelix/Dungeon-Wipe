using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnCollectibles", menuName = "SpawnCollectibles")]
/// <summary>
/// Represents the properties for spawning collectibles in the game.
/// </summary>
public class SpawnCollectibles : ScriptableObject
{
    [SerializeField] private float spawnInterval; // Minimum time between spawns
    [SerializeField] private float spawnTimeVariation; // Variation in time between spawns

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
}
