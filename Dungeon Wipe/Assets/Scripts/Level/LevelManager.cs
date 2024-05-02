using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;

    [SerializeField] private Stats stats;
    [SerializeField] private LevelEditorSO levelEditor;

    [SerializeField] private GameObject[] potionPrefabs;
    [SerializeField] private SpawnCollectibles potionSpawnSettings;
    [SerializeField] private GameObject[] coinPrefabs;
    [SerializeField] private SpawnCollectibles coinSpawnSettings;

    private List<Vector3> spawnedGrounds = new List<Vector3>();

    void Start()
    {
        stats.Score = 0;
        stats.NumberOfSpawns = 0;
        LoadLevel(levelEditor.SelectedLevelPath);
        print(spawnedGrounds.Count);
        StartCoroutine(SpawnPotions());
        StartCoroutine(SpawnCoins());
        print(stats.NumberOfSpawns);
        print(spawnedGrounds.Count);
    }

    IEnumerator SpawnPotions()
    {
        while (true)
        {
            float timeDelay = potionSpawnSettings.SpawnInterval + Random.Range(0, potionSpawnSettings.SpawnTimeVariation);
            yield return new WaitForSeconds(timeDelay);

            if (spawnedGrounds.Count > 0)
            {
                int randomIndex = Random.Range(0, spawnedGrounds.Count);
                Vector3 spawnPosition = spawnedGrounds[randomIndex];
                spawnPosition.y += 0.4f;

                Instantiate(potionPrefabs[Random.Range(0, potionPrefabs.Length)], spawnPosition, Quaternion.identity);
            }
        }
    }

    IEnumerator SpawnCoins()
    {
        while (true)
        {
            float timeDelay = coinSpawnSettings.SpawnInterval + Random.Range(0, coinSpawnSettings.SpawnTimeVariation);
            yield return new WaitForSeconds(timeDelay);

            if (spawnedGrounds.Count > 0)
            {
                int randomIndex = Random.Range(0, spawnedGrounds.Count);
                Vector3 spawnPosition = spawnedGrounds[randomIndex];
                spawnPosition.y += 0.4f; 

                Instantiate(coinPrefabs[Random.Range(0, coinPrefabs.Length)], spawnPosition, Quaternion.identity);
            }
        }
    }

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
                Instantiate(toInstantiate, prefabData.Position, prefabData.Rotation);

                // If it's a ground tile, add its position to spawnedGrounds
                if (prefabData.Name == "Floor")
                {
                    spawnedGrounds.Add(prefabData.Position);
                }
                if (prefabData.Name == "Spawn")
                {
                    stats.NumberOfSpawns ++;
                }
            }
            else
            {
                Debug.LogWarning($"Prefab '{prefabData.Name}' not found.");
            }
        }
    }

    GameObject FindPrefabByName(string prefabName)
    {
        if (prefabName == "Player")
        {
            prefabName = "Player 1";
        }
        else if (prefabName == "Spawn")
        {
            prefabName = "Spawn 1";
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
