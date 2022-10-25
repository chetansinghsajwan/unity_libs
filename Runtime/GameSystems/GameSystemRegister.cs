using System;
using System.Collections;
using System.Collections.Generic;

using SystemType = System.Type;

namespace GameFramework
{
    public class GameSystemRegister : IReadOnlyCollection<GameSystem>
    {
        public interface IPriorityResolver
        {
            int ResolvePriority(GameSystem system, SystemType type);
        }

        public interface ISystemTypeValidator
        {
            bool Validate(SystemType type);
        }

        protected readonly struct NodeKey
        {
            public readonly int priority;
            public readonly SystemType type;

            public NodeKey(SystemType type, int priority = 0)
            {
                this.type = type;
                this.priority = priority;
            }
        }

        protected readonly struct Comparer : IComparer<NodeKey>
        {
            public int Compare(NodeKey x, NodeKey y)
            {
                if (x.type == y.type) return 0;

                // we only return equal, when types are sames
                if (x.priority == y.priority) return -1;

                return x.priority.CompareTo(y.priority);
            }
        }

        protected readonly struct PriorityResolver : IPriorityResolver
        {
            public int ResolvePriority(GameSystem system, SystemType type)
            {
                return 0;
            }
        }

        protected readonly struct TypeValidator : ISystemTypeValidator
        {
            public bool Validate(SystemType type)
            {
                // allow systems to be registered for null TypeObjects,
                // to allow anonymous systems
                if (type is null) return true;

                // allow systems to be registered only for types deriving
                // from GameSystem and not even GameSystem itself
                // to promote structured workflow
                return type.IsSubclassOf(typeof(GameSystem));
            }
        }

        public GameSystemRegister()
        {
            typeValidator = new TypeValidator();
            priorityResolver = new PriorityResolver();
            mKeyComparer = new Comparer();

            mSystems = new SortedList<NodeKey, GameSystem>(mKeyComparer);
        }

        public bool RegisterSystem(GameSystem system)
        {
            return RegisterSystem(system, null as SystemType);
        }

        public bool RegisterSystem(GameSystem system, int priority)
        {
            return RegisterSystem(system, null as SystemType, priority);
        }

        public bool RegisterSystem<T>(GameSystem system, bool force = false)
        {
            return RegisterSystem(system, typeof(T), null, force);
        }

        public bool RegisterSystem<T>(GameSystem system, int? priority, bool force = false)
        {
            return RegisterSystem(system, typeof(T), priority, force);
        }

        public bool RegisterSystem(GameSystem system, SystemType type, bool force = false)
        {
            return RegisterSystem(system, type, null, force);
        }

        public virtual bool RegisterSystem(GameSystem system, SystemType type, int? priority, bool force = false)
        {
            if (typeValidator.Validate(type) is false)
            {
                return false;
            }

            if (priority.HasValue is false)
            {
                priority = priorityResolver.ResolvePriority(system, type);
            }

            // cache previous system before registering new one
            // to generate events
            GameSystem prevSystem = null;
            if (type is not null)
            {
                mSystems.TryGetValue(new NodeKey(type, 0), out prevSystem);
            }

            if (prevSystem is null || force)
            {
                mSystems[new NodeKey(type, priority.Value)] = system;

                if (prevSystem is not null)
                {
                    OnSubSystemRegistered?.Invoke(prevSystem, type);
                }

                OnSubSystemUnregistered?.Invoke(system, type);
                return true;
            }

            return false;
        }

        public bool UnregisterSystem<T>()
        {
            return UnregisterSystem(typeof(T));
        }

        public bool UnregisterSystem<T>(out GameSystem system)
        {
            return UnregisterSystem(typeof(T), out system);
        }

        public bool UnregisterSystem(SystemType type)
        {
            return UnregisterSystem(type, out _);
        }

        public virtual bool UnregisterSystem(SystemType type, out GameSystem system)
        {
            if (typeValidator.Validate(type) is false)
            {
                system = null;
                return false;
            }

            bool unregistered = mSystems.Remove(new NodeKey(type, 0), out system);
            if (system is not null)
            {
                OnSubSystemUnregistered?.Invoke(system, type);
            }

            return unregistered;
        }

        public GameSystem GetSystem<T>()
        {
            return GetSystem(typeof(T));
        }

        public virtual GameSystem GetSystem(SystemType type)
        {
            if (typeValidator.Validate(type) is false)
            {
                return null;
            }

            mSystems.TryGetValue(new NodeKey(type, 0), out GameSystem system);
            return system;
        }

        public virtual T GetSystemAs<T>() where T : GameSystem
        {
            foreach (var system in mSystems.Values)
            {
                if (system is T value)
                {
                    return value;
                }
            }

            return null;
        }

        public bool HasSystem<T>()
        {
            return GetSystem<T>() is not null;
        }

        public bool HasSystem(SystemType type)
        {
            return GetSystem(type) is not null;
        }

        public virtual IEnumerator<GameSystem> GetEnumerator()
        {
            return mSystems.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public readonly IPriorityResolver priorityResolver;
        public readonly ISystemTypeValidator typeValidator;
        protected readonly IComparer<NodeKey> mKeyComparer;

        public event Action<GameSystem, SystemType> OnSubSystemRegistered;
        public event Action<GameSystem, SystemType> OnSubSystemUnregistered;

        public int Count => mSystems.Count;

        protected IDictionary<NodeKey, GameSystem> mSystems;
    }
}