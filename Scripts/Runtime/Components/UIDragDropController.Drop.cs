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
using Object = UnityEngine.Object;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public sealed partial class UIDragDropController
    {
        private void HandleDrop()
        {
            var dropTargets = new List<(string name, (IPointerDropTarget target, DragDropInfo info)[] data)>();
            foreach (var dragDropItem in DragDropSettings.Singleton.Items)
            {
                //Try get info from drag start list
                var dragDropInfo = _dragStartList.ContainsKey(dragDropItem.Name) ? _dragStartList[dragDropItem.Name] : new List<DragDropInfo>();

                var raycasterInfo = dragDropItem.GetSecondaryRaycasterInfo();
                if (raycasterInfo == null)
                    throw new InvalidOperationException("Raycaster not found: " + dragDropItem.GetSecondaryRaycasterReference());

                //Get all drop targets
                dropTargets.Add((
                    dragDropItem.Name,
                    raycasterInfo.Type switch
                    {
                        RaycastType.Physics3D => HandleDrop(false, dragDropItem.Name, dragDropItem.HitType, raycasterInfo, dragDropInfo,
                            key => dragDropItem.HitType == DragDropHitType.FirstHit ? Raycaster.GetFirst3DHit(key).ToSingleArray() : Raycaster.GetAll3DHits(key),
                            hit => hit.collider.FindComponent<IPointerDropTarget>()),
                        RaycastType.Physics2D => HandleDrop(false, dragDropItem.Name, dragDropItem.HitType, raycasterInfo, dragDropInfo,
                            key => dragDropItem.HitType == DragDropHitType.FirstHit ? Raycaster.GetFirst2DHit(key).ToSingleArray() : Raycaster.GetAll2DHits(key),
                            hit => hit.collider.FindComponent<IPointerDropTarget>()),
                        RaycastType.UI => HandleDrop(false, dragDropItem.Name, dragDropItem.HitType, raycasterInfo, dragDropInfo,
                            key => dragDropItem.HitType == DragDropHitType.FirstHit ? Raycaster.GetFirstUIHit(key).ToSingleArray() : Raycaster.GetAllUIHits(key),
                            hit => hit.gameObject.FindComponent<IPointerDropTarget>()),
                        _ => throw new NotImplementedException(raycasterInfo.Type.ToString())
                    }
                ));
            } //For each drag drop system!

            //Start handling on all results from above over all drag drop systems

            //All accepted drop targets
            var successDropTargets = dropTargets
                .SelectMany(x => x.data, (x, data) => (x.name, data))
                .Where(x => x.data.target.Accept(x.name, x.data.info.Data.GetType()))
                .ToArray();
            //All not accepted drop targets
            var failedDropTargets = dropTargets
                .SelectMany(x => x.data, (x, data) => (x.name, data))
                .Where(x => !x.data.target.Accept(x.name, x.data.info.Data.GetType()))
                .ToArray();

            //Handle success
            foreach (var dropTarget in successDropTargets)
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] Finish successfully, accepted", (Object)dropTarget.data.target);
#endif
                //Call callback methods on target
                dropTarget.data.target.OnDrop(dropTarget.data.info.Data);
                dropTarget.data.target.OnDropExit(dropTarget.data.info.Data);

                //Call callback methods on start
                dropTarget.data.info.DragSource.OnDropSuccessfully(dropTarget.data.target, dropTarget.data.info.Data);
                DropSuccessfully?.Invoke(this, new DropEventArgs(dropTarget.name, dropTarget.data.target, dropTarget.data.info.Data));
            }

            //Handle failed
            foreach (var dropTarget in failedDropTargets)
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] Finish failed, not accepted", (Object)dropTarget.data.target);
#endif

                //Call callback methods on start
                dropTarget.data.info.DragSource.OnDropCanceled(dropTarget.data.info.Data);
                DropCanceled?.Invoke(this, new DropEventArgs(dropTarget.name, dropTarget.data.target, dropTarget.data.info.Data));
            }

            //Clear all and cancel rest
            foreach (var key in _dragStartList.Keys)
            {
                var removedList = dropTargets.SelectMany(x => x.data).Select(x => x.info).ToArray();
                _dragStartList[key].RemoveAll(x => removedList.Contains(x));

                //Rest of list will canceled
                foreach (var item in _dragStartList[key])
                {
                    DropCanceled?.Invoke(this, new DropEventArgs(key, null, item.Data));
                    item.DragSource.OnDropCanceled(item.Data);
                }
            }

            //Cleanup drag & drop list
            _dragStartList.Clear();
            _currentDropTargets.Clear();
        }

        private (IPointerDropTarget, DragDropInfo)[] HandleDrop<T>(bool moving, string dragDropName, DragDropHitType hitType, RaycastItem raycastItem, IList<DragDropInfo> dragDropInfo,
            Func<string, T[]> getHits, Func<T, IPointerDropTarget> getDropTarget) where T : struct
        {
            //Get all hits from extern
            var hits = getHits(raycastItem.Key);
            if (hits is { Length: > 0 })
            {
                //Special handling for each hit type
                var targets = hitType switch
                {
                    //Get drop target only from first hit
                    DragDropHitType.FirstHit => getDropTarget(hits[0]).ToSingleArray(),
                    //Get drop target only from first usage of Drop Target Interface (overlying with other)
                    DragDropHitType.FirstTarget => hits.Select(getDropTarget).FirstOrDefault(x => x != null).ToSingleArray(),
                    //Get all drop targets with Drop Target interface (overlying with multiple targets)
                    DragDropHitType.AllTargets => hits.Select(getDropTarget).Where(x => x != null).ToArray(),
                    _ => throw new NotImplementedException(hitType.ToString())
                };

                //Return if found targets
                if (targets is { Length: > 0 })
                    return (from target in targets from info in dragDropInfo select (target, info)).ToArray();

#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] <" + dragDropName + "> " + (moving ? "Moving outside" : "Nothing to finish"));
#endif
            }
            else
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] <" + dragDropName + "> " + (moving ? "Moving outside, no hit" : " Nothing to finish, no hit"));
#endif
            }

            return Array.Empty<(IPointerDropTarget, DragDropInfo)>();
        }
    }
}
#endif