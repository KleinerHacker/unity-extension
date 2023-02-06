#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public abstract class EnvironmentTargetList : ReorderableList
    {
        private const float HeaderMarginLeft = 15f;

        protected EnvironmentTargetList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            drawHeaderCallback += DrawHeaderCallback;
            drawElementCallback += DrawElementCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            GUI.Label(new Rect(rect.x + HeaderMarginLeft, rect.y, 150f, 20f), new GUIContent("Name", null, "Used as identifier"), EditorStyles.boldLabel);
            GUI.Label(new Rect(rect.x + HeaderMarginLeft + 177.5f, rect.y, 20f, 20f), new GUIContent(EditorGUIUtility.IconContent("d_InputField Icon").image, "Keyboard"));
            GUI.Label(new Rect(rect.x + HeaderMarginLeft + 202.5f, rect.y, 20f, 20f), new GUIContent(EditorGUIUtility.IconContent("d_EventTrigger Icon").image, "Mouse"));
            GUI.Label(new Rect(rect.x + HeaderMarginLeft + 227.5f, rect.y, 20f, 20f), new GUIContent(EditorGUIUtility.IconContent("d_TouchInputModule Icon").image, "Touch"));
            GUI.Label(new Rect(rect.x + HeaderMarginLeft + 252.5f, rect.y, 20f, 20f), new GUIContent(EditorGUIUtility.IconContent("d_UnityEditor.GameView").image, "Gamepad"));

            DoDrawHeaderCallback(new Rect(rect.x + HeaderMarginLeft + 305f, rect.y, rect.width - 305f, rect.height));
        }

        private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i);
            var nameProp = prop.FindPropertyRelative("name");
            var keyboardProp = prop.FindPropertyRelative("requiresKeyboard");
            var mouseProp = prop.FindPropertyRelative("requiresMouse");
            var touchProp = prop.FindPropertyRelative("requiresTouch");
            var gamepadProp = prop.FindPropertyRelative("requiresGamepad");

            EditorGUI.PropertyField(new Rect(rect.x, rect.y + 1f, 150f, 20f), nameProp, GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 180f, rect.y, 20f, 20f), keyboardProp, GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 205f, rect.y, 20f, 20f), mouseProp, GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 230f, rect.y, 20f, 20f), touchProp, GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 255f, rect.y, 20f, 20f), gamepadProp, GUIContent.none);

            DoDrawElementCallback(prop, new Rect(rect.x + 305f, rect.y, rect.width - 305f, rect.height), i, isactive, isfocused);
        }

        protected virtual void DoDrawHeaderCallback(Rect rect)
        {
            //Empty
        }

        protected virtual void DoDrawElementCallback(SerializedProperty selectedProperty, Rect rect, int i, bool isactive, bool isfocused)
        {
            //Empty
        }
    }

    public sealed class WindowsEnvironmentTargetList : EnvironmentTargetList
    {
        public WindowsEnvironmentTargetList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
        }
    }

    public sealed class LinuxEnvironmentTargetList : EnvironmentTargetList
    {
        public LinuxEnvironmentTargetList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
        }

        protected override void DoDrawHeaderCallback(Rect rect)
        {
#if PCSOFT_ENV_STEAM && STEAMWORKS_NET
            GUI.Label(new Rect(rect.x-2.5f, rect.y, 20f, 20f), new GUIContent(EditorGUIUtility.IconContent("d_BuildSettings.PSP2.Small").image, "Steam Deck"));
#endif
        }

        protected override void DoDrawElementCallback(SerializedProperty selectedProperty, Rect rect, int i, bool isactive, bool isfocused)
        {
#if PCSOFT_ENV_STEAM && STEAMWORKS_NET
            var steamDeckProperty = selectedProperty.FindPropertyRelative("requiresSteamDeck");
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 20f, 20f), steamDeckProperty, GUIContent.none);
#endif
        }
    }

    public sealed class MacEnvironmentTargetList : EnvironmentTargetList
    {
        public MacEnvironmentTargetList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
        }
    }

    public sealed class AndroidEnvironmentTargetList : EnvironmentTargetList
    {
        public AndroidEnvironmentTargetList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
        }

        protected override void DoDrawHeaderCallback(Rect rect)
        {
            GUI.Label(new Rect(rect.x - 2.5f, rect.y, 20f, 20f), new GUIContent(EditorGUIUtility.IconContent("d_UnityEditor.Timeline.TimelineWindow").image, "TV"));
        }

        protected override void DoDrawElementCallback(SerializedProperty selectedProperty, Rect rect, int i, bool isactive, bool isfocused)
        {
            var tvProp = selectedProperty.FindPropertyRelative("requiresTv");

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 20f, 20f), tvProp, GUIContent.none);
        }
    }

    public sealed class IOSEnvironmentTargetList : EnvironmentTargetList
    {
        public IOSEnvironmentTargetList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
        }
    }
}