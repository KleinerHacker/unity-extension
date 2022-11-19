using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class DebugSettings : ProviderAsset<DebugSettings>
    {
        #region Static Area

        public static DebugSettings Singleton => GetSingleton("Debug", "debug.asset");

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => GetSerializedSingleton("Debug", "debug.asset");
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