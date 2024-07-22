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
        private SerializedProperty _useUICursorsProperty;
        private SerializedProperty _uiCursorCheckProperty;
        private SerializedProperty _uiMainCursorProperty;
        private SerializedProperty _uiCursorItemsProperty;
        private SerializedProperty _cursorItemsProperty;

        private CursorList _cursorList;
        private CursorList _uiCursorList;

        private bool _uiFold;

        public CursorProvider() : base("Project/UI/Cursors", SettingsScope.Project, new[] { "Tooling", "Cursor", "Mouse" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = CursorSettings.SerializedSingleton;
            if (_settings == null)
                return;

            var uiCursorsProperty = _settings.FindProperty("uiCursor");
            _useUICursorsProperty = uiCursorsProperty.FindPropertyRelative("useUICursors");
            _uiCursorCheckProperty = uiCursorsProperty.FindPropertyRelative("uiCursorCheckDelay");
            _uiMainCursorProperty = uiCursorsProperty.FindPropertyRelative("defaultCursor");
            _uiCursorItemsProperty = uiCursorsProperty.FindPropertyRelative("items");
            _cursorItemsProperty = _settings.FindProperty("items");

            _cursorList = new CursorList(_settings, _cursorItemsProperty);
            _uiCursorList = new CursorList(_settings, _uiCursorItemsProperty);
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

            _uiFold = EditorGUILayout.BeginFoldoutHeaderGroup(_uiFold, "UI");
            EditorGUI.indentLevel = 1;
            if (_uiFold)
            {
                EditorGUILayout.PropertyField(_useUICursorsProperty, new GUIContent("Use other cursors for UI"));
                EditorGUI.BeginDisabledGroup(!_useUICursorsProperty.boolValue);
                {
                    EditorGUILayout.PropertyField(_uiCursorCheckProperty, new GUIContent("Delay to check cursor is in or out of UI area"));
                    EditorGUILayout.PropertyField(_uiMainCursorProperty, new GUIContent("Alternative UI default cursor"), true);
                    EditorGUILayout.Space();
                    _uiCursorList.DoLayoutList();
                }
                EditorGUI.EndDisabledGroup();
            }

            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndFoldoutHeaderGroup();
#else
            EditorGUILayout.HelpBox("Cursor System is deactivated", MessageType.Info);
#endif

            _settings.ApplyModifiedProperties();
        }
    }
}