using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawns enemies at a specified rate when the player enters a trigger area.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;  // The spawn point where enemies will be instantiated.
    private GameObject[] enemyPrefabs; // Array to hold the enemy prefabs.
    private float timeBetweenSpawns; // Minimum time between spawns.
    private float spawnTimeVariation; // Variation in time between spawns.
    [SerializeField] private EnemySpawnRate spawnRate; // ScriptableObject defining the spawn rate.

    private bool playerInTrigger = false; // To check if the player is inside the trigger area.
    private Collider[] colliders; // Cache of colliders
    private float nextSpawnTime = 0f; // Time until next spawn

    private EnemyDetector enemyDetector; // Reference to the EnemyDetector component

    /// <summary>
    /// Initializes the spawner with spawn rate data.
    /// </summary>
    void Start()
    {
        timeBetweenSpawns = spawnRate.TimeBetweenSpawns;
        spawnTimeVariation = spawnRate.SpawnTimeVariation;
        enemyPrefabs = spawnRate.EnemyPrefabs;
        colliders = GetComponents<Collider>();

        enemyDetector = spawnPoint.GetComponent<EnemyDetector>();

        CheckAndDeactivateCollider();
    }

    /// <summary>
    /// Checks the collider and deactivates it if the scene is not "Game".
    /// </summary>
    private void CheckAndDeactivateCollider()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            foreach (var collider in colliders)
            {
                if (collider.isTrigger)
                {
                    collider.enabled = false;
                }
            }
            this.enabled = false;
        }
    }

    /// <summary>
    /// Activates enemy spawning when the player enters the trigger area.
    /// </summary>
    /// <param name="other">The collider entering the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            nextSpawnTime = Time.time + GetRandomSpawnDelay();
        }
    }

    /// <summary>
    /// Deactivates enemy spawning when the player exits the trigger area.
    /// </summary>
    /// <param name="other">The collider exiting the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    /// <summary>
    /// Update method to handle enemy spawning.
    /// </summary>
    void Update()
    {
        if (playerInTrigger && Time.time >= nextSpawnTime)
        {
            if (enemyDetector != null && !enemyDetector.StationaryEnemy)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + GetRandomSpawnDelay();
            }
        }
    }

    /// <summary>
    /// Spawns a random enemy at the spawn point.
    /// </summary>
    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];
        Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    /// <summary>
    /// Calculates the next spawn delay time.
    /// </summary>
    /// <returns>A float representing the delay time.</returns>
    private float GetRandomSpawnDelay()
    {
        return timeBetweenSpawns + Random.Range(0f, spawnTimeVariation);
    }
}
