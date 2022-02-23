using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton
{
    internal sealed class SingletonRegistry<T> where T : Component
    {
        private readonly IDictionary<Type, T> _singletonRegister = new Dictionary<Type, T>();
        
        public SingletonAttribute Attribute { get; }
        public Action<T> PostInstantiationCallback { get; set; }

        public SingletonRegistry()
        {
            Attribute = typeof(T).GetCustomAttribute<SingletonAttribute>();
            if (Attribute == null)
                throw new InvalidOperationException("Missing required annotation " + nameof(Components.Singleton.SingletonAttribute) + " on class " +
                                                    typeof(T).FullName);
        }

        public T Singleton
        {
            get
            {
                var type = typeof(T);

                if (_singletonRegister.ContainsKey(type))
                    return _singletonRegister[type];

                if (Attribute.Instance == SingletonInstance.HasExistingInstance)
                    throw new InvalidOperationException("Unable to find singleton in registry of type " + typeof(T).FullName);

                var singleton = CreateSingleton();
                Debug.Log("[SINGLETON] Add singleton to registry for " + typeof(T).FullName);
                _singletonRegister.Add(type, singleton);

                return singleton;
            }
        }

        public bool TryRegisterSingleton(T value)
        {
            if (_singletonRegister.ContainsKey(value.GetType()))
                return false;
            
            _singletonRegister.Add(value.GetType(), value);
            return true;
        }

        public bool TryUnregisterSingleton(T value)
        {
            if (!_singletonRegister.ContainsKey(value.GetType()))
                return false;

            _singletonRegister.Remove(value.GetType());
            return true;
        }

        private T CreateSingleton()
        {
            Debug.Log("[SINGLETON] Create new singleton for " + typeof(T).FullName);

            return Attribute.Scope switch
            {
                SingletonScope.Application => CreateInApplicationScope(),
                SingletonScope.Scene => CreateInSceneScope(),
                _ => throw new NotImplementedException(Attribute.Scope.ToString())
            };
        }

        private T CreateInApplicationScope()
        {
            Debug.Log("[SINGLETON] Singleton in application scope for " + typeof(T).FullName);
            
            var newInstance = CreateInstance();
            Object.DontDestroyOnLoad(newInstance);

            return newInstance;
        }

        private T CreateInSceneScope()
        {
            Debug.Log("[SINGLETON] Singleton in scene scope for " + typeof(T).FullName);
            return CreateInstance();
        }

        private T CreateInstance()
        {
            Debug.Log("[SINGLETON] Create new singleton instance for " + typeof(T).FullName);

            var go = new GameObject(typeof(T).FullName);
            var value = go.AddComponent<T>();
            
            PostInstantiationCallback?.Invoke(value);

            return value;
        }
    }
}