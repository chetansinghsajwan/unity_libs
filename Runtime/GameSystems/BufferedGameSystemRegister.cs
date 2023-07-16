using System.Collections.Generic;
using SystemType = System.Type;

namespace GameFramework
{
    public class BufferedGameSystemRegister : GameSystemRegister
    {
        public BufferedGameSystemRegister()
        {
            _systemsBuffer = new SortedList<NodeKey, GameSystem>(mKeyComparer);
            _WriteChanges();
        }

        public void WriteChanges()
        {
            if (_hasChanges)
            {
                _hasChanges = false;
                _WriteChanges();
            }
        }

        public override bool RegisterSystem(GameSystem system, SystemType type, int? priority,
            bool force = false)
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

        protected virtual void _WriteChanges()
        {
            _systemsBuffer.Clear();

            if (mSystems is not null)
            {
                _systemsBuffer.Capacity = mSystems.Count;
                foreach (var item in mSystems)
                {
                    _systemsBuffer.Add(item.Key, item.Value);
                }
            }
        }

        protected SortedList<NodeKey, GameSystem> _systemsBuffer;
        protected bool _hasChanges;
    }
}