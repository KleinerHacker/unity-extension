using System;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton
{
    /// <summary>
    /// Base class for a singleton behavior.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
    {
        private static readonly SingletonRegistry<T> Registry = new SingletonRegistry<T>();

        public static T Singleton => Registry.Singleton;

        /// <summary>
        /// Add callback to handle instance creation actions
        /// </summary>
        protected static Action<T> PostInstantiationCallback
        {
            get => Registry.PostInstantiationCallback;
            set => Registry.PostInstantiationCallback = value;
        }

        #region Builtin Methods

        protected void Awake()
        {
            var attribute = Registry.Attribute;
            
            Debug.Log("[SINGLETON] Try add instance to Singleton registry for " + typeof(T).FullName);
            if (!Registry.TryRegisterSingleton((T)this))
            {
                Debug.Log("[SINGLETON] Instance already in Singleton registry for " + typeof(T).FullName + ", destroy this instance");

                Destroy(this);
                return;
            }

            if (attribute.Scope == SingletonScope.Application)
            {
                Debug.Log("[SINGLETON] Instance marked for application scope (" + nameof(DontDestroyOnLoad) + ") for " + typeof(T).FullName);
                DontDestroyOnLoad(this);
            }
            
            DoAwake();
        }
        
        protected virtual void DoAwake() {}

        protected void OnDestroy()
        {
            var attribute = Registry.Attribute;
            
            if (attribute.Scope == SingletonScope.Application)
            {
                Debug.Log("[SINGLETON] Singleton marked as application scoped, no cleanup for " + typeof(T).FullName);
                return;
            }

            Debug.Log("[SINGLETON] Try Singleton registry cleanup for " + typeof(T).FullName);
            Registry.TryUnregisterSingleton((T)this);
            
            DoDestroy();
        }
        
        protected virtual void DoDestroy() {} 

        #endregion
    }
}