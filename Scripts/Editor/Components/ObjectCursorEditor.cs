#if PCSOFT_CURSOR
using System.Linq;
using UnityBase.Runtime.Projects.unity_base.Scripts.Runtime.Utils.Extensions;
using UnityCommons.Runtime.Projects.unity_commons.Scripts.Runtime.Assets;
using UnityEditor;
using UnityEditorEx.Editor.Projects.unity_editor_ex.Scripts.Editor;
using UnityEngine;
using UnityExtensions.Runtime.Projects.unity_extensions.Scripts.Runtime.Components;

namespace UnityExtensions.Editor.Projects.unity_extensions.Scripts.Editor.Components
{
    [CustomEditor(typeof(ObjectCursor))]
    public sealed class ObjectCursorEditor : ExtendedEditor
    {
        private SerializedProperty cursorProperty;
        private GUIContent[] cursors;
        
        private void OnEnable()
        {
            cursorProperty = serializedObject.FindProperty("cursorKey");
            cursors = CursorSettings.Singleton.Items.Select(x => new GUIContent(x.Identifier, x.Cursor)).ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Cursor:");
                DrawCursorPopup();
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCursorPopup()
        {
            var index = CursorSettings.Singleton.Items.IndexOf(x => x.Identifier == cursorProperty.stringValue);
            var newIndex = EditorGUILayout.Popup(index, cursors);
            if (newIndex != index)
            {
                cursorProperty.stringValue = cursors[newIndex].text;
            }
        }
    }
}
#endif