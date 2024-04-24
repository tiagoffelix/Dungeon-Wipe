using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemySpawnRate", menuName = "EnemySpawnRate")]
public class EnemySpawnRate : ScriptableObject
{
    [SerializeField] private int timeBetweenSpawns;
    [SerializeField] private int spawnTimeVariation;
    [SerializeField] private GameObject[] enemyPrefabs; // Array to hold the enemy prefabs.


    public int TimeBetweenSpawns
    {
        get { return this.timeBetweenSpawns; }
        set {  this.timeBetweenSpawns = value; }
    }

    public int SpawnTimeVariation
    {
        get { return this.spawnTimeVariation; }
        set { this.spawnTimeVariation = value; }
    }

    public GameObject[] EnemyPrefabs
    {
        get { return this.enemyPrefabs; }
    }
}
