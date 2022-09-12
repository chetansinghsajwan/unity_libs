using System;

namespace GameFramework
{
    [GameSystemRegistration(typeof(LevelManagerSystem))]
    public class LevelManagerSystem : GameSystem
    {
        protected override void OnRegistered(GameSystem system)
        {
            base.OnRegistered(system);

            LevelManager.System = this;
        }

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

        public event Action<LevelAsset> BeforeLevelLoad;
        public event Action<LevelAsset> AfterLevelLoad;
        public event Action<LevelAsset> BeforeLevelUnload;
        public event Action<LevelAsset> AfterLevelUnload;

        protected LevelAsset _activeLevel;
        public LevelAsset ActiveLevel => _activeLevel;
    }
}