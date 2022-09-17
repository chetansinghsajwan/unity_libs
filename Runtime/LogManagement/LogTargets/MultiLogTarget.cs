using System.Collections.Generic;

namespace GameFramework.Logging
{
    public class MultiLogTarget : ILogTarget
    {
        public MultiLogTarget(IEnumerable<ILogTarget> targets)
        {
            this._targets = new LinkedList<ILogTarget>(targets);
        }

        public void Log(LogEvent logEvent)
        {
            foreach (var target in _targets)
            {
                if (target is not null)
                {
                    target.Log(logEvent);
                }
            }
        }

        public void Flush()
        {
            foreach (var target in _targets)
            {
                if (target is not null)
                {
                    target.Flush();
                }
            }
        }

        protected IEnumerable<ILogTarget> _targets;
        public IEnumerable<ILogTarget> targets
        {
            get => _targets;
            set
            {
                value ??= new ILogTarget[0];
                _targets = value;
            }
        }
    }
}