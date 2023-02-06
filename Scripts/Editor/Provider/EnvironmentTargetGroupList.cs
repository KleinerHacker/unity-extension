using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils.Extensions;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class EnvironmentTargetGroupList : TableReorderableList
    {
        public EnvironmentTargetGroupList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            Columns.Add(new FixedColumn {Header = new GUIContent("Name", null, "Used as identifier"), AbsoluteWidth = 150f, MaxHeight = 20f, ElementCallback = NameElementCallback});
            Columns.Add(new FixedColumn {Header = new GUIContent("Windows", EditorGUIUtility.IconContent("BuildSettings.Metro On").image), AbsoluteWidth = 100f, ElementCallback = WindowsElementCallback});
            Columns.Add(new FixedColumn {Header = new GUIContent("Linux", Resources.Load<Texture2D>("linux")), AbsoluteWidth = 100f, ElementCallback = LinuxElementCallback});
            Columns.Add(new FixedColumn {Header = new GUIContent("Mac", EditorGUIUtility.IconContent("BuildSettings.Standalone On").image), AbsoluteWidth = 100f, ElementCallback = MacElementCallback});
            Columns.Add(new FixedColumn {Header = new GUIContent("Android", EditorGUIUtility.IconContent("BuildSettings.Android On").image), AbsoluteWidth = 100f, ElementCallback = AndroidElementCallback});
            Columns.Add(new FixedColumn {Header = new GUIContent("IOS", EditorGUIUtility.IconContent("BuildSettings.iPhone On").image), AbsoluteWidth = 100f, ElementCallback = IOSElementCallback});
        }

        private void NameElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i);
            var nameProp = prop.FindPropertyRelative("name");
            EditorGUI.PropertyField(rect, nameProp, GUIContent.none);
        }

        private void WindowsElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i);
            var itemsProp = prop.FindPropertyRelative("items");
            
            DrawGroupPlatform(rect, EnvironmentSupportedPlatform.Windows, itemsProp, "windows");
        }
        
        private void LinuxElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i);
            var itemsProp = prop.FindPropertyRelative("items");
            
            DrawGroupPlatform(rect, EnvironmentSupportedPlatform.Linux, itemsProp, "linux");
        }
        
        private void MacElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i);
            var itemsProp = prop.FindPropertyRelative("items");
            
            DrawGroupPlatform(rect, EnvironmentSupportedPlatform.Mac, itemsProp, "mac");
        }
        
        private void AndroidElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i);
            var itemsProp = prop.FindPropertyRelative("items");
            
            DrawGroupPlatform(rect, EnvironmentSupportedPlatform.Android, itemsProp, "android");
        }
        
        private void IOSElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i);
            var itemsProp = prop.FindPropertyRelative("items");
            
            DrawGroupPlatform(rect, EnvironmentSupportedPlatform.IOS, itemsProp, "ios");
        }

        private void DrawGroupPlatform(Rect rect, EnvironmentSupportedPlatform platform, SerializedProperty groupItemsProp, string platformName)
        {
            var platformGroupProp = groupItemsProp
                .Where(x => x.FindPropertyRelative("platform").intValue == (int)platform)
                .FirstOrDefault();
            var platformProps = serializedProperty.serializedObject.FindProperties(platformName);

            var all = platformProps.Select(x => x.GetRelativeString("name")).ToArray();
            var selected = all.IndexOf(x => platformGroupProp?.GetRelativeString("targetName") == x);
            var newSelected = EditorGUI.Popup(rect, selected, all);
            if (selected != newSelected)
            {
                platformGroupProp.SetRelativeString("targetName", newSelected < 0 ? null : all[newSelected]);
            }
        }
    }
}