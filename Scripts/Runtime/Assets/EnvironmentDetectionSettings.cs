using System;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Extra;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class EnvironmentDetectionSettings : ProviderAsset<EnvironmentDetectionSettings>
    {
        #region Static Area

        public static EnvironmentDetectionSettings Singleton => GetSingleton("Environment Detection", "environment-detection.asset");

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => GetSerializedSingleton("Environment Detection", "environment-detection.asset");
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

#if UNITY_EDITOR
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
#endif

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