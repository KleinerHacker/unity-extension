using System.Linq;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class RaycastProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new RaycastProvider();
        }

        #endregion

        private SerializedObject _settings;
        private SerializedProperty _itemsProperty;

        private RaycastList _raycastList;

        public RaycastProvider() : base("Project/Physics/Raycast", SettingsScope.Project, new[] { "Tooling", "Physics", "Raycast", "Mouse", "Pointer", "Click" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = RaycastSettings.SerializedSingleton;
            if (_settings == null)
                return;

            _itemsProperty = _settings.FindProperty("items");

            _raycastList = new RaycastList(_settings, _itemsProperty);
        }

        public override void OnTitleBarGUI()
        {
            GUILayout.BeginVertical();
            {
                ExtendedEditorGUILayout.SymbolField("Activate System", "PCSOFT_RAYCASTER");
                EditorGUI.BeginDisabledGroup(
#if PCSOFT_RAYCASTER
                    false
#else
                    true
#endif
                );
                {
                    ExtendedEditorGUILayout.SymbolField("Verbose Logging", "PCSOFT_RAYCASTER_LOGGING");
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndVertical();
        }

        public override void OnGUI(string searchContext)
        {
            _settings.Update();

            GUILayout.Space(15f);

#if PCSOFT_RAYCASTER
            EditorGUILayout.BeginHorizontal(GUILayout.Width(300f));
            {
                ExtendedEditorGUILayout.SymbolFieldLeft(new GUIContent("Mouse Support", EditorGUIUtility.IconContent("d_EventTrigger Icon").image), "PCSOFT_RAYCASTER_MOUSE");
                ExtendedEditorGUILayout.SymbolFieldLeft(new GUIContent("Touch Support", EditorGUIUtility.IconContent("d_TouchInputModule Icon").image), "PCSOFT_RAYCASTER_TOUCH");
                ExtendedEditorGUILayout.SymbolFieldLeft(new GUIContent("Pen Support", EditorGUIUtility.IconContent("d_editicon.sml").image), "PCSOFT_RAYCASTER_PEN");
            }
            EditorGUILayout.EndHorizontal();

#if !PCSOFT_RAYCASTER_MOUSE && !PCSOFT_RAYCASTER_PEN && !PCSOFT_RAYCASTER_TOUCH
            EditorGUILayout.HelpBox("To use raycaster select one of the devices above.", MessageType.Warning);
#endif

            if (RaycastSettings.Singleton.Items.Any(x => string.IsNullOrEmpty(x.Key)))
            {
                EditorGUILayout.HelpBox("Some key values are empty.", MessageType.Warning);
            }

            if (RaycastSettings.Singleton.Items.GroupBy(x => x.Key).Any(x => x.Count() > 1))
            {
                EditorGUILayout.HelpBox("There are double key values.", MessageType.Warning);
            }

            _raycastList.DoLayoutList();
#else
            EditorGUILayout.HelpBox("Raycaster System deactivated", MessageType.Info);
#endif

            _settings.ApplyModifiedProperties();
        }
    }
}