using System.Collections.Generic;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public abstract class CursorSystem<TDefault, TSpecific> : MonoBehaviour
        where TSpecific : CursorItem where TDefault : CursorItem
    {
        #region Properties
        
        public bool IsCursorVisible
        {
            get => Cursor.visible;
            set => Cursor.visible = true;
        }

        public CursorLockMode CursorLockMode
        {
            get => Cursor.lockState;
            set => Cursor.lockState = value;
        }

        public bool IsDefaultCursorOverwritten => _currentDefaultOverwrite != null;

        #endregion

        private TDefault _currentDefaultOverwrite = null;
        private readonly IList<TSpecific> _currentSpecificCursorStack = new List<TSpecific>();

        public void OverwriteDefaultCursor(TDefault cursor)
        {
            _currentDefaultOverwrite = cursor;
            UpdateCursor(cursor);
        }

        public void ResetDefaultCursor()
        {
            _currentDefaultOverwrite = null;
            UpdateCursor(null);
        }

        public void ShowSpecificCursor(TSpecific cursor)
        {
            UpdateCursor(cursor);

            //Cursor in stack?
            if (_currentSpecificCursorStack.Contains(cursor))
            {
                //Remove from stack (than is added to 0, so it is on top of stack)
                _currentSpecificCursorStack.Remove(cursor);
            }

            //Add to stack
            _currentSpecificCursorStack.Insert(0, cursor);
        }

        public void HideSpecificCursor(TSpecific cursor)
        {
            if (!_currentSpecificCursorStack.Contains(cursor))
                return;

            //Is active cursor?
            if (_currentSpecificCursorStack.IndexOf(cursor) == 0)
            {
                //If last specific cursor in stack
                if (_currentSpecificCursorStack.Count == 1)
                {
                    //Fallback to default
                    UpdateCursor(_currentDefaultOverwrite);
                }
                else
                {
                    //Update to next in stack
                    UpdateCursor(_currentSpecificCursorStack[1]);
                }
            }

            //Remove from stack
            _currentSpecificCursorStack.Remove(cursor);
        }

        protected static void UpdateCursor(CursorItem cursorItem)
        {
            if (cursorItem == null)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(cursorItem.Cursor, cursorItem.Hotspot, CursorMode.Auto);
            }
        }
    }
    
    [AddComponentMenu(UnityExtensionConstants.Root + "/Cursor System")]
    [DisallowMultipleComponent]
    public class CursorSystem : CursorSystem<CursorItem, CursorItem>
    {
        #region Static Area

        public static CursorSystem Singleton => FindObjectsOfType<CursorSystem>()[0];

        #endregion
    } 
}