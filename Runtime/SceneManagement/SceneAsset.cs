using System;
using UnityEngine;

#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnitySceneAsset = UnityEditor.SceneAsset;

#endif

namespace GameFramework
{
    [Serializable]
    public struct SceneAsset : ISerializationCallbackReceiver
    {
        [SerializeField]
        private string _scenePath;
        public string scenePath => _scenePath;

        public static implicit operator string(SceneAsset sceneReference)
        {
            return sceneReference._scenePath;
        }

#if UNITY_EDITOR

        [SerializeField]
        private UnitySceneAsset _sceneAsset;

        public void OnBeforeSerialize()
        {
            string prevScenePath = _scenePath;
            _scenePath = "";

            if (_sceneAsset != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(_sceneAsset);
                if (String.IsNullOrEmpty(assetPath) is false)
                {
                    // to remove assets from starting
                    _scenePath = Path.ChangeExtension(assetPath, null).Substring(7);
                }
            }
        }

        public void OnAfterDeserialize() { }

#else

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() { }

#endif
    }
}