using System;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Components
{
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