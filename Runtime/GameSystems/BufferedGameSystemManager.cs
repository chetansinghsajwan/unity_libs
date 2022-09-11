using System;
using System.Collections;
using System.Collections.Generic;

using SystemType = System.Type;

namespace GameFramework
{
    public class BufferedGameSystemManager : GameSystemManager
    {
        public BufferedGameSystemManager()
        {
            _systemsBuffer = new SortedList<NodeKey, GameSystem>(keyComparer);
            InternalWriteChanges();
        }

        public void WriteChanges()
        {
            if (_hasChanges)
            {
                _hasChanges = false;
                InternalWriteChanges();
            }
        }

        public override bool RegisterSystem(GameSystem system, SystemType type, int? priority, bool force = false)
        {
            if (base.RegisterSystem(system, type, priority, force))
            {
                _hasChanges = true;
                return true;
            }

            return false;
        }

        public override bool UnregisterSystem(SystemType type, out GameSystem system)
        {
            if (base.UnregisterSystem(type, out system))
            {
                _hasChanges = true;
                return true;
            }

            return false;
        }

        public override IEnumerator<GameSystem> GetEnumerator()
        {
            return _systemsBuffer.Values.GetEnumerator();
        }

        protected virtual void InternalWriteChanges()
        {
            _systemsBuffer.Clear();

            if (_systems is not null)
            {
                _systemsBuffer.Capacity = _systems.Count;
                foreach (var item in _systems)
                {
                    _systemsBuffer.Add(item.Key, item.Value);
                }
            }
        }

        protected SortedList<NodeKey, GameSystem> _systemsBuffer;
        protected bool _hasChanges;
    }
}