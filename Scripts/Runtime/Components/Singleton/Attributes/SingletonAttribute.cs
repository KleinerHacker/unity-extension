using System;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Singleton.Attributes
{
    /// <summary>
    /// Represent the required attribute for <see cref="SingletonBehavior{T}"/> and <see cref="SingletonUIBehavior{T}"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonAttribute : Attribute
    {
        /// <summary>
        /// Scope to use for this singleton
        /// </summary>
        public SingletonScope Scope { get; set; } = SingletonScope.Scene;

        /// <summary>
        /// Type of instance for this singleton
        /// </summary>
        public SingletonInstance Instance { get; set; } = SingletonInstance.HasExistingInstance;

        /// <summary>
        /// Defines time of creation
        /// </summary>
        public SingletonCreationTime CreationTime { get; set; } = SingletonCreationTime.Loading;

        /// <summary>
        /// Name of object in case of create new instance. Same names of singleton objects results in one merged game object.
        /// </summary>
        public string ObjectName { get; set; } = "";

        /// <summary>
        /// Throws an exception if singleton was not created or not exists (found in scene), otherwise singleton returns <c>null</c>
        /// </summary>
        public bool ExceptionIfNotExists { get; set; } = true;
    }

    /// <summary>
    /// Describe a scope to use for a singleton
    /// </summary>
    public enum SingletonScope
    {
        /// <summary>
        /// Scope in application - The singleton object is marked with <c>DontDestroyOnLoad</c> 
        /// </summary>
        Application,
        /// <summary>
        /// Scope only in loaded scene - The singleton is destroyed on load
        /// </summary>
        Scene,
    }

    /// <summary>
    /// Represent the instance type of the singleton
    /// </summary>
    public enum SingletonInstance
    {
        /// <summary>
        /// Singleton is already instantiated from loaded scene (is already added to an game object)
        /// </summary>
        HasExistingInstance,
        /// <summary>
        /// Singleton is not instantiated yet and must created new (a new game object will created)
        /// </summary>
        RequiresNewInstance
    }

    /// <summary>
    /// Creation time of a singleton
    /// </summary>
    public enum SingletonCreationTime
    {
        /// <summary>
        /// Create on scene / app loading
        /// </summary>
        Loading,
        /// <summary>
        /// Create on first singleton invocation
        /// </summary>
        FirstInvocation,
    }
}