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

    [SerializeField] private Transform ScrollViewGridsContent;
    [SerializeField] private GameObject buttonGridPrefab;

    private Dictionary<GameObject, string> originalTags = new Dictionary<GameObject, string>();

    [SerializeField] private GridGenerator gridGenerator;

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
        levelEditor.GridsDeactivated.Clear();
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

    public void GenerateGridButtons()
    {
        foreach (Transform child in ScrollViewGridsContent)
        {
            Destroy(child.gameObject); // Clear existing buttons
        }

        for (int i = 0; i < levelEditor.Grids; i++)
        {
            GameObject btn = Instantiate(buttonGridPrefab, ScrollViewGridsContent);
            string gridName = $"Grid {i}";
            btn.GetComponentInChildren<TextMeshProUGUI>().text = gridName;

            // Check if this grid is active or inactive
            bool isActive = !levelEditor.GridsDeactivated.Contains(i);

            // Assuming an Image is used as an indicator on the button
            Image indicatorImage = btn.transform.Find("Indicator").GetComponent<Image>();
            indicatorImage.gameObject.SetActive(isActive); // Show indicator if inactive

            btn.GetComponent<Button>().onClick.AddListener(() => ToggleGridActivation(gridName, indicatorImage));
        }
    }

    private void ToggleGridActivation(string gridName, Image indicatorImage)
    {
        if (int.TryParse(gridName.Replace("Grid ", ""), out int gridNumber))
        {
            // Check if grid is currently deactivated
            bool isDeactivated = levelEditor.GridsDeactivated.Contains(gridNumber);

            if (isDeactivated)
            {
                // Activate the grid
                levelEditor.GridsDeactivated.Remove(gridNumber);
                SetObjectsToOriginalTags(gridNumber);
                indicatorImage.gameObject.SetActive(true); // Hide indicator when active
            }
            else
            {
                // Deactivate the grid
                levelEditor.GridsDeactivated.Add(gridNumber);
                SetObjectsToDeactivatedLayer(gridNumber);
                indicatorImage.gameObject.SetActive(false); // Show indicator when inactive
            }
        }
    }

    private void SetObjectsToDeactivatedLayer(int gridLayer)
    {
        foreach (GameObject cube in gridGenerator.GridCubes)
        {
            if (Mathf.RoundToInt(cube.transform.position.y) == gridLayer)
            {
                SetLayerRecursively(cube, LayerMask.NameToLayer("DeactivatedLayer"));
            }
        }

        foreach (GameObject prefab in instantiatedPrefabs)
        {
            if (Mathf.RoundToInt(prefab.transform.position.y) == gridLayer)
            {
                SetLayerRecursively(prefab, LayerMask.NameToLayer("DeactivatedLayer"));
            }
        }
    }

    // Helper method to set the layer recursively for an object and its children
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        if (obj == null) return;

        // Check if the layer is valid
        if (layer < 0 || layer > 31)
        {
            Debug.LogWarning($"Invalid layer index {layer}. Object '{obj.name}' could not be assigned to this layer.");
            return;
        }

        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }


    private void SetObjectsToOriginalTags(int gridLayer)
    {
        foreach (GameObject cube in gridGenerator.GridCubes)
        {
            if (Mathf.RoundToInt(cube.transform.position.y) == gridLayer)
            {
                SetLayerRecursively(cube, LayerMask.NameToLayer(cube.tag));
            }
        }

        foreach (GameObject prefab in instantiatedPrefabs)
        {
            if (Mathf.RoundToInt(prefab.transform.position.y) == gridLayer)
            {
                SetLayerRecursively(prefab, LayerMask.NameToLayer(prefab.tag));
            }
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
                        GameObject instantiatedPrefab = InstantiatePrefab(prefab, prefabData.Position, prefabData.Rotation);

                        int gridLayer = Mathf.RoundToInt(instantiatedPrefab.transform.position.y);
                        if (levelEditor.GridsDeactivated.Contains(gridLayer))
                        {
                            SetLayerRecursively(instantiatedPrefab, LayerMask.NameToLayer("DeactivatedLayer"));
                        }
                        else
                        {
 
                            SetLayerRecursively(instantiatedPrefab, LayerMask.NameToLayer(instantiatedPrefab.tag));
                        }

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
