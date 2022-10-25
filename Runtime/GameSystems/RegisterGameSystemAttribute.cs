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
            this.force = false;
        }

        public readonly SystemType type;
        public bool force;
    }
}