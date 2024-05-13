using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewContent : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs; // List of prefabs available for instantiation
    [SerializeField] private GameObject buttonPrefab; // Prefab for the buttons in the scroll view
    [SerializeField] private ObjectManager objectManager; // Reference to the ObjectManager script
    [SerializeField] private LevelEditorSO levelEditor; // Reference to the LevelEditorSO scriptable object


    // Start is called before the first frame update
    void Start()
    {
       if(prefabs.Count != 0) 
       {
           foreach (var prefab in prefabs)
           {
               GameObject btn = Instantiate(buttonPrefab, transform);
               btn.GetComponentInChildren<TextMeshProUGUI>().text = prefab.name;
               btn.GetComponent<Button>().onClick.AddListener(() => PrefabManager.Instance.SetCurrentPrefab(prefab));
               btn.GetComponent<Button>().onClick.AddListener(() => objectManager.SetDeletingFalse());
           }
       }
    }
}
