using System;
using System.Linq;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils.Extensions;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components;

namespace UnityExtension.Editor.extension.Scripts.Editor.Components
{
    [CustomEditor(typeof(RaycasterController))]
    public sealed class RaycasterControllerEditor : ExtendedEditor
    {
        private SerializedProperty _raycastProperty;
        
        private void OnEnable()
        {
            _raycastProperty = serializedObject.FindProperty("raycasters");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            foreach (var item in RaycastSettings.Singleton.Items)
            {
                var active = _raycastProperty.Any(x => string.Equals(x.stringValue, item.Key, StringComparison.Ordinal));
                var newActive = EditorGUILayout.Toggle(item.Key, active);
                if (active != newActive)
                {
                    if (newActive)
                    {
                        _raycastProperty.InsertArrayElementAtIndex(0);
                        var property = _raycastProperty.GetArrayElementAtIndex(0);
                        property.stringValue = item.Key;
                    }
                    else
                    {
                        var indexOf = _raycastProperty.IndexOf(x => string.Equals(x.stringValue, item.Key, StringComparison.Ordinal));
                        if (indexOf >= 0)
                        {
                            _raycastProperty.DeleteArrayElementAtIndex(indexOf);
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}