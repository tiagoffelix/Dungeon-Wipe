using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    [SerializeField] private LayerMask cubeLayerMask;
    private CubeScript currentCube;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast using the cube layer mask
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cubeLayerMask))
        {
            CubeScript cubeScript = hit.collider.GetComponent<CubeScript>();

            if (cubeScript != null)
            {
                // Handle the cube that is currently hovered over
                HandleHoveredCube(cubeScript);

                if (Input.GetMouseButton(0))
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