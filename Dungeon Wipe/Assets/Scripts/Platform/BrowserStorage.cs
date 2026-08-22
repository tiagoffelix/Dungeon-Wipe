using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

/// <summary>
/// Pushes pending writes under Application.persistentDataPath into the browser's
/// IndexedDB. Unity flushes that filesystem itself, but a level the player just
/// authored is worth flushing immediately rather than at the engine's next
/// opportunity. Does nothing outside a browser build.
/// </summary>
public static class BrowserStorage
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void DungeonWipeSyncFileSystem();
#endif

    /// <summary>Requests a flush of persistent storage.</summary>
    public static void Flush()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        try
        {
            DungeonWipeSyncFileSystem();
        }
        catch (System.EntryPointNotFoundException)
        {
            Debug.LogWarning("Browser storage flush is unavailable in this build.");
        }
#endif
    }
}
