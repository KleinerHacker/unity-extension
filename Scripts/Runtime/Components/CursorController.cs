using System;
using System.Linq;
using UnityBase.Runtime.Projects.unity_base.Scripts.Runtime.Components.Singleton;
using UnityBase.Runtime.Projects.unity_base.Scripts.Runtime.Components.Singleton.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Components
{
#if PCSOFT_CURSOR
    [Singleton(Scope = SingletonScope.Application, Instance = SingletonInstance.RequiresNewInstance, CreationTime = SingletonCreationTime.Loading, ObjectName = "Cursor System")]
    public sealed class CursorController : SingletonBehavior<CursorController>
    {
        #region Properties

        private CursorState _cursorState = CursorState.Default;
        private CursorState _cursorUIState = CursorState.Default;
        private bool _isUICursorActive;
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

        internal CursorState CursorUIState
        {
            get => _cursorUIState;
            private set
            {
                _cursorUIState = value;
                OnCursorChanged();
            }
        }

        internal bool IsUICursorActive
        {
            get => _isUICursorActive;
            private set
            {
                _isUICursorActive = value;
                OnCursorChanged();
            }
        }

        #endregion

        private CursorSettings _settings;
        private CursorItem _defaultUICursor;

        private string _cursorKey;
        private string _cursorUIKey;

        #region Builtin Methods

        protected override void DoAwake()
        {
            _settings = CursorSettings.Singleton;
            _defaultUICursor = _settings.UICursor.DefaultCursor;
        }

        private void LateUpdate()
        {
            if (_settings.UICursor.UseUICursors)
            {
                _counter += Time.deltaTime;
                if (_counter < _settings.UICursor.UICursorCheckDelay)
                    return;

                _counter = 0f;
            }

            if (_settings.UICursor.UseUICursors && _settings.UICursor.DefaultCursor.Active && EventSystem.current.IsPointerOverGameObject())
            {
                if (!IsUICursorActive)
                {
                    IsUICursorActive = true;
                }
            }
            else
            {
                if (IsUICursorActive)
                {
                    IsUICursorActive = false;
                }
            }
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

        public void ChangeUICursor(string key)
        {
            _cursorUIKey = key;
            CursorUIState = CursorState.Changed;
        }

        public void ResetUICursor()
        {
            _cursorUIKey = null;
            CursorUIState = CursorState.Default;
        }

        private void OnCursorChanged()
        {
            var cursorSettings = CursorSettings.Singleton;

            if (IsUICursorActive)
            {
                switch (_cursorUIState)
                {
                    case CursorState.Default:
#if PCSOFT_CURSOR_LOGGING
                        Debug.Log("[CURSOR] > Set cursor to default");
#endif

                        if (cursorSettings.UICursor.UseUICursors && cursorSettings.UICursor.DefaultCursor.Active)
                        {
#if PCSOFT_CURSOR_LOGGING
                            Debug.Log("[CURSOR] > UI cursor");
#endif
                            Cursor.SetCursor(cursorSettings.UICursor.DefaultCursor.Cursor, cursorSettings.UICursor.DefaultCursor.Hotspot, CursorMode.Auto);
                        }
                        else
                        {
#if PCSOFT_CURSOR_LOGGING
                            Debug.Log("[CURSOR] > Native cursor");
#endif
                            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                        }

                        break;
                    case CursorState.Changed:
#if PCSOFT_CURSOR_LOGGING
                        Debug.Log("[CURSOR] > Change cursor");
#endif
                        
                        if (cursorSettings.UICursor.UseUICursors)
                        {
#if PCSOFT_CURSOR_LOGGING
                            Debug.Log("[CURSOR] > UI cursor");
#endif
                            
                            var cursorItem = cursorSettings.UICursor.Items.FirstOrDefault(x => x.Identifier == _cursorUIKey);
                            if (cursorItem != null)
                            {
                                Cursor.SetCursor(cursorItem.Cursor, cursorItem.Hotspot, CursorMode.Auto);
                            }
                            else
                            {
                                Debug.LogWarning("[CURSOR] UI cursor cannot changed: Cursor with key " + _cursorUIKey + " not exists");
                            }
                        }
                        else
                        {
#if PCSOFT_CURSOR_LOGGING
                            Debug.Log("[CURSOR] > Native cursor");
#endif
                        }

                        break;
                    default:
                        throw new NotImplementedException(_cursorUIState.ToString());
                }
            }
            else
            {
                switch (_cursorState)
                {
                    case CursorState.Default:
#if PCSOFT_CURSOR_LOGGING
                        Debug.Log("[CURSOR] > Set cursor to default");
                        Debug.Log("[CURSOR] > Native cursor");
#endif
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                        break;
                    case CursorState.Changed:
#if PCSOFT_CURSOR_LOGGING
                        Debug.Log("[CURSOR] > Change cursor");
                        Debug.Log("[CURSOR] > Native cursor");
#endif
                        var cursorItem = cursorSettings.Items.FirstOrDefault(x => x.Identifier == _cursorKey);
                        if (cursorItem != null)
                        {
                            Cursor.SetCursor(cursorItem.Cursor, cursorItem.Hotspot, CursorMode.Auto);
                        }
                        else
                        {
                            Debug.LogWarning("[CURSOR] System Cursor cannot changed: Cursor with key " + _cursorKey + " not exists");
                        }

                        break;
                    default:
                        throw new NotImplementedException(_cursorState.ToString());
                }
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