using System;

namespace GameFramework
{
    public static class GameInstanceFactory
    {
        public static Func<GameInstance> Create = DefaultFactory;

        public static GameInstance DefaultFactory()
        {
            return new GameInstance();
        }
    }
}