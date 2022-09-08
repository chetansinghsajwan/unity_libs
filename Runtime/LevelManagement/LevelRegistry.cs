using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public interface IKeyResolver<TKey, TValue>
    {
        TKey GetKey(TValue value);
    }

    public interface ILevelKeyResolver : IKeyResolver<string, LevelAsset> { }

    public class LevelRegistry
    {
        public static LevelRegistry Instance;

        public struct DefaultKeyResolver : ILevelKeyResolver
        {
            public string GetKey(LevelAsset value)
            {
                if (value is not null)
                {
                    return value.name;
                }

                return null;
            }
        }

        public LevelRegistry()
        {
            _levels = new Dictionary<string, LevelAsset>();
            _keyResolver = new DefaultKeyResolver();
        }

        public virtual void UpdateRegistry()
        {
            Register(Resources.LoadAll<LevelAsset>(""));
        }

        public bool GetLevel(string name, out LevelAsset level)
        {
            return _levels.TryGetValue(name, out level);
        }

        public bool HasLevel(LevelAsset level)
        {
            return _levels.ContainsValue(level);
        }

        public bool HasKey(string name)
        {
            return _levels.ContainsKey(name);
        }

        public bool GetKey(LevelAsset level, out string key)
        {
            foreach (var pair in _levels)
            {
                if (pair.Value == level)
                {
                    key = pair.Key;
                    return true;
                }
            }

            key = "";
            return false;
        }

        public void GetAllKeys(LevelAsset level, out string[] keys)
        {
            List<string> keyList = new List<string>(5);
            foreach (var pair in _levels)
            {
                if (pair.Value == level)
                {
                    keyList.Add(pair.Key);
                }
            }

            keys = keyList.ToArray();
        }

        public bool Register(IReadOnlyCollection<LevelAsset> levels)
        {
            _levels.EnsureCapacity(levels.Count);
            foreach (var level in levels)
            {
                _levels.TryAdd(_keyResolver.GetKey(level), level);
            }

            return true;
        }

        public bool Register(IEnumerable<LevelAsset> levels)
        {
            foreach (var level in levels)
            {
                _levels.TryAdd(_keyResolver.GetKey(level), level);
            }

            return true;
        }

        public bool Register(LevelAsset level, out string name)
        {
            name = _keyResolver.GetKey(level);
            return Register(level, name);
        }

        public bool Register(LevelAsset level, string name)
        {
            return _levels.TryAdd(name, level);
        }

        public bool Update(LevelAsset level, string name)
        {
            return _levels[name] = level;
        }

        public bool Unregister(string name)
        {
            return _levels.Remove(name);
        }

        public bool Unregister(LevelAsset level)
        {
            foreach (var pair in _levels)
            {
                if (pair.Value == level)
                {
                    _levels.Remove(pair.Key);
                    return true;
                }
            }

            return false;
        }

        public void UnregisterAll(LevelAsset level)
        {
            while (Unregister(level) is true) { }
        }

        protected Dictionary<string, LevelAsset> _levels;
        public virtual IReadOnlyCollection<LevelAsset> Levels
        {
            get => _levels.Values;
        }

        protected ILevelKeyResolver _keyResolver;
        public virtual ILevelKeyResolver KeyResolver
        {
            get => _keyResolver;
            set => _keyResolver = value;
        }
    }
}