using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Utils
{
    internal static class RaycastUtils
    {
        public static int HandleRaycast3D(Ray ray, RaycasterController.RaycastInstancePhysics3D instance) =>
            Physics.RaycastNonAlloc(ray, instance.Hits, instance.Item.MaxDistance, instance.Item.LayerMask.value);

        public static int HandleRaycast2D(Ray ray, RaycasterController.RaycastInstancePhysics2D instance) =>
            Physics2D.RaycastNonAlloc(ray.origin, ray.direction, instance.Hits, instance.Item.MaxDistance, instance.Item.LayerMask.value);

        public static List<RaycastResult> HandleRaycastUI(Vector2 pointer, RaycasterController.RaycastInstanceUI instance)
        {
            var pointerEvent = new PointerEventData(EventSystem.current)
            {
                position = pointer
            };

            var resultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEvent, resultList);

            return resultList;
        }
    }
}