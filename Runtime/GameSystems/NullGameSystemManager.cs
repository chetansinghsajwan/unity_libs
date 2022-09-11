using SystemType = System.Type;

namespace GameFramework
{
    public sealed class NullGameSystemManager : GameSystemManager
    {
        public static readonly GameSystemManager Instance = new NullGameSystemManager();

        private NullGameSystemManager() { }

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