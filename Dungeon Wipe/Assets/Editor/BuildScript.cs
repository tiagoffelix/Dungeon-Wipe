using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>
/// Reproducible command line builds. Invoked from Unity in batch mode, so the
/// browser build produced for itch.io is the same one anybody can rebuild from
/// a clean checkout:
///
/// "Unity.exe" -quit -batchmode -nographics -logFile - ^
///   -projectPath "&lt;repo&gt;\Dungeon Wipe" -buildTarget WebGL ^
///   -executeMethod BuildScript.BuildWebGL
///
/// The Windows build is left exactly as it was; BuildWindows only exists so the
/// desktop build is reproducible from the same entry point.
/// </summary>
public static class BuildScript
{
    private const string WebGLOutput = "Builds/WebGL/DungeonWipe";
    private const string WindowsOutput = "Builds/Windows/Dungeon Wipe.exe";

    /// <summary>Scenes enabled in Build Settings, in order.</summary>
    private static string[] Scenes
    {
        get { return EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(); }
    }

    /// <summary>Builds the browser version used by the itch.io page.</summary>
    [MenuItem("Build/Dungeon Wipe WebGL")]
    public static void BuildWebGL()
    {
        // itch.io serves .br files with the right Content-Encoding, so Brotli
        // without the JavaScript decompression fallback is the smallest and
        // fastest combination there.
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        PlayerSettings.WebGL.decompressionFallback = false;
        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;
        PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.WebGL, ManagedStrippingLevel.Low);

        Run(BuildTarget.WebGL, BuildTargetGroup.WebGL, WebGLOutput);
    }

    /// <summary>Builds the Windows version.</summary>
    [MenuItem("Build/Dungeon Wipe Windows")]
    public static void BuildWindows()
    {
        Run(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, WindowsOutput);
    }

    private static void Run(BuildTarget target, BuildTargetGroup group, string relativeOutput)
    {
        string[] scenes = Scenes;
        if (scenes.Length == 0)
        {
            throw new Exception("No scenes are enabled in Build Settings.");
        }

        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string output = Path.Combine(projectRoot, relativeOutput.Replace('/', Path.DirectorySeparatorChar));

        string outputFolder = Path.HasExtension(output) ? Path.GetDirectoryName(output) : output;
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        if (EditorUserBuildSettings.activeBuildTarget != target)
        {
            Debug.Log($"Switching active build target to {target}.");
            EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);
        }

        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = output,
            target = target,
            targetGroup = group,
            options = BuildOptions.None
        });

        BuildSummary summary = report.summary;
        Debug.Log($"{target} build {summary.result}: {summary.totalSize / (1024 * 1024)} MB in {summary.totalTime}, at {output}");

        if (summary.result != BuildResult.Succeeded)
        {
            throw new Exception($"{target} build failed with {summary.totalErrors} error(s).");
        }
    }
}
