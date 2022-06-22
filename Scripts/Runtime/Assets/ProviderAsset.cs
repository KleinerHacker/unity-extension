#if !UNITY_EDITOR
using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime.Loader;
#endif
using UnityEditor;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public abstract class ProviderAsset<T> : ScriptableObject where T : ProviderAsset<T>
    {
        #region Static Area

        private const string AssetFolder = "Assets";

        protected static T GetSingleton(string debugName, string fileName, string path = "Resources")
        {
#if UNITY_EDITOR
            var assetPath = AssetFolder + "/" + path;
            var assetFile = assetPath + "/" + fileName;
            
            var settings = AssetDatabase.LoadAssetAtPath<T>(assetFile);
            if (settings == null)
            {
                Debug.Log("Unable to find " + debugName + " settings, create new");

                settings = ScriptableObject.CreateInstance<T>();
                if (!AssetDatabase.IsValidFolder(assetPath))
                {
                    AssetDatabase.CreateFolder(AssetFolder, path);
                }

                AssetDatabase.CreateAsset(settings, assetFile);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            return settings;
#else
                return AssetResourcesLoader.Instance.GetAsset<T>();
#endif
        }

#if UNITY_EDITOR
        protected static SerializedObject GetSerializedSingleton(string debugName, string fileName, string path = "Resources") => 
            new SerializedObject(GetSingleton(debugName, fileName, path));
#endif

        #endregion
    }
}