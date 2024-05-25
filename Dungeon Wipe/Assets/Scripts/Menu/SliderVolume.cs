using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    [SerializeField] private Stats playerStats; // Reference to PlayerStats
    private Slider volumeSlider; // Reference to the Slider component

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();

        if (playerStats != null && volumeSlider != null)
        {
            // Set the slider's value to the playerStats.Volume
            volumeSlider.value = playerStats.Volume;

            // Add listener to handle value changes
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    /// <summary>
    /// Called when the slider's value changes.
    /// </summary>
    /// <param name="value">The new value of the slider.</param>
    private void OnSliderValueChanged(float value)
    {
        if (playerStats != null)
        {
            // Update playerStats.Volume with the new slider value
            playerStats.Volume = value;
        }
    }
}
