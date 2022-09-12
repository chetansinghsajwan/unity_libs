using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public sealed class NullLevelRegistry : LevelRegistry
    {
        public static readonly NullLevelRegistry Instance = new NullLevelRegistry();

        new private struct KeyResolver : ILevelKeyResolver
        {
            public string ResolveKey(LevelAsset value)
            {
                return "";
            }
        }

        private NullLevelRegistry()
        {
            _levels = new Dictionary<string, LevelAsset>();
            _keyResolver = new KeyResolver();
        }

        public override void UpdateRegistry() { }

        public override bool Register(LevelAsset level, string name)
        {
            return false;
        }
    }
}