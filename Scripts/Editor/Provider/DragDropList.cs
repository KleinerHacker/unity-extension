using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils.Extensions;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class DragDropList : TableReorderableList
    {
        public DragDropList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            Columns.Add(new FixedColumn { HeaderText = "", AbsoluteWidth = 20f, ElementCallback = ActivateElementCallback });
            Columns.Add(new FlexibleColumn { HeaderText = "Name", MaxHeight = 20f, ElementCallback = NameElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Hit Type", AbsoluteWidth = 150f, ElementCallback = HitTypeElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "", AbsoluteWidth = 20f, MaxHeight = 20f, ElementCallback = HitTypeHintElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Primary Raycaster", AbsoluteWidth = 150f, ElementCallback = PrimaryRaycasterElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "", AbsoluteWidth = 20f, ElementCallback = SecondaryRaycasterRequiredElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Secondary Raycaster", AbsoluteWidth = 150f, ElementCallback = SecondaryRaycasterElementCallback });
        }

        private void ActivateElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var activeProperty = property.FindPropertyRelative("active");

            EditorGUI.PropertyField(rect, activeProperty, GUIContent.none);
        }

        private void NameElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var nameProperty = property.FindPropertyRelative("name");

            EditorGUI.PropertyField(rect, nameProperty, GUIContent.none);
        }

        private void HitTypeElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var hitTypeProperty = property.FindPropertyRelative("hitType");

            EditorGUI.PropertyField(rect, hitTypeProperty, GUIContent.none);
        }

        private void HitTypeHintElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var hitTypeProperty = property.FindPropertyRelative("hitType");
            var raycasterRefProperty = property.FindPropertyRelative("raycasterReference");

            var raycasterItem = RaycastSettings.Singleton.Items.FirstOrDefault(x => x.Key == raycasterRefProperty.stringValue);
            if (hitTypeProperty.GetEnum<DragDropHitType>() != DragDropHitType.FirstHit && (raycasterItem?.CountOfHits ?? 0) < 2)
            {
                GUI.Label(rect, new GUIContent(EditorGUIUtility.IconContent("console.warnicon.sml").image,
                    "Raycaster '" + raycasterRefProperty.stringValue + "' has only one hit"));
            }
        }

        private void PrimaryRaycasterElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var primaryRaycasterProperty = property.FindPropertyRelative("raycasterReference");

            var all = RaycastSettings.Singleton.Items.Select(x => x.Key).ToArray();
            var primarySelected = all.IndexOf(x => x == primaryRaycasterProperty.stringValue);
            var newPrimarySelected = EditorGUI.Popup(rect, primarySelected, all);
            if (primarySelected != newPrimarySelected)
            {
                primaryRaycasterProperty.stringValue = newPrimarySelected < 0 ? null : all[newPrimarySelected];
            }
        }

        private void SecondaryRaycasterRequiredElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var secondaryRaycasterRequiredProperty = property.FindPropertyRelative("useAlternativeRaycasterForMove");

            EditorGUI.PropertyField(rect, secondaryRaycasterRequiredProperty, GUIContent.none);
        }

        private void SecondaryRaycasterElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var secondaryRaycasterRequiredProperty = property.FindPropertyRelative("useAlternativeRaycasterForMove");
            var secondaryRaycasterProperty = property.FindPropertyRelative("raycasterMoveReference");

            var all = RaycastSettings.Singleton.Items.Select(x => x.Key).ToArray();
            EditorGUI.BeginDisabledGroup(!secondaryRaycasterRequiredProperty.boolValue);
            {
                var secondarySelected = all.IndexOf(x => x == secondaryRaycasterProperty.stringValue);
                var newSecondarySelected = EditorGUI.Popup(rect, secondarySelected, all);
                if (secondarySelected != newSecondarySelected)
                {
                    secondaryRaycasterProperty.stringValue = newSecondarySelected < 0 ? null : all[newSecondarySelected];
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}