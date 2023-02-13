#if PCSOFT_RAYCASTER
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public sealed partial class RaycasterController
    {
        private void RunRaycast(Vector2 pointer, Ray ray, RaycastInstance instance)
        {
            switch (instance.Item.Type)
            {
                case RaycastType.Physics3D:
                    RunRaycastPhysics3D(ray, instance);
                    break;
                case RaycastType.Physics2D:
                    RunRaycastPhysics2D(ray, instance);
                    break;
                case RaycastType.UI:
                    RunRaycastUI(pointer, instance);
                    break;
                default:
                    throw new NotImplementedException(instance.Item.Type.ToString());
            }
        }

        private void RunRaycastPhysics3D(Ray ray, RaycastInstance instance)
        {
            if (instance.Item.OverUI && EventSystem.current.IsPointerOverGameObject())
                return;

            var raycastHits = ((RaycastInstancePhysics3D)instance).Hits;
            var hitCount = Physics.RaycastNonAlloc(ray, raycastHits, instance.Item.MaxDistance, instance.Item.LayerMask.value);

#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER] <Physics3D> raycast result, hits: " + hitCount +
                      ", change: " + (hitCount != instance.HitCount));
#endif

            if (hitCount != instance.HitCount)
            {
                instance.HitCount = (byte)hitCount;
                Raycaster.RaiseRaycast3DChanged(this, instance.Item.Key, raycastHits, hitCount);
            }

            Raycaster.RaiseRaycast3D(this, instance.Item.Key, raycastHits, hitCount);
        }

        private void RunRaycastPhysics2D(Ray ray, RaycastInstance instance)
        {
            if (instance.Item.OverUI && EventSystem.current.IsPointerOverGameObject())
                return;

            var raycastHits = ((RaycastInstancePhysics2D)instance).Hits;
            var hitCount = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, raycastHits, instance.Item.MaxDistance, instance.Item.LayerMask.value);

#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER] <Physics2D> raycast result, hits: " + hitCount +
                      ", change: " + (hitCount != instance.HitCount));
#endif

            if (hitCount != instance.HitCount)
            {
                instance.HitCount = (byte)hitCount;
                Raycaster.RaiseRaycast2DChanged(this, instance.Item.Key, raycastHits, hitCount);
            }

            Raycaster.RaiseRaycast2D(this, instance.Item.Key, raycastHits, hitCount);
        }

        private void RunRaycastUI(Vector2 pointer, RaycastInstance instance)
        {
            if (instance.Item.OverUI && !EventSystem.current.IsPointerOverGameObject())
                return;

            var pointerEvent = new PointerEventData(EventSystem.current)
            {
                position = pointer,
                delta = Vector2.one * 100f
            };
            var resultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEvent, resultList);
            var results = resultList.ToArray();

#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER] <UI> raycast result, hits: " + results.Length +
                      ", change: " + (results.Length != instance.HitCount));
#endif

            if (results.Length != instance.HitCount)
            {
                instance.HitCount = (byte)results.Length;
                ((RaycastInstanceUI)instance).Hits = results;
                Raycaster.RaiseRaycastUIChanged(this, instance.Item.Key, results);
            }

            Raycaster.RaiseRaycastUI(this, instance.Item.Key, results);
        }
    }
}
#endif