using System;

namespace GameFramework
{
    public static class LevelManager
    {
        public static LevelManagerSystem System
        {
            get => _system;
            set
            {
                if (value is null)
                {
                    value = NullLevelManagerSystem.Instance;
                }

                _system = value;
            }
        }
        private static LevelManagerSystem _system = NullLevelManagerSystem.Instance;

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
    }
}