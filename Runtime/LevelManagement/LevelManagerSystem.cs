using System;
using System.Diagnostics.Contracts;

namespace GameFramework.LevelManagement
{
    [RegisterGameSystem(typeof(LevelManagerSystem))]
    public class LevelManagerSystem : GameSystem
    {
        protected override void OnRegistered(GameSystem system)
        {
            base.OnRegistered(system);

            LevelManager.System = this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="level"></param>
        /// 
        /// <todo>
        /// Do not accept null to unload the level. Instead write unload logic in UnloadLevel.
        /// </todo>
        /// 
        /// <returns></returns>
        public virtual LevelAsyncOperation LoadLevelAsync(LevelAsset level)
        {
            // Contract.Assume(level is not null);

            if (level == _activeLevel)
            {
                return LevelAsyncOperation.Completed;
            }

            LevelAsyncOperation levelOp = new LevelAsyncOperation();

            // opSrc to hold the completion of op explicitly
            AsyncOperationSource opSrc = new AsyncOperationSource();
            levelOp.AddOperation(opSrc);

            _LoadLevelAsync(levelOp, opSrc, level);

            return levelOp;
        }

        public virtual LevelAsyncOperation UnloadLevelAsync()
        {
            return LoadLevelAsync(null);
        }

        protected virtual async void _LoadLevelAsync(LevelAsyncOperation op, 
            AsyncOperationSource opSrc, LevelAsset level)
        {
            LevelAsset currentLevel = _activeLevel;

            if (level is not null)
            {
                BeforeLevelLoadEvent?.Invoke(level);

                LevelAsyncOperation loadOp = level.PerformLoad();

                if (currentLevel is null)
                {
                    op.AddOperation(loadOp, 1f);
                }
                else
                {
                    op.AddOperation(loadOp, .9f);
                }

                await loadOp;

                _activeLevel = level;
                AfterLevelLoadEvent?.Invoke(level);
            }

            if (currentLevel is not null)
            {
                BeforeLevelUnloadEvent?.Invoke(currentLevel);

                LevelAsyncOperation unloadOp = currentLevel.PerformUnload();
                op.AddOperation(unloadOp, .1f);

                await unloadOp;

                AfterLevelUnloadEvent?.Invoke(currentLevel);
            }

            opSrc.SetCompleted();
        }

        public event Action<LevelAsset> BeforeLevelLoadEvent;
        public event Action<LevelAsset> AfterLevelLoadEvent;
        public event Action<LevelAsset> BeforeLevelUnloadEvent;
        public event Action<LevelAsset> AfterLevelUnloadEvent;

        protected LevelAsset _activeLevel;
        public LevelAsset ActiveLevel => _activeLevel;
    }
}