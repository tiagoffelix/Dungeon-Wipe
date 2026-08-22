using UnityEditor;
using UnityEngine;

/// <summary>
/// Caps the resolution of the imported skybox textures for browser builds only.
///
/// The skybox pack is around 374 MB of HDR cubemaps, and it is almost the whole
/// weight of a WebGL build. A player waiting on a browser tab is not going to
/// notice 4K skies, so the browser build caps them; the Windows build keeps the
/// textures at full resolution because the platform override applies to WebGL
/// alone.
///
/// Run once from the Build menu. The settings are stored in each texture's
/// .meta file, so this does not need running again unless new skies are added.
/// </summary>
public static class WebTextureBudget
{
    private const string SkyboxFolder = "Assets/Assets/SkySeries Freebie";
    private const string Platform = "WebGL";
    private const int MaxSize = 1024;

    /// <summary>Applies the browser resolution cap to every skybox texture.</summary>
    [MenuItem("Build/Apply Web texture budget to skyboxes")]
    public static void Apply()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture", new[] { SkyboxFolder });
        int changed = 0;

        try
        {
            AssetDatabase.StartAssetEditing();

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null)
                {
                    continue;
                }

                TextureImporterPlatformSettings settings = importer.GetPlatformTextureSettings(Platform);
                if (settings.overridden && settings.maxTextureSize == MaxSize)
                {
                    continue;
                }

                settings.name = Platform;
                settings.overridden = true;
                settings.maxTextureSize = MaxSize;
                importer.SetPlatformTextureSettings(settings);
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
                changed++;
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
        }

        Debug.Log($"Web texture budget: {changed} of {guids.Length} skybox texture(s) capped at {MaxSize}px for {Platform}.");
    }
}
