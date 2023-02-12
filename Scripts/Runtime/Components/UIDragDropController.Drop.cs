#if PCSOFT_DRAGDROP && PCSOFT_RAYCASTER
using System.Collections.Generic;
using System.Linq;
using UnityBase.Runtime.@base.Scripts.Runtime.Utils.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;
using UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public sealed partial class UIDragDropController
    {
        private void HandleDrop()
        {
            var dropTarget = EventSystem.current.IsPointerOverGameObject() ? HandleDropUI(false) : HandleDrop3D(false);
            if (dropTarget != null)
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] Finish", (Object)dropTarget);
#endif
                
                dropTarget.OnDrop(_dragDrop?.data);

                _dragDrop?.dragStart.OnDropSuccessfully(dropTarget);
                DropSuccessfully?.Invoke(this, new DropEventArgs(dropTarget, _dragDrop?.data));
            }
            else
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] Finish failed, drop is canceled");
#endif
                
                _dragDrop?.dragStart.OnDropCanceled();
                DropCanceled?.Invoke(this, new DropEventArgs(null, _dragDrop?.data));
            }

            _currentDropTarget?.OnDropExit();
            _currentDropTarget = null;
            _dragDrop = null;
        }
        
        private IPointerDropTarget HandleDropUI(bool moving)
        {
#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log(moving ? "[DRAG-DROP] Moving UI" : "[DRAG-DROP] Finish on UI");
#endif
            
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = InputUtils.GetValueFromDevice(Pointer.current, pointer => pointer.position.ReadValue())
            };
            var resultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, resultList);

            var result = resultList
                .Select(x => x.gameObject.GetComponent<IPointerDropTarget>())
                .FirstOrDefault(x => x != null);

            if (result != null && !result.AcceptType(_dragDrop?.data.GetType()))
                return null;

            return result;
        }

        private IPointerDropTarget HandleDrop3D(bool moving)
        {
#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log(moving ? "[DRAG-DROP] Moving 3D" : "[DRAG-DROP] Finish on 3D");
#endif
            
            var hit = Raycaster.GetFirst3DHit(DragDropSettings.Singleton.GetMoveRaycasterReference());
            if (hit != null)
            {
                var result = hit.Value.collider.FindComponent<IPointerDropTarget>();
                if (result != null && !result.AcceptType(_dragDrop?.data.GetType()))
                    return null;

                return result;
            }

            return null;
        }
    }
}
#endif