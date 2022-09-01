using System;

namespace GameFramework
{
    public class LevelManagerFactory
    {
        public static Func<LevelManager> Create = DefaultFactory;

        public static LevelManager DefaultFactory()
        {
            return new LevelManager();
        }
    }
}