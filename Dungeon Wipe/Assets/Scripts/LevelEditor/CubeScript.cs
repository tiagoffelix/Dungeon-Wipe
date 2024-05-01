using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    private int gridX; // Coordenada X do cubo na grade
    private int gridY; // Coordenada Y do cubo na grade

    public int GridX { get { return gridX; } set { gridX = value; } }

    public int GridY { get { return gridY; } set { gridY = value; } }

    private Renderer renderer;

    [SerializeField] private Color hoverColorFree;
    [SerializeField] private Color hoverColorOccupied;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void OnMouseEnter()
    {
        if(transform.childCount == 5) 
        {
            renderer.material.color = hoverColorOccupied;
        }
        else 
        {
            renderer.material.color = hoverColorFree;
        }
        if (PrefabManager.Instance.CurrentPrefab != null)
        {
            PrefabManager.Instance.CurrentPrefab.transform.position = transform.position;
            PrefabManager.Instance.CurrentPrefab.GetComponent<PrefabFollower>().enabled = false;
        }
    }

    void OnMouseExit()
    {
        renderer.material.color = Color.black; // Revert to original color
        if (PrefabManager.Instance.CurrentPrefab != null)
        {
            PrefabManager.Instance.CurrentPrefab.GetComponent<PrefabFollower>().enabled = true;
        }
    }

    void OnMouseDown()
    {
        if (PrefabManager.Instance.CurrentPrefab != null)
        {
            // Check if a prefab is already instantiated on this cube
            if (transform.childCount == 4) // No children, safe to instantiate
            {
                GameObject newPrefab = Instantiate(PrefabManager.Instance.CurrentPrefab, transform.position, PrefabManager.Instance.CurrentPrefab.transform.rotation);
                newPrefab.GetComponent<PrefabFollower>().enabled = false;
                newPrefab.transform.SetParent(transform); // Set the cube as the parent of the new prefab
            }
        }
    }
}
