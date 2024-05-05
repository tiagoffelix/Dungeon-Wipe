using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves a prefab to follow the mouse cursor on the screen.
/// </summary>
public class PrefabFollower : MonoBehaviour
{
    /// <summary>
    /// The distance from the camera to the prefab.
    /// </summary>
    private float distanceFromCamera = 10f;

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Set the z-coordinate of the mouse position to the desired distance from the camera
        mousePosition.z = distanceFromCamera;

        // Convert the screen position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Move the prefab to the calculated world position
        transform.position = worldPosition;
    }
}
