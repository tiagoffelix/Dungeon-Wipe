using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance { get; private set; } // Singleton pattern

    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private ObjectManager objectManager;

    private GameObject currentPrefab;

    public GameObject CurrentPrefab { get => currentPrefab; }

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
        foreach (var prefab in prefabs)
        {
            GameObject btn = Instantiate(buttonPrefab, scrollViewContent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = prefab.name;
            btn.GetComponent<Button>().onClick.AddListener(() => SetCurrentPrefab(prefab));
            btn.GetComponent<Button>().onClick.AddListener(() => objectManager.SetDeletingFalse());
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
}
