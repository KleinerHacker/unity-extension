using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common.Merge;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Editor.extension.Scripts.Editor
{
    internal sealed class LoggerList : ReorderableList
    {
        private readonly Texture _fatalIcon;
        private readonly Texture _errorIcon;
        private readonly Texture _warnIcon;
        private readonly Texture _infoIcon;
        private readonly Texture _debugIcon;
        private readonly Texture _traceIcon;

        public LoggerList(IList<LoggerWindow.LogMessage> elements) : base(elements.ToArray(), typeof(LoggerWindow.LogMessage), false, false, false, false)
        {
            drawElementCallback += DrawElementCallback;

            _fatalIcon = EditorGUIUtility.IconContent("console.erroricon.sml").image;
            _errorIcon = EditorGUIUtility.IconContent("d_console.erroricon.sml").image;
            _warnIcon = EditorGUIUtility.IconContent("d_console.warnicon.sml").image;
            _infoIcon = EditorGUIUtility.IconContent("d_console.infoicon.sml").image;
            _debugIcon = EditorGUIUtility.IconContent("d_DebuggerAttached").image;
            _traceIcon = EditorGUIUtility.IconContent("TextMesh Icon").image;
        }

        private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var message = (LoggerWindow.LogMessage)list[i];
            
            GUI.DrawTexture(rect, i % 2 == 0 ? Texture2D.blackTexture : Texture2D.grayTexture);
            Handles.BeginGUI();
            {
                Handles.color = new Color(0.15f, 0.15f, 0.15f);
                Handles.DrawLine(new Vector3(rect.x + 22.5f, rect.y), new Vector3(rect.x + 22.5f, rect.yMax));
                Handles.DrawLine(new Vector3(rect.x + 25f + (rect.width - 25f) * 0.2f + 2.5f, rect.y), new Vector3(rect.x + 25f + (rect.width - 25f) * 0.2f + 2.5f, rect.yMax));
            }
            Handles.EndGUI();

            var icon = message.Level switch
            {
                LoggerLevel.Fatal => _fatalIcon,
                LoggerLevel.Error => _errorIcon,
                LoggerLevel.Warn => _warnIcon,
                LoggerLevel.Info => _infoIcon,
                LoggerLevel.Debug => _debugIcon,
                LoggerLevel.Trace => _traceIcon,
                _ => throw new NotImplementedException(message.Level.ToString())
            };
            GUI.DrawTexture(new Rect(rect.x + 2.5f, rect.y + 2.5f, 16f, 16f), icon);
            GUI.Label(new Rect(rect.x + 25f, rect.y, (rect.width - 25f) * 0.2f, rect.height), message.Tag);
            GUI.Label(new Rect(rect.x + 25f + (rect.width - 25f) * 0.2f + 5f, rect.y, (rect.width - 25f) * 0.8f - 5f, rect.height), message.Message);
        }
    }
}