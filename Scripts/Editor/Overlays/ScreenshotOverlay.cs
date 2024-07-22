using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.Overlays
{
    [Overlay(typeof(SceneView), "Screenshot")]
    public class ScreenshotOverlay : ToolbarOverlay
    {
        public ScreenshotOverlay() : base("UnityExtensions/ScreenshotEditorButton", "UnityExtensions/ScreenshotGameButton")
        {
        }
    }

    [EditorToolbarElement("UnityExtensions/ScreenshotEditorButton", typeof(SceneView))]
    public class ScreenshotButton : EditorToolbarButton
    {
        public ScreenshotButton()
        {
            tooltip = "Create a screenshot from current editor camera";
            icon = (Texture2D)EditorGUIUtility.IconContent("d_Image Icon").image;
            clicked += OnClicked;
        }

        private void OnClicked()
        {
            var window = ScriptableObject.CreateInstance<ScreenshotWindow>();
            window.Type = ScreenshotWindowType.SceneView;
            window.Show();
        }
    }
    
    [EditorToolbarElement("UnityExtensions/ScreenshotGameButton", typeof(SceneView))]
    public class ScreenshotGameButton : EditorToolbarButton
    {
        public ScreenshotGameButton()
        {
            tooltip = "Create a screenshot from current game camera";
            icon = (Texture2D)EditorGUIUtility.IconContent("d_AspectRatioFitter Icon").image;
            clicked += OnClicked;
        }

        private void OnClicked()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("Screenshot", "To create a screenshot from game view start the game first.", "OK");
                return;
            }
            
            var window = ScriptableObject.CreateInstance<ScreenshotWindow>();
            window.Type = ScreenshotWindowType.GameView;
            window.Show();
        }
    }
}