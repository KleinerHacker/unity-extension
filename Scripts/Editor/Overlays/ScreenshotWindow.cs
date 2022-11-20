using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions;

namespace UnityExtension.Editor.extension.Scripts.Editor.Overlays
{
    public sealed class ScreenshotWindow : EditorWindow
    {
        private static readonly Vector2 size = new Vector2(500, 350);
        
        #region Properties

        public ScreenshotWindowType Type { get; set; }

        #endregion
        
        private Texture2D _screenshot;
        private bool _hideUI;

        private void OnEnable()
        {
            titleContent = new GUIContent("Screenshot - " + (Type == ScreenshotWindowType.GameView ? "Game" : "Editor"));
            minSize = size;
            maxSize = size;
            
            _screenshot = CreateScreenshot();
        }

        private void OnGUI()
        {
            var height = 500 * _screenshot.height / _screenshot.width;
            GUI.DrawTexture(new Rect(0, 0, 500, height), _screenshot);

            GUILayout.Space(height + 2f);
            
            GUILayout.BeginHorizontal();
            {
                _hideUI = GUILayout.Toggle(_hideUI, "Try hide UI");
            }
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Update Screenshot"))
            {
                _screenshot = CreateScreenshot();
            }
        }

        private Texture2D CreateScreenshot()
        {
            var canvases = new List<Canvas>();
            if (_hideUI)
            {
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    foreach (var gameObject in scene.GetRootGameObjects())
                    {
                        canvases.AddRange(
                            gameObject.FindComponents<Canvas>()
                                .Where(x => x.gameObject.activeSelf)
                                .Where(x => x.renderMode != RenderMode.WorldSpace)
                        );
                    }
                }

                foreach (var canvas in canvases)
                {
                    canvas.gameObject.SetActive(false);
                }
            }

            try
            {
                return Type switch
                {
                    ScreenshotWindowType.GameView => CreateGameScreenshot(),
                    ScreenshotWindowType.SceneView => CreateEditorScreenshot(),
                    _ => throw new NotImplementedException(Type.ToString())
                };
            }
            finally
            {
                foreach (var canvas in canvases)
                {
                    canvas.gameObject.SetActive(true);
                }
            }

            Texture2D CreateEditorScreenshot()
            {
                var camera = SceneView.lastActiveSceneView.camera;
                var cameraTargetTexture = camera.activeTexture;

                var currentRT = RenderTexture.active;
                RenderTexture.active = cameraTargetTexture;

                camera.Render();

                var image = new Texture2D(cameraTargetTexture.width, cameraTargetTexture.height);
                image.ReadPixels(new Rect(0, 0, cameraTargetTexture.width, cameraTargetTexture.height), 0, 0);
                image.Apply();
                RenderTexture.active = currentRT;

                return image;
            }

            Texture2D CreateGameScreenshot()
            {
                return ScreenCapture.CaptureScreenshotAsTexture();
            }
        }
    }

    public enum ScreenshotWindowType
    {
        SceneView,
        GameView
    }
}