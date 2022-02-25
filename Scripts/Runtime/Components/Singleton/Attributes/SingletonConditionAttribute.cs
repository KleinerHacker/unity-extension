using System;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Attributes
{
    /// <summary>
    /// Attribute to mark a static method in a singleton as condition method (create singleton or not). Syntax: <c>public static bool 'MethodName'()</c>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SingletonConditionAttribute : Attribute
    {
    }
}