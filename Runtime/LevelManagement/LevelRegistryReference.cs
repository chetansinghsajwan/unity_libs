using System;
using UnityEngine;

namespace GameFramework
{
    [Serializable]
    public struct LevelRegistryReference
    {
        public LevelAsset Get()
        {
            if (_level is null)
            {
                Update();
            }

            return _level;
        }

        public LevelAsset Update()
        {
            _registry = LevelManager.Registry;
            _level = null;

            if (_registry is not null)
            {
                _registry.GetLevel(_key, out _level);
            }

            return _level;
        }

        [SerializeField]
        private string _key;
        private LevelRegistry _registry;
        private LevelAsset _level;

#if UNITY_EDITOR
        [SerializeField] private bool _isKeyExplicit;
        [SerializeField] private string _levelGUID;
#endif
    }
}