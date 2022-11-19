using System;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class RaycastSettings : ProviderAsset<RaycastSettings>
    {
        #region Static Area

        public static RaycastSettings Singleton => GetSingleton("Raycaster", "raycaster.asset");

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton =>GetSerializedSingleton("Raycaster", "raycaster.asset");
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