using System;

namespace GameFramework
{
    public static class SceneManagerFactory
    {
        public static Func<SceneManager> Create = DefaultFactory;

        public static SceneManager DefaultFactory()
        {
            return new SceneManager();
        }
    }
}