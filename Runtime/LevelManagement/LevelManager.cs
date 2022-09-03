using System;
using UnityEngine;

namespace GameFramework
{
    public class LevelManager
    {
        public static LevelManager Impl;

        public static event Action<LevelAsset> BeforeLevelLoad;
        public static event Action<LevelAsset> AfterLevelLoad;
        public static event Action<LevelAsset> BeforeLevelUnload;
        public static event Action<LevelAsset> AfterLevelUnload;

        protected static LevelAsset _activeLevel;
        public static LevelAsset ActiveLevel => _activeLevel;

        public static LevelAsyncOperation LoadLevelAsync(LevelAsset level)
        {
            return Impl.LoadLevelAsyncImpl(level);
        }

        public static LevelAsyncOperation UnloadLevelAsync()
        {
            return Impl.UnloadLevelAsyncImpl();
        }

        //////////////////////////////////////////////////////////////////

        public virtual LevelAsyncOperation LoadLevelAsyncImpl(LevelAsset level)
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

        public virtual LevelAsyncOperation UnloadLevelAsyncImpl()
        {
            return LoadLevelAsyncImpl(null);
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

                await loadOp;

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
    }
}