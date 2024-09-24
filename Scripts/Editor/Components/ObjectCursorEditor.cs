using System;
using System.Linq;
using UnityBase.Runtime.Projects.unity_base.Scripts.Runtime.Utils.Extensions;
using UnityCommons.Runtime.Projects.unity_commons.Scripts.Runtime.Assets;
using UnityEditor;
using UnityEditorEx.Editor.Projects.unity_editor_ex.Scripts.Editor;
using UnityEngine;
using UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Components;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.Components
{
    [CustomEditor(typeof(ObjectCursor))]
    public sealed class ObjectCursorEditor : ExtendedEditor
    {
        private SerializedProperty cursorProperty;
        private string[] cursors;
        
        private void OnEnable()
        {
            cursorProperty = serializedObject.FindProperty("cursorKey");
            cursors = CursorSettings.Singleton.Items.Select(x => x.Identifier).ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawCursorPopup();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCursorPopup()
        {
            var index = CursorSettings.Singleton.Items.IndexOf(x => x.Identifier == cursorProperty.stringValue);
            var newIndex = EditorGUILayout.Popup(index, cursors);
            if (newIndex != index)
            {
                cursorProperty.stringValue = cursors[newIndex];
            }
        }
    }
}