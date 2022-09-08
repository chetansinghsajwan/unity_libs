using System;

namespace GameFramework
{
    public static class LevelRegistryFactory
    {
        private static Func<LevelRegistry> _factory = DefaultFactory;

        public static LevelRegistry Create()
        {
            return _factory();
        }

        public static void SetFactory(Func<LevelRegistry> factory)
        {
            _factory = factory;
        }

        public static LevelRegistry DefaultFactory()
        {
            return new LevelRegistry();
        }
    }
}