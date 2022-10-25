using System;
using SystemType = System.Type;

namespace GameFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RegisterGameSystemAttribute : Attribute
    {
        public RegisterGameSystemAttribute(SystemType type)
        {
            this.type = type;
            this.parent = null;
            this.force = false;
            this._priority = 0;
            this._hasPriority = false;
        }

        public SystemType type;
        public SystemType parent;
        public bool force;
        public int priority
        {
            get => _priority;
            set
            {
                _hasPriority = true;
                _priority = value;
            }
        }
        public bool hasPriority => _hasPriority;

        protected int _priority;
        protected bool _hasPriority;
    }
}