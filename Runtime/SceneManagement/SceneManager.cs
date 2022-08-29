using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace GameFramework
{
    public static class SceneManager
    {
        public const LoadSceneMode DEFAULT_LOAD_SCENE_MODE = LoadSceneMode.Additive;

        public static void Load(SceneAsset scene, LoadSceneMode loadSceneMode = DEFAULT_LOAD_SCENE_MODE)
        {
            UnitySceneManager.LoadScene(scene.scenePath, loadSceneMode);
        }

        public static void LoadAsync(SceneAsset scene, LoadSceneMode loadSceneMode = DEFAULT_LOAD_SCENE_MODE)
        {
            UnitySceneManager.LoadSceneAsync(scene.scenePath, loadSceneMode);
        }

        public static SceneObject FindSceneObjectFor(SceneAsset scene)
        {
            string sceneName = Path.GetFileName(scene.scenePath);
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(SceneObject.GAME_OBJECT_TAG);
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.scene.name == sceneName)
                {
                    SceneObject sceneObject = gameObject.GetComponent<SceneObject>();
                    if (sceneObject is not null)
                    {
                        return sceneObject;
                    }
                }
            }

            return null;
        }
    }
}