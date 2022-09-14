using System;

namespace GameFramework.LevelManagement
{
    public sealed class NullLevelManagerSystem : LevelManagerSystem
    {
        public static readonly NullLevelManagerSystem Instance = new NullLevelManagerSystem();

        private NullLevelManagerSystem() { }

        public override LevelAsyncOperation LoadLevelAsync(LevelAsset level)
        {
            throw new NullReferenceException("NullLevelManagerSystem: LevelManagerSystem is null");
        }
    }
}