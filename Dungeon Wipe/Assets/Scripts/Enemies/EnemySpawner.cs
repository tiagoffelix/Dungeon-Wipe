using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawns enemies at a specified rate when the player enters a trigger area.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;  // The spawn point where enemies will be instantiated.
    private GameObject[] enemyPrefabs;          // Array to hold the enemy prefabs.
    private float timeBetweenSpawns;      // Minimum time between spawns.
    private float spawnTimeVariation;     // Variation in time between spawns.

    [SerializeField] private EnemySpawnRate spawnRate; // ScriptableObject defining the spawn rate.

    private bool playerInTrigger = false; // To check if the player is inside the trigger area.
    private bool isSpawning = false; // To check if enemies are being spawned

    /// <summary>
    /// Initializes the spawner with spawn rate data.
    /// </summary>
    void Start()
    {
        timeBetweenSpawns = spawnRate.TimeBetweenSpawns;
        spawnTimeVariation = spawnRate.SpawnTimeVariation;
        enemyPrefabs = spawnRate.EnemyPrefabs;
        CheckAndDeactivateCollider();
    }

    /// <summary>
    /// Checks the collider and deactivates it if the scene is not "Game".
    /// </summary>
    private void CheckAndDeactivateCollider()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            Collider[] colliders = GetComponents<Collider>();
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

            // If spawning is not currently happening, start the coroutine
            if (!isSpawning && SceneManager.GetActiveScene().name == "Game")
            {
                StartCoroutine(SpawnEnemies());
            }
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
    /// Coroutine to continuously spawn enemies with random delays.
    /// </summary>
    private IEnumerator SpawnEnemies()
    {
        isSpawning = true; // Set flag to indicate spawning has started

        while (true)
        {
            if (playerInTrigger)
            {
                // Calculate next spawn time between base time and variation.
                float spawnDelay = timeBetweenSpawns + Random.Range(0f, spawnTimeVariation);
                yield return new WaitForSeconds(spawnDelay);

                // Randomly pick an enemy to spawn.
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyPrefab = enemyPrefabs[randomIndex];

                // Instantiate the enemy at the spawn point's position and rotation.
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                // Pause until the player re-enters the trigger
                yield return null;
            }
        }
    }
}
