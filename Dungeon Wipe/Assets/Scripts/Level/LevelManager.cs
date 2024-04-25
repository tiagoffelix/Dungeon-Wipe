using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;

    [SerializeField] private Stats stats;

    [SerializeField] private GameObject[] potionPrefabs;
    [SerializeField] private SpawnCollectibles potionSpawnSettings;
    [SerializeField] private GameObject[] coinPrefabs;
    [SerializeField] private SpawnCollectibles coinSpawnSettings;

    private List<Vector3> spawnedGrounds;

    // Start is called before the first frame update
    void Start()
    {
        spawnedGrounds = new List<Vector3>();
        stats.Score = 0;
        stats.NumberOfSpawns = 0;
        LoadLevel(stats.SelectedLevelPath);
        StartCoroutine(SpawnPotions());
        StartCoroutine(SpawnCoins());
    }


    IEnumerator SpawnPotions()
    {
        while (true)
        {
            // Calculate random time variation
            float timeDelay = potionSpawnSettings.SpawnInterval + Random.Range(0, potionSpawnSettings.SpawnTimeVariation);
            yield return new WaitForSeconds(timeDelay);

            if (spawnedGrounds.Count > 0)
            {
                int randomIndex = Random.Range(0, spawnedGrounds.Count);
                Instantiate(potionPrefabs[Random.Range(0, potionPrefabs.Length)], spawnedGrounds[randomIndex], Quaternion.identity);
            }
        }
    }

    IEnumerator SpawnCoins()
    {
        while (true)
        {
            // Calculate random time variation
            float timeDelay = coinSpawnSettings.SpawnInterval + Random.Range(0, coinSpawnSettings.SpawnTimeVariation);
            yield return new WaitForSeconds(timeDelay);

            if (spawnedGrounds.Count > 0)
            {
                int randomIndex = Random.Range(0, spawnedGrounds.Count);
                Instantiate(coinPrefabs[Random.Range(0, coinPrefabs.Length)], spawnedGrounds[randomIndex], Quaternion.identity);
            }
        }
    }

    void LoadLevel(string path)
    {
        string[] lines = File.ReadAllLines(path);
        Vector3[] prefabSizes = new Vector3[prefabs.Length];

        // Calculate sizes for each type of prefab
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].GetComponent<Renderer>())
            {
                prefabSizes[i] = prefabs[i].GetComponent<Renderer>().bounds.size;
            }
        }

        // Load the level
        for (int z = 0; z < lines.Length; z++)
        {
            for (int x = 0; x < lines[z].Length; x++)
            {
                char tileType = lines[z][x];
                Vector3 position = new Vector3(x * prefabSizes[0].x, 0, z * prefabSizes[0].z);
                GameObject toInstantiate = null;
                Quaternion rotation = Quaternion.identity;

                switch (tileType)
                {
                    case '#':
                        toInstantiate = prefabs[0]; // Wall Prefab
                        position = new Vector3(x * prefabSizes[0].x, 0, z * prefabSizes[0].x);
                        break;
                    case '.':
                        toInstantiate = prefabs[1]; // Ground Prefab
                        position = new Vector3(x * prefabSizes[1].x, 0, z * prefabSizes[1].z);
                        Vector3 changedPosition = position;
                        changedPosition.y = 1;
                        spawnedGrounds.Add(changedPosition);
                        break;
                    case 'D':
                        toInstantiate = prefabs[2]; // Door Prefab
                        position = new Vector3(x * prefabSizes[0].x, 0, z * prefabSizes[0].x);
                        stats.NumberOfSpawns++;
                        break;
                    case 'S':
                        toInstantiate = prefabs[3]; // Spikes Prefab
                        position = new Vector3(x * prefabSizes[3].x, 0, z * prefabSizes[3].z);
                        break;
                    case 'B':
                        toInstantiate = prefabs[4]; // Barrel Prefab
                        position = new Vector3(x * prefabSizes[4].x, 0, z * prefabSizes[4].x);
                        break;
                    case 'E':
                        toInstantiate = prefabs[5]; // Estante Prefab
                        position = new Vector3(x * prefabSizes[5].x, 0, z * prefabSizes[5].x);
                        break;
                    case 'P':
                        toInstantiate = prefabs[6]; // Parede com estante Prefab
                        position = new Vector3(x * prefabSizes[6].x, 0, z * prefabSizes[6].x);
                        break;
                    case 'R':
                        toInstantiate = prefabs[7]; // Rumble
                        position = new Vector3(x * prefabSizes[7].x, 0, z * prefabSizes[7].x);
                        break;
                    case 'K':
                        toInstantiate = prefabs[8]; // Character
                        position = new Vector3(x * prefabSizes[8].x, 0, z * prefabSizes[8].x);
                        break;
                }

                // Determine rotation based on specified rules
                if (toInstantiate) // Adjust rotation for walls
                {
                    if (z == 0) // First line
                    {
                        rotation = Quaternion.Euler(0, 0, 0);
                        position.z += 2f;
                        position.y = 1f;
                    }
                    else if (z == lines.Length - 1) // Last line
                    {
                        rotation = Quaternion.Euler(0, 180, 0);
                        position.z -= 2f;
                        position.y = 1f;
                    }
                    else if (x == 0) // First element of each line (after the first and before the last)
                    {
                        rotation = Quaternion.Euler(0, 90, 0);
                        position.x += 2f;
                        position.y = 1f;
                    }
                    else if (x == lines[z].Length - 1) // Last element of each line (after the first and before the last)
                    {
                        rotation = Quaternion.Euler(0, -90, 0); // Default rotation or any specific adjustment
                        position.x -= 2f;
                        position.y = 1f;
                    }
                }

                if (toInstantiate != null)
                {
                    Instantiate(toInstantiate, position, rotation);
                }
            }
        }
    }
}
