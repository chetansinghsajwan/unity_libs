using System;
using SystemType = System.Type;

namespace GameFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class UpdateGameSystemBeforeAttribute : Attribute
    {
        public UpdateGameSystemBeforeAttribute(SystemType type)
        {
            this.type = type;
        }

        public readonly SystemType type;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class UpdateGameSystemAfterAttribute : Attribute
    {
        public UpdateGameSystemAfterAttribute(SystemType type)
        {
            this.type = type;
        }

        public readonly SystemType type;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class UpdateGameSystemAttribute : Attribute
    {
        public UpdateGameSystemAttribute(SystemType parent)
        {
            this.parent = parent;
        }

        public readonly SystemType parent;
    }
}