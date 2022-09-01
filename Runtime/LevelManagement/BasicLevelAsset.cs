using UnityEngine;

namespace GameFramework
{
    [CreateAssetMenu(menuName = MENU_PATH + "Basic LevelAsset", fileName = "Basic LevelAsset")]
    public class BasicLevelAsset : LevelAsset
    {
        public override LevelAsyncOperation PerformLoad()
        {
            LevelAsyncOperation operation = new LevelAsyncOperation();

            foreach (var scene in _scenes)
            {
                operation.AddOperation(SceneManager.Instance
                    .LoadSceneAsync(scene, LoadSceneMode.Additive), 1f);
                
            }

            return operation;
        }

        public override LevelAsyncOperation PerformUnload()
        {
            LevelAsyncOperation operation = new LevelAsyncOperation();

            foreach (var scene in _scenes)
            {
                operation.AddOperation(SceneManager.Instance
                    .UnloadSceneAsync(scene), 1f);
            }

            return operation;
        }

        [SerializeField]
        protected SceneAsset[] _scenes;
    }
}