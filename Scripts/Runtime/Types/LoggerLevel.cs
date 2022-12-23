using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Types
{
    public enum LoggerLevel
    {
        Fatal,
        Error,
        Warn,
        Info,
        Debug,
        Trace,
    }

    public static class LoggerLevelExtensions
    {
        public static LogType Convert(this LoggerLevel level)
        {
            return level switch
            {
                LoggerLevel.Fatal => LogType.Exception,
                LoggerLevel.Error => LogType.Error,
                LoggerLevel.Warn => LogType.Warning,
                LoggerLevel.Info => LogType.Log,
                LoggerLevel.Debug => LogType.Log,
                LoggerLevel.Trace => LogType.Log,
                _ => throw new NotImplementedException(level.ToString())
            };
        }

        public static LoggerLevel Convert(this LogType type)
        {
            return type switch
            {
                LogType.Assert => LoggerLevel.Fatal,
                LogType.Exception => LoggerLevel.Fatal,
                LogType.Error => LoggerLevel.Error,
                LogType.Warning => LoggerLevel.Warn,
                LogType.Log => LoggerLevel.Info,
                _ => throw new NotImplementedException(type.ToString())
            };
        }
    }
}