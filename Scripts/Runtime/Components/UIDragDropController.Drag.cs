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
            foreach (var dragDropItem in DragDropSettings.Singleton.Items)
            {
                var raycasterInfo = dragDropItem.GetPrimaryRaycasterInfo();
                var dragStart = raycasterInfo.Type switch
                {
                    RaycastType.Physics3D => HandleDrag(dragDropItem.Name, raycasterInfo,
                        Raycaster.GetFirst3DHit, hit => hit.collider.FindComponent<IPointerDragSource>()),
                    RaycastType.Physics2D => HandleDrag(dragDropItem.Name, raycasterInfo,
                        Raycaster.GetFirst2DHit, hit => hit.collider.FindComponent<IPointerDragSource>()),
                    RaycastType.UI => HandleDrag(dragDropItem.Name, raycasterInfo,
                        Raycaster.GetFirstUIHit, hit => hit.gameObject.FindComponent<IPointerDragSource>()),
                    _ => throw new NotImplementedException(raycasterInfo.Type.ToString())
                };

                if (dragStart != null)
                {
                    if (!dragStart.Accept(dragDropItem.Name))
                    {
#if PCSOFT_DRAGDROP_LOGGING
                        Debug.Log("[DRAG-DROP] <" + dragDropItem.Name + "> Start failed, not accepted", (Object)dragStart);
#endif
                        return;
                    }

#if PCSOFT_DRAGDROP_LOGGING
                    Debug.Log("[DRAG-DROP] <" + dragDropItem.Name + "> Start", (Object)dragStart);
#endif

                    dragStart.OnStartDrag(out var data);
                    if (_dragDropList.ContainsKey(dragDropItem.Name))
                    {
                        _dragDropList.Remove(dragDropItem.Name);
                    }

                    _dragDropList.Add(dragDropItem.Name, new DragDropInfo(dragStart, data));

                    DragStarted?.Invoke(this, new DragEventArgs(dragDropItem.Name, dragStart, data));
                }
                else
                {
#if PCSOFT_DRAGDROP_LOGGING
                    Debug.Log("[DRAG-DROP] <" + dragDropItem.Name + "> Start failed, object is ignored");
#endif
                }
            }
        }

        private IPointerDragSource HandleDrag<T>(string dragDropName, RaycastItem raycastItem, Func<string, T?> getHit, Func<T, IPointerDragSource> getDragStart) where T : struct
        {
            var hit = getHit(raycastItem.Key);
            if (hit != null)
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] <" + dragDropName + "> > Find hit...");
#endif
                return getDragStart(hit.Value);
            }

#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log("[DRAG-DROP] <" + dragDropName + "> > No hit...");
#endif

            return null;
        }
    }
}
#endif