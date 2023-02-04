using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils.Extensions;
using UnityEditorInternal;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class EnvironmentTargetGroupList : ReorderableList
    {
        private const float HeaderMarginLeft = 15f;

        public EnvironmentTargetGroupList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            drawHeaderCallback += DrawHeaderCallback;
            drawElementCallback += DrawElementCallback;
            elementHeightCallback += ElementHeightCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            GUI.Label(new Rect(rect.x + HeaderMarginLeft, rect.y, 150f, 20f), new GUIContent("Name", null, "Used as identifier"), EditorStyles.boldLabel);
            GUI.Label(new Rect(rect.x + HeaderMarginLeft + 150f, rect.y, 100f, 20f), new GUIContent("Windows", EditorGUIUtility.IconContent("BuildSettings.Metro On").image));
            GUI.Label(new Rect(rect.x + HeaderMarginLeft + 255f, rect.y, 100f, 20f), new GUIContent("Linux", EditorGUIUtility.IconContent("BuildSettings.Lumin On").image));
            GUI.Label(new Rect(rect.x + HeaderMarginLeft + 360f, rect.y, 100f, 20f), new GUIContent("Mac", EditorGUIUtility.IconContent("BuildSettings.Standalone On").image));
            GUI.Label(new Rect(rect.x + HeaderMarginLeft + 465f, rect.y, 100f, 20f), new GUIContent("Android", EditorGUIUtility.IconContent("BuildSettings.Android On").image));
            GUI.Label(new Rect(rect.x + HeaderMarginLeft + 570f, rect.y, 100f, 20f), new GUIContent("IOS", EditorGUIUtility.IconContent("BuildSettings.iPhone On").image));
        }

        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(index);
            var nameProp = prop.FindPropertyRelative("name");
            var itemsProp = prop.FindPropertyRelative("items");

            EditorGUI.PropertyField(new Rect(rect.x, rect.y + 1f, 150f, 20f), nameProp, GUIContent.none);
            DrawGroupPlatform(new Rect(rect.x + 155f, rect.y, 100f, rect.height), EnvironmentSupportedPlatform.Windows, nameProp.stringValue,
                itemsProp, "windows");
            DrawGroupPlatform(new Rect(rect.x + 260f, rect.y, 100f, rect.height), EnvironmentSupportedPlatform.Linux, nameProp.stringValue,
                itemsProp, "linux");
            DrawGroupPlatform(new Rect(rect.x + 365f, rect.y, 100f, rect.height), EnvironmentSupportedPlatform.Mac, nameProp.stringValue,
                itemsProp, "mac");
            DrawGroupPlatform(new Rect(rect.x + 470f, rect.y, 100f, rect.height), EnvironmentSupportedPlatform.Android, nameProp.stringValue,
                itemsProp, "android");
            DrawGroupPlatform(new Rect(rect.x + 575f, rect.y, 100f, rect.height), EnvironmentSupportedPlatform.IOS, nameProp.stringValue,
                itemsProp, "ios");
        }

        private float ElementHeightCallback(int i)
        {
            var windowsProps = serializedProperty.serializedObject.FindProperties("windows");
            var linuxProps = serializedProperty.serializedObject.FindProperties("linux");
            var macProps = serializedProperty.serializedObject.FindProperties("mac");
            var androidProps = serializedProperty.serializedObject.FindProperties("android");
            var iosProps = serializedProperty.serializedObject.FindProperties("ios");

            return Mathf.Max(windowsProps.Length, linuxProps.Length, macProps.Length, androidProps.Length, iosProps.Length) * 20f;
        }

        private void DrawGroupPlatform(Rect rect, EnvironmentSupportedPlatform platform, string groupName, SerializedProperty groupItemsProp, string platformName)
        {
            var platformGroupProps = groupItemsProp
                .Where(x => x.FindPropertyRelative("platform").intValue == (int)platform)
                .ToArray();
            var platformProps = serializedProperty.serializedObject.FindProperties(platformName);
            
            for (var i = 0; i < platformProps.Length; i++)
            {
                var p = platformProps[i];
                var platformPropName = p.GetRelativeString("name");

                var active = platformGroupProps.Any(x => x.EqualsString("name", p, "name"));
                var newActive = EditorGUI.ToggleLeft(new Rect(rect.x, rect.y + i * 20f, rect.width, 20f), platformPropName, active);
                if (active != newActive)
                {
                    var group = EnvironmentDetectionSettings.Singleton.Groups
                        .First(x => x.Name == groupName);
                    if (newActive)
                    {
                        group.Items = group.Items
                            .Append(new EnvironmentTargetGroupItem
                            {
                                Platform = platform,
                                Name = platformPropName
                            })
                            .ToArray();
                    }
                    else
                    {
                        group.Items = group.Items
                            .RemoveIf(x => x.Platform == platform && x.Name == platformPropName)
                            .ToArray();
                    }
                    
                    EditorUtility.SetDirty(EnvironmentDetectionSettings.Singleton);
                }
            }
        }
    }
}