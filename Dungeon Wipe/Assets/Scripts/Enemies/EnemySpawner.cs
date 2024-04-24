using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;  // The spawn point where enemies will be instantiated.
    private GameObject[] enemyPrefabs;          // Array to hold the enemy prefabs.
    private float timeBetweenSpawns;      // Minimum time between spawns.
    private float spawnTimeVariation;     // Variation in time between spawns.

    [SerializeField] private EnemySpawnRate spawnRate;

    void Start()
    {
        timeBetweenSpawns = spawnRate.TimeBetweenSpawns;
        spawnTimeVariation = spawnRate.SpawnTimeVariation;
        enemyPrefabs = spawnRate.EnemyPrefabs;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Calculate next spawn time between 5 to 8 seconds.
            float spawnDelay = timeBetweenSpawns + Random.Range(0f, spawnTimeVariation);
            yield return new WaitForSeconds(spawnDelay);

            // Randomly pick an enemy to spawn.
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            // Instantiate the enemy at the spawn point's position and rotation.
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
