using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework.Extensions;

using UnitySceneAsset = UnityEditor.SceneAsset;

namespace GameFramework
{
    [CustomEditor(typeof(SceneAsset))]
    public class SceneAssetEditor : Editor
    {
        public static readonly bool AutoAddSceneToBuildSettings = true;

        public override void OnInspectorGUI()
        {
            // serialized object and properties
            SerializedObject serializedObject = this.serializedObject;
            SerializedProperty propScenePath = serializedObject.FindProperty("_scenePath");

            string scenePath = propScenePath.stringValue;   // path to address Scene using UnitySceneManager
            string sceneAssetPath = string.Empty;           // path to address SceneAsset using AssetDatabase
            UnitySceneAsset sceneAsset = null;              // UnityEditor.SceneAsset addressed using sceneAssetPath

            // avoid null values
            scenePath ??= string.Empty;

            // create sceneAssetPath and load SceneAsset
            if (String.IsNullOrEmpty(scenePath) == false)
            {
                sceneAssetPath = $"Assets/{scenePath}.unity";
                sceneAsset = AssetDatabase.LoadAssetAtPath<UnitySceneAsset>(
                    sceneAssetPath);
            }

            // editor field for assigning SceneAsset
            UnitySceneAsset newSceneAsset = EditorGUILayout.ObjectField("SceneAsset",
                sceneAsset, typeof(UnitySceneAsset), false) as UnitySceneAsset;

            if (newSceneAsset != sceneAsset)
            {
                // if new SceneAsset is assigned, update paths
                scenePath = string.Empty;
                sceneAssetPath = string.Empty;

                if (newSceneAsset is not null)
                {
                    sceneAssetPath = AssetDatabase.GetAssetPath(newSceneAsset);
                    scenePath = Path.ChangeExtension(sceneAssetPath, null);
                    scenePath = scenePath.Substring(7);     // to remove "Assets/"
                }

                // serialize path into Asset
                propScenePath.stringValue = scenePath;
                serializedObject.ApplyModifiedProperties();
            }

            // check if scene is included in build settings
            if (SceneUtility.GetBuildIndexByScenePath(sceneAssetPath) < 0)
            {
                if (AutoAddSceneToBuildSettings)
                {
                    EditorBuildSettingsExtensions.AddSceneToBuildSettings(sceneAssetPath);
                }
                else
                {
                    EditorGUILayout.HelpBox("Scene is not included in build settings.", MessageType.Warning);
                    if (GUILayout.Button("Add to build"))
                    {
                        EditorBuildSettingsExtensions.AddSceneToBuildSettings(sceneAssetPath);
                    }
                }
            }
        }
    }
}