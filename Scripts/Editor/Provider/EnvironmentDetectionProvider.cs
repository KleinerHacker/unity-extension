using System;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils.Extensions;
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
        private SerializedProperty[] _itemsProperties;
        private SerializedProperty _itemsProperty;
        
        public EnvironmentDetectionProvider() : base("Project/Player/Environment", SettingsScope.Project, new []{"Tooling", "Environment"})
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = EnvironmentDetectionSettings.SerializedSingleton;
            if (_settings == null)
                return;

            _itemsProperty = _settings.FindProperty("items");
            _itemsProperties = _settings.FindProperties("items");
        }

        public override void OnGUI(string searchContext)
        {
            _settings.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Environment Constraints", EditorStyles.boldLabel);
            EditorGUILayout.Space(1f, true);
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), EditorStyles.iconButton))
            {
                AddConstraint();
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Minus"), EditorStyles.iconButton))
            {
                RemoveConstraint();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Setup conditions to detect a specific environment to optimize your code to interact with this environment.", MessageType.None);
            EditorGUILayout.Space();

            foreach (var itemsProperty in _itemsProperties)
            {
                EditorGUILayout.PropertyField(itemsProperty, new GUIContent(itemsProperty.FindPropertyRelative("name").stringValue));
            }
            
            _settings.ApplyModifiedProperties();
        }

        private void AddConstraint()
        {
            _itemsProperty.InsertArrayElementAtIndex(_itemsProperty.arraySize);
            var property = _itemsProperty.GetArrayElementAtIndex(_itemsProperty.arraySize - 1);
            property.FindPropertyRelative("guid").stringValue = null;
            property.FindPropertyRelative("name").stringValue = Guid.NewGuid().ToString();
            
            UpdateItems();
        }

        private void RemoveConstraint()
        {
            var genericMenu = new GenericMenu();
            foreach (var itemsProperty in _itemsProperties)
            {
                var name = itemsProperty.FindPropertyRelative("name").stringValue;
                genericMenu.AddItem(new GUIContent(name), false, () =>
                {
                    if (EditorUtility.DisplayDialog("Remove environment constraint", "You are sure to remove environment constraint '" + name + "'?", "Yes", "No"))
                    {
                        var indexOf = _itemsProperty.IndexOf(x => x.FindPropertyRelative("name").stringValue == name);
                        if (indexOf >= 0)
                        {
                            _itemsProperty.DeleteArrayElementAtIndex(indexOf);
                            UpdateItems();
                        }
                    }
                });
            }
            genericMenu.ShowAsContext();
        }

        private void UpdateItems()
        {
            _settings.ApplyModifiedProperties();
            _settings.Update();

            _itemsProperties = _settings.FindProperties("items");
        }
    }
}