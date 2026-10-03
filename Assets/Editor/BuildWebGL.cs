using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildWebGL
{
    private static readonly string[] WebGLScenes =
    {
        "Assets/Scenes/StartMenu.unity",
        "Assets/Scenes/LevelTimeLimit.unity",
        "Assets/Scenes/Levels.unity",
    };

    [MenuItem("Build/Build FrogGame WebGL")]
    public static void Build()
    {
        var outputPath = "Build/WebGL";
        var build = BuildPipeline.BuildPlayer(WebGLScenes, outputPath, BuildTarget.WebGL, BuildOptions.None);

        if (build.summary.result != BuildResult.Succeeded)
        {
            throw new Exception($"WebGL build failed: {build.summary.result}");
        }

        Debug.Log($"WebGL build created at {outputPath} ({build.summary.totalSize / 1024 / 1024} MB).");
    }
}
