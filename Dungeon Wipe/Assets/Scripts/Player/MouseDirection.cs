using UnityEngine;

/// <summary>
/// Controls the camera direction based on mouse input, including vertical and horizontal rotation.
/// </summary>
public class MouseDirection : MonoBehaviour
{
    [SerializeField] private Stats playerStats; // Reference to the player's statistics for sensitivity

    [SerializeField] private Transform player; // Reference to the player's transform

    private float xRotation; // Stores the current vertical rotation value

    /// <summary>
    /// Initializes the vertical rotation value.
    /// </summary>
    private void Start()
    {
        xRotation = 0f;
    }

    /// <summary>
    /// Updates the camera's direction based on horizontal and vertical mouse input.
    /// </summary>
    void Update()
    {
        // Get the horizontal and vertical mouse input, multiply by sensitivity and delta time
        float mouseX = Input.GetAxis("Mouse X") * playerStats.Sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * playerStats.Sensitivity * Time.deltaTime;

        // Adjust the vertical rotation based on the mouse's Y-axis input
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -15f, 45f); // Clamp the rotation to prevent extreme angles

        // Apply the vertical rotation to the camera
        transform.localEulerAngles = Vector3.right * xRotation;

        // Rotate the player horizontally based on the mouse's X-axis input
        player.Rotate(Vector3.up * mouseX);
    }
}
