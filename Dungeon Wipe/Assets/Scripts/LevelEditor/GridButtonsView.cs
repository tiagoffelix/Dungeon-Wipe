using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridButtonsView : MonoBehaviour
{
    void OnEnable()
    {
        // When the scroll view is enabled, generate the grid buttons
        if (PrefabManager.Instance != null)
        {
            PrefabManager.Instance.GenerateGridButtons();
        }
    }
}
