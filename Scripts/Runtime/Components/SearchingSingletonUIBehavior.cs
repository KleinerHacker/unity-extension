using UnityEngine.EventSystems;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public abstract class SearchingSingletonUIBehavior<T> : UIBehaviour where T : SearchingSingletonUIBehavior<T>
    {
        public static T Singleton => FindObjectOfType<T>();
    }
}