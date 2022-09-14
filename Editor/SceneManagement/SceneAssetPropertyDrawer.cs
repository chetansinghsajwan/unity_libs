using UnityEditor;
using UnityEngine;

using UnitySceneAsset = UnityEditor.SceneAsset;

namespace GameFramework
{
    [CustomPropertyDrawer(typeof(SceneAsset))]
    public class SceneAssetPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // serialized object and properties
            SerializedObject serializedObject = property.serializedObject;
            SerializedProperty propScenePath = property.FindPropertyRelative("_scenePath");
            SerializedProperty propSceneAsset = property.FindPropertyRelative("_sceneAsset");

            // UnitySceneAsset value stored in SceneReference for holding direct reference
            UnitySceneAsset sceneAsset = propSceneAsset.objectReferenceValue as UnitySceneAsset;

            EditorGUI.BeginChangeCheck();

            // editor field for assigning UnitySceneAsset
            sceneAsset = EditorGUI.ObjectField(position, label,
                sceneAsset, typeof(UnitySceneAsset), false) as UnitySceneAsset;

            if (EditorGUI.EndChangeCheck())
            {
                propSceneAsset.objectReferenceValue = sceneAsset;
            }

            EditorGUI.EndProperty();
        }
    }
}