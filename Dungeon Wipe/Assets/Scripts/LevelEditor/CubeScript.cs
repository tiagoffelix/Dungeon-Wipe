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

    // Your original OnMouseDown logic here
    private void HandleMouseDown()
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
