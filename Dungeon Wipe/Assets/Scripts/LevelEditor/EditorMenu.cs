using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles menu interactions in the editor mode.
/// </summary>
public class EditorMenu : MonoBehaviour
{
    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
