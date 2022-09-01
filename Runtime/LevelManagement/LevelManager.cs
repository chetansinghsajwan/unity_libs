using System;
using UnityEngine;

namespace GameFramework
{
    public class LevelManager : ScriptableObject
    {
        public static LevelManager Instance;

        public virtual LevelAsyncOperation LoadLevelAsync(LevelAsset level)
        {
            if (level == _activeLevel)
            {
                return LevelAsyncOperation.Completed;
            }

            LevelAsyncOperation levelOperation = new LevelAsyncOperation();

            // operationSource to hold the completion of operation explicitly
            AsyncOperationSource operationSource = new AsyncOperationSource();
            levelOperation.AddOperation(operationSource);

            InternalLoadLevelAsync(levelOperation, operationSource, level);

            return levelOperation;
        }

        public virtual LevelAsyncOperation UnloadLevelAsync()
        {
            return LoadLevelAsync(null);
        }

        protected virtual async void InternalLoadLevelAsync(LevelAsyncOperation operation, AsyncOperationSource operationSource, LevelAsset level)
        {
            LevelAsset currentLevel = _activeLevel;

            if (level is not null)
            {
                BeforeLevelLoad?.Invoke(level);

                LevelAsyncOperation loadOp = level.PerformLoad();

                if (currentLevel is null)
                {
                    operation.AddOperation(loadOp, 1f);
                }
                else
                {
                    operation.AddOperation(loadOp, .9f);
                }

                Debug.Log("LevelManager: awaiting loadOp");
                await loadOp;
                Debug.Log("LevelManager: completed loadOp");

                _activeLevel = level;
                AfterLevelLoad?.Invoke(level);
            }

            if (currentLevel is not null)
            {
                BeforeLevelUnload?.Invoke(currentLevel);

                LevelAsyncOperation unloadOp = currentLevel.PerformUnload();
                operation.AddOperation(unloadOp, .1f);

                await unloadOp;

                AfterLevelUnload?.Invoke(currentLevel);
            }

            operationSource.SetCompleted();
        }

        private LevelAsset _activeLevel;
        public LevelAsset activeLevel => _activeLevel;

        public event Action<LevelAsset> BeforeLevelLoad;
        public event Action<LevelAsset> AfterLevelLoad;
        public event Action<LevelAsset> BeforeLevelUnload;
        public event Action<LevelAsset> AfterLevelUnload;
    }
}