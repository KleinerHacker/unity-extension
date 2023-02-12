#if PCSOFT_DRAGDROP && PCSOFT_RAYCASTER
using System;
using UnityBase.Runtime.@base.Scripts.Runtime.Utils.Extensions;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;
using UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions;
using Object = UnityEngine.Object;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public sealed partial class UIDragDropController
    {
        private void HandleDragStart()
        {
            var raycasterInfo = DragDropSettings.Singleton.GetPrimaryRaycasterInfo();
            var dragStart = raycasterInfo.Type switch
            {
                RaycastType.Physics3D => HandleDrag(Raycaster.GetFirst3DHit, hit => hit.collider.FindComponent<IPointerDragStart>()),
                RaycastType.Physics2D => HandleDrag(Raycaster.GetFirst2DHit, hit => hit.collider.FindComponent<IPointerDragStart>()),
                RaycastType.UI => HandleDrag(Raycaster.GetFirstUIHit, hit => hit.gameObject.FindComponent<IPointerDragStart>()),
                _ => throw new NotImplementedException(raycasterInfo.Type.ToString())
            };

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

        private IPointerDragStart HandleDrag<T>(Func<string, T?> getHit, Func<T, IPointerDragStart> getDragStart) where T : struct
        {
            var hit = getHit(DragDropSettings.Singleton.RaycasterReference);
            if (hit != null)
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] > Find hit...");
#endif
                return getDragStart(hit.Value);
            }

#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log("[DRAG-DROP] > No hit...");
#endif

            return null;
        }
    }
}
#endif