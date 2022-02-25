using UnityEngine;
using UnityEngine.SceneManagement;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Attributes;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Internals
{
    public static class SingletonHandler
    {
        internal static SingletonRegistry Registry { get; } = new SingletonRegistry();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeOnLoad()
        {
            Registry.VisitAllSingletons((type, attribute) =>
            {
                if (attribute.CreationTime == SingletonCreationTime.Loading)
                {
#if SINGLETON_LOGGING
                    Debug.Log("[SINGLETON] Initialize On Load methods for singleton of " + type.FullName);
#endif
                    if (attribute.Scope == SingletonScope.Application)
                    {
#if SINGLETON_LOGGING
                        Debug.Log("[SINGLETON] > Initialize now (" + nameof(SingletonScope.Application) + ")");
#endif
                        var _ = Registry.GetSingleton(type);
                    }
                    else
                    {
#if SINGLETON_LOGGING
                        Debug.Log("[SINGLETON] > Add scene listener (" + nameof(SingletonScope.Scene) + ")");
#endif
                        SceneManager.sceneLoaded += (_, _) =>
                        {
                            var _ = Registry.GetSingleton(type);
                        };
                    }
                }
            });
        }
    }
}