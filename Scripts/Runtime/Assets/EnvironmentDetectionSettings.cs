using System;
using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
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

        [SerializeField]
        private EnvironmentTargetGroup[] groups = Array.Empty<EnvironmentTargetGroup>();

        #endregion

        #region Properties

        public WindowsEnvironmentTarget[] Windows => windows;

        public LinuxEnvironmentTarget[] Linux => linux;

        public MacEnvironmentTarget[] Mac => mac;

        public AndroidEnvironmentTarget[] Android => android;

        public IOSEnvironmentTarget[] IOS => ios;

        public EnvironmentTargetGroup[] Groups => groups;

        #endregion

        #region Builtin Methods

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var group in groups)
            {
                var removeList = group.Items.Where(x => Enum.GetValues(typeof(EnvironmentSupportedPlatform)).Cast<EnvironmentSupportedPlatform>().All(y => x.Platform != y));
                var addList = Enum.GetValues(typeof(EnvironmentSupportedPlatform)).Cast<EnvironmentSupportedPlatform>().Where(x => group.Items.All(y => x != y.Platform));

                group.Items = group.Items.RemoveAll(removeList.ToArray()).ToArray();
                group.Items = group.Items.Concat(addList.Select(x => new EnvironmentTargetGroupItem { Platform = x })).ToArray();
            }
        }
#endif

        #endregion

        public EnvironmentTarget[] GetTargets(EnvironmentSupportedPlatform platform)
        {
            switch (platform)
            {
                case EnvironmentSupportedPlatform.Windows:
                    return windows.Cast<EnvironmentTarget>().ToArray();
                case EnvironmentSupportedPlatform.Linux:
                    return linux.Cast<EnvironmentTarget>().ToArray();
                case EnvironmentSupportedPlatform.Mac:
                    return mac.Cast<EnvironmentTarget>().ToArray();
                case EnvironmentSupportedPlatform.Android:
                    return android.Cast<EnvironmentTarget>().ToArray();
                case EnvironmentSupportedPlatform.IOS:
                    return ios.Cast<EnvironmentTarget>().ToArray();
                default:
                    throw new NotImplementedException(platform.ToString());
            }
        }
    }

    public enum EnvironmentSupportedPlatform
    {
        Windows,
        Linux,
        Mac,
        Android,
        IOS,
    }
}