using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityLoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;
using UnityAsyncOperation = UnityEngine.AsyncOperation;

namespace GameFramework
{
    // same as UnityEngine.SceneManagement.LoadSceneMode
    // to avoid including unity-namespace
    public enum LoadSceneMode
    {
        Single = 0,
        Additive = 1
    }

    public class SceneManager
    {
        public static SceneManager Impl;

        public static event Action<SceneAsset, LoadSceneMode> BeforeSceneLoad;
        public static event Action<SceneAsset, LoadSceneMode> AfterSceneLoad;
        public static event Action<SceneAsset> BeforeSceneUnload;
        public static event Action<SceneAsset> AfterSceneUnload;

        public const LoadSceneMode DEFAULT_LOAD_SCENE_MODE = LoadSceneMode.Additive;

        public static SceneAsyncOperation LoadSceneAsync(SceneAsset scene, UnityLoadSceneMode mode)
        {
            return LoadSceneAsync(scene, (LoadSceneMode)mode);
        }

        public static SceneAsyncOperation LoadSceneAsync(SceneAsset scene, LoadSceneMode mode = DEFAULT_LOAD_SCENE_MODE)
        {
            return Impl.LoadSceneAsyncImpl(scene, mode);
        }

        public static SceneAsyncOperation UnloadSceneAsync(SceneAsset scene)
        {
            return Impl.UnloadSceneAsyncImpl(scene);
        }

        //////////////////////////////////////////////////////////////////

        public virtual SceneObject FindSceneObjectFor(SceneAsset scene)
        {
            string sceneName = Path.GetFileName(scene.scenePath);
            return GameObject.FindGameObjectsWithTag(SceneObject.GAME_OBJECT_TAG)
                .Select((GameObject gameObject) =>
                {
                    if (gameObject.scene.name == sceneName)
                    {
                        return gameObject.GetComponent<SceneObject>();
                    }

                    return null;
                })
                .Single((SceneObject sceneObject) => sceneObject is not null);
        }

        public virtual SceneObject[] FindSceneObjectsFor(SceneAsset scene)
        {
            string sceneName = Path.GetFileName(scene.scenePath);
            return GameObject.FindGameObjectsWithTag(SceneObject.GAME_OBJECT_TAG)
                .Select((GameObject gameObject) =>
                {
                    if (gameObject.scene.name == sceneName)
                    {
                        return gameObject.GetComponent<SceneObject>();
                    }

                    return null;
                })
                .ToArray();
        }

        public virtual void LoadSceneImpl(SceneAsset scene, LoadSceneMode mode)
        {
            AssertSceneIsNull(scene);

            BeforeSceneLoad?.Invoke(scene, mode);

            scene.BeforeLoad(mode);
            UnitySceneManager.LoadScene(scene.scenePath, (UnityLoadSceneMode)mode);
            scene.AfterLoad(mode);

            AfterSceneLoad?.Invoke(scene, mode);
        }

        public virtual SceneAsyncOperation LoadSceneAsyncImpl(SceneAsset scene, LoadSceneMode mode)
        {
            AssertSceneIsNull(scene);

            BeforeSceneLoad?.Invoke(scene, mode);

            scene.BeforeLoad(mode);
            UnityAsyncOperation asyncOperation = UnitySceneManager
                .LoadSceneAsync(scene.scenePath, (UnityLoadSceneMode)mode);

            asyncOperation.completed += (AsyncOperation) =>
            {
                scene.AfterLoad(mode);
                AfterSceneLoad?.Invoke(scene, mode);
            };

            return new SceneAsyncOperation(asyncOperation);
        }

        [Obsolete("Use SceneManager.UnloadSceneAsync. This function is not safe to use during triggers and under other circumstances. See Scripting reference for more details.")]
        public virtual void UnloadSceneImpl(SceneAsset scene)
        {
            AssertSceneIsNull(scene);

            if (BeforeSceneUnload is not null)
            {
                BeforeSceneUnload(scene);
            }

            scene.BeforeUnload();
            UnitySceneManager.UnloadScene(scene.scenePath);
            scene.AfterUnload();

            if (AfterSceneUnload is not null)
            {
                AfterSceneUnload(scene);
            }
        }

        public virtual SceneAsyncOperation UnloadSceneAsyncImpl(SceneAsset scene)
        {
            AssertSceneIsNull(scene);

            BeforeSceneUnload?.Invoke(scene);

            scene.BeforeUnload();
            UnityAsyncOperation asyncOperation = UnitySceneManager
                .UnloadSceneAsync(scene.scenePath);

            asyncOperation.completed += (AsyncOperation) =>
            {
                scene.AfterUnload();
                AfterSceneUnload?.Invoke(scene);
            };

            return new SceneAsyncOperation(asyncOperation);
        }

        protected virtual void AssertSceneIsNull(SceneAsset scene)
        {
            if (scene is null)
            {
                throw new NullReferenceException("cannot load null scene");
            }
        }
    }
}