using UnityAsyncOperation = UnityEngine.AsyncOperation;

namespace GameFramework
{
    public class SceneAsyncOperation : AsyncOperation
    {
        public SceneAsyncOperation(UnityAsyncOperation operation)
        {
            _operation = operation;
            _operation.completed += OnUnityAsyncOperationCompleted;
        }

        protected void OnUnityAsyncOperationCompleted(UnityAsyncOperation unityAsyncOperation)
        {
            SafeInvokeOnCompleted();
        }

        // Has the operation finished?
        public override bool isCompleted => _operation.isDone;

        // What's the operation's progress.
        public override float progress => _operation.progress;

        // Priority lets you tweak in which order async operation calls will be performed.
        public int priority
        {
            get => _operation.priority;
            set => _operation.priority = value;
        }

        // Allow Scenes to be activated as soon as it is ready.
        public bool allowSceneActivation
        {
            get => _operation.allowSceneActivation;
            set => _operation.allowSceneActivation = value;
        }

        // Underlying UnityEngine.AsyncOperation
        public UnityAsyncOperation unityAsyncOperation
        {
            get => _operation;
        }

        protected UnityAsyncOperation _operation;
    }
}