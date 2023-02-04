using System;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
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

        [SerializeField]
        private string name;

        #endregion

        #region Properties

        public EnvironmentSupportedPlatform Platform
        {
            get => platform;
#if UNITY_EDITOR
            set => platform = value;
#endif
        }

        public string Name
        {
            get => name;
#if UNITY_EDITOR
            set => name = value;
#endif
        }

        #endregion
    }
}