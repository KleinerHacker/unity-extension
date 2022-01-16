using System;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.UDebug
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public sealed class DebugFramerate : MonoBehaviour
    {
        #region Static Area

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            if (!DebugSettings.Singleton.ShowFramerate)
                return;

            var go = new GameObject("Debug Framerate");
            go.AddComponent<DebugFramerate>();
            DontDestroyOnLoad(go);
        }

        #endregion

        private GUIStyle _style;
        private float _deltaTime;
        private byte _counter;
        private string _text = "";

        private void Awake()
        {
            _style = new GUIStyle
            {
                alignment = DebugSettings.Singleton.FrameratePosition,
                fontSize = DebugSettings.Singleton.FramerateTextSize,
                normal =
                {
                    textColor = DebugSettings.Singleton.FramerateColor
                }
            };
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        private void FixedUpdate()
        {
            _counter++;
            if (_counter >= DebugSettings.Singleton.FramerateUpdateRate)
            {
                _counter = 0;

                var msec = _deltaTime * 1000.0f;
                var fps = 1.0f / _deltaTime;
                _text = $"{msec:0.0} ms ({fps:0.} fps)";
            }
        }

        private void OnGUI()
        {
            GUI.Label(Screen.safeArea, _text, _style);
        }
    }
#endif
}