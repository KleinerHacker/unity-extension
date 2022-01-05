using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class CursorProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateCursorSettingsProvider()
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
        
        public CursorProvider() : base("Project/Player/Cursors", SettingsScope.Project, new [] { "Tooling", "Cursor", "Mouse" })
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

        public override void OnGUI(string searchContext)
        {
            _settings.Update();

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

            _settings.ApplyModifiedProperties();
        }
    }
}