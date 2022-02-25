using System;
using UnityEngine.EventSystems;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Attributes;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    [Obsolete("Use class SingletonUIBehavior instead (more effective) together with attribute " + nameof(SingletonAttribute))]
    public abstract class SearchingSingletonUIBehavior<T> : UIBehaviour where T : SearchingSingletonUIBehavior<T>
    {
        public static T Singleton => FindObjectOfType<T>();
    }
}