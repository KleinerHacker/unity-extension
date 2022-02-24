using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton
{
    /// <summary>
    /// Base class for a singleton behavior.
    /// </summary>
    /// <typeparam name="T">Must the final implementation type of itself</typeparam>
    public abstract class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
    {
        public static T Singleton => SingletonHandler.Registry.GetSingleton<T>();

        #region Builtin Methods

        protected void Awake()
        {
            var attribute = SingletonHandler.Registry.GetAttribute(GetType());
            
            Debug.Log("[SINGLETON] Try add instance to Singleton registry for " + GetType().FullName);
            if (!SingletonHandler.Registry.TryRegisterSingleton(this))
            {
                Debug.Log("[SINGLETON] Instance already in Singleton registry for " + GetType().FullName + ", destroy this instance");

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
            var attribute = SingletonHandler.Registry.GetAttribute(GetType());
            
            if (attribute.Scope == SingletonScope.Application)
            {
                Debug.Log("[SINGLETON] Singleton marked as application scoped, no cleanup for " + GetType().FullName);
                return;
            }

            Debug.Log("[SINGLETON] Try Singleton registry cleanup for " + typeof(T).FullName);
            SingletonHandler.Registry.TryUnregisterSingleton(this);
            
            DoDestroy();
        }
        
        protected virtual void DoDestroy() {} 

        #endregion
    }
}