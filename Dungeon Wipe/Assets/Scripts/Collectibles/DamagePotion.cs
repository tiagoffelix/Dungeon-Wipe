using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Potion", menuName = "Damage Potion")]
/// <summary>
/// Represents a damage potion that grants a temporary damage boost to the player.
/// </summary>
public class DamagePotion : Potion
{
    [SerializeField] private float damageBoost; // Boost value for increasing player's damage

    /// <summary>
    /// Gets or sets the damage boost value. Ensures the value is non-negative.
    /// </summary>
    public float DamageBoost
    {
        get { return damageBoost; }
        set { damageBoost = Mathf.Max(0, value); } // Ensure that damage boost cannot be negative
    }
}
