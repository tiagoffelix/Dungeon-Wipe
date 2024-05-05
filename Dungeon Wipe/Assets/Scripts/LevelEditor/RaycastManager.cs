using UnityEngine;

/// <summary>
/// Manages raycasting and interaction with cubes in the scene.
/// </summary>
public class RaycastManager : MonoBehaviour
{
    [SerializeField] private LayerMask cubeLayerMask; // Layer mask for cubes
    private CubeScript currentCube; // Reference to the currently hovered cube

    /// <summary>
    /// Called every frame to check for mouse interaction with cubes.
    /// </summary>
    void Update()
    {
        // Cast a ray from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform a raycast using the cube layer mask
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cubeLayerMask))
        {
            // Get the CubeScript component attached to the hit cube
            CubeScript cubeScript = hit.collider.GetComponent<CubeScript>();

            if (cubeScript != null)
            {
                // Handle interaction with the hovered cube
                HandleHoveredCube(cubeScript);

                // Check if the left mouse button is clicked
                if (Input.GetMouseButton(0))
                {
                    // Handle mouse down event on the cube
                    cubeScript.HandleMouseDown();
                }
            }
        }
        else
        {
            // Reset the color of the current cube if no cube is hit
            if (currentCube != null)
            {
                currentCube.ResetCubeColor();
                currentCube = null;
            }
        }
    }

    /// <summary>
    /// Handles the hover state of the cube.
    /// </summary>
    /// <param name="cubeScript">The CubeScript component of the hovered cube.</param>
    private void HandleHoveredCube(CubeScript cubeScript)
    {
        // Reset the color of the previous hovered cube if it's different
        if (currentCube != null && currentCube != cubeScript)
        {
            currentCube.ResetCubeColor();
        }

        // Update the reference to the currently hovered cube
        currentCube = cubeScript;
        // Update the hover state of the cube
        currentCube.UpdateHoverState();
    }
}
