#if PCSOFT_DRAGDROP && PCSOFT_RAYCASTER
using System;
using UnityBase.Runtime.@base.Scripts.Runtime.Utils.Extensions;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;
using UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public sealed partial class UIDragDropController
    {
        private void HandleDragDropMove()
        {
            foreach (var dragDropItem in DragDropSettings.Singleton.Items)
            {
                var dragDropInfo = _dragDropList.ContainsKey(dragDropItem.Name) ? _dragDropList[dragDropItem.Name] : null;

                var raycasterInfo = dragDropItem.GetSecondaryRaycasterInfo();
                var dropTarget = raycasterInfo.Type switch
                {
                    RaycastType.Physics3D => HandleDrop(true, dragDropItem.Name, raycasterInfo, dragDropInfo,
                        Raycaster.GetFirst3DHit, hit => hit.collider.FindComponent<IPointerDropTarget>()),
                    RaycastType.Physics2D => HandleDrop(true, dragDropItem.Name, raycasterInfo, dragDropInfo,
                        Raycaster.GetFirst2DHit, hit => hit.collider.FindComponent<IPointerDropTarget>()),
                    RaycastType.UI => HandleDrop(true, dragDropItem.Name, raycasterInfo, dragDropInfo,
                        Raycaster.GetFirstUIHit, hit => hit.gameObject.FindComponent<IPointerDropTarget>()),
                    _ => throw new NotImplementedException(raycasterInfo.Type.ToString())
                };

                if (dropTarget != null)
                {
                    if (dropTarget.Accept(dragDropItem.Name, dragDropInfo?.Data.GetType()))
                    {
#if PCSOFT_DRAGDROP_LOGGING
                        Debug.Log("[DRAG-DROP] <" + dragDropItem.Name + "> Move over correct target");
#endif

                        if (_currentDropTarget != dropTarget)
                        {
                            _currentDropTarget?.OnDropExit();

                            dropTarget.OnDropEnter();
                            _currentDropTarget = dropTarget;
                        }
                    }
                    else
                    {
#if PCSOFT_DRAGDROP_LOGGING
                        Debug.Log("[DRAG-DROP] <" + dragDropItem.Name + "> Move over other target");
#endif
                    }
                }
                else
                {
#if PCSOFT_DRAGDROP_LOGGING
                    Debug.Log("[DRAG-DROP] <" + dragDropItem.Name + "> Move outside");
#endif

                    if (_currentDropTarget != null && _currentDropTarget.Accept(dragDropItem.Name, dragDropInfo?.Data.GetType()))
                    {
                        _currentDropTarget.OnDropExit();
                        _currentDropTarget = null;
                    }
                }
            }
        }
    }
}
#endif