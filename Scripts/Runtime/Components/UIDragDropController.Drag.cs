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
        /// <summary>
        /// Handle drag start (pointer press this frame)
        /// </summary>
        private void HandleDragStart()
        {
            var dragStarts = new List<(string name, IPointerDragSource[] info)>();
            foreach (var dragDropItem in DragDropSettings.Singleton.Items)
            {
                var raycasterInfo = dragDropItem.GetPrimaryRaycasterInfo();
                if (raycasterInfo == null)
                    throw new InvalidOperationException("Raycaster not found: " + dragDropItem.RaycasterReference);

                //Get all drag starts
                dragStarts.Add((
                    dragDropItem.Name,
                    (raycasterInfo.Type switch
                    {
                        RaycastType.Physics3D => HandleDrag(dragDropItem.Name, dragDropItem.HitType, raycasterInfo,
                            key => dragDropItem.HitType == DragDropHitType.FirstHit ? Raycaster.GetFirst3DHit(key).ToSingleArray() : Raycaster.GetAll3DHits(key),
                            hit => hit.collider.FindComponent<IPointerDragSource>()),
                        RaycastType.Physics2D => HandleDrag(dragDropItem.Name, dragDropItem.HitType, raycasterInfo,
                            key => dragDropItem.HitType == DragDropHitType.FirstHit ? Raycaster.GetFirst2DHit(key).ToSingleArray() : Raycaster.GetAll2DHits(key),
                            hit => hit.collider.FindComponent<IPointerDragSource>()),
                        RaycastType.UI => HandleDrag(dragDropItem.Name, dragDropItem.HitType, raycasterInfo,
                            key => dragDropItem.HitType == DragDropHitType.FirstHit ? Raycaster.GetFirstUIHit(key).ToSingleArray() : Raycaster.GetAllUIHits(key),
                            hit => hit.gameObject.FindComponent<IPointerDragSource>()),
                        _ => throw new NotImplementedException(raycasterInfo.Type.ToString())
                    })
                    //Remove all drag starts that not accept this drag drop name (identifier)
                    .Where(x =>
                    {
                        var accept = x.Accept(dragDropItem.Name);
#if PCSOFT_DRAGDROP_LOGGING
                        if (!accept)
                        {
                            Debug.Log("[DRAG-DROP] <" + dragDropItem.Name + "> Start failed, not accepted", (Object)x);
                        }
                        else
                        {
                            Debug.Log("[DRAG-DROP] <" + dragDropItem.Name + "> Start successfully, accepted", (Object)x);
                        }
#endif
                        return accept;
                    })
                    .ToArray()
                ));
            } //For each drag drop system!

            //Start handling on all results from above over all drag drop systems
            foreach (var dragStart in dragStarts)
            {
                //Add missing entries in drag starter list
                if (!_dragStartList.ContainsKey(dragStart.name))
                {
                    _dragStartList.Add(dragStart.name, new List<DragDropInfo>());
                }

                //Replace all drag starts
                _dragStartList[dragStart.name].Clear();
                _dragStartList[dragStart.name].AddRange(
                    dragStarts
                        .SelectMany(x => x.info)
                        .Select(x =>
                        {
                            //Invoke callback method for drag start to get drag drop data
                            x.OnStartDrag(out var data);
                            //Invoke event with all required data
                            DragStarted?.Invoke(this, new DragEventArgs(dragStart.name, x, data));
                            return new DragDropInfo(x, data);
                        })
                );
            }
        }

        private IPointerDragSource[] HandleDrag<T>(string dragDropName, DragDropHitType hitType, RaycastItem raycastItem, Func<string, T[]> getHits,
            Func<T, IPointerDragSource> getDragStart) where T : struct
        {
            //Get all hits from extern logic
            var hits = getHits(raycastItem.Key);
            if (hits is { Length: > 0 })
            {
#if PCSOFT_DRAGDROP_LOGGING
                Debug.Log("[DRAG-DROP] <" + dragDropName + "> > Find hits...");
#endif
                //Specific handling for each hit type
                return hitType switch
                {
                    //Get only the first hit and try to get interface
                    DragDropHitType.FirstHit => getDragStart(hits[0]).ToSingleArray(),
                    //Get only the first hit with existing interface from all hits (overlying with other)
                    DragDropHitType.FirstTarget => hits.Select(getDragStart).FirstOrDefault(x => x != null).ToSingleArray(),
                    //Get all hits with existing interface (overlying with multiple starts)
                    DragDropHitType.AllTargets => hits.Select(getDragStart).Where(x => x != null).ToArray(),
                    _ => throw new NotImplementedException(hitType.ToString())
                };
            }

#if PCSOFT_DRAGDROP_LOGGING
            Debug.Log("[DRAG-DROP] <" + dragDropName + "> > No hits...");
#endif

            return Array.Empty<IPointerDragSource>();
        }
    }
}
#endif