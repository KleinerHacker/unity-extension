using System;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class CursorSettings : ProviderAsset<CursorSettings>
    {
        #region Static Area

        public static CursorSettings Singleton => GetSingleton("Cursor System", "cursor-system.asset");

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => GetSerializedSingleton("Cursor System", "cursor-system.asset");
#endif

        #endregion
        
        #region Inspector Data

        [SerializeField]
        private UICursor uiCursor;

        [SerializeField]
        private NamedCursorItem[] items;

        #endregion

        #region Properties

        public UICursor UICursor => uiCursor;

        public NamedCursorItem[] Items => items;

        #endregion
    }

    [Serializable]
    public sealed class UICursor
    {
        #region Inspector Data

        [SerializeField]
        private bool useUICursors;

        [SerializeField]
        private float uiCursorCheckDelay = 0.1f;
        
        [SerializeField]
        private OptionalCursorItem defaultCursor;
        
        [SerializeField]
        private NamedCursorItem[] items;

        #endregion

        #region Properties

        public bool UseUICursors => useUICursors;

        public float UICursorCheckDelay => uiCursorCheckDelay;

        public OptionalCursorItem DefaultCursor => defaultCursor;

        public NamedCursorItem[] Items => items;

        #endregion
    }

    [Serializable]
    public sealed class NamedCursorItem : CursorItem
    {
        #region Inspector Data

        [SerializeField]
        private string identifier;

        #endregion

        #region Properties

        public string Identifier => identifier;

        #endregion
    }

    [Serializable]
    public sealed class OptionalCursorItem : CursorItem
    {
        #region Inspector Data

        [SerializeField]
        private bool active;

        #endregion

        #region Properties

        public bool Active => active;

        #endregion
    }

    [Serializable]
    public abstract class CursorItem
    {
        #region Inspector Data

        [SerializeField]
        private Texture2D cursor;

        [SerializeField]
        private Vector2 hotspot;

        #endregion

        #region Properties

        public Texture2D Cursor => cursor;

        public Vector2 Hotspot => hotspot;

        #endregion
    }
}