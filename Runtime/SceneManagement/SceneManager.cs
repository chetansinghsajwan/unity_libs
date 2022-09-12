using System;
using UnityLoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;

namespace GameFramework
{
    // same as UnityEngine.SceneManagement.LoadSceneMode
    // to avoid including unity-namespace
    public enum LoadSceneMode
    {
        Single = 0,
        Additive = 1
    }

    public static class SceneManager
    {
        public static SceneManagerSystem System;

        public static event Action<SceneAsset, LoadSceneMode> BeforeSceneLoad
        {
            add => System.BeforeSceneLoad += value;
            remove => System.BeforeSceneLoad -= value;
        }

        public static event Action<SceneAsset, LoadSceneMode> AfterSceneLoad
        {
            add => System.AfterSceneLoad += value;
            remove => System.AfterSceneLoad -= value;
        }

        public static event Action<SceneAsset> BeforeSceneUnload
        {
            add => System.BeforeSceneUnload += value;
            remove => System.BeforeSceneUnload -= value;
        }

        public static event Action<SceneAsset> AfterSceneUnload
        {
            add => System.AfterSceneUnload += value;
            remove => System.AfterSceneUnload -= value;
        }

        public const LoadSceneMode DEFAULT_LOAD_SCENE_MODE = LoadSceneMode.Additive;

        public static SceneAsyncOperation LoadSceneAsync(SceneAsset scene, UnityLoadSceneMode mode)
        {
            return LoadSceneAsync(scene, (LoadSceneMode)mode);
        }

        public static SceneAsyncOperation LoadSceneAsync(SceneAsset scene, LoadSceneMode mode = DEFAULT_LOAD_SCENE_MODE)
        {
            return System.LoadSceneAsync(scene, mode);
        }

        public static SceneAsyncOperation UnloadSceneAsync(SceneAsset scene)
        {
            return System.UnloadSceneAsync(scene);
        }
    }
}