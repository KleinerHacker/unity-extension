#if PCSOFT_DRAGDROP && PCSOFT_RAYCASTER
using System.Collections.Generic;
using System.Linq;
using UnityBase.Runtime.@base.Scripts.Runtime.Utils.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public sealed partial class UIDragDropController
    {
        private void HandleDragStart()
        {
            var dragStart = EventSystem.current.IsPointerOverGameObject() ? HandleDragUI() : HandleDrag3D();
            if (dragStart != null)
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] Start", (Object)dragStart);
#endif

                dragStart.OnStartDrag(out var data);
                _dragDrop = (dragStart, data);

                DragStarted?.Invoke(this, new DragEventArgs(dragStart, data));
            }
            else
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] Start failed, object is ignored");
#endif
            }
        }

        private IPointerDragStart HandleDragUI()
        {
#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log("[DRAG-DROP] Start for UI");
#endif
            
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = InputUtils.GetValueFromDevice(Pointer.current, pointer => pointer.position.ReadValue())
            };
            var resultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, resultList);
            
#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log("[DRAG-DROP] > Find hits: " + resultList.Count);
#endif

            return resultList
                .Select(x => x.gameObject.GetComponent<IPointerDragStart>())
                .FirstOrDefault(x => x != null);
        }

        private IPointerDragStart HandleDrag3D()
        {
#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log("[DRAG-DROP] Start for 3D");
#endif
            
            var hit = Raycaster.GetFirst3DHit(DragDropSettings.Singleton.RaycasterReference);
            if (hit != null)
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] > Find hit...");
#endif
                return hit.Value.collider.FindComponent<IPointerDragStart>();
            }
            
#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log("[DRAG-DROP] > No hit...");
#endif

            return null;
        }
    }
}
#endif