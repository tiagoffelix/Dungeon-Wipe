using UnityEngine;

/// <summary>
/// Represents a generic potion, including its properties and effects.
/// </summary>
public class Potion : ScriptableObject
{
    [SerializeField] private string potionName; // Name of the potion
    [SerializeField] private AudioClip sound; // Sound effect associated with the potion
    [SerializeField] private float duration; // Duration of the potion's effect in seconds
    [SerializeField] private int timeToDespawn; // Time in seconds before the potion despawns

    /// <summary>
    /// Gets the name of the potion.
    /// </summary>
    public string PotionName
    {
        get { return potionName; }
    }

    /// <summary>
    /// Gets the sound effect associated with the potion.
    /// </summary>
    public AudioClip Sound
    {
        get { return sound; }
    }

    /// <summary>
    /// Gets the duration of the potion's effect in seconds.
    /// </summary>
    public float Duration
    {
        get { return duration; }
    }

    /// <summary>
    /// Gets the time in seconds before the potion despawns.
    /// </summary>
    public int TimeToDespawn
    {
        get { return timeToDespawn; }
    }
}
