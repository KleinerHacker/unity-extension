#if !UNITY_EDITOR
using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime.Loader;
#endif
using System;
using UnityEditor;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public abstract class ProviderAsset<T> : ScriptableObject where T : ProviderAsset<T>
    {
        #region Static Area

        private const string AssetFolder = "Assets";

        protected static T GetSingleton(string debugName, string fileName, string path = "Resources", Func<T> creator = null)
        {
#if UNITY_EDITOR
            var assetPath = AssetFolder + "/" + path;
            var assetFile = assetPath + "/" + fileName;
            
            var settings = AssetDatabase.LoadAssetAtPath<T>(assetFile);
            if (settings == null)
            {
                Debug.Log("Unable to find " + debugName + " settings, create new");

                settings = creator == null ? CreateInstance<T>() : creator();
                if (!AssetDatabase.IsValidFolder(assetPath))
                {
                    var paths = path.Split('/');

                    var current = "";
                    foreach (var p in paths)
                    {
                        if (!AssetDatabase.IsValidFolder(AssetFolder + current + "/" + p))
                        {
                            AssetDatabase.CreateFolder(AssetFolder + current, p);
                        }

                        current += "/" + p;
                    }
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
        protected static SerializedObject GetSerializedSingleton(string debugName, string fileName, string path = "Resources", Func<T> creator = null) => 
            new SerializedObject(GetSingleton(debugName, fileName, path, creator));
#endif

        #endregion
    }
}