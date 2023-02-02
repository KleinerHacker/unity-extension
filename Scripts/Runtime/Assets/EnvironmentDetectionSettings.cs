using System;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;

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
        private WindowsEnvironmentTarget[] windows = Array.Empty<WindowsEnvironmentTarget>();

        [SerializeField]
        private LinuxEnvironmentTarget[] linux = Array.Empty<LinuxEnvironmentTarget>();

        [SerializeField]
        private MacEnvironmentTarget[] mac = Array.Empty<MacEnvironmentTarget>();

        [SerializeField]
        private AndroidEnvironmentTarget[] android = Array.Empty<AndroidEnvironmentTarget>();

        [SerializeField]
        private IOSEnvironmentTarget[] ios = Array.Empty<IOSEnvironmentTarget>();

        #endregion

        #region Properties

        public WindowsEnvironmentTarget[] Windows => windows;

        public LinuxEnvironmentTarget[] Linux => linux;

        public MacEnvironmentTarget[] Mac => mac;

        public AndroidEnvironmentTarget[] Android => android;

        public IOSEnvironmentTarget[] IOS => ios;

        #endregion
    }

    [Serializable]
    public abstract class EnvironmentTarget
    {
        #region Inspector Data

        [SerializeField]
        private string name;

        [SerializeField]
        private bool requiresKeyboard;
        
        [SerializeField]
        private bool requiresMouse;
        
        [SerializeField]
        private bool requiresTouch;
        
        [SerializeField]
        private bool requiresGamepad;

        #endregion

        #region Properties

        public string Name => name;

        public bool RequiresKeyboard => requiresKeyboard;

        public bool RequiresMouse => requiresMouse;

        public bool RequiresTouch => requiresTouch;

        public bool RequiresGamepad => requiresGamepad;

        #endregion
    }

    [Serializable]
    public sealed class WindowsEnvironmentTarget : EnvironmentTarget
    {
    }

    [Serializable]
    public sealed class LinuxEnvironmentTarget : EnvironmentTarget
    {
        #region Inspector Data

        [SerializeField]
        private bool requiresSteamDeck;

        #endregion

        #region Properties

        public bool RequiresSteamDeck => requiresSteamDeck;

        #endregion
    }

    [Serializable]
    public sealed class MacEnvironmentTarget : EnvironmentTarget
    {
    }

    [Serializable]
    public sealed class AndroidEnvironmentTarget : EnvironmentTarget
    {
        #region Inspector Data

        [SerializeField]
        private bool requiresTv;

        #endregion

        #region Properties

        public bool RequiresTv => requiresTv;

        #endregion
    }
    
    [Serializable]
    public sealed class IOSEnvironmentTarget : EnvironmentTarget
    {
    }
}