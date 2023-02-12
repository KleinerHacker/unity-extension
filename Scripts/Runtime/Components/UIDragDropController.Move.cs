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
            var raycasterInfo = DragDropSettings.Singleton.GetSecondaryRaycasterInfo();
            var dropTarget = raycasterInfo.Type switch
            {
                RaycastType.Physics3D => HandleDrop(true, Raycaster.GetFirst3DHit, hit => hit.collider.FindComponent<IPointerDropTarget>()),
                RaycastType.Physics2D => HandleDrop(true, Raycaster.GetFirst2DHit, hit => hit.collider.FindComponent<IPointerDropTarget>()),
                RaycastType.UI => HandleDrop(true, Raycaster.GetFirstUIHit, hit => hit.gameObject.FindComponent<IPointerDropTarget>()),
                _ => throw new NotImplementedException(raycasterInfo.Type.ToString())
            };

            if (dropTarget != null)
            {
                if (dropTarget.AcceptType(_dragDrop?.data.GetType()))
                {
#if PCSOFT_DRAGDROP_LOGGING
                    Debug.Log("[DRAG-DROP] Move over correct target");
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
                    Debug.Log("[DRAG-DROP] Move over other target");
#endif
                }
            }
            else
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] Move outside");
#endif

                if (_currentDropTarget != null && _currentDropTarget.AcceptType(_dragDrop?.data.GetType()))
                {
                    _currentDropTarget.OnDropExit();
                    _currentDropTarget = null;
                }
            }
        }
    }
}
#endif