using UnityEngine;

public class Potion : ScriptableObject
{
    [SerializeField] private string potionName;
    [SerializeField] private AudioClip sound;
    [SerializeField] private float duration;
    [SerializeField] private int timeToDespawn;

    // Public getter and setter for potionName
    public string PotionName
    {
        get { return potionName; }
    }

    // Public getter and setter for sound
    public AudioClip Sound
    {
        get { return sound; }
    }

    // Public getter and setter for duration
    public float Duration
    {
        get { return duration; }
    }

    public int TimeToDespawn
    {
        get { return timeToDespawn; }
    }
}