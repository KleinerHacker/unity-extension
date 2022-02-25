using System;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Attributes
{
    /// <summary>
    /// Attribute to mark a static method in a singleton as initializer method. Syntax: <c>public static void 'MethodName'('TypeOfOwn' arg)</c>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SingletonInitializerAttribute : Attribute
    {
    }
}