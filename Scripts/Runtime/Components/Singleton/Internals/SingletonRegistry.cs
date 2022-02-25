using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Attributes;
using Object = UnityEngine.Object;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Internals
{
    internal sealed class SingletonRegistry
    {
        private readonly IDictionary<Type, InstanceInfo> _singletonRegister;

        public SingletonRegistry()
        {
            _singletonRegister = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(Component).IsAssignableFrom(x))
                .Where(x => !x.IsAbstract && !x.IsInterface)
                .Where(x => x.GetCustomAttribute<SingletonAttribute>() != null)
                .ToDictionary(x => x, x => new InstanceInfo(x.GetCustomAttribute<SingletonAttribute>()));
        }

        public SingletonAttribute GetAttribute<T>() where T : Component => GetAttribute(typeof(T));

        public SingletonAttribute GetAttribute(Type type)
        {
            CheckRegistry(type);
            return _singletonRegister[type].Attribute;
        }

        public T GetSingleton<T>() where T : Component
        {
            return (T)GetSingleton(typeof(T));
        }

        public Component GetSingleton(Type type)
        {
            CheckRegistry(type);

            var instanceInfo = _singletonRegister[type];
            if (instanceInfo.Instance != null)
                return instanceInfo.Instance;

            if (instanceInfo.Attribute.Instance == SingletonInstance.HasExistingInstance)
                throw new InvalidOperationException("Unable to find singleton in registry of type " + type.FullName);

            var singleton = CreateSingleton(instanceInfo.Attribute, type);
            if (singleton == null)
            {
                if (instanceInfo.Attribute.ExceptionIfNotExists)
                    throw new InvalidOperationException("There is no singleton instance yet");

                return null;
            }

#if SINGLETON_LOGGING
            Debug.Log("[SINGLETON] Add singleton to registry for " + type.FullName);
#endif
            instanceInfo.Instance = singleton;

            return singleton;
        }

        public bool TryRegisterSingleton(Component value)
        {
            CheckRegistry(value.GetType());

            var instanceInfo = _singletonRegister[value.GetType()];
            if (instanceInfo.Instance != null)
                return false;

            instanceInfo.Instance = value;
            return true;
        }

        public bool TryUnregisterSingleton(Component value)
        {
            CheckRegistry(value.GetType());

            var instanceInfo = _singletonRegister[value.GetType()];
            if (instanceInfo.Instance == null)
                return false;

            instanceInfo.Instance = null;
            return true;
        }

        public void VisitAllSingletons(Action<Type, SingletonAttribute> visitor)
        {
            foreach (var item in _singletonRegister)
            {
                visitor.Invoke(item.Key, item.Value.Attribute);
            }
        }

        private Component CreateSingleton(SingletonAttribute attribute, Type type)
        {
#if SINGLETON_LOGGING
            Debug.Log("[SINGLETON] Create new singleton for " + type.FullName);
#endif

            return attribute.Scope switch
            {
                SingletonScope.Application => CreateInApplicationScope(type, attribute.ObjectName),
                SingletonScope.Scene => CreateInSceneScope(type, attribute.ObjectName),
                _ => throw new NotImplementedException(attribute.Scope.ToString())
            };
        }

        private Component CreateInApplicationScope(Type type, string objectName)
        {
#if SINGLETON_LOGGING
            Debug.Log("[SINGLETON] Singleton in application scope for " + type.FullName);
#endif
            
            var newInstance = CreateInstance(type, objectName);
            if (newInstance == null)
                return null;
            Object.DontDestroyOnLoad(newInstance);

            return newInstance;
        }

        private Component CreateInSceneScope(Type type, string objectName)
        {
#if SINGLETON_LOGGING
            Debug.Log("[SINGLETON] Singleton in scene scope for " + type.FullName);
#endif
            return CreateInstance(type, objectName);
        }

        private Component CreateInstance(Type type, string objectName)
        {
#if SINGLETON_LOGGING
            Debug.Log("[SINGLETON] Create new singleton instance for " + type.FullName);
#endif

            var conditionMethod = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(x => x.GetCustomAttribute<SingletonConditionAttribute>() != null);
            if (conditionMethod != null && (conditionMethod.GetParameters().Length != 0 || conditionMethod.ReturnType != typeof(bool)))
                throw new InvalidOperationException("Method " + type.FullName + "." + conditionMethod.Name +
                                                    " must have zero parameter and must return a bool value");

            if (!((bool?)conditionMethod?.Invoke(null, Array.Empty<object>()) ?? true))
            {
#if SINGLETON_LOGGING
                Debug.Log("[SINGLETON] Skip creation of " + type.FullName + " cause condition method " + conditionMethod.Name + " returns FALSE");
#endif
                return null;
            }

            GameObject go;
            if (string.IsNullOrWhiteSpace(objectName) || (go = GameObject.Find(objectName)) == null)
            {
                go = new GameObject(string.IsNullOrWhiteSpace(objectName) ? type.Name : objectName);
            }

            var value = go.AddComponent(type);

            var initMethod = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(x => x.GetCustomAttribute<SingletonInitializerAttribute>() != null);
            if (initMethod != null && (initMethod.GetParameters().Length != 1 || !type.IsAssignableFrom(initMethod.GetParameters()[0].ParameterType)))
                throw new InvalidOperationException(
                    "Method " + type.FullName + "." + initMethod.Name + " must have one parameter of own type: " + type.FullName);

            initMethod?.Invoke(this, new object[] { value });

            return value;
        }

        private void CheckRegistry(Type type)
        {
            if (!_singletonRegister.ContainsKey(type))
                throw new InvalidOperationException("Type is not a registered singleton. You forget attribute " + nameof(SingletonAttribute) + "?");
        }

        private sealed class InstanceInfo
        {
            public Component Instance { get; internal set; }
            public SingletonAttribute Attribute { get; }

            public InstanceInfo(SingletonAttribute attribute)
            {
                Attribute = attribute;
            }
        }
    }
}