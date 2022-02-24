using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton
{
    internal sealed class SingletonRegistry
    {
        private readonly IDictionary<Type, InstanceInfo> _singletonRegister;

        public SingletonRegistry()
        {
            _singletonRegister = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
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
            Debug.Log("[SINGLETON] Add singleton to registry for " + type.FullName);
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
            Debug.Log("[SINGLETON] Create new singleton for " + type.FullName);

            return attribute.Scope switch
            {
                SingletonScope.Application => CreateInApplicationScope(type),
                SingletonScope.Scene => CreateInSceneScope(type),
                _ => throw new NotImplementedException(attribute.Scope.ToString())
            };
        }

        private Component CreateInApplicationScope(Type type)
        {
            Debug.Log("[SINGLETON] Singleton in application scope for " + type.FullName);

            var newInstance = CreateInstance(type);
            Object.DontDestroyOnLoad(newInstance);

            return newInstance;
        }

        private Component CreateInSceneScope(Type type)
        {
            Debug.Log("[SINGLETON] Singleton in scene scope for " + type.FullName);
            return CreateInstance(type);
        }

        private Component CreateInstance(Type type)
        {
            Debug.Log("[SINGLETON] Create new singleton instance for " + type.FullName);

            var go = new GameObject(type.Name);
            var value = go.AddComponent(type);

            var initMethod = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(x => x.GetCustomAttribute<SingletonInitializerAttribute>() != null);
            if (initMethod != null && (initMethod.GetParameters().Length != 1 || initMethod.GetParameters()[0].ParameterType == type))
                throw new InvalidOperationException("Method " + type.FullName + "." + initMethod.Name + " must have one parameter of own type");

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