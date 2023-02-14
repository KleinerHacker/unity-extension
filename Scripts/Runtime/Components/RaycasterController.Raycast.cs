#if PCSOFT_RAYCASTER
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
using UnityExtension.Runtime.extension.Scripts.Runtime.Utils;

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

            var instancePhysics3D = (RaycastInstancePhysics3D)instance;
            var hitCount = RaycastUtils.HandleRaycast3D(ray, instancePhysics3D);
            instancePhysics3D.HitCount = (byte)hitCount; //Preset values

#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER] <Physics3D> raycast result, hits: " + string.Join(',', instancePhysics3D.Hits.Where(x => x.collider != null).Select(x => x.collider.gameObject.name)) +
                      ", change: " + (hitCount != instancePhysics3D.HitCount) + ", touch: " + instancePhysics3D.Item.Touch + ", dirty: " + instancePhysics3D.IsDirty);
#endif

            if (instancePhysics3D.IsDirty) //Is dirty by values?
            {
                instancePhysics3D.SubmitHits(); //Submit hit changes (IsDirty will be false)
                Raycaster.RaiseRaycast3DChanged(this, instancePhysics3D.Item.Key, instancePhysics3D.Hits, hitCount);
            }

            Raycaster.RaiseRaycast3D(this, instancePhysics3D.Item.Key, instancePhysics3D.Hits, hitCount);
        }

        private void RunRaycastPhysics2D(Ray ray, RaycastInstance instance)
        {
            if (instance.Item.OverUI && EventSystem.current.IsPointerOverGameObject())
                return;

            var instancePhysics2D = (RaycastInstancePhysics2D)instance;
            var hitCount = RaycastUtils.HandleRaycast2D(ray, instancePhysics2D);
            instancePhysics2D.HitCount = (byte)hitCount; //Preset values

#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER] <Physics2D> raycast result, hits: " + string.Join(',', instancePhysics2D.Hits.Where(x => x.collider != null).Select(x => x.collider.gameObject.name)) +
                      ", change: " + (hitCount != instancePhysics2D.HitCount) + ", touch: " + instancePhysics2D.Item.Touch + ", dirty: " + instancePhysics2D.IsDirty);
#endif

            if (instancePhysics2D.IsDirty) //Is dirty by values?
            {
                instancePhysics2D.SubmitHits(); //Submit hit changes (IsDirty will be false)
                Raycaster.RaiseRaycast2DChanged(this, instancePhysics2D.Item.Key, instancePhysics2D.Hits, hitCount);
            }

            Raycaster.RaiseRaycast2D(this, instancePhysics2D.Item.Key, instancePhysics2D.Hits, hitCount);
        }

        private void RunRaycastUI(Vector2 pointer, RaycastInstance instance)
        {
            if (instance.Item.OverUI && !EventSystem.current.IsPointerOverGameObject())
                return;

            var instanceUI = (RaycastInstanceUI)instance;
            var results = RaycastUtils.HandleRaycastUI(pointer, instanceUI).ToArray();
            instanceUI.HitCount = (byte)results.Length; //Preset values
            instanceUI.Hits = results;

#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER] <UI> raycast result, hits: " + string.Join(',', results.Select(x => x.gameObject.name)) +
                      ", change: " + (results.Length != instanceUI.HitCount) + ", touch: " + instanceUI.Item.Touch + ", dirty: " + instanceUI.IsDirty);
#endif

            if (instanceUI.IsDirty) //Is dirty by values?
            {
                instanceUI.SubmitHits(); //Submit hit changes (IsDirty will be false)
                Raycaster.RaiseRaycastUIChanged(this, instance.Item.Key, results);
            }

            Raycaster.RaiseRaycastUI(this, instance.Item.Key, results);
        }
    }
}
#endif