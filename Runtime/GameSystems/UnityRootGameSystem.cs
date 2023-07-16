using System;
using UnityEngine.LowLevel;

namespace GameFramework
{
    public class UnityRootGameSystem : RootGameSystem
    {
        public UnityRootGameSystem() :
            base(new BufferedGameSystemRegister()) { }

        protected override void Init()
        {
            base.Init();

            AddToUnityPlayerLoop();
        }

        protected virtual void AddToUnityPlayerLoop()
        {
            PlayerLoopSystem rootSystem = PlayerLoop.GetCurrentPlayerLoop();

            PlayerLoopSystem customSystem = new PlayerLoopSystem();
            customSystem.updateDelegate += () => PreUpdate();
            customSystem.updateDelegate += () => Update();
            customSystem.updateDelegate += () => PostUpdate();

            Array.Resize(ref rootSystem.subSystemList, rootSystem.subSystemList.Length);
            rootSystem.subSystemList[rootSystem.subSystemList.Length - 1] = customSystem;

            PlayerLoop.SetPlayerLoop(rootSystem);
        }
    }
}