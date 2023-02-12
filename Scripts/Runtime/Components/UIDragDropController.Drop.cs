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
        private void HandleDrop()
        {
            var raycasterInfo = DragDropSettings.Singleton.GetSecondaryRaycasterInfo();
            var dropTarget = raycasterInfo.Type switch
            {
                RaycastType.Physics3D => HandleDrop(false, Raycaster.GetFirst3DHit, hit => hit.collider.FindComponent<IPointerDropTarget>()),
                RaycastType.Physics2D => HandleDrop(false, Raycaster.GetFirst2DHit, hit => hit.collider.FindComponent<IPointerDropTarget>()),
                RaycastType.UI => HandleDrop(false, Raycaster.GetFirstUIHit, hit => hit.gameObject.FindComponent<IPointerDropTarget>()),
                _ => throw new NotImplementedException(raycasterInfo.Type.ToString())
            };

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

        private IPointerDropTarget HandleDrop<T>(bool moving, Func<string, T?> getHit, Func<T, IPointerDropTarget> getDropTarget) where T : struct
        {
            var hit = getHit(DragDropSettings.Singleton.GetSecondaryRaycasterReference());
            if (hit != null)
            {
                var result = getDropTarget(hit.Value);
                if (result != null && !result.AcceptType(_dragDrop?.data.GetType()))
                {
#if PCSOFT_DRAGDROP_LOGGING
                    Debug.Log(moving ? "[DRAG-DROP] Moving over other" : "[DRAG-DROP] Finish failed, not accepted");
#endif
                    return null;
                }

#if PCSOFT_DRAGDROP_LOGGING
                if (result != null)
                {
                    Debug.Log(moving ? "[DRAG-DROP] Moving over accepted" : "[DRAG-DROP] Finish, accepted");
                }
                else
                {
                    Debug.Log(moving ? "[DRAG-DROP] Moving outside" : "[DRAG-DROP] Nothing to finish");
                }
#endif

                return result;
            }

#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log(moving ? "[DRAG-DROP] Moving outside" : "[DRAG-DROP] Nothing to finish");
#endif

            return null;
        }
    }
}
#endif