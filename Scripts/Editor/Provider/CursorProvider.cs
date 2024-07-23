using UnityEditor;
using UnityEditorEx.Editor.Projects.unity_editor_ex.Scripts.Editor.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.Provider
{
    public sealed class CursorProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new CursorProvider();
        }

        #endregion

        private SerializedObject _settings;
        private SerializedProperty _cursorItemsProperty;

        private CursorList _cursorList;

        public CursorProvider() : base("Project/Extensions/Cursors", SettingsScope.Project, new[] { "Tooling", "Cursor", "Mouse" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = CursorSettings.SerializedSingleton;
            if (_settings == null)
                return;

            _cursorItemsProperty = _settings.FindProperty("items");
            _cursorList = new CursorList(_settings, _cursorItemsProperty);
        }

        public override void OnTitleBarGUI()
        {
            GUILayout.BeginVertical();
            {
                ExtendedEditorGUILayout.SymbolField("Activate System", "PCSOFT_CURSOR");
                EditorGUI.BeginDisabledGroup(
#if PCSOFT_CURSOR
                    false
#else
                    true
#endif
                );
                {
                    ExtendedEditorGUILayout.SymbolField("Verbose Logging", "PCSOFT_CURSOR_LOGGING");
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndVertical();
        }

        public override void OnGUI(string searchContext)
        {
            _settings.Update();
            
            GUILayout.Space(15f);

#if PCSOFT_CURSOR
            _cursorList.DoLayoutList();
            EditorGUILayout.Space();

            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndFoldoutHeaderGroup();
#else
            EditorGUILayout.HelpBox("Cursor System is deactivated", MessageType.Info);
#endif

            _settings.ApplyModifiedProperties();
        }
    }
}