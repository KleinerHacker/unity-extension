using System;
using System.Linq;
using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
    public static class UnityExtensionStartupEvents
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("Load debug...");
            AssetResourcesLoader.LoadFromResources<DebugSettings>("");
#endif

#if PCSOFT_RAYCASTER
            Debug.Log("Initialize raycaster system");
            AssetResourcesLoader.LoadFromResources<RaycastSettings>("");
#endif

#if PCSOFT_RAYCASTER && PCSOFT_PREVIEW
            Debug.Log("Initialize preview system");
            AssetResourcesLoader.LoadFromResources<PreviewSettings>("");
#endif

#if PCSOFT_CURSOR
            Debug.Log("Load cursor system...");
            AssetResourcesLoader.LoadFromResources<CursorSettings>("");
#endif

#if PCSOFT_DRAGDROP
            Debug.Log("Load drag drop system");
            AssetResourcesLoader.LoadFromResources<DragDropSettings>("");
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void LateInitialize()
        {
#if PCSOFT_RAYCASTER && PCSOFT_HOVER
            Debug.Log("Initialize hover system");
            var hoverTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(HoverController).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToArray();
            foreach (var hoverType in hoverTypes)
            {
                Debug.Log("> Create hover system " + hoverType.Name);

                var go = new GameObject("Hover System (" + hoverType.Name + ")");
                go.AddComponent(hoverType);
                GameObject.DontDestroyOnLoad(go);
            }
#endif
        }
    }
}