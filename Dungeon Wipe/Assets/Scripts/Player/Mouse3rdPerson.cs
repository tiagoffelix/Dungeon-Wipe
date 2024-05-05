using UnityEngine;

/// <summary>
/// Controls third-person camera movement based on mouse input.
/// </summary>
public class Mouse3rdPerson : MonoBehaviour
{
    [SerializeField] private Stats playerStats; // Reference to the player's statistics for sensitivity

    [SerializeField] private Transform player; // Reference to the player's transform

    /// <summary>
    /// Updates the camera orientation based on horizontal mouse input.
    /// </summary>
    void Update()
    {
        // Get the horizontal mouse input and multiply it by sensitivity and delta time
        float mouseX = Input.GetAxis("Mouse X") * playerStats.Sensitivity * Time.deltaTime;

        // Rotate the player based on the horizontal mouse input
        player.Rotate(Vector3.up * mouseX);
    }
}
