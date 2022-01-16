#if !UNITY_EDITOR
using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime.Loader;
#endif
using UnityEditor;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class DebugSettings : ScriptableObject
    {
        #region Static Area

#if UNITY_EDITOR
        private const string Path = "Assets/Resources/debug.asset";
#endif

        public static DebugSettings Singleton
        {
            get
            {
#if UNITY_EDITOR
                var settings = AssetDatabase.LoadAssetAtPath<DebugSettings>(Path);
                if (settings == null)
                {
                    Debug.Log("Unable to find cursor settings, create new");

                    settings = new DebugSettings();
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
                return AssetResourcesLoader.Instance.GetAsset<DebugSettings>();
#endif
            }
        }

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => new SerializedObject(Singleton);
#endif

        #endregion

        #region Inspector Data

        [SerializeField]
        private bool showFramerate;

        [SerializeField]
        private int framerateTextSize = 25;

        [SerializeField]
        private Color framerateColor = Color.white;

        [SerializeField]
        private TextAnchor frameratePosition = TextAnchor.UpperLeft;

        [SerializeField]
        [Min(1)]
        private byte framerateUpdateRate = 10;

        #endregion

        #region Properties

        public bool ShowFramerate => showFramerate;

        public int FramerateTextSize => framerateTextSize;

        public Color FramerateColor => framerateColor;

        public TextAnchor FrameratePosition => frameratePosition;

        public byte FramerateUpdateRate => framerateUpdateRate;

        #endregion
    }
}