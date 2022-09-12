using System.Reflection;
using UnityEngine;
using GameFramework.LogManagement;

namespace GameFramework
{
    public static class GameInstance
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            RootSystem = new UnityRootGameSystem();

            if (LogManager.Instance is null)
            {
                LogManager.Instance = LogManagerFactory.Create();
                LogManager.Instance.Init();
            }

            if (SceneManager.Impl is null)
            {
                SceneManager.Impl = SceneManagerFactory.Create();
            }
        }

        public static void Shutdown()
        {
        }

        public static GameSystem RootSystem;
    }
}