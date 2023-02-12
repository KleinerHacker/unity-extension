using System.Collections.Generic;
using System.Linq;
using UnityBase.Runtime.@base.Scripts.Runtime.Utils.Extensions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
                dragStart.OnStartDrag(out var data);
                _dragDrop = (dragStart, data);

                DragStarted?.Invoke(this, new DragEventArgs(dragStart, data));
            }
        }
        
        private IPointerDragStart HandleDragUI()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = InputUtils.GetValueFromDevice(Pointer.current, pointer => pointer.position.ReadValue())
            };
            var resultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, resultList);

            return resultList
                .Select(x => x.gameObject.GetComponent<IPointerDragStart>())
                .FirstOrDefault(x => x != null);
        }

        private IPointerDragStart HandleDrag3D()
        {
            var hit = Raycaster.GetFirst3DHit("drag_move");
            if (hit != null)
                return hit.Value.collider.FindComponent<IPointerDragStart>();

            return null;
        }
    }
}