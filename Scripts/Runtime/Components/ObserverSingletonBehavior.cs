using System;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Attributes;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    [Obsolete("Use class SingletonBehavior instead (more effective) together with attribute " + nameof(SingletonAttribute))]
    public abstract class ObserverSingletonBehavior<T> : MonoBehaviour where T : ObserverSingletonBehavior<T>
    {
        public static T Singleton { get; private set; }
        
        public static event EventHandler SingletonSpawned;
        public static event EventHandler SingletonDestroying;

        protected virtual void OnEnable()
        {
            Singleton = (T) this;
            SingletonSpawned?.Invoke(null, EventArgs.Empty);
        }

        protected virtual void OnDisable()
        {
            SingletonDestroying?.Invoke(null, EventArgs.Empty);
            Singleton = null;
        }
    }
}