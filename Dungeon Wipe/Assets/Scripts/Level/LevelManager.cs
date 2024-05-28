using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Manages the spawning of game objects in a level.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs; // Array of prefabs used in the level.

    [SerializeField] private Stats stats; // Reference to the Stats scriptable object.
    [SerializeField] private LevelEditorSO levelEditor; // Reference to the LevelEditorSO scriptable object.

    [SerializeField] private SpawnCollectibles potionSpawnSettings; // Settings for spawning potions.
    [SerializeField] private SpawnCollectibles coinSpawnSettings; // Settings for spawning coins.

    // Store ground objects instead of positions
    private List<GameObject> spawnedGrounds = new List<GameObject>(); // List of spawned ground objects.

    [SerializeField] private GameObject parentGameObject; // Array of coin prefabs.

    /// <summary>
    /// Initializes the level with default values and spawns collectibles.
    /// </summary>
    void Start()
    {
        stats.Score = 0;
        stats.NumberOfSpawns = 0;
        LoadLevel(levelEditor.SelectedLevelPath);
        parentGameObject.GetComponent<PrefabScript>().BuildMesh();
        StartCoroutine(SpawnPotions());
        StartCoroutine(SpawnCoins());
    }

    /// <summary>
    /// Coroutine to spawn potions at regular intervals.
    /// </summary>
    IEnumerator SpawnPotions()
    {
        while (true)
        {
            bool potionSpawned = false;
            while (!potionSpawned)
            {
                if (spawnedGrounds.Count > 0)
                {
                    int randomIndex = Random.Range(0, spawnedGrounds.Count);
                    GameObject groundObject = spawnedGrounds[randomIndex];
                    FloorScript floorScript = groundObject.GetComponent<FloorScript>() ?? groundObject.GetComponentInChildren<FloorScript>();
                    if (!floorScript.HasCollectible && floorScript.PlayerInRange)
                    {
                        Vector3 spawnPosition = groundObject.transform.position + Vector3.up * 0.4f;

                        if (potionSpawnSettings.OnlyHealth)
                        {
                            GameObject potion = Instantiate(potionSpawnSettings.HealthPrefabs[Random.Range(0, potionSpawnSettings.HealthPrefabs.Count)],
                                spawnPosition, Quaternion.identity);
                            potion.transform.SetParent(groundObject.transform); // Set as child of the ground
                        }
                        else
                        {
                            GameObject potion = Instantiate(potionSpawnSettings.Prefabs[Random.Range(0, potionSpawnSettings.Prefabs.Count)],
                                spawnPosition, Quaternion.identity);
                            potion.transform.SetParent(groundObject.transform); // Set as child of the ground
                        }

                        floorScript.HasCollectible = true;
                        potionSpawned = true;
                    }
                }

                if (!potionSpawned)
                {
                    yield return null; // Wait until next frame to recheck conditions
                }
            }

            float timeDelay = potionSpawnSettings.SpawnInterval + Random.Range(0, potionSpawnSettings.SpawnTimeVariation);
            yield return new WaitForSeconds(timeDelay);
        }
    }

    /// <summary>
    /// Coroutine to spawn coins at regular intervals.
    /// </summary>
    IEnumerator SpawnCoins()
    {
        while (true)
        {
            bool coinSpawned = false;
            while (!coinSpawned)
            {
                if (spawnedGrounds.Count > 0)
                {
                    int randomIndex = Random.Range(0, spawnedGrounds.Count);
                    GameObject groundObject = spawnedGrounds[randomIndex];

                    FloorScript floorScript = groundObject.GetComponent<FloorScript>() ?? groundObject.GetComponentInChildren<FloorScript>();

                    if (!floorScript.HasCollectible && floorScript.PlayerInRange)
                    {
                        Vector3 spawnPosition = groundObject.transform.position + Vector3.up * 0.4f;

                        GameObject coin = Instantiate(coinSpawnSettings.Prefabs[Random.Range(0, coinSpawnSettings.Prefabs.Count)], spawnPosition, Quaternion.identity);
                        coin.transform.SetParent(groundObject.transform); // Set as child of the ground

                        floorScript.HasCollectible = true;
                        coinSpawned = true;
                    }
                }

                if (!coinSpawned)
                {
                    yield return null; // Wait until next frame to recheck conditions
                }
            }

            float timeDelay = coinSpawnSettings.SpawnInterval + Random.Range(0, coinSpawnSettings.SpawnTimeVariation);
            yield return new WaitForSeconds(timeDelay);
        }
    }

    /// <summary>
    /// Loads a level from a JSON file.
    /// </summary>
    /// <param name="path">The path to the JSON file.</param>
    void LoadLevel(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            Debug.LogError("Invalid or missing JSON file path.");
            return;
        }

        string jsonContent = File.ReadAllText(path);

        PrefabManager.PrefabDataList prefabDataWrapper = JsonUtility.FromJson<PrefabManager.PrefabDataList>(jsonContent);
        if (prefabDataWrapper == null || prefabDataWrapper.prefabData == null)
        {
            Debug.LogError("Failed to parse the JSON file.");
            return;
        }

        foreach (PrefabManager.PrefabData prefabData in prefabDataWrapper.prefabData)
        {
            GameObject toInstantiate = FindPrefabByName(prefabData.Name);

            if (toInstantiate != null)
            {
                GameObject instance = Instantiate(toInstantiate, prefabData.Position, prefabData.Rotation);

                instance.transform.SetParent(parentGameObject.transform);

                // If it's a ground tile, add the GameObject to spawnedGrounds
                if (prefabData.Name == "Floor" || prefabData.Name == "Wood Corner Left"
                    || prefabData.Name == "Wood Corner Right" || prefabData.Name == "Wood Floor" 
                    || prefabData.Name == "Wood Rails")
                {
                    spawnedGrounds.Add(instance);
                }
                if (prefabData.Name == "Spawn")
                {
                    stats.NumberOfSpawns++;
                }
            }
            else
            {
                Debug.LogWarning($"Prefab '{prefabData.Name}' not found.");
            }
        }
    }

    /// <summary>
    /// Finds a prefab by its name.
    /// </summary>
    /// <param name="prefabName">The name of the prefab to find.</param>
    /// <returns>The prefab GameObject if found, otherwise null.</returns>
    GameObject FindPrefabByName(string prefabName)
    {
        if (prefabName == "Player")
        {
            prefabName = "Player 1";
        }
        else if (prefabName == "Spikes")
        {
            prefabName = "Spikes 1";
        }

        foreach (GameObject prefab in prefabs)
        {
            if (prefab.name == prefabName)
            {
                return prefab;
            }
        }

        return null;
    }
}
