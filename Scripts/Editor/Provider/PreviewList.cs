using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class PreviewList : ReorderableList
    {
        private const float LeftMargin = 15f;
        private const float BottomMargin = 2f;
        private const float ColumnSpace = 5f;

        private const float AutoRotateWidth = 25f;
        private const float MaxRotateWidth = 150f;
        private const float PreviewWidth = 150f;
        private const float PositionWidth = 250f;
        private const float RotationWidth = 250f;
        private const float CommonWidth = AutoRotateWidth + MaxRotateWidth + PreviewWidth + PositionWidth + RotationWidth;

        public PreviewList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            drawHeaderCallback += DrawHeaderCallback;
            drawElementCallback += DrawElementCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            var commonWidth = rect.width - (CommonWidth + LeftMargin);
            var pos = new Rect(rect.x + LeftMargin, rect.y, commonWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Key"));

            pos = new Rect(rect.x + LeftMargin + commonWidth, rect.y, AutoRotateWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("AR", "Auto Rotation Support"));
            
            pos = new Rect(rect.x + LeftMargin + commonWidth + AutoRotateWidth, rect.y, MaxRotateWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Max Rotation"));
            
            pos = new Rect(rect.x + LeftMargin + commonWidth + AutoRotateWidth + MaxRotateWidth, rect.y, PreviewWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Preview Object"));
            
            pos = new Rect(rect.x + LeftMargin + commonWidth + AutoRotateWidth + MaxRotateWidth + PreviewWidth, rect.y, PositionWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Preview Basic Position"));
            
            pos = new Rect(rect.x + LeftMargin + commonWidth + AutoRotateWidth + MaxRotateWidth + PreviewWidth + PositionWidth, rect.y, RotationWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Preview Basic Rotation"));
        }

        private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var keyProperty = property.FindPropertyRelative("key");
            var autoRotateProperty = property.FindPropertyRelative("autoRotate");
            var maxRotationProperty = property.FindPropertyRelative("maxRotation");
            var previewProperty = property.FindPropertyRelative("previewPrefab");
            var positionProperty = property.FindPropertyRelative("previewPosition");
            var rotationProperty = property.FindPropertyRelative("previewRotation");

            var commonWidth = rect.width - CommonWidth;
            var pos = new Rect(rect.x, rect.y, commonWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, keyProperty, GUIContent.none);

            pos = new Rect(rect.x + commonWidth, rect.y, AutoRotateWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, autoRotateProperty, GUIContent.none);
            
            pos = new Rect(rect.x + commonWidth + AutoRotateWidth, rect.y, MaxRotateWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.BeginDisabledGroup(!autoRotateProperty.boolValue);
            EditorGUI.PropertyField(pos, maxRotationProperty, GUIContent.none);
            EditorGUI.EndDisabledGroup();
            
            pos = new Rect(rect.x + commonWidth + AutoRotateWidth + MaxRotateWidth, rect.y, PreviewWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, previewProperty, GUIContent.none);
            
            pos = new Rect(rect.x + commonWidth + AutoRotateWidth + MaxRotateWidth + PreviewWidth, rect.y, PositionWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, positionProperty, GUIContent.none);
            
            pos = new Rect(rect.x + commonWidth + AutoRotateWidth + MaxRotateWidth + PreviewWidth + PositionWidth, rect.y, RotationWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.BeginDisabledGroup(!autoRotateProperty.boolValue);
            EditorGUI.PropertyField(pos, rotationProperty, GUIContent.none);
            EditorGUI.EndDisabledGroup();
        }
    }
}