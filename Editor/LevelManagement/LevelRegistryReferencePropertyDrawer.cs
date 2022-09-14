using UnityEngine;
using UnityEditor;

namespace GameFramework.LevelManagement.Editor
{
    [CustomPropertyDrawer(typeof(LevelRegistryReference))]
    public class LevelRegistryReferenceEditor : PropertyDrawer
    {
        protected float _height;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            // store starting rect value, to calculate height later
            Rect positionStart = position;

            SerializedProperty propKey = property.FindPropertyRelative("_key");
            SerializedProperty propLevelGUID = property.FindPropertyRelative("_levelGUID");
            SerializedProperty propIsKeyExplicit = property.FindPropertyRelative("_isKeyExplicit");

            LevelAsset prevLevel = AssetDatabase.LoadAssetAtPath<LevelAsset>(
                AssetDatabase.GUIDToAssetPath(propLevelGUID.stringValue));
            LevelAsset newLevel = prevLevel;

            string prevKey = propKey.stringValue;
            string newKey = prevKey;
            bool isKeyExplicit = propIsKeyExplicit.boolValue;

            if (isKeyExplicit)
            {
                // change label to denote that key is explicit
                label.text = $"{label.text} (ExplicitKey)";
            }

            // this takes current values and returns new values
            InternalOnGUI(ref position, property, label,
                ref newLevel, ref newKey, ref isKeyExplicit);

            // assign modified properties
            propIsKeyExplicit.boolValue = isKeyExplicit;
            propKey.stringValue = newKey;

            // we check this to avoid searching for asset in database every update frame
            if (prevLevel != newLevel)
            {
                // assign levelAsset to property
                propLevelGUID.stringValue = AssetDatabase.AssetPathToGUID(
                    AssetDatabase.GetAssetPath(newLevel));
            }

            // calculate height based on initial and final positions
            _height = position.y - positionStart.y + position.height;
        }

        protected void InternalOnGUI(ref Rect position, SerializedProperty property, GUIContent label,
            ref LevelAsset newLevel, ref string newKey, ref bool isKeyExplicit)
        {
            const float SEPARATOR = 4f;

            LevelRegistry registry = LevelManager.Registry;
            LevelAsset prevLevel = newLevel;
            string prevKey = newKey;

            // registry = null;

            // object field for level asset
            newLevel = EditorGUI.ObjectField(
                new Rect(position.x, position.y, position.width - 22f, position.height),
                label, prevLevel, typeof(LevelAsset), false) as LevelAsset;

            // foldout for advanced options
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x + position.width - 4f, position.y, 20f, position.height),
                property.isExpanded, GUIContent.none);

            if (property.isExpanded)
            {
                // text field for level key
                position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                newKey = EditorGUI.TextField(position, "Level Key", prevKey);
            }

            // if user assigned LevelAsset explicitly, so key is not explicit anymore
            isKeyExplicit = newLevel == prevLevel ? isKeyExplicit : false;

            // if user assigned the key explicitly, LevelAsset will be updated base on the key now
            isKeyExplicit = newKey == prevKey ? isKeyExplicit : true;

            if (registry is null)
            {
                if (isKeyExplicit)
                {
                    newLevel = null;
                }
                else
                {
                    newKey = "";
                }

                if (property.isExpanded)
                {
                    // if LevelRegistry is null, we cannot do much
                    position.y += position.height + SEPARATOR;
                    EditorGUI.HelpBox(position, "LevelRegistry is NULL", MessageType.Warning);
                }
            }
            else
            {
                if (isKeyExplicit)
                {
                    registry.GetLevel(newKey, out newLevel);
                }
                else
                {
                    registry.GetLevel(newKey, out LevelAsset levelFromKey);
                    bool isThisKeyValid = levelFromKey == newLevel;

                    if (isThisKeyValid is false)
                    {
                        registry.GetKey(newLevel, out newKey);
                    }
                }

                if (property.isExpanded)
                {
                    registry.GetAllKeys(newLevel, out string[] levelKeys);

                    // present user with alternative option for keys
                    foreach (var key in levelKeys)
                    {
                        const float buttonWidth = 150f;
                        if (key == newKey)
                        {
                            continue;
                        }

                        position.y += position.height;

                        GUI.enabled = false;
                        EditorGUI.TextField(
                            new Rect(position.x, position.y, position.width - buttonWidth - 2f, position.height),
                            key, GUIStyle.none);
                        GUI.enabled = true;

                        // button to assign the this key
                        // NOTE: this will not set key to explicit
                        bool pressed = GUI.Button(
                            new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height),
                            "Use this key");

                        if (pressed)
                        {
                            // newKey = key;
                        }
                    }

                    if (isKeyExplicit)
                    {
                        if (newLevel is null)
                        {
                            position.y += position.height + SEPARATOR;
                            EditorGUI.HelpBox(position, $"No LevelAsset for key[{newKey}] found in LevelRegistry", MessageType.Warning);
                        }
                    }
                    else
                    {
                        // check if there are no keys registered for LevelAsset
                        if (newLevel is not null && levelKeys.Length is 0)
                        {
                            position.y += position.height + SEPARATOR;
                            EditorGUI.HelpBox(position, $"Level [{newLevel.name}] not found in LevelRegistry", MessageType.Warning);

                            position.y += position.height + SEPARATOR;
                            if (GUI.Button(position, "Add to Registry"))
                            {
                                registry.Register(newLevel, out newKey);
                            }
                        }
                    }

                    // option to update registry
                    position.y += position.height + SEPARATOR;
                    if (GUI.Button(position, "Update Registry"))
                    {
                        registry.UpdateRegistry();
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // _height is updated in OnGUI()
            return _height;
        }
    }
}