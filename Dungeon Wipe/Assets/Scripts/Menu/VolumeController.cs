using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    /// <summary>
    /// Reference to PlayerStats.
    /// </summary>
    [SerializeField] private Stats playerStats;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        if (playerStats != null)
        {
            // Set initial volume from playerStats
            SetVolume(playerStats.Volume);
        }
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (playerStats != null)
        {
            // Update volume from playerStats every frame
            SetVolume(playerStats.Volume);
        }
        print(playerStats.Volume);
    }

    /// <summary>
    /// Sets the volume of the AudioListener.
    /// </summary>
    /// <param name="volume">The volume level to set.</param>
    private void SetVolume(float volume)
    {
        // Assuming the AudioListener component is on the same GameObject
        AudioListener.volume = volume;
    }
}
