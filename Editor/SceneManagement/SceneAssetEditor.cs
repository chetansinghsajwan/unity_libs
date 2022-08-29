using System;
using System.IO;
using UnityEditor;

using UnitySceneAsset = UnityEditor.SceneAsset;

namespace GameFramework
{
    [CustomEditor(typeof(SceneAsset))]
    public class SceneAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject serializedObject = this.serializedObject;
            SerializedProperty propScenePath = serializedObject.FindProperty("_scenePath");
            string scenePath = propScenePath.stringValue;

            UnitySceneAsset sceneAsset = null;
            if (String.IsNullOrEmpty(scenePath) == false)
            {
                sceneAsset = AssetDatabase.LoadAssetAtPath<UnitySceneAsset>(
                $"Assets/{scenePath}.unity");
            }

            UnitySceneAsset newSceneAsset = EditorGUILayout.ObjectField("SceneAsset",
                sceneAsset, typeof(UnitySceneAsset), false) as UnitySceneAsset;

            if (newSceneAsset != sceneAsset)
            {
                scenePath = string.Empty;
                if (newSceneAsset is not null)
                {
                    scenePath = AssetDatabase.GetAssetPath(newSceneAsset);
                    scenePath = Path.ChangeExtension(scenePath, null);
                    scenePath = scenePath.Substring(7);     // to remove "Assets/"
                }

                propScenePath.stringValue = scenePath;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}