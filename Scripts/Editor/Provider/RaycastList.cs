using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEngine;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class RaycastList : TableReorderableList
    {
        public RaycastList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            Columns.Add(new FlexibleColumn { HeaderText = "Key", MaxHeight = 20f, ElementCallback = KeyElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Layer Mask", AbsoluteWidth = 100f, ElementCallback = LayerMaskElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Raycast Type", AbsoluteWidth = 100f, ElementCallback = RaycastTypeElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Multi Hits", AbsoluteWidth = 150f, MaxHeight = 20f, ElementCallback = MultiHitsElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Over UI", AbsoluteWidth = 50f, ElementCallback = OverUIElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Raycast Offset", AbsoluteWidth = 100f, ElementCallback = OffsetElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Max Dist", AbsoluteWidth = 60f, MaxHeight = 20f, ElementCallback = MaxDistanceElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Fixed Update Delay", AbsoluteWidth = 150f, MaxHeight = 20f, ElementCallback = UpdateCountElementCallback });
        }

        private void KeyElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var keyProperty = property.FindPropertyRelative("key");
            EditorGUI.PropertyField(rect, keyProperty, GUIContent.none);
        }

        private void LayerMaskElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var layerMaskProperty = property.FindPropertyRelative("layerMask");
            EditorGUI.PropertyField(rect, layerMaskProperty, GUIContent.none);
        }

        private void RaycastTypeElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var typeProperty = property.FindPropertyRelative("type");
            EditorGUI.PropertyField(rect, typeProperty, GUIContent.none);
        }

        private void MultiHitsElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var countOfHitsProperty = property.FindPropertyRelative("countOfHits");
            EditorGUI.PropertyField(rect, countOfHitsProperty, GUIContent.none);
        }

        private void OverUIElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var overUIProperty = property.FindPropertyRelative("overUI");
            EditorGUI.PropertyField(rect, overUIProperty, GUIContent.none);
        }

        private void OffsetElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var offsetProperty = property.FindPropertyRelative("offset");
            EditorGUI.PropertyField(rect, offsetProperty, GUIContent.none);
        }

        private void MaxDistanceElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var distanceProperty = property.FindPropertyRelative("maxDistance");
            EditorGUI.PropertyField(rect, distanceProperty, GUIContent.none);
        }

        private void UpdateCountElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var checkCountProperty = property.FindPropertyRelative("fixedCheckCount");
            EditorGUI.PropertyField(rect, checkCountProperty, GUIContent.none);
        }
    }
}