
using System;
using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class PreviewProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new PreviewProvider();
        }

        #endregion

        private SerializedObject _serializedObject;
        private SerializedProperty _raycasterProperty;
        private SerializedProperty _itemsProperty;

        private string[] _raycastItems;

        private PreviewList _previewList;

        public PreviewProvider() : base("Project/Tooling/Preview System", SettingsScope.Project, new[] { "tooling", "preview" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _serializedObject = PreviewSettings.SerializedSingleton;
            if (_serializedObject == null)
                return;

            _raycasterProperty = _serializedObject.FindProperty("raycaster");
            _itemsProperty = _serializedObject.FindProperty("items");

            _raycastItems = RaycastSettings.Singleton.Items.Select(x => x.Key).ToArray();

            _previewList = new PreviewList(_serializedObject, _itemsProperty);
        }

        public override void OnTitleBarGUI()
        {
            GUILayout.BeginVertical();
            {
                EditorGUI.BeginDisabledGroup(
#if PCSOFT_RAYCASTER
                    false
#else
                    true
#endif
                );
                {
                    ExtendedEditorGUILayout.SymbolField("Activate System", "PCSOFT_PREVIEW");
                    EditorGUI.BeginDisabledGroup(
#if PCSOFT_PREVIEW
                        false
#else
                        true
#endif
                    );
                    {
                        ExtendedEditorGUILayout.SymbolField("Verbose Logging", "PCSOFT_PREVIEW_LOGGING");
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndVertical();
        }

        public override void OnGUI(string searchContext)
        {
            _serializedObject.Update();

            GUILayout.Space(15f);

#if PCSOFT_PREVIEW && PCSOFT_RAYCASTER
            EditorGUILayout.HelpBox("Setup existing preview objects to show or hide this with or setup defined states.", MessageType.None);
            
            if (string.IsNullOrEmpty(PreviewSettings.Singleton.Raycaster))
            {
                EditorGUILayout.HelpBox("Please choose a raycaster. A raycaster is required for this tooling!", MessageType.Error);
            }

            var index = _raycastItems.IndexOf(x => string.Equals(x, _raycasterProperty.stringValue, StringComparison.Ordinal));
            var newIndex = EditorGUILayout.Popup("Raycaster for preview:", index, _raycastItems);
            if (index != newIndex)
            {
                _raycasterProperty.stringValue = newIndex < 0 ? null : _raycastItems[newIndex];
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Previews", EditorStyles.boldLabel);
            if (PreviewSettings.Singleton.Items.Any(x => string.IsNullOrEmpty(x.Key)))
            {
                EditorGUILayout.HelpBox("Some keys are empty!", MessageType.Warning);
            }

            if (PreviewSettings.Singleton.Items.HasDoublets(x => x.Key))
            {
                EditorGUILayout.HelpBox("Some keys are defined multiple times", MessageType.Warning);
            }

            _previewList.DoLayoutList();
#elif !PCSOFT_RAYCASTER
            EditorGUILayout.HelpBox("Raycaster System deactivated but required", MessageType.Warning);
#else
            EditorGUILayout.HelpBox("Preview System deactivated", MessageType.Info);
#endif

            _serializedObject.ApplyModifiedProperties();
        }
    }
}