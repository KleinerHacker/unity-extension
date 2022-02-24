using System;
using UnityEngine.EventSystems;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    [Obsolete("Use class SingletonUIBehavior instead (more effective) together with attribute " + nameof(SingletonAttribute))]
    public abstract class ObserverSingletonUIBehavior<T> : UIBehaviour where T : ObserverSingletonUIBehavior<T>
    {
        public static T Singleton { get; private set; }

        protected override void OnEnable()
        {
            Singleton = (T) this;
        }

        protected override void OnDisable()
        {
            Singleton = null;
        }
    }
}