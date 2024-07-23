using System;
using System.Linq;
using UnityBase.Runtime.Projects.unity_base.Scripts.Runtime.Components.Singleton;
using UnityBase.Runtime.Projects.unity_base.Scripts.Runtime.Components.Singleton.Attributes;
using UnityEngine;
using UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Components
{
#if PCSOFT_CURSOR
    [Singleton(Scope = SingletonScope.Application, Instance = SingletonInstance.RequiresNewInstance,
        CreationTime = SingletonCreationTime.Loading, ObjectName = "Cursor System")]
    public sealed class CursorController : SingletonBehavior<CursorController>
    {
        #region Properties

        private CursorState _cursorState = CursorState.Default;
        private float _counter;

        public bool IsVisible
        {
            get => Cursor.visible;
            set => Cursor.visible = value;
        }

        public CursorLockMode LockMode
        {
            get => Cursor.lockState;
            set => Cursor.lockState = value;
        }

        internal CursorState CursorState
        {
            get => _cursorState;
            private set
            {
                _cursorState = value;
                OnCursorChanged();
            }
        }

        #endregion

        private CursorSettings _settings;

        private string _cursorKey;

        #region Builtin Methods

        protected override void DoAwake()
        {
            _settings = CursorSettings.Singleton;
        }

        #endregion

        public void ChangeCursor(string key)
        {
            _cursorKey = key;
            CursorState = CursorState.Changed;
        }

        public void ResetCursor()
        {
            _cursorKey = null;
            CursorState = CursorState.Default;
        }

        private void OnCursorChanged()
        {
            var cursorSettings = CursorSettings.Singleton;


            switch (_cursorState)
            {
                case CursorState.Default:
#if PCSOFT_CURSOR_LOGGING
                    Debug.Log("[CURSOR] > Set cursor to default");
#endif
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorState.Changed:
#if PCSOFT_CURSOR_LOGGING
                    Debug.Log("[CURSOR] > Change cursor");
#endif
                    var cursorItem = cursorSettings.Items.FirstOrDefault(x => x.Identifier == _cursorKey);
                    if (cursorItem != null)
                    {
                        Cursor.SetCursor(cursorItem.Cursor, cursorItem.Hotspot, CursorMode.Auto);
                    }
                    else
                    {
                        Debug.LogWarning("[CURSOR] System Cursor cannot changed: Cursor with key " + _cursorKey +
                                         " not exists");
                    }

                    break;
                default:
                    throw new NotImplementedException(_cursorState.ToString());
            }
        }
    }

    public enum CursorState
    {
        Default,
        Changed,
    }
#endif
}