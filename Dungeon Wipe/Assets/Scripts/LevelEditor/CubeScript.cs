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

    [SerializeField] private Color hoverColor;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void OnMouseEnter()
    {
        renderer.material.color = hoverColor; // Change to hover color
    }

    void OnMouseExit()
    {
        renderer.material.color = Color.black; // Revert to original color
    }

    void OnMouseDown()
    {
        if (PrefabManager.Instance.CurrentPrefab != null)
        {
            // Check if a prefab is already instantiated on this cube
            if (transform.childCount == 4) // No children, safe to instantiate
            {
                GameObject prefab = PrefabManager.Instance.CurrentPrefab;
                prefab.transform.position = Vector3.zero;
                GameObject newPrefab = Instantiate(prefab, transform.position, Quaternion.identity);
                newPrefab.transform.SetParent(transform); // Set the cube as the parent of the new prefab
            }
        }
    }
}
