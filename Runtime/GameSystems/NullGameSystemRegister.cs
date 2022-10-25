using SystemType = System.Type;

namespace GameFramework
{
    public sealed class NullGameSystemRegister : GameSystemRegister
    {
        public static readonly GameSystemRegister Instance = new NullGameSystemRegister();

        private NullGameSystemRegister() { }

        public override bool RegisterSystem(GameSystem system, SystemType type, int? priority, bool force = false)
        {
            if (typeValidator.Validate(type) is false)
            {
                // TODO: throw error
                return false;
            }

            return false;
        }
    }
}