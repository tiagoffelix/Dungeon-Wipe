using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnCollectibles", menuName = "SpawnCollectibles")]
public class SpawnCollectibles : ScriptableObject
{
    [SerializeField] private float spawnInterval;      // Minimum time between spawns.
    [SerializeField] private float spawnTimeVariation;     // Variation in time between spawns.

    public float SpawnInterval
    {
        get { return this.spawnInterval; }
        set { this.spawnInterval = value; }
    }

    public float SpawnTimeVariation
    {
        get { return this.spawnTimeVariation; }
        set { this.spawnTimeVariation = value; }
    }

}
