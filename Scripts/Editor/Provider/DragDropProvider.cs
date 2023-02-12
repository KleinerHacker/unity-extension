using System.Linq;
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
        private SerializedProperty _raycasterRefProperty;
        private SerializedProperty _secondRaycasterProperty;
        private SerializedProperty _secondRaycasterRefProperty;

        public DragDropProvider() : base("Project/Tooling/Drag Drop", SettingsScope.Project, new[] { "tooling", "drag", "drop" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _serializedObject = DragDropSettings.SerializedSingleton;
            if (_serializedObject == null)
                return;

            _raycasterRefProperty = _serializedObject.FindProperty("raycasterReference");
            _secondRaycasterProperty = _serializedObject.FindProperty("useAlternativeRaycasterForMove");
            _secondRaycasterRefProperty = _serializedObject.FindProperty("raycasterMoveReference");
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
            var all = RaycastSettings.Singleton.Items.Select(x => x.Key).ToArray();
            var primarySelected = all.IndexOf(x => x == _raycasterRefProperty.stringValue);
            var newPrimarySelected = EditorGUILayout.Popup("Raycaster Reference", primarySelected, all);
            if (primarySelected != newPrimarySelected)
            {
                _raycasterRefProperty.stringValue = newPrimarySelected < 0 ? null : all[newPrimarySelected];
            }

            EditorGUILayout.PropertyField(_secondRaycasterProperty, new GUIContent("Use other raycaster for moving"));
            
            EditorGUI.BeginDisabledGroup(!_secondRaycasterProperty.boolValue);
            {
                var secondarySelected = all.IndexOf(x => x == _secondRaycasterRefProperty.stringValue);
                var newSecondarySelected = EditorGUILayout.Popup("Moving Raycaster Reference", secondarySelected, all);
                if (secondarySelected != newSecondarySelected)
                {
                    _secondRaycasterRefProperty.stringValue = newSecondarySelected < 0 ? null : all[newSecondarySelected];
                }
            }
            EditorGUI.EndDisabledGroup();
#elif !PCSOFT_RAYCASTER
            EditorGUILayout.HelpBox("Raycaster System deacrivated but required", MessageType.Warning);
#else
            EditorGUILayout.HelpBox("Drag Drop System deacrivated", MessageType.Info);
#endif

            _serializedObject.ApplyModifiedProperties();
        }
    }
}