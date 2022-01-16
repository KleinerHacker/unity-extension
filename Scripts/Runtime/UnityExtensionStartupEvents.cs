using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime.Loader;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
    public class UnityExtensionStartupEvents
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("Load debug...");
            AssetResourcesLoader.Instance.LoadAssets<DebugSettings>("");
#endif

            Debug.Log("Load cursor system...");
            AssetResourcesLoader.Instance.LoadAssets<CursorSettings>("");
        }
    }
}