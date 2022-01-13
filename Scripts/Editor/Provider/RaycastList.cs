using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class RaycastList : ReorderableList
    {
        private const float LeftMargin = 15f;
        private const float BottomMargin = 2f;
        private const float ColumnSpace = 5f;

        private const float LayerMaskWidth = 150f;
        private const float DistanceWidth = 150f;
        private const float CheckCountWidth = 300f;
        private const float CommonWidth = LayerMaskWidth + DistanceWidth + CheckCountWidth;

        public RaycastList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            drawHeaderCallback += DrawHeaderCallback;
            drawElementCallback += DrawElementCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            var commonWidth = rect.width - (CommonWidth + LeftMargin);
            var pos = new Rect(rect.x + LeftMargin, rect.y, commonWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Key"));

            pos = new Rect(rect.x + LeftMargin + commonWidth, rect.y, LayerMaskWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Layer Mask"));
            
            pos = new Rect(rect.x + LeftMargin + commonWidth + LayerMaskWidth, rect.y, DistanceWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Maximum Distance"));
            
            pos = new Rect(rect.x + LeftMargin + commonWidth + LayerMaskWidth + DistanceWidth, rect.y, CheckCountWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Count of fixed update checks"));
        }

        private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var keyProperty = property.FindPropertyRelative("key");
            var layerMaskProperty = property.FindPropertyRelative("layerMask");
            var distanceProperty = property.FindPropertyRelative("maxDistance");
            var checkCountProperty = property.FindPropertyRelative("fixedCheckCount");

            var commonWidth = rect.width - CommonWidth;
            var pos = new Rect(rect.x, rect.y, commonWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, keyProperty, GUIContent.none);

            pos = new Rect(rect.x + commonWidth, rect.y, LayerMaskWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, layerMaskProperty, GUIContent.none);
            
            pos = new Rect(rect.x + commonWidth + LayerMaskWidth, rect.y, DistanceWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, distanceProperty, GUIContent.none);
            
            pos = new Rect(rect.x + commonWidth + LayerMaskWidth + DistanceWidth, rect.y, CheckCountWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, checkCountProperty, GUIContent.none);
        }
    }
}