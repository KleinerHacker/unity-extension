using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
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

        public static void ChangeUICursor(string key) => CursorController.Singleton.ChangeUICursor(key);

        public static void ResetUICursor() => CursorController.Singleton.ResetUICursor();
    }
}