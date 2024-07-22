using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.Provider
{
    public sealed class CursorList : ReorderableList
    {
        private const float LeftMargin = 15f;
        private const float BottomMargin = 2f;
        private const float ColumnSpace = 5f;

        private const float HotspotWidth = 150f;
        private const float SpriteWidth = 300f;
        private const float CommonWidth = HotspotWidth + SpriteWidth;

        public CursorList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            drawHeaderCallback += DrawHeaderCallback;
            drawElementCallback += DrawElementCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            var commonWidth = rect.width - (CommonWidth + LeftMargin);
            var pos = new Rect(rect.x + LeftMargin, rect.y, commonWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Key"));

            pos = new Rect(rect.x + LeftMargin + commonWidth, rect.y, HotspotWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Hotspot"));
            
            pos = new Rect(rect.x + LeftMargin + commonWidth + HotspotWidth, rect.y, SpriteWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Cursor"));
        }

        private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var identifierProperty = property.FindPropertyRelative("identifier");
            var hotspotProperty = property.FindPropertyRelative("hotspot");
            var spriteProperty = property.FindPropertyRelative("cursor");

            var commonWidth = rect.width - CommonWidth;
            var pos = new Rect(rect.x, rect.y, commonWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, identifierProperty, GUIContent.none);

            pos = new Rect(rect.x + commonWidth, rect.y, HotspotWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, hotspotProperty, GUIContent.none);
            
            pos = new Rect(rect.x + commonWidth + HotspotWidth, rect.y, SpriteWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, spriteProperty, GUIContent.none);
        }
    }
}