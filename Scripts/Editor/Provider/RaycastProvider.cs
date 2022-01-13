using System.Linq;
using UnityEditor;
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

        public override void OnGUI(string searchContext)
        {
            _settings.Update();

            if (RaycastSettings.Singleton.Items.Any(x => string.IsNullOrEmpty(x.Key)))
            {
                EditorGUILayout.HelpBox("Some key values are empty.", MessageType.Warning);
            }

            if (RaycastSettings.Singleton.Items.GroupBy(x => x.Key).Any(x => x.Count() > 1))
            {
                EditorGUILayout.HelpBox("There are double key values.", MessageType.Warning);
            }
            
            _raycastList.DoLayoutList();

            _settings.ApplyModifiedProperties();
        }
    }
}