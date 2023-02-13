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
            foreach (var dragDropItem in DragDropSettings.Singleton.Items)
            {
                var dragDropInfo = _dragDropList.ContainsKey(dragDropItem.Name) ? _dragDropList[dragDropItem.Name] : null;

                var raycasterInfo = dragDropItem.GetSecondaryRaycasterInfo();
                var dropTarget = raycasterInfo.Type switch
                {
                    RaycastType.Physics3D => HandleDrop(false, dragDropItem.Name, raycasterInfo, dragDropInfo,
                        Raycaster.GetFirst3DHit, hit => hit.collider.FindComponent<IPointerDropTarget>()),
                    RaycastType.Physics2D => HandleDrop(false, dragDropItem.Name, raycasterInfo, dragDropInfo,
                        Raycaster.GetFirst2DHit, hit => hit.collider.FindComponent<IPointerDropTarget>()),
                    RaycastType.UI => HandleDrop(false, dragDropItem.Name, raycasterInfo, dragDropInfo,
                        Raycaster.GetFirstUIHit, hit => hit.gameObject.FindComponent<IPointerDropTarget>()),
                    _ => throw new NotImplementedException(raycasterInfo.Type.ToString())
                };

                if (dropTarget != null)
                {
#if PCSOFT_DRAGDROP_LOGGING
                    Debug.Log("[DRAG-DROP] <" + dragDropItem.Name + "> Finish", (Object)dropTarget);
#endif

                    dropTarget.OnDrop(dragDropInfo?.Data);

                    dragDropInfo?.DragSource.OnDropSuccessfully(dropTarget);
                    DropSuccessfully?.Invoke(this, new DropEventArgs(dragDropItem.Name, dropTarget, dragDropInfo?.Data));
                }
                else
                {
#if PCSOFT_DRAGDROP_LOGGING
                    Debug.Log("[DRAG-DROP] <" + dragDropItem.Name + "> Finish failed, drop is canceled");
#endif

                    dragDropInfo?.DragSource.OnDropCanceled();
                    DropCanceled?.Invoke(this, new DropEventArgs(dragDropItem.Name, null, dragDropInfo?.Data));
                }

                _currentDropTarget?.OnDropExit();
                _currentDropTarget = null;
                _dragDropList.Remove(dragDropItem.Name);
            }
        }

        private IPointerDropTarget HandleDrop<T>(bool moving, string dragDropName, RaycastItem raycastItem, DragDropInfo dragDropInfo, Func<string, T?> getHit, Func<T, IPointerDropTarget> getDropTarget) where T : struct
        {
            var hit = getHit(raycastItem.Key);
            if (hit != null)
            {
                var result = getDropTarget(hit.Value);
                if (result != null && !result.Accept(dragDropName, dragDropInfo?.Data.GetType()))
                {
#if PCSOFT_DRAGDROP_LOGGING
                    Debug.Log("[DRAG-DROP] <" + dragDropName + "> " + (moving ? "Moving over other" : "Finish failed, not accepted"));
#endif
                    return null;
                }

#if PCSOFT_DRAGDROP_LOGGING
                if (result != null)
                {
                    Debug.Log("[DRAG-DROP] <" + dragDropName + "> " + (moving ? "Moving over accepted" : "Finish, accepted"));
                }
                else
                {
                    Debug.Log("[DRAG-DROP] <" + dragDropName + "> " + (moving ? "Moving outside" : "Nothing to finish"));
                }
#endif

                return result;
            }

#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log("[DRAG-DROP] <" + dragDropName + "> " + (moving ? "Moving outside, no hit" : " Nothing to finish, no hit"));
#endif

            return null;
        }
    }
}
#endif