using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    private int gridX; // Coordinate X of the cube on the grid
    private int gridY; // Coordinate Y of the cube on the grid

    public int GridX { get { return gridX; } set { gridX = value; } }
    public int GridY { get { return gridY; } set { gridY = value; } }

    private new Renderer renderer;
    [SerializeField] private Color hoverColorFree;
    [SerializeField] private Color hoverColorOccupied;
    [SerializeField] private Color defaultColor;

    private Collider cubeCollider;

    // Public property to track if a player prefab has been placed

    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.color = defaultColor; // Initialize with the default color
        cubeCollider = GetComponent<Collider>(); // Cache the collider
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray intersects this specific cube's collider
        if (cubeCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Mouse Enter logic
            if (transform.childCount == 5)
            {
                renderer.material.color = hoverColorOccupied;
            }
            else
            {
                renderer.material.color = hoverColorFree;
            }

            if (PrefabManager.Instance.CurrentPrefab != null && transform.childCount == 4)
            {
                PrefabManager.Instance.CurrentPrefab.transform.position = transform.position;
                PrefabManager.Instance.CurrentPrefab.GetComponent<PrefabFollower>().enabled = false;
                if (Input.GetKeyDown(KeyCode.R))
                {
                    PrefabManager.Instance.CurrentPrefab.transform.Rotate(0, 90, 0); // Rotate 90 degrees around the Y axis
                }
            }

            // Handle OnMouseDown
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseDown();
            }
        }
        else
        {
            // Mouse Exit logic
            renderer.material.color = defaultColor;
            if (PrefabManager.Instance.CurrentPrefab != null)
            {
                PrefabManager.Instance.CurrentPrefab.GetComponent<PrefabFollower>().enabled = true;
            }
        }
    }

    // Method to handle prefab placement
    private void HandleMouseDown()
    {
        if (PrefabManager.Instance.CurrentPrefab != null)
        {
            // Check if a player prefab has already been placed
            if (PrefabManager.Instance.CurrentPrefab.layer == LayerMask.NameToLayer("Player") && PrefabManager.Instance.PlayerPlaced)
            {
                return; // Exit if a player prefab is already placed
            }

            // Check if a prefab is already instantiated on this cube
            if (PrefabManager.Instance.CurrentPrefab != null && transform.childCount == 4)
            {
                GameObject newPrefab = PrefabManager.Instance.InstantiatePrefab(
                    PrefabManager.Instance.CurrentPrefab, transform.position,
                    PrefabManager.Instance.CurrentPrefab.transform.rotation);

                if (newPrefab.layer == LayerMask.NameToLayer("Player"))
                {
                    PrefabManager.Instance.PlayerPlaced = true;
                    PrefabManager.Instance.DestroyCurrentPrefab();
                }
            }
        }
    }
}
