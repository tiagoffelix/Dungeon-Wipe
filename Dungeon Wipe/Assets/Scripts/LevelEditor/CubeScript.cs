using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of a cube in the game world.
/// </summary>
public class CubeScript : MonoBehaviour
{
    private new Renderer renderer; // The renderer component of the cube.
    [SerializeField] private Color hoverColorFree; // The color to display when the cube is hovered and unoccupied.
    [SerializeField] private Color hoverColorOccupied; // The color to display when the cube is hovered and occupied.
    [SerializeField] private Color defaultColor; // The default color of the cube.

    private bool hasObjectOnTop; // Indicates whether there is an object on top of the cube.

    /// <summary>
    /// Initializes the cube's state and color.
    /// </summary>
    void Start()
    {
        hasObjectOnTop = false;
        renderer = GetComponent<Renderer>();
        renderer.material.color = defaultColor;
    }

    /// <summary>
    /// Updates the hover state of the cube based on whether an object is on top.
    /// </summary>
    public void UpdateHoverState()
    {
        hasObjectOnTop = HasObjectOnTop();

        if (hasObjectOnTop)
        {
            renderer.material.color = hoverColorOccupied;
        }
        else
        {
            renderer.material.color = hoverColorFree;
        }

        if (PrefabManager.Instance.CurrentPrefab != null && !hasObjectOnTop)
        {
            PrefabManager.Instance.CurrentPrefab.transform.position = transform.position;
            PrefabManager.Instance.CurrentPrefab.GetComponent<PrefabFollower>().enabled = false;
        }
    }

    /// <summary>
    /// Resets the color of the cube to its default color.
    /// </summary>
    public void ResetCubeColor()
    {
        renderer.material.color = defaultColor;
        if (PrefabManager.Instance.CurrentPrefab != null)
        {
            PrefabManager.Instance.CurrentPrefab.GetComponent<PrefabFollower>().enabled = true;
        }
    }

    /// <summary>
    /// Handles the mouse down event on the cube.
    /// </summary>
    public void HandleMouseDown()
    {
        if (PrefabManager.Instance.CurrentPrefab != null)
        {
            // Check if a player prefab has already been placed
            if (PrefabManager.Instance.CurrentPrefab.layer == LayerMask.NameToLayer("Player") && PrefabManager.Instance.PlayerPlaced)
            {
                return; // Exit if a player prefab is already placed
            }

            // Check if there is no object on top
            if (!hasObjectOnTop)
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

    /// <summary>
    /// Checks if there is any object on top of the cube.
    /// </summary>
    /// <returns>True if an object is on top, false otherwise.</returns>
    private bool HasObjectOnTop()
    {
        Vector3 boxSize = new Vector3(transform.localScale.x * 0.15f, transform.localScale.y * 0.05f, transform.localScale.z * 0.15f);
        int obstacleLayer = LayerMask.NameToLayer("Obstacle");
        int layerMask = ~(1 << obstacleLayer);  

        Collider[] colliders = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity, layerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject != PrefabManager.Instance.CurrentPrefab)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        // Set the Gizmo color to red
        Gizmos.color = Color.red;

        // Calculate the box size
        Vector3 boxSize = new Vector3(transform.localScale.x * 0.15f, transform.localScale.y * 0.05f, transform.localScale.z * 0.15f);

        // Draw the wire cube at the object's position with the specified box size
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
