using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

using UnitySceneAsset = UnityEditor.SceneAsset;

namespace GameFramework
{
    [CustomPropertyDrawer(typeof(SceneAsset))]
    public class SceneAssetPropertyDrawer : PropertyDrawer
    {
        protected const float XBoxPadSize = 4f;
        protected const float YBoxPadSize = 4f;
        protected const float XSeparatorSize = 2f;
        protected const float YSeparatorSize = 2f;

        protected static readonly float LineHeight = EditorGUIUtility.singleLineHeight + 1f;
        protected static readonly GUIContent BoxGUIContent = GUIContent.none;
        protected static readonly GUIStyle BoxGUIStyle = EditorStyles.helpBox;
        protected static readonly GUIStyle ButtonGUIStyle = EditorStyles.miniButton;

        protected float finalHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = LineHeight;
            position.width -= XBoxPadSize;
            Rect startPosition = position;

            EditorGUI.BeginProperty(position, label, property);

            // serialized object and properties
            SerializedProperty propSceneAsset = property.FindPropertyRelative("_sceneAsset");

            // UnitySceneAsset value stored in SceneReference for holding direct reference
            UnitySceneAsset sceneAsset = propSceneAsset.objectReferenceValue as UnitySceneAsset;

            // create background box
            Rect boxRect = new Rect
            {
                x = position.x - XBoxPadSize,
                y = position.y - YBoxPadSize,
                width = position.width + XBoxPadSize + XBoxPadSize, // 2 pads for left and right

                height = finalHeight + YBoxPadSize + YBoxPadSize    // // 2 pads for up and down
                // [height] is based on [_height] which is calculated later,
                // this creates an lagging effect in th UI, 
                // where property background box is expanded one frame after clicking
                // 
                // this approach is necessary to keep a lose connection between
                // expanded property options and background box
            };

            GUI.Box(boxRect, BoxGUIContent, BoxGUIStyle);

            // scene asset field
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                // editor field for assigning UnitySceneAsset
                sceneAsset = EditorGUI.ObjectField(
                    // -30f to leave space for advanced options foldout
                    new Rect(position.x, position.y, position.width - 30f, position.height),
                    label, sceneAsset, typeof(UnitySceneAsset), false) as UnitySceneAsset;

                if (scope.changed)
                {
                    propSceneAsset.objectReferenceValue = sceneAsset;
                }
            }

            // advanced options foldout
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.width + 10f, position.y, 20f, position.height),
                property.isExpanded, GUIContent.none);

            if (property.isExpanded)
            {
                ShowInfo(ref position, property, label);
            }

            EditorGUI.EndProperty();

            finalHeight = position.y - startPosition.y + position.height;
        }

        protected virtual void ShowInfo(ref Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedObject serializedObject = property.serializedObject;
            SerializedProperty propScenePath = property.FindPropertyRelative("_scenePath");
            SerializedProperty propSceneAsset = property.FindPropertyRelative("_sceneAsset");
            UnitySceneAsset sceneAsset = propSceneAsset.objectReferenceValue as UnitySceneAsset;

            // scene path for assigned scene asset
            position.y += position.height + YSeparatorSize;
            using (new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUI.TextField(position, "Scene Path", propScenePath.stringValue, EditorStyles.textField);
            }

            // values to provide scene status
            bool isInBuild = false;
            bool isEnabledInBuild = true;

            // update scene status properties
            var editorScene = GetEditorScene(sceneAsset);
            if (editorScene is not null)
            {
                isInBuild = true;
                isEnabledInBuild = editorScene.enabled;
            }

            position.width = position.width * 0.612f;
            position.x = position.x + 160f;

            // button to add scene to build settings
            // only available when scene is not in build settings
            using (new EditorGUI.DisabledGroupScope(sceneAsset is null || isInBuild))
            {
                position.y += position.height + YSeparatorSize;
                GUIContent content = new GUIContent
                {
                    text = "Add to Build",
                    tooltip = "Adds the scene to build settings with build index"
                };

                if (GUI.Button(position, content, ButtonGUIStyle))
                {
                    AddSceneToBuildSettings(sceneAsset, true);
                }
            }

            // button to enable scene in build settings
            // only available when scene is in build settings,
            // but not enabled
            // using (new EditorGUI.DisabledGroupScope(isInBuild is false && isEnabledInBuild))
            using (new EditorGUI.DisabledGroupScope(sceneAsset is null || isInBuild is false || isEnabledInBuild))
            {
                position.y += position.height + YSeparatorSize;
                GUIContent content = new GUIContent
                {
                    text = "Enable in Build",
                    tooltip = "Enables the scene in build settings"
                };

                if (GUI.Button(position, content, ButtonGUIStyle))
                {
                    EnableSceneInBuildSettings(sceneAsset);
                }
            }

            // button to open build settings
            // always available
            using (new EditorGUI.DisabledGroupScope(false))
            {
                position.y += position.height + YSeparatorSize;
                GUIContent content = new GUIContent
                {
                    text = "Build Settings",
                    tooltip = "Opens build settings"
                };

                if (GUI.Button(position, content, ButtonGUIStyle))
                {
                    EditorWindow.GetWindow(typeof(BuildPlayerWindow));
                }
            }
        }

        protected EditorBuildSettingsScene GetEditorScene(UnitySceneAsset sceneAsset)
        {
            if (sceneAsset is not null)
            {
                GUID sceneGUID = AssetDatabase.GUIDFromAssetPath(
                    AssetDatabase.GetAssetPath(sceneAsset));

                foreach (var scene in EditorBuildSettings.scenes)
                {
                    if (scene.guid == sceneGUID)
                    {
                        return scene;
                    }
                }
            }

            return null;
        }

        protected void AddSceneToBuildSettings(UnitySceneAsset sceneAsset, bool enabled = true)
        {
            GUID guid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(sceneAsset));

            EditorBuildSettingsScene[] editorScenes = EditorBuildSettings.scenes;
            ArrayUtility.Add(ref editorScenes, new EditorBuildSettingsScene(guid, enabled));
            EditorBuildSettings.scenes = editorScenes;
        }

        protected bool EnableSceneInBuildSettings(UnitySceneAsset sceneAsset, bool enable = true)
        {
            if (sceneAsset is not null)
            {
                GUID guid = AssetDatabase.GUIDFromAssetPath(
                    AssetDatabase.GetAssetPath(sceneAsset));

                var scenes = EditorBuildSettings.scenes;
                foreach (var scene in scenes)
                {
                    if (scene.guid == guid)
                    {
                        scene.enabled = enable;

                        EditorBuildSettings.scenes = scenes;
                        return true;
                    }
                }
            }

            return false;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return finalHeight;
        }
    }
}