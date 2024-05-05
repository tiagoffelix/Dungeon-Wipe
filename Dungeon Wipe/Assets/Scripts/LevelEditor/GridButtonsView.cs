using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the generation of grid buttons when the scroll view is enabled.
/// </summary>
public class GridButtonsView : MonoBehaviour
{
    /// <summary>
    /// Called when the GameObject becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        // When the scroll view is enabled, generate the grid buttons
        if (PrefabManager.Instance != null)
        {
            PrefabManager.Instance.GenerateGridButtons();
        }
    }
}
