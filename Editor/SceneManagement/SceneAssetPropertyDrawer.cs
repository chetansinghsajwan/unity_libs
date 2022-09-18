using UnityEditor;
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
        protected const float FieldValueFactor = 0.612f;

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

            using (new EditorGUI.PropertyScope(position, label, property))
            {
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

                    height = finalHeight + YBoxPadSize + YBoxPadSize    // 2 pads for up and down
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

                // shows a indicator to represent state of scene asset
                ShowIndicator(ref position, property, sceneAsset);

                // advanced options foldout
                property.isExpanded = EditorGUI.Foldout(
                    new Rect(position.width + 10f, position.y, 20f, position.height),
                    property.isExpanded, GUIContent.none);

                if (property.isExpanded)
                {
                    ShowInfo(ref position, property, label);
                }
            }

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

            // we display buttons only to field value side,
            // so set width accordingly
            position.x += position.width * (1f - FieldValueFactor);
            position.width = position.width * FieldValueFactor;

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

        protected virtual void ShowIndicator(ref Rect position, SerializedProperty property, UnitySceneAsset sceneAsset)
        {
            // referenced from: https://github.com/r2d2meuleu/unity-scene-reference.git

            // values to provide scene status
            bool isInBuild = false;
            bool isEnabledInBuild = true;

            var editorScene = GetEditorScene(sceneAsset);
            if (editorScene is not null)
            {
                isInBuild = true;
                isEnabledInBuild = editorScene.enabled;
            }

            GUIContent content = null;

            // not in build scenes
            if (isInBuild is false)
            {
                content = EditorGUIUtility.IconContent("d_winbtn_mac_close");
                content.tooltip = "This scene is NOT in build settings.\nIt will be NOT included in builds.";
            }
            // in build scenes but disabled
            else if (isEnabledInBuild is false)
            {
                content = EditorGUIUtility.IconContent("d_winbtn_mac_min");
                content.tooltip = @"This scene is in build settings but DISABLED.
                                        It will be NOT included in builds.";
            }
            // in build scenes and enabled
            else
            {
                content = EditorGUIUtility.IconContent("d_winbtn_mac_max");
                content.tooltip = @"This scene is in build settings and ENABLED.
                                        It will be included in builds.";
            }

            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            Rect indicatorPosition = position;
            indicatorPosition.x = position.width - 5f;

            EditorGUI.PrefixLabel(indicatorPosition, controlId, content);
        }

        protected static EditorBuildSettingsScene GetEditorScene(UnitySceneAsset sceneAsset)
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

        protected static void AddSceneToBuildSettings(UnitySceneAsset sceneAsset, bool enabled = true)
        {
            if (sceneAsset is not null)
            {
                GUID guid = AssetDatabase.GUIDFromAssetPath(
                    AssetDatabase.GetAssetPath(sceneAsset));

                EditorBuildSettingsScene[] editorScenes = EditorBuildSettings.scenes;
                ArrayUtility.Add(ref editorScenes, new EditorBuildSettingsScene(guid, enabled));
                EditorBuildSettings.scenes = editorScenes;
            }
        }

        protected static bool EnableSceneInBuildSettings(UnitySceneAsset sceneAsset, bool enable = true)
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
            // final height is updated in OnGUI()
            // to keep editor dynamically and easily extensible
            return finalHeight;
        }
    }
}