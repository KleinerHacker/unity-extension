#if PCSOFT_DRAGDROP && PCSOFT_RAYCASTER
using System;
using System.Collections.Generic;
using System.Linq;
using UnityBase.Runtime.@base.Scripts.Runtime.Utils.Extensions;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
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
            var dropTargets = new List<(string name, (IPointerDropTarget target, DragDropInfo info)[] data)>();
            foreach (var dragDropItem in DragDropSettings.Singleton.Items)
            {
                //Try get drag drop info
                var dragDropInfo = _dragStartList.ContainsKey(dragDropItem.Name) ? _dragStartList[dragDropItem.Name] : new List<DragDropInfo>();

                var raycasterInfo = dragDropItem.GetSecondaryRaycasterInfo();
                if (raycasterInfo == null)
                    throw new InvalidOperationException("Raycaster not found: " + dragDropItem.GetSecondaryRaycasterReference());

                //Get all drop targets
                dropTargets.Add((
                    dragDropItem.Name,
                    raycasterInfo.Type switch
                    {
                        RaycastType.Physics3D => HandleDrop(true, dragDropItem.Name, dragDropItem.HitType, raycasterInfo, dragDropInfo,
                            key => dragDropItem.HitType == DragDropHitType.FirstHit ? Raycaster.GetFirst3DHit(key).ToSingleArray() : Raycaster.GetAll3DHits(key),
                            hit => hit.collider.FindComponent<IPointerDropTarget>()),
                        RaycastType.Physics2D => HandleDrop(true, dragDropItem.Name, dragDropItem.HitType, raycasterInfo, dragDropInfo,
                            key => dragDropItem.HitType == DragDropHitType.FirstHit ? Raycaster.GetFirst2DHit(key).ToSingleArray() : Raycaster.GetAll2DHits(key),
                            hit => hit.collider.FindComponent<IPointerDropTarget>()),
                        RaycastType.UI => HandleDrop(true, dragDropItem.Name, dragDropItem.HitType, raycasterInfo, dragDropInfo,
                            key => dragDropItem.HitType == DragDropHitType.FirstHit ? Raycaster.GetFirstUIHit(key).ToSingleArray() : Raycaster.GetAllUIHits(key),
                            hit => hit.gameObject.FindComponent<IPointerDropTarget>()),
                        _ => throw new NotImplementedException(raycasterInfo.Type.ToString())
                    }
                ));
            } //For all drag drop systems!

            //Start handling on all results from above over all drag drop systems

            //All accepted drop targets
            var successDropTargets = dropTargets
                .SelectMany(x => x.data, (x, data) => (x.name, data))
                .Where(x => x.data.target.Accept(x.name, x.data.info.Data.GetType()))
                .ToArray();

            //Drop targets that was leave
            var removedTargets = _currentDropTargets
                .Where(x => successDropTargets.All(y => x != y.data.target))
                .ToArray();
            //Drop targets that was entered
            var addedTargets = successDropTargets
                .Where(x => _currentDropTargets.All(y => x.data.target != y))
                .ToArray();

            //Call on leaved targets callback method exit
            foreach (var dropTarget in removedTargets)
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] Move outside of target, exit");
#endif
                foreach (var key in _dragStartList.Keys)
                {
                    foreach (var info in _dragStartList[key])
                    {
                        DropExited?.Invoke(this, new DropEventArgs(key, dropTarget, info.Data));
                        dropTarget.OnDropExit(info.Data);
                    }
                }
            }

            //Call on entered targets callback method enter
            foreach (var target in addedTargets)
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] Move over target, entered");
#endif
                DropEntered?.Invoke(this, new DropEventArgs(target.name, target.data.target, target.data.info.Data));
                target.data.target.OnDropEnter(target.data.info.Data);
            }

            //Update drop targets
            _currentDropTargets.RemoveAll(x => removedTargets.Contains(x));
            _currentDropTargets.AddRange(addedTargets.Select(x => x.data.target));
        }
    }
}
#endif