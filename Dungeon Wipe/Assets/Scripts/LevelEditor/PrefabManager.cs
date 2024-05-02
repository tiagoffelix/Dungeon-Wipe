using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance { get; private set; } // Singleton pattern

    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private ObjectManager objectManager;

    [SerializeField] private LevelEditorSO levelEditor;

    private GameObject currentPrefab;
    public GameObject CurrentPrefab { get => currentPrefab; }

    public bool PlayerPlaced { get; set; }
    private List<GameObject> instantiatedPrefabs = new List<GameObject>();

    [SerializeField] private GameObject warningPlayer;

    [SerializeField] private GameObject savedButton;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        PlayerPlaced = false; // Initialize the player placed flag
        foreach (var prefab in prefabs)
        {
            GameObject btn = Instantiate(buttonPrefab, scrollViewContent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = prefab.name;
            btn.GetComponent<Button>().onClick.AddListener(() => SetCurrentPrefab(prefab));
            btn.GetComponent<Button>().onClick.AddListener(() => objectManager.SetDeletingFalse());
        }
        LoadPrefabsFromJson();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentPrefab != null)
        {
            currentPrefab.transform.Rotate(0, 90, 0); // Rotate 90 degrees around the Y axis
        }
    }
    void SetCurrentPrefab(GameObject prefab)
    {
        if (currentPrefab != null)
        {
            Destroy(currentPrefab);
        }

        currentPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        currentPrefab.AddComponent<PrefabFollower>();
    }

    public void DestroyCurrentPrefab()
    {
        if (currentPrefab != null)
        {
            Destroy(currentPrefab);
        }
    }

    public GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject newPrefab = Instantiate(prefab, position, rotation);
        if(prefab.GetComponent<PrefabFollower>() != null) { newPrefab.GetComponent<PrefabFollower>().enabled = false; }   
        instantiatedPrefabs.Add(newPrefab);
        return newPrefab;
    }

    public void SerializePrefabs()
    {
        PlayerPlaced = instantiatedPrefabs.Exists(prefab => prefab.name.Replace("(Clone)", "").Trim() == "Player");

        // If the Player prefab is not found, log a message and skip saving
        if (!PlayerPlaced)
        {
            warningPlayer.SetActive(true);
            return;
        }

        List<PrefabData> prefabDataList = new List<PrefabData>();
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            string prefabName = prefab.name.Replace("(Clone)", "").Trim();
            prefabDataList.Add(new PrefabData
            {
                Name = prefabName,
                Position = prefab.transform.position,
                Rotation = prefab.transform.rotation
            });
        }

        PrefabDataList wrapper = new PrefabDataList();
        wrapper.prefabData = prefabDataList;

        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(levelEditor.SelectedLevelPath, json);
        #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
        #endif
        savedButton.SetActive(true);
    }

    private void LoadPrefabsFromJson()
    {
        if (!string.IsNullOrEmpty(levelEditor.SelectedLevelPath) && File.Exists(levelEditor.SelectedLevelPath))
        {
            string jsonContent = File.ReadAllText(levelEditor.SelectedLevelPath);

            // Deserialize the JSON content into the wrapper class
            PrefabDataList prefabDataWrapper = JsonUtility.FromJson<PrefabDataList>(jsonContent);

            // Make sure the deserialized wrapper is not null before proceeding
            if (prefabDataWrapper != null && prefabDataWrapper.prefabData != null)
            {
                foreach (PrefabData prefabData in prefabDataWrapper.prefabData)
                {
                    GameObject prefab = prefabs.Find(p => p.name == prefabData.Name);
                    if (prefab != null)
                    {
                        InstantiatePrefab(prefab, prefabData.Position, prefabData.Rotation);
                        if (prefabData.Name == "Player")
                        {
                            PlayerPlaced = true;
                        }
                    }
                }
            }
        }
    }

    public void RemovePrefab(GameObject prefab)
    {
        if (instantiatedPrefabs.Contains(prefab))
        {
            instantiatedPrefabs.Remove(prefab);
            Destroy(prefab);
        }
    }

    public void DeleteAllPrefabs()
    {
        // Loop through the instantiated prefabs and destroy them
        foreach (var prefab in instantiatedPrefabs)
        {
            Destroy(prefab);
        }

        // Clear the instantiatedPrefabs list
        instantiatedPrefabs.Clear();

        // Also clear the currentPrefab reference if it exists
        if (currentPrefab != null)
        {
            Destroy(currentPrefab);
            currentPrefab = null;
        }

        // Reset the PlayerPlaced flag
        PlayerPlaced = false;
    }

    public void CloseWarningPlayer() 
    {
        warningPlayer.SetActive(false);
    }
    public void CloseSavedWarning()
    {
        savedButton.SetActive(false);
    }

    [System.Serializable]
    public class PrefabData
    {
        public string Name;
        public Vector3 Position;
        public Quaternion Rotation;
    }

    [System.Serializable]
    public class PrefabDataList
    {
        public List<PrefabData> prefabData;
    }
}
