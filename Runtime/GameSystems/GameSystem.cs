using SystemType = System.Type;

namespace GameFramework
{
    public abstract class GameSystem
    {
        public GameSystem() : this(new GameSystemManager()) { }

        public GameSystem(GameSystemManager subSystemManager)
        {
            subSystemManager ??= NullGameSystemManager.Instance;

            this.SubSystems = subSystemManager;
            this.SubSystems.OnSubSystemRegistered += OnSubSystemRegistered;
            this.SubSystems.OnSubSystemUnregistered += OnSubSystemUnregistered;
        }

        protected virtual void Init() { }

        protected virtual void PreUpdate()
        {
            PreUpdateSystems();
        }

        protected virtual void Update()
        {
            UpdateSystems();
        }

        protected virtual void PostUpdate()
        {
            PostUpdateSystems();
        }

        protected virtual void PreUpdateSystems()
        {
            foreach (var system in SubSystems)
            {
                if (system is not null)
                {
                    system.PreUpdate();
                }
            }
        }

        protected virtual void UpdateSystems()
        {
            foreach (var system in SubSystems)
            {
                if (system is not null)
                {
                    system.Update();
                }
            }
        }

        protected virtual void PostUpdateSystems()
        {
            foreach (var system in SubSystems)
            {
                if (system is not null)
                {
                    system.PostUpdate();
                }
            }
        }

        protected virtual void Shutdown() { }

        protected virtual void OnRegistered(GameSystem system) { }
        protected virtual void OnUnregistered(GameSystem system) { }

        protected virtual void OnSubSystemRegistered(GameSystem system, SystemType type)
        {
            system?.OnRegistered(this);
        }

        protected virtual void OnSubSystemUnregistered(GameSystem system, SystemType type)
        {
            system?.OnRegistered(this);
        }

        public readonly GameSystemManager SubSystems;
    }
}