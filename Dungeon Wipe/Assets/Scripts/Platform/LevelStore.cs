using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Single access point for the dungeon level JSON files.
///
/// The level editor, the main menu, the high score manager and the runtime level
/// loader all used to build their own path into &lt;dataPath&gt;/Resources/Levels.
/// That folder is writable next to a Windows player, but on WebGL and Android
/// Application.dataPath is not a writable directory on disk, so every one of
/// those call sites failed on those platforms.
///
/// Windows and the Editor keep the original folder, so existing builds, existing
/// saved levels and existing high score keys are untouched.
///
/// Other platforms use Application.persistentDataPath/Levels. On WebGL that path
/// is backed by the browser's IndexedDB, so authored levels survive a page
/// reload. The folder is seeded once from the TextAssets compiled into
/// Resources/Levels so the shipped dungeons are present on first run.
/// </summary>
public static class LevelStore
{
    /// <summary>Name of the Resources sub folder holding the shipped levels.</summary>
    public const string ResourcesFolder = "Levels";

    private const string Extension = ".json";

    private static string root;
    private static bool ready;

    /// <summary>
    /// Absolute path of the folder the game reads and writes levels in.
    /// </summary>
    public static string Root
    {
        get
        {
            if (string.IsNullOrEmpty(root))
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                root = Path.Combine(Application.dataPath, "Resources", ResourcesFolder);
#else
                root = Path.Combine(Application.persistentDataPath, ResourcesFolder);
#endif
            }

            return root;
        }
    }

    /// <summary>
    /// True when the platform stores levels outside the installed game data and
    /// therefore needs the shipped levels copied in on first run.
    /// </summary>
    public static bool UsesWritableCopy
    {
        get
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return false;
#else
            return true;
#endif
        }
    }

    /// <summary>
    /// Turns either a full file path or a bare level name into the level id.
    /// </summary>
    public static string ToId(string pathOrId)
    {
        return string.IsNullOrEmpty(pathOrId) ? string.Empty : Path.GetFileNameWithoutExtension(pathOrId);
    }

    /// <summary>
    /// Full path of a level, accepting either a full path or a bare level name.
    /// </summary>
    public static string PathFor(string pathOrId)
    {
        string id = ToId(pathOrId);
        return string.IsNullOrEmpty(id) ? string.Empty : Path.Combine(Root, id + Extension);
    }

    /// <summary>
    /// Every level currently available, as full file paths, sorted by name.
    /// </summary>
    public static string[] ListPaths()
    {
        EnsureReady();

        try
        {
            string[] paths = Directory.GetFiles(Root, "*" + Extension);
            System.Array.Sort(paths);
            return paths;
        }
        catch (IOException exception)
        {
            Debug.LogError($"LevelStore could not list '{Root}': {exception.Message}");
            return new string[0];
        }
    }

    /// <summary>Whether a level exists in the store.</summary>
    public static bool Exists(string pathOrId)
    {
        EnsureReady();
        string path = PathFor(pathOrId);
        return !string.IsNullOrEmpty(path) && File.Exists(path);
    }

    /// <summary>
    /// Reads a level's JSON. Returns false and logs when the level is missing or
    /// unreadable, so callers can bail out instead of throwing.
    /// </summary>
    public static bool TryRead(string pathOrId, out string json)
    {
        EnsureReady();
        json = null;

        string path = PathFor(pathOrId);
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            return false;
        }

        try
        {
            json = File.ReadAllText(path);
            return !string.IsNullOrEmpty(json);
        }
        catch (IOException exception)
        {
            Debug.LogError($"LevelStore could not read '{path}': {exception.Message}");
            return false;
        }
    }

    /// <summary>
    /// Writes a level's JSON and flushes it to browser storage on WebGL.
    /// </summary>
    public static bool Write(string pathOrId, string json)
    {
        EnsureReady();

        string path = PathFor(pathOrId);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("LevelStore was asked to save a level with no name.");
            return false;
        }

        try
        {
            File.WriteAllText(path, json);
            BrowserStorage.Flush();
            return true;
        }
        catch (IOException exception)
        {
            Debug.LogError($"LevelStore could not write '{path}': {exception.Message}");
            return false;
        }
    }

    /// <summary>
    /// Creates the store folder and, on platforms that cannot write into the
    /// installed game data, copies in any shipped level that is not there yet.
    /// </summary>
    private static void EnsureReady()
    {
        if (ready)
        {
            return;
        }

        ready = true;

        try
        {
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }

            if (!UsesWritableCopy)
            {
                return;
            }

            bool seeded = false;
            foreach (TextAsset shipped in Resources.LoadAll<TextAsset>(ResourcesFolder))
            {
                string path = Path.Combine(Root, shipped.name + Extension);
                if (File.Exists(path))
                {
                    continue;
                }

                File.WriteAllText(path, shipped.text);
                seeded = true;
            }

            if (seeded)
            {
                BrowserStorage.Flush();
            }
        }
        catch (IOException exception)
        {
            Debug.LogError($"LevelStore could not prepare '{Root}': {exception.Message}");
        }
    }
}
