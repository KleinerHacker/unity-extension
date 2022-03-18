using System;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Attributes;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Internals;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton
{
    /// <summary>
    /// Base class for a singleton behavior.
    /// </summary>
    /// <typeparam name="T">Must the final implementation type of itself</typeparam>
    public abstract class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
    {
        /// <summary>
        /// Returns the singleton of this behavior. Can return <c>null</c> in case of <see cref="SingletonConditionAttribute"/> method returns FALSE. 
        /// </summary>
        public static T Singleton => SingletonHandler.Registry.GetSingleton<T>();

        #region Builtin Methods

        protected void Awake()
        {
            var attribute = SingletonHandler.Registry.GetAttribute(GetType());
            
#if SINGLETON_LOGGING
            Debug.Log("[SINGLETON] Try add instance to Singleton registry for " + GetType().FullName);
#endif
            if (!SingletonHandler.Registry.TryRegisterSingleton(this))
            {
#if SINGLETON_LOGGING
                Debug.Log("[SINGLETON] Instance already in Singleton registry for " + GetType().FullName + ", destroy this instance");
#endif

                Destroy(this);
                return;
            }

            if (attribute.Scope == SingletonScope.Application)
            {
#if SINGLETON_LOGGING
                Debug.Log("[SINGLETON] Instance marked for application scope (" + nameof(DontDestroyOnLoad) + ") for " + typeof(T).FullName);
#endif
                DontDestroyOnLoad(this);
            }
            
            DoAwake();
        }
        
        protected virtual void DoAwake() {}

        protected void OnDestroy()
        {
            var attribute = SingletonHandler.Registry.GetAttribute(GetType());

            if (attribute.Scope == SingletonScope.Application)
                throw new InvalidOperationException("Unable to destroy game object cause it is an application scoped singleton: " + gameObject.name);

#if SINGLETON_LOGGING
            Debug.Log("[SINGLETON] Try Singleton registry cleanup for " + typeof(T).FullName);
#endif
            SingletonHandler.Registry.TryUnregisterSingleton(this);
            
            DoDestroy();
        }
        
        protected virtual void DoDestroy() {} 

        #endregion
    }
}