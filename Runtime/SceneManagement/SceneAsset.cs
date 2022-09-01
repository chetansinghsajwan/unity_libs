using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    [CreateAssetMenu(menuName = MENU_PATH + MENU_NAME, fileName = FILE_NAME)]
    public class SceneAsset : ScriptableObject
    {
        public const string MENU_PATH = GameFrameworkConstants.MENU_PATH + "Scenes/";
        public const string MENU_NAME = "SceneAsset";
        public const string FILE_NAME = "SceneAsset";

        public SceneAsset()
        {
            _scenePath = string.Empty;
            _sceneObject = null;
        }

        public virtual void BeforeLoad(LoadSceneMode loadSceneMode)
        {
        }

        public virtual void AfterLoad(LoadSceneMode loadSceneMode)
        {
        }

        public virtual void BeforeUnload()
        {
        }

        public virtual void AfterUnload()
        {
        }

        public SceneObject GetSceneObject(bool find = false)
        {
            if (_sceneObject is null || find)
            {
                // _sceneObject = SceneManager.FindSceneObjectFor(this);
            }

            return _sceneObject;
        }

        [SerializeField]
        protected string _scenePath;
        public string scenePath
        {
            get => _scenePath;
        }

        protected SceneObject _sceneObject;
    }
}