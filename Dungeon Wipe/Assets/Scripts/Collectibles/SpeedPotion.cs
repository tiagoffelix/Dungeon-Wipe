using UnityEngine;

[CreateAssetMenu(fileName = "New Speed Potion", menuName = "Speed Potion")]
/// <summary>
/// Represents a speed potion that grants a temporary speed boost to the player.
/// </summary>
public class SpeedPotion : Potion
{
    [SerializeField] private float speedBoost; // Boost value for increasing the player's speed

    /// <summary>
    /// Gets or sets the speed boost value.
    /// </summary>
    public float SpeedBoost
    {
        get { return speedBoost; }
        set { speedBoost = value; }
    }
}
