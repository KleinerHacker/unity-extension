using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Assets
{
    [Serializable]
    public sealed class EnvironmentTargetGroup
    {
        #region Inspector Data

        [SerializeField]
        private string name;

        [SerializeField]
        private EnvironmentTargetGroupItem[] items = Array.Empty<EnvironmentTargetGroupItem>();

        #endregion

        #region Properties

        public string Name => name;

        public EnvironmentTargetGroupItem[] Items
        {
            get => items;
#if UNITY_EDITOR
            set => items = value;
#endif
        }

        #endregion
    }

    [Serializable]
    public sealed class EnvironmentTargetGroupItem
    {
        #region Insepctor Data

        [SerializeField]
        private EnvironmentSupportedPlatform platform;

        [FormerlySerializedAs("name")]
        [SerializeField]
        private string targetName;

        #endregion

        #region Properties

        public EnvironmentSupportedPlatform Platform
        {
            get => platform;
#if UNITY_EDITOR
            set => platform = value;
#endif
        }

        public string TargetName
        {
            get => targetName;
#if UNITY_EDITOR
            set => targetName = value;
#endif
        }

        #endregion
    }
}