﻿using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class DragDropProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new DragDropProvider();
        }

        #endregion

        private SerializedObject _serializedObject;
        private SerializedProperty _itemsProperty;

        private DragDropList _dragDropList;

        public DragDropProvider() : base("Project/Tooling/Drag Drop", SettingsScope.Project, new[] { "tooling", "drag", "drop" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _serializedObject = DragDropSettings.SerializedSingleton;
            if (_serializedObject == null)
                return;

            _itemsProperty = _serializedObject.FindProperty("items");
            _dragDropList = new DragDropList(_serializedObject, _itemsProperty);
        }

        public override void OnTitleBarGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUI.BeginDisabledGroup(
#if PCSOFT_RAYCASTER
                    false
#else
                    true
#endif
                );
                {
                    ExtendedEditorGUILayout.SymbolFieldLeft("Activate System", "PCSOFT_DRAGDROP");
                    EditorGUI.BeginDisabledGroup(
#if PCSOFT_DRAGDROP
                        false
#else
                        true
#endif
                    );
                    {
                        ExtendedEditorGUILayout.SymbolFieldLeft("Verbose Logging", "PCSOFT_DRAGDROP_LOGGING");
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }

        public override void OnGUI(string searchContext)
        {
            _serializedObject.Update();

            GUILayout.Space(15f);

#if PCSOFT_DRAGDROP && PCSOFT_RAYCASTER
            _dragDropList.DoLayoutList();
#elif !PCSOFT_RAYCASTER
            EditorGUILayout.HelpBox("Raycaster System deacrivated but required", MessageType.Warning);
#else
            EditorGUILayout.HelpBox("Drag Drop System deacrivated", MessageType.Info);
#endif

            _serializedObject.ApplyModifiedProperties();
        }
    }
}