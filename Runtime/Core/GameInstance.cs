using UnityEngine;

namespace GameFramework
{
    public static class GameInstance
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            RootSystem = new UnityRootGameSystem();
        }

        public static void Shutdown()
        {
        }

        public static GameSystem RootSystem;
    }
}