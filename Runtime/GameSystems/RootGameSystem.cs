using System;
using UnityEngine.LowLevel;

namespace GameFramework
{
    public class UnityRootSystem : GameSystem
    {
        public UnityRootSystem()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            PlayerLoopSystem rootSystem = PlayerLoop.GetCurrentPlayerLoop();

            PlayerLoopSystem customSystem = new PlayerLoopSystem();
            customSystem.updateDelegate += () => this.Update();

            Array.Resize(ref rootSystem.subSystemList, rootSystem.subSystemList.Length);
            rootSystem.subSystemList[rootSystem.subSystemList.Length - 1] = customSystem;

            PlayerLoop.SetPlayerLoop(rootSystem);
        }
    }
}