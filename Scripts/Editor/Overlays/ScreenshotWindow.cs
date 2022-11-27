using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions;

namespace UnityExtension.Editor.extension.Scripts.Editor.Overlays
{
    public sealed class ScreenshotWindow : EditorWindow
    {
        private static readonly Vector2 Size = new Vector2(500, 500);

        #region Properties

        public ScreenshotWindowType Type
        {
            get => _type;
            set
            {
                _type = value;
                CreateScreenshot(tex => _screenshot = tex);
            }
        }

        #endregion

        private Texture2D _screenshot;
        private bool _hideUI;
        private ScreenshotWindowType _type;

        private void OnEnable()
        {
            titleContent = new GUIContent("Screenshot - " + (Type == ScreenshotWindowType.GameView ? "Game" : "Editor"));
            minSize = Size;
            maxSize = Size;

            CreateScreenshot(tex => _screenshot = tex);
        }

        private void OnGUI()
        {
            GUI.DrawTexture(new Rect(0f, 0f, 500f, 300f), _screenshot, ScaleMode.ScaleToFit);

            GUILayout.Space(302f);

            GUILayout.BeginHorizontal();
            {
                _hideUI = GUILayout.Toggle(_hideUI, "Try hide UI");
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Update Screenshot"))
            {
                CreateScreenshot(tex => _screenshot = tex);
            }

            if (GUILayout.Button("Save Screenshot"))
            {
                var fileName = EditorUtility.SaveFilePanel("Save Screenshot", Application.dataPath, "screenshot.png", "png");
                if (!string.IsNullOrEmpty(fileName))
                {
                    File.WriteAllBytes(fileName, _screenshot.EncodeToPNG());
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
            }
        }

        private void CreateScreenshot(Action<Texture2D> onFinished)
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
                switch (Type)
                {
                    case ScreenshotWindowType.GameView:
                        CreateGameScreenshot(onFinished);
                        break;
                    case ScreenshotWindowType.SceneView:
                        onFinished(CreateEditorScreenshot());
                        break;
                    default:
                        throw new NotImplementedException(Type.ToString());
                }
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

            void CreateGameScreenshot(Action<Texture2D> onFinished)
            {
                //Bug in ScreenCapture.CaptureScreenshotAsTexture()
                var fileName = Path.GetTempPath() + "/screenshot.png";
                ScreenCapture.CaptureScreenshot(fileName);

                var webRequest = UnityWebRequestTexture.GetTexture("file://" + fileName);
                EditorCoroutineUtility.StartCoroutine(LoadingAsync(webRequest, () =>
                {
                    if (webRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Unable to load texture: " + webRequest.result + " - " + webRequest.error + " - " + webRequest.downloadHandler.error);
                        return;
                    }

                    var content = DownloadHandlerTexture.GetContent(webRequest);
                    onFinished(content);
                }), this);

                IEnumerator LoadingAsync(UnityWebRequest webRequest, Action onFinished)
                {
                    yield return webRequest.SendWebRequest();
                    onFinished();
                }
            }
        }
    }

    public enum ScreenshotWindowType
    {
        SceneView,
        GameView
    }
}