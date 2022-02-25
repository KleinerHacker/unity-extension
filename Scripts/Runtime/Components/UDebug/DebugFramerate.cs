using System;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Attributes;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.UDebug
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    [Singleton(Scope = SingletonScope.Application, Instance = SingletonInstance.RequiresNewInstance, CreationTime = SingletonCreationTime.Loading, ObjectName = "Debug Framerate")]
    public sealed class DebugFramerate : SingletonBehavior<DebugFramerate>
    {
        [SingletonCondition]
        public static bool IsSingletonAlive() => DebugSettings.Singleton.ShowFramerate;
        
        private GUIStyle _style;
        private float _deltaTime;
        private byte _counter;
        private string _text = "";

        protected override void DoAwake()
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