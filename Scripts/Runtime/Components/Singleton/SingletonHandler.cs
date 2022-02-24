using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton
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
                    Debug.Log("[SINGLETON] Initialize On Load methods for singleton of " + type.FullName);
                    if (attribute.Scope == SingletonScope.Application)
                    {
                        Debug.Log("[SINGLETON] > Initialize now (" + nameof(SingletonScope.Application) + ")");
                        var _ = Registry.GetSingleton(type);
                    }
                    else
                    {
                        Debug.Log("[SINGLETON] > Add scene listener (" + nameof(SingletonScope.Scene) + ")");
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