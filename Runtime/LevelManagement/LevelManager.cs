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
            add => _system.BeforeLevelLoad += value;
            remove => _system.BeforeLevelLoad -= value;
        }

        public static event Action<LevelAsset> AfterLevelLoad
        {
            add => _system.AfterLevelLoad += value;
            remove => _system.AfterLevelLoad -= value;
        }

        public static event Action<LevelAsset> BeforeLevelUnload
        {
            add => _system.BeforeLevelUnload += value;
            remove => _system.BeforeLevelUnload -= value;
        }

        public static event Action<LevelAsset> AfterLevelUnload
        {
            add => _system.AfterLevelUnload += value;
            remove => _system.AfterLevelUnload -= value;
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