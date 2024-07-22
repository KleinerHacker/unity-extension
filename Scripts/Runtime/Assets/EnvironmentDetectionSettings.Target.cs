using System;
using UnityEngine;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Assets
{
    [Serializable]
    public abstract class EnvironmentTarget
    {
        #region Inspector Data

        [SerializeField]
        private string name;

        [SerializeField]
        private bool requiresKeyboard;
        
        [SerializeField]
        private bool requiresMouse;
        
        [SerializeField]
        private bool requiresTouch;
        
        [SerializeField]
        private bool requiresGamepad;

        #endregion

        #region Properties

        public string Name => name;

        public bool RequiresKeyboard => requiresKeyboard;

        public bool RequiresMouse => requiresMouse;

        public bool RequiresTouch => requiresTouch;

        public bool RequiresGamepad => requiresGamepad;

        #endregion
    }

    [Serializable]
    public sealed class WindowsEnvironmentTarget : EnvironmentTarget
    {
    }

    [Serializable]
    public sealed class LinuxEnvironmentTarget : EnvironmentTarget
    {
        #region Inspector Data

        [SerializeField]
        private bool requiresSteamDeck;

        #endregion

        #region Properties

        public bool RequiresSteamDeck => requiresSteamDeck;

        #endregion
    }

    [Serializable]
    public sealed class MacEnvironmentTarget : EnvironmentTarget
    {
    }

    [Serializable]
    public sealed class AndroidEnvironmentTarget : EnvironmentTarget
    {
        #region Inspector Data

        [SerializeField]
        private bool requiresTv;

        #endregion

        #region Properties

        public bool RequiresTv => requiresTv;

        #endregion
    }
    
    [Serializable]
    public sealed class IOSEnvironmentTarget : EnvironmentTarget
    {
    }
}