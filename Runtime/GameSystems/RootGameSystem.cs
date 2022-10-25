using System;
using System.Reflection;
using System.Collections.Generic;
using SystemType = System.Type;

namespace GameFramework
{
    public class RootGameSystem : GameSystem
    {
        public RootGameSystem() : this(new BufferedGameSystemManager()) { }

        public RootGameSystem(GameSystemManager systemManager) : base(systemManager)
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            FindAndRegisterSystems();
        }

        protected virtual void FindAndRegisterSystems()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            IEnumerable<SystemType> enumerator()
            {
                foreach (var assembly in assemblies)
                    foreach (var systemType in FindSystemsFromAssembly(assembly))
                    {
                        yield return systemType;
                    }
            }

            CreateAndRegisterSystemsFromTypes(enumerator());
        }

        protected virtual void RegisterSystemsFromAssembly(Assembly assembly)
        {
            // no need to check for (assembly is null), FindSystemsFromAssembly(assembly) does that for us
            CreateAndRegisterSystemsFromTypes(FindSystemsFromAssembly(assembly));
        }

        protected virtual IEnumerable<SystemType> FindSystemsFromAssembly(Assembly assembly)
        {
            if (assembly is not null)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(GameSystem)) is false) continue;

                    var attribute = type.GetCustomAttribute<RegisterGameSystemAttribute>();
                    if (attribute is null) continue;

                    yield return type;
                }
            }
        }

        protected void CreateAndRegisterSystemsFromTypes(IEnumerable<SystemType> systemTypes)
        {
            foreach (var systemType in systemTypes)
            {
                try
                {
                    CreateAndRegisterSystemFromType(systemType, out _);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"Failed registering GameSystem {systemType?.FullName}\nException: {ex.Message}");
                }
            }
        }

        protected void CreateAndRegisterSystemFromType(SystemType systemType, out GameSystem system)
        {
            if (systemType is null)
            {
                throw new Exception($"cannot create a system of null type");
            }

            var attribute = systemType.GetCustomAttribute<RegisterGameSystemAttribute>();
            if (attribute is null)
            {
                throw new Exception($"{systemType.FullName} does not contain {nameof(RegisterGameSystemAttribute)} attribute");
            }

            GameSystem parentSystem = null;
            SystemType parentSystemType = null;

            if (parentSystemType is not null)
            {
                // check if parent system already exists
                parentSystem = SubSystems.GetSystem(parentSystemType);
                if (parentSystem is null)
                {
                    try
                    {
                        // create parent system first
                        CreateAndRegisterSystemFromType(parentSystemType, out parentSystem);
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogError($"could not create parent system {parentSystemType.FullName} for {systemType.FullName}\nException: {ex.Message}");
                        throw;
                    }

                    // 1. fail
                    // 2.a. set parent system to root
                    // 2.b. set parent system to root in group (name:failed)
                    // 3.a. set parent fallback
                    // 3.b. set parent fallback in group (name:failed)
                    // 4 create failed group instead
                }
            }
            else
            {
                parentSystem = this;
            }

            try
            {
                system = Activator.CreateInstance(systemType) as GameSystem;
                if (system is null)
                {
                    throw new SystemException($"could not create instance of GameSystem {systemType.FullName}");
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.Message);
                throw;
            }

            parentSystem.SubSystems.RegisterSystem(system,
                attribute.type, 0, attribute.force);
        }
    }
}