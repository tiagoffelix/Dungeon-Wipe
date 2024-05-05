using UnityEngine;

[CreateAssetMenu(fileName = "New Health Potion", menuName = "Health Potion")]
/// <summary>
/// Represents a health potion that restores health to the player.
/// </summary>
public class HealthPotion : Potion
{
    [SerializeField] private int health; // Amount of health restored by the potion

    /// <summary>
    /// Gets or sets the amount of health restored. Ensures the value is non-negative.
    /// </summary>
    public int Health
    {
        get { return health; }
        set { health = Mathf.Max(0, value); } // Ensure that health cannot be negative
    }
}
