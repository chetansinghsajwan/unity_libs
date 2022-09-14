using UnityEngine;

namespace GameFramework.LevelManagement
{
    [CreateAssetMenu(menuName = MENU_PATH + "Basic LevelAsset", fileName = "Basic LevelAsset")]
    public class BasicLevelAsset : LevelAsset
    {
        public override LevelAsyncOperation PerformLoad()
        {
            LevelAsyncOperation operation = new LevelAsyncOperation();

            foreach (var scene in _scenes)
            {
                operation.AddOperation(SceneManager
                    .LoadSceneAsync(scene, LoadSceneMode.Additive), 1f);
            }

            return operation;
        }

        public override LevelAsyncOperation PerformUnload()
        {
            LevelAsyncOperation operation = new LevelAsyncOperation();

            foreach (var scene in _scenes)
            {
                operation.AddOperation(SceneManager
                    .UnloadSceneAsync(scene), 1f);
            }

            return operation;
        }

        [SerializeField]
        protected SceneAsset[] _scenes;
    }
}