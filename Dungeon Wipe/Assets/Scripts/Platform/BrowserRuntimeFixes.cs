#if UNITY_WEBGL && !UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Small browser-only adjustments applied without touching the scenes, so the
/// Windows build and the scene assets stay exactly as they are:
/// pointer lock is re-requested on click, and Quit buttons are hidden because
/// Application.Quit does nothing in a browser tab.
/// </summary>
public static class BrowserRuntimeFixes
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Install()
    {
        GameObject helper = new GameObject("BrowserPointerLock");
        Object.DontDestroyOnLoad(helper);
        helper.AddComponent<BrowserPointerLock>();

        SceneManager.sceneLoaded += HideQuitButtons;
    }

    private static void HideQuitButtons(Scene scene, LoadSceneMode mode)
    {
        foreach (Button button in Object.FindObjectsOfType<Button>(true))
        {
            for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
            {
                if (button.onClick.GetPersistentMethodName(i) != "QuitGame")
                {
                    continue;
                }

                button.gameObject.SetActive(false);
                break;
            }
        }
    }
}
#endif
