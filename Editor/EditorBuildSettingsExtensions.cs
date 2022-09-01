using System;
using UnityEditor;

namespace GameFramework.Extensions
{
    public static class EditorBuildSettingsExtensions
    {
        public static void AddSceneToBuildSettings(string sceneAssetPath)
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            EditorBuildSettingsScene scene = Array.Find(scenes, path => path.path == sceneAssetPath);
            if (scene is not null)
            {
                scene = new EditorBuildSettingsScene(sceneAssetPath, true);
                ArrayUtility.Add(ref scenes, scene);
                EditorBuildSettings.scenes = scenes;
            }
        }
    }
}