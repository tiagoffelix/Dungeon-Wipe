using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Represents a floor in the game world.
/// </summary>
public class FloorScript : MonoBehaviour
{
    /// <summary>
    /// Indicates if the floor has a collectible.
    /// </summary>
    public bool HasCollectible { get; set; } = false;

    /// <summary>
    /// Indicates if the player is in range of the floor.
    /// </summary>
    public bool PlayerInRange { get; private set; }

    private Collider[] colliders; // Cache of colliders

    /// <summary>
    /// Initializes the floor.
    /// </summary>
    void Start()
    {
        colliders = GetComponents<Collider>();
        CheckAndDeactivateCollider();
    }

    /// <summary>
    /// Checks the collider and deactivates it if the scene is not "Game".
    /// </summary>
    private void CheckAndDeactivateCollider()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            foreach (var collider in colliders)
            {
                if (collider.isTrigger)
                {
                    collider.enabled = false;
                }
            }
        }
    }

    /// <summary>
    /// Handles logic when another collider enters the trigger area of the floor.
    /// </summary>
    /// <param name="other">The collider entering the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
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
            PlayerInRange = false;
        }
    }
}
