#if !UNITY_EDITOR
using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime.Loader;
#endif
using System;
using System.Linq;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Extra;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class EnvironmentDetectionSettings : ScriptableObject
    {
        #region Static Area

#if UNITY_EDITOR
        private const string Path = "Assets/Resources/environment-detection.asset";
#endif

        public static EnvironmentDetectionSettings Singleton
        {
            get
            {
#if UNITY_EDITOR
                var settings = AssetDatabase.LoadAssetAtPath<EnvironmentDetectionSettings>(Path);
                if (settings == null)
                {
                    Debug.Log("Unable to find environment detection settings, create new");

                    settings = new EnvironmentDetectionSettings();
                    if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    AssetDatabase.CreateAsset(settings, Path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                return settings;
#else
                return AssetResourcesLoader.Instance.GetAsset<EnvironmentDetectionSettings>();
#endif
            }
        }

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => new SerializedObject(Singleton);
#endif

        #endregion

        #region Inspector Data

        [SerializeField]
        private EnvironmentConstraintItem[] items = Array.Empty<EnvironmentConstraintItem>();

        #endregion

        #region Properties

        public EnvironmentConstraintItem[] Items => items;

        #endregion

        #region Builtin Methods

        private void OnValidate()
        {
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Guid))
                {
                    item.Guid = Guid.NewGuid().ToString();
                }
            }
        }

        #endregion
    }

    [Serializable]
    public sealed class EnvironmentConstraintItem : IdentifiableObject
    {
        #region Inspector Data

        [SerializeField]
        private string name;

        [Space]
        [InputDevice]
        [Tooltip("All required input devices (AND)")]
        [SerializeField]
        private string[] inputs;

        [Tooltip("Only available on given runtime systems (OR)")]
        [SerializeField]
        private EnvironmentSystemConstraintItem[] runtimeSystemItems;

        #endregion

        #region Properties

        public string Name => name;

        public string[] Inputs => inputs;

        public EnvironmentSystemConstraintItem[] RuntimeSystemItems => runtimeSystemItems;

        #endregion
    }

    [Serializable]
    public sealed class EnvironmentSystemConstraintItem
    {
        #region Inspector Data

        [SerializeField]
        private RuntimePlatform platform;

        [Header("Android / iOS")]
        [SerializeField]
        private bool tvRequired;

        #endregion

        #region Properties

        public RuntimePlatform Platform => platform;

        public bool TVRequired => tvRequired;

        #endregion
    }
}