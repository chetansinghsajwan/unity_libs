using System;

namespace GameFramework.LevelManagement
{
    public static class LevelManager
    {
        private static LevelManagerSystem _system = NullLevelManagerSystem.Instance;
        public static LevelManagerSystem System
        {
            get => _system;
            set
            {
                value ??= NullLevelManagerSystem.Instance;

                _system = value;
            }
        }

        private static LevelRegistry _registry;
        public static LevelRegistry Registry
        {
            get => _registry;
            set
            {
                value ??= NullLevelRegistry.Instance;

                _registry = value;
            }
        }

        public static event Action<LevelAsset> BeforeLevelLoad
        {
            add => _system.BeforeLevelLoadEvent += value;
            remove => _system.BeforeLevelLoadEvent -= value;
        }

        public static event Action<LevelAsset> AfterLevelLoad
        {
            add => _system.AfterLevelLoadEvent += value;
            remove => _system.AfterLevelLoadEvent -= value;
        }

        public static event Action<LevelAsset> BeforeLevelUnload
        {
            add => _system.BeforeLevelUnloadEvent += value;
            remove => _system.BeforeLevelUnloadEvent -= value;
        }

        public static event Action<LevelAsset> AfterLevelUnload
        {
            add => _system.AfterLevelUnloadEvent += value;
            remove => _system.AfterLevelUnloadEvent -= value;
        }

        public static LevelAsset ActiveLevel
        {
            get => _system.ActiveLevel;
        }

        public static LevelAsyncOperation LoadLevelAsync(LevelAsset level)
        {
            return _system.LoadLevelAsync(level);
        }

        public static LevelAsyncOperation UnloadLevelAsync()
        {
            return _system.UnloadLevelAsync();
        }

        public static LevelAsyncOperation LoadLevelAsyncFromRegistry(string levelKey)
        {
            _registry.GetLevel(levelKey, out LevelAsset level);
            return _system.LoadLevelAsync(level);
        }
    }
}