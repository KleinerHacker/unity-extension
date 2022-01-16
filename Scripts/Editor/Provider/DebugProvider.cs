using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class DebugProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new DebugProvider();
        }

        #endregion
        
        private SerializedObject _settings;
        private SerializedProperty _showFramerateProperty;
        private SerializedProperty _framerateTextSizeProperty;
        private SerializedProperty _framerateColorProperty;
        private SerializedProperty _frameratePositionProperty;
        private SerializedProperty _framerateUpdateRateProperty;
        
        public DebugProvider() : base("Project/Player/Debug", SettingsScope.Project, new []{"Tooling", "Debug", "Development"})
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = DebugSettings.SerializedSingleton;
            if (_settings == null)
                return;

            _showFramerateProperty = _settings.FindProperty("showFramerate");
            _framerateTextSizeProperty = _settings.FindProperty("framerateTextSize");
            _framerateColorProperty = _settings.FindProperty("framerateColor");
            _frameratePositionProperty = _settings.FindProperty("frameratePosition");
            _framerateUpdateRateProperty = _settings.FindProperty("framerateUpdateRate");
        }

        public override void OnGUI(string searchContext)
        {
            _settings.Update();
            
            EditorGUILayout.HelpBox("Note: All given debug features are only used if it is a development build!", MessageType.Info);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Framerate", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Shows the framerate in gaming screen.", MessageType.None);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_showFramerateProperty, GUIContent.none, GUILayout.Width(25f));
            EditorGUI.BeginDisabledGroup(!_showFramerateProperty.boolValue);
            EditorGUILayout.PropertyField(_framerateTextSizeProperty, new GUIContent("Framerate Text Size:"));
            EditorGUILayout.PropertyField(_framerateColorProperty, new GUIContent("Framerate Color:"));
            EditorGUILayout.PropertyField(_frameratePositionProperty, new GUIContent("Screen Position:"));
            EditorGUILayout.PropertyField(_framerateUpdateRateProperty, new GUIContent("Update Rate (Fixed):"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            _settings.ApplyModifiedProperties();
        }
    }
}