using UnityEngine.EventSystems;

namespace UnityExtension.Runtime.extension.Scripts.Components
{
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