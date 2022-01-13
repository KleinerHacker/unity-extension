#if !UNITY_EDITOR
using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime.Loader;
#endif
using System;
using UnityEditor;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class RaycastSettings : ScriptableObject
    {
        #region Static Area

#if UNITY_EDITOR
        private const string Path = "Assets/Resources/raycaster.asset";
#endif

        public static RaycastSettings Singleton
        {
            get
            {
#if UNITY_EDITOR
                var settings = AssetDatabase.LoadAssetAtPath<RaycastSettings>(Path);
                if (settings == null)
                {
                    Debug.Log("Unable to find raycast settings, create new");

                    settings = new RaycastSettings();
                    AssetDatabase.CreateFolder("Assets", "Resources");
                    AssetDatabase.CreateAsset(settings, Path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                return settings;
#else
                return AssetResourcesLoader.Instance.GetAsset<RaycastSettings>();
#endif
            }
        }

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => new SerializedObject(Singleton);
#endif

        #endregion

        #region Inspector Data

        [SerializeField]
        private RaycastItem[] items;

        #endregion

        #region Properties

        public RaycastItem[] Items => items;

        #endregion
    }

    [Serializable]
    public sealed class RaycastItem
    {
        #region Inspector Data

        [SerializeField]
        private string key;
        
        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private float maxDistance = 100f;

        [SerializeField]
        [Range(1, 100)]
        private byte fixedCheckCount = 3;

        #endregion

        #region Properties

        public string Key => key;

        public LayerMask LayerMask => layerMask;

        public float MaxDistance => maxDistance;

        public byte FixedCheckCount => fixedCheckCount;

        #endregion
    }
}