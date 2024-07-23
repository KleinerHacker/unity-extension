#if PCSOFT_CURSOR
using UnityEngine;
using UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Components;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime
{
    public static class CursorSystem
    {
        public static bool IsVisible
        {
            get => CursorController.Singleton.IsVisible;
            set => CursorController.Singleton.IsVisible = value;
        }
        
        public static CursorLockMode LockMode
        {
            get => CursorController.Singleton.LockMode;
            set => CursorController.Singleton.LockMode = value;
        }

        public static void ChangeCursor(string key) => CursorController.Singleton.ChangeCursor(key);

        public static void ResetCursor() => CursorController.Singleton.ResetCursor();
    }
}
#endif