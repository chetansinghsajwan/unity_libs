namespace GameFramework
{
    public class GameInstance
    {
        public static GameInstance Instance;
        
        public virtual void Init()
        {
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