using System;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Attributes;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    [Obsolete("Use class SingletonBehavior instead (more effective) together with attribute " + nameof(SingletonAttribute))]
    public abstract class SearchingSingletonBehavior<T> : MonoBehaviour where T : SearchingSingletonBehavior<T>
    {
        public static T Singleton => FindObjectOfType<T>();
    }
}