using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabFollower : MonoBehaviour
{
    private float distanceFromCamera = 10f;  // Distance from the camera to the prefab

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition; // Get the mouse position in screen coordinates
        mousePosition.z = distanceFromCamera; // Set how far from the camera the object should be placed
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition); // Convert to world position
        transform.position = worldPosition; // Move the prefab to this position
    }
}
