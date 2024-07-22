using UnityEditor;
using UnityEditorEx.Editor.Projects.unity_editor_ex.Scripts.Editor.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.Provider
{
    public sealed class EnvironmentDetectionProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new EnvironmentDetectionProvider();
        }

        #endregion

        private SerializedObject _settings;
        private SerializedProperty _windowsProperty;
        private SerializedProperty _linuxProperty;
        private SerializedProperty _macProperty;
        private SerializedProperty _androidProperty;
        private SerializedProperty _iosProperty;
        private SerializedProperty _groupsProperty;

        private WindowsEnvironmentTargetList _windowsList;
        private LinuxEnvironmentTargetList _linuxList;
        private MacEnvironmentTargetList _macList;
        private AndroidEnvironmentTargetList _androidList;
        private IOSEnvironmentTargetList _iosList;
        private EnvironmentTargetGroupList _groupList;

        private int _tab = 0;

        public EnvironmentDetectionProvider() : base("Project/Player/Environment Detection", SettingsScope.Project, new[] { "Tooling", "Environment", "Detection" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = EnvironmentDetectionSettings.SerializedSingleton;
            if (_settings == null)
                return;

            _windowsProperty = _settings.FindProperty("windows");
            _linuxProperty = _settings.FindProperty("linux");
            _macProperty = _settings.FindProperty("mac");
            _androidProperty = _settings.FindProperty("android");
            _iosProperty = _settings.FindProperty("ios");
            _groupsProperty = _settings.FindProperty("groups");

            _windowsList = new WindowsEnvironmentTargetList(_settings, _windowsProperty);
            _linuxList = new LinuxEnvironmentTargetList(_settings, _linuxProperty);
            _macList = new MacEnvironmentTargetList(_settings, _macProperty);
            _androidList = new AndroidEnvironmentTargetList(_settings, _androidProperty);
            _iosList = new IOSEnvironmentTargetList(_settings, _iosProperty);
            _groupList = new EnvironmentTargetGroupList(_settings, _groupsProperty);
        }

        public override void OnTitleBarGUI()
        {
            GUILayout.BeginVertical();
            {
                ExtendedEditorGUILayout.SymbolField("Activate System", "PCSOFT_ENV");
                EditorGUI.BeginDisabledGroup(
#if PCSOFT_ENV
                    false
#else
                    true
#endif
                );
                {
                    ExtendedEditorGUILayout.SymbolField("Verbose Logging", "PCSOFT_ENV_LOGGING");
                    ExtendedEditorGUILayout.SymbolField("Activate Steam Support", "PCSOFT_ENV_STEAM");
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndVertical();
        }

        public override void OnGUI(string searchContext)
        {
            _settings.Update();
            
            GUILayout.Space(15f);

#if PCSOFT_ENV
            GUILayout.Label("Environment Target Groups", EditorStyles.boldLabel);
            _groupList.DoLayoutList();

            GUILayout.Space(15f);
            GUILayout.Label("Environment Target Conditions", EditorStyles.boldLabel);
            _tab = GUILayout.Toolbar(_tab, new []
            {
                new GUIContent(EditorGUIUtility.IconContent("BuildSettings.Metro On").image, "Windows"),
                new GUIContent(Resources.Load<Texture2D>("linux"), "Linux"),
                new GUIContent(EditorGUIUtility.IconContent("BuildSettings.Standalone On").image, "Mac"),
                new GUIContent(EditorGUIUtility.IconContent("BuildSettings.Android On").image, "Android"),
                new GUIContent(EditorGUIUtility.IconContent("BuildSettings.iPhone On").image, "IOS"),
            });
            switch (_tab)
            {
                case 0:
                    GUILayout.Label("Windows", EditorStyles.boldLabel);
                    _windowsList.DoLayoutList();
                    break;
                case 1:
                    GUILayout.Label("Linux", EditorStyles.boldLabel);
                    _linuxList.DoLayoutList();
                    break;
                case 2:
                    GUILayout.Label("Mac", EditorStyles.boldLabel);
                    _macList.DoLayoutList();
                    break;
                case 3:
                    GUILayout.Label("Android", EditorStyles.boldLabel);
                    _androidList.DoLayoutList();
                    break;
                case 4:
                    GUILayout.Label("IOS", EditorStyles.boldLabel);
                    _iosList.DoLayoutList();
                    break;
                default:
                    EditorGUILayout.HelpBox("Unknown page", MessageType.Error);
                    break;
            }
#else
            EditorGUILayout.HelpBox("Environment Detection System is deactivated", MessageType.Info);
#endif

            _settings.ApplyModifiedProperties();
        }
    }
}