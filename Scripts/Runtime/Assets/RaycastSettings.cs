using System;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class RaycastSettings : ProviderAsset<RaycastSettings>
    {
        #region Static Area

        public static RaycastSettings Singleton => GetSingleton("Raycaster", "raycaster.asset");

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => GetSerializedSingleton("Raycaster", "raycaster.asset");
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

        [SerializeField]
        [Range(1, 100)]
        private byte countOfHits = 1;

        [SerializeField]
        private bool overUI;

        [SerializeField]
        private Vector2 offset;

        [SerializeField]
        private RaycastType type = RaycastType.Physics3D;

        [SerializeField]
        private bool touch;

        #endregion

        #region Properties

        public string Key => key;

        public LayerMask LayerMask => layerMask;

        public float MaxDistance => maxDistance;

        public byte FixedCheckCount => fixedCheckCount;

        public byte CountOfHits => countOfHits;

        public bool OverUI => overUI;

        public Vector2 Offset => offset;

        public RaycastType Type
        {
            get => type;
            set => type = value;
        }

        public bool Touch => touch;

        #endregion
    }

    public enum RaycastType
    {
        Physics3D = 0x10,
        Physics2D = 0x20,
        UI = 0x30,
    }
}