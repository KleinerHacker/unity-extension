using System;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;
using Object = UnityEngine.Object;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
    public static class Logger
    {
        #region FATAL

        public static bool IsFatalEnabled => true;

        public static void Fatal(string message, Object context = null) => Fatal(null, message, context);

        public static void Fatal(string tag, string message, Object context = null) => Fatal(tag, () => message, context);

        public static void Fatal(Func<string> message, Object context = null) => Fatal(null, message, context);

        public static void Fatal(string tag, Func<string> message, Object context = null) =>
            UnityEngine.Debug.LogAssertion(BuildLevel(LoggerLevel.Fatal) + " " + BuildTag(tag) + " " + message, context);

        #endregion
        
        #region ERROR

        public static bool IsErrorEnabled => true;

        public static void Error(string message, Object context = null) => Error(null, message, context);

        public static void Error(string tag, string message, Object context = null) => Error(tag, () => message, context);

        public static void Error(Func<string> message, Object context = null) => Error(null, message, context);

        public static void Error(string tag, Func<string> message, Object context = null) =>
            UnityEngine.Debug.LogError(BuildLevel(LoggerLevel.Error) + " " + BuildTag(tag) + " " + message, context);

        #endregion
        
        #region WARN

        public static bool IsWarnEnabled => true;

        public static void Warn(string message, Object context = null) => Warn(null, message, context);

        public static void Warn(string tag, string message, Object context = null) => Warn(tag, () => message, context);

        public static void Warn(Func<string> message, Object context = null) => Warn(null, message, context);

        public static void Warn(string tag, Func<string> message, Object context = null) => 
            UnityEngine.Debug.LogWarning(BuildLevel(LoggerLevel.Warn) + " " + BuildTag(tag) + " " + message, context);

        #endregion
        
        #region INFO

        public static bool IsInfoEnabled => true;

        public static void Info(string message, Object context = null) => Info(null, message, context);

        public static void Info(string tag, string message, Object context = null) => Info(tag, () => message, context);

        public static void Info(Func<string> message, Object context = null) => Info(null, message, context);

        public static void Info(string tag, Func<string> message, Object context = null) =>
            UnityEngine.Debug.Log(BuildLevel(LoggerLevel.Info) + " " + BuildTag(tag) + " " + message, context);

        #endregion

        #region DEBUG

        public static bool IsDebugEnabled => true;

        public static void Debug(string message, Object context = null) => Debug(null, message, context);

        public static void Debug(string tag, string message, Object context = null) => Debug(tag, () => message, context);

        public static void Debug(Func<string> message, Object context = null) => Debug(null, message, context);

        public static void Debug(string tag, Func<string> message, Object context = null) =>
            UnityEngine.Debug.Log(BuildLevel(LoggerLevel.Debug) + " " + BuildTag(tag) + " " + message, context);

        #endregion
        
        #region TRACE

        public static bool IsTraceEnabled => true;

        public static void Trace(string message, Object context = null) => Trace(null, message, context);

        public static void Trace(string tag, string message, Object context = null) => Trace(tag, () => message, context);

        public static void Trace(Func<string> message, Object context = null) => Trace(null, message, context);

        public static void Trace(string tag, Func<string> message, Object context = null) => 
            UnityEngine.Debug.Log(BuildLevel(LoggerLevel.Trace) + " " + BuildTag(tag) + " " + message, context);

        #endregion

        private static string BuildTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return "";

            return "[" + tag + "]";
        }

        private static string BuildLevel(LoggerLevel level) => "<" + level + ">";
    }
}