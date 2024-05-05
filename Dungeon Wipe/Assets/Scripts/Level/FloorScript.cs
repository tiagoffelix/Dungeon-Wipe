using UnityEngine;

/// <summary>
/// Represents a floor in the game world.
/// </summary>
public class FloorScript : MonoBehaviour
{
    private bool hasCollectible; // Indicates if the floor has a collectible.
    public bool HasCollectible { get { return hasCollectible; } set { hasCollectible = value; } }

    private bool playerInRange; // Indicates if the player is in range of the floor.

    /// <summary>
    /// Gets a value indicating whether the player is in range of the floor.
    /// </summary>
    public bool PlayerInRange { get { return playerInRange; } }

    /// <summary>
    /// Initializes the floor with no collectible.
    /// </summary>
    void Start()
    {
        hasCollectible = false;
    }

    /// <summary>
    /// Handles logic when another collider enters the trigger area of the floor.
    /// </summary>
    /// <param name="other">The collider entering the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    /// <summary>
    /// Handles logic when another collider exits the trigger area of the floor.
    /// </summary>
    /// <param name="other">The collider exiting the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
