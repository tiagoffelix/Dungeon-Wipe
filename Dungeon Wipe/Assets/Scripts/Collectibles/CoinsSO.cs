using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCoinsSO", menuName = "CoinsSO")]
/// <summary>
/// Represents the data for coin objects, including score, sound, and despawn time.
/// </summary>
public class CoinsSO : ScriptableObject
{
    [SerializeField] private int score; // Score value assigned to the coin
    [SerializeField] private AudioClip sound; // Sound to be played upon coin collection
    [SerializeField] private int timeToDespawn; // Time in seconds before the coin despawns

    /// <summary>
    /// Gets the score value assigned to the coin.
    /// </summary>
    public int Score
    {
        get { return score; }
    }

    /// <summary>
    /// Gets the audio source to be played upon coin collection.
    /// </summary>
    public AudioClip Sound
    {
        get { return sound; }
    }

    /// <summary>
    /// Gets the time in seconds before the coin despawns.
    /// </summary>
    public int TimeToDespawn
    {
        get { return timeToDespawn; }
    }
}
