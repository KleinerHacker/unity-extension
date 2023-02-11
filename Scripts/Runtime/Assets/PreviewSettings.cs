using System;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class PreviewSettings : ProviderAsset<PreviewSettings>
    {
        #region Static Area

        public static PreviewSettings Singleton => GetSingleton("Gaming Preview", "gaming-preview.asset");

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => GetSerializedSingleton("Gaming Preview", "gaming-preview.asset");
#endif

        #endregion

        #region Inspector Data

        [SerializeField]
        private string raycaster;

        [SerializeField]
        private PreviewItem[] items;

        #endregion

        #region Properties

        public string Raycaster => raycaster;

        public PreviewItem[] Items => items;

        #endregion
    }

    [Serializable]
    public sealed class PreviewItem
    {
        #region Inspector Data

        [SerializeField]
        private string key;

        [SerializeField]
        private bool autoRotate;

        [SerializeField]
        private float maxRotation;

        [SerializeField]
        private GameObject previewPrefab;

        [SerializeField]
        private Vector3 previewPosition;

        [SerializeField]
        private Vector3 previewRotation;

        #endregion

        #region Properties

        public string Key => key;

        public bool AutoRotate => autoRotate;

        public float MaxRotation => maxRotation;

        public GameObject PreviewPrefab => previewPrefab;

        public Vector3 PreviewPosition => previewPosition;

        public Vector3 PreviewRotation => previewRotation;

        #endregion
    } 
}