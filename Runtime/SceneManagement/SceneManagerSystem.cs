using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityLoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;
using UnityAsyncOperation = UnityEngine.AsyncOperation;

namespace GameFramework
{
    [RegisterGameSystem(typeof(SceneManagerSystem))]
    public class SceneManagerSystem : GameSystem
    {
        protected override void OnRegistered(GameSystem system)
        {
            base.OnRegistered(system);

            SceneManager.System = this;
        }

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

        public virtual void LoadScene(SceneAsset scene, LoadSceneMode mode)
        {
            AssertSceneIsNull(scene);

            BeforeSceneLoad?.Invoke(scene, mode);
            UnitySceneManager.LoadScene(scene.scenePath, (UnityLoadSceneMode)mode);
            AfterSceneLoad?.Invoke(scene, mode);
        }

        public virtual SceneAsyncOperation LoadSceneAsync(SceneAsset scene, LoadSceneMode mode)
        {
            AssertSceneIsNull(scene);

            BeforeSceneLoad?.Invoke(scene, mode);
            UnityAsyncOperation asyncOperation = UnitySceneManager
                .LoadSceneAsync(scene.scenePath, (UnityLoadSceneMode)mode);

            asyncOperation.completed += (AsyncOperation) =>
            {
                AfterSceneLoad?.Invoke(scene, mode);
            };

            return new SceneAsyncOperation(asyncOperation);
        }

        [Obsolete("Use SceneManager.UnloadSceneAsync. This function is not safe to use during triggers and under other circumstances. See Scripting reference for more details.")]
        public virtual void UnloadScene(SceneAsset scene)
        {
            AssertSceneIsNull(scene);

            if (BeforeSceneUnload is not null)
            {
                BeforeSceneUnload(scene);
            }

            UnitySceneManager.UnloadScene(scene.scenePath);

            if (AfterSceneUnload is not null)
            {
                AfterSceneUnload(scene);
            }
        }

        public virtual SceneAsyncOperation UnloadSceneAsync(SceneAsset scene)
        {
            AssertSceneIsNull(scene);

            BeforeSceneUnload?.Invoke(scene);

            UnityAsyncOperation asyncOperation = UnitySceneManager
                .UnloadSceneAsync(scene.scenePath);

            asyncOperation.completed += (AsyncOperation) =>
            {
                AfterSceneUnload?.Invoke(scene);
            };

            return new SceneAsyncOperation(asyncOperation);
        }

        protected virtual void AssertSceneIsNull(SceneAsset scene)
        {
            if (String.IsNullOrEmpty(scene.scenePath))
            {
                throw new NullReferenceException("cannot load null scene");
            }
        }

        public event Action<SceneAsset, LoadSceneMode> BeforeSceneLoad;
        public event Action<SceneAsset, LoadSceneMode> AfterSceneLoad;
        public event Action<SceneAsset> BeforeSceneUnload;
        public event Action<SceneAsset> AfterSceneUnload;
    }
}