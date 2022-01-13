using System;
#if !UNITY_EDITOR
using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime.Loader;
#endif
using UnityEditor;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class CursorSettings : ScriptableObject
    {
        #region Static Area

#if UNITY_EDITOR
        private const string Path = "Assets/Resources/cursor-system.asset";
#endif

        public static CursorSettings Singleton
        {
            get
            {
#if UNITY_EDITOR
                var settings = AssetDatabase.LoadAssetAtPath<CursorSettings>(Path);
                if (settings == null)
                {
                    Debug.Log("Unable to find cursor settings, create new");

                    settings = new CursorSettings();
                    AssetDatabase.CreateFolder("Assets", "Resources");
                    AssetDatabase.CreateAsset(settings, Path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                return settings;
#else
                return AssetResourcesLoader.Instance.GetAsset<CursorSettings>();
#endif
            }
        }

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => new SerializedObject(Singleton);
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