#if PCSOFT_DRAGDROP && PCSOFT_RAYCASTER
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public sealed partial class UIDragDropController
    {
        private void HandleDragDropMove()
        {
            var dropTarget = EventSystem.current.IsPointerOverGameObject() ? HandleDropUI(true) : HandleDrop3D(true);
            if (dropTarget != null)
            {
                if (_currentDropTarget == null && dropTarget.AcceptType(_dragDrop?.data.GetType()))
                {
#if PCSOFT_DRAGDROP_LOGGING
                    Debug.Log("[DRAG-DROP] Move over correct target");
#endif
                    
                    dropTarget.OnDropEnter();
                    _currentDropTarget = dropTarget;
                }
                else
                {
#if PCSOFT_DRAGDROP_LOGGING
                    Debug.Log("[DRAG-DROP] Move over other target");
#endif
                }
            }
            else if (_currentDropTarget != null && _currentDropTarget.AcceptType(_dragDrop?.data.GetType()))
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] Move outside");
#endif
                
                _currentDropTarget.OnDropExit();
                _currentDropTarget = null;
            }
        }
    }
}
#endif