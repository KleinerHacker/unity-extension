using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Editor.extension.Scripts.Editor
{
    public sealed class LoggerWindow : EditorWindow
    {
        private static readonly Regex MessageRegex = new Regex(@"^<([A-Za-z]+)> \[([^\]]*)\] (.*)$");
        
        [MenuItem("Window/General/Logging")]
        public static void ShowLoggingWindow()
        {
            CreateInstance<LoggerWindow>().Show();
        }

        private readonly IList<LogMessage> _messages = new List<LogMessage>();
        private LoggerList _loggerList;
        
        private Vector2 _scroll = Vector2.zero;

        private void OnEnable()
        {
            titleContent = new GUIContent("Logging", EditorGUIUtility.IconContent("align_vertically_center_active").image);
            _loggerList = new LoggerList(_messages);
            
            Application.logMessageReceived += OnLogMessageReceived;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        private void OnGUI()
        {
            //GUILayout.Toolbar(0, new[] { "Hello", "World" });
            
            _scroll = GUILayout.BeginScrollView(_scroll, false, true);
            {
                _loggerList.DoLayoutList();
            }
            GUILayout.EndScrollView();
            
            GUILayout.Label("Log Lines: " + _messages.Count);
        }

        private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (!MessageRegex.IsMatch(condition))
            {
                _messages.Add(new LogMessage
                {
                    Message = condition,
                    Level = type.Convert(),
                });
                _loggerList = new LoggerList(_messages);
                Repaint();
                
                return;
            }
            
            var match = MessageRegex.Match(condition);

            _messages.Add(new LogMessage
            {
                Message = match.Groups[3].Value,
                Level = Parse(match.Groups[1].Value),
                Tag = match.Groups[2].Value,
            });
            _loggerList = new LoggerList(_messages);
            Repaint();
        }

        private static LoggerLevel Parse(string value)
        {
            if (!Enum.TryParse(value, out LoggerLevel level))
                return LoggerLevel.Info;

            return level;
        }
        
        internal struct LogMessage
        {
            public string Message { get; set; }
            public LoggerLevel Level { get; set; }
            public string Tag { get; set; }
        }
    }
}