using GameFramework.LogManagement;

namespace GameFramework
{
    public class GameInstance
    {
        public static GameInstance Instance;
        
        public virtual void Init()
        {
            if (LogManager.Instance is null)
            {
                LogManager.Instance = LogManagerFactory.Create();
                LogManager.Instance.Init();
            }

            if (LevelManager.Impl is null)
            {
                LevelManager.Impl = LevelManagerFactory.Create();
            }

            if (SceneManager.Impl is null)
            {
                SceneManager.Impl = SceneManagerFactory.Create();
            }
        }

        public virtual void Shutdown()
        {
        }
    }
}