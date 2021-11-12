using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Components
{
    public abstract class SearchingSingletonBehavior<T> : MonoBehaviour where T : SearchingSingletonBehavior<T>
    {
        public static T Singleton => FindObjectOfType<T>();
    }
}