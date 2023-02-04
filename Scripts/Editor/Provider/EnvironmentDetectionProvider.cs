using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
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
        
        public EnvironmentDetectionProvider() : base("Project/Player/Environment Detection", SettingsScope.Project, new []{"Tooling", "Environment", "Detection"})
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
            ExtendedEditorGUILayout.SymbolField("Activate Steam Support", "ENV_STEAM");
        }

        public override void OnGUI(string searchContext)
        {
            _settings.Update();
            
            GUILayout.Label("Environment Target Groups", EditorStyles.boldLabel);
            _groupList.DoLayoutList();

            GUILayout.Space(15f);
            GUILayout.Label("Environment Target Conditions", EditorStyles.boldLabel);
            _tab = GUILayout.Toolbar(_tab, new []
            {
                new GUIContent(EditorGUIUtility.IconContent("BuildSettings.Metro On").image, "Windows"),
                new GUIContent(EditorGUIUtility.IconContent("BuildSettings.Lumin On").image, "Linux"),
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
            
            _settings.ApplyModifiedProperties();
        }
    }
}