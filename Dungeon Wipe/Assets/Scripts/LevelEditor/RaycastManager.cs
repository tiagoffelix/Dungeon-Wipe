using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    public LayerMask cubeLayerMask; // Set this to the layer used by the cubes in Unity's Inspector
    private CubeScript currentCube;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask combinedMask = cubeLayerMask & LayerMask.NameToLayer("DeactivatedLayer");

        // Perform the raycast using the cube layer mask
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, combinedMask))
        {
            CubeScript cubeScript = hit.collider.GetComponent<CubeScript>();

            if (cubeScript != null)
            {
                // Handle the cube that is currently hovered over
                HandleHoveredCube(cubeScript);

                if (Input.GetMouseButtonDown(0))
                {
                    cubeScript.HandleMouseDown();
                }
            }
        }
        else
        {
            // Reset the current cube if the raycast doesn't hit any cube
            if (currentCube != null)
            {
                currentCube.ResetCubeColor();
                currentCube = null;
            }
        }
    }

    // Handle the hover state of the cube
    private void HandleHoveredCube(CubeScript cubeScript)
    {
        if (currentCube != null && currentCube != cubeScript)
        {
            currentCube.ResetCubeColor();
        }

        currentCube = cubeScript;
        currentCube.UpdateHoverState();
    }
}
