using System;
using System.Collections.Generic;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
    public static partial class Raycaster
    {
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs<RaycastHit2D>>> Raycast2DChangedDict = new Dictionary<string, EventHandler<RaycasterEventArgs<RaycastHit2D>>>();
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs<RaycastHit2D>>> Raycast2DDict = new Dictionary<string, EventHandler<RaycasterEventArgs<RaycastHit2D>>>();
        private static readonly IDictionary<string, RaycastHit2D[]> Raycast2DHitDict = new Dictionary<string, RaycastHit2D[]>();

        #region Events

        private static event EventHandler<RaycasterEventArgs<RaycastHit2D>> OnRaycast2DChanged;
        private static event EventHandler<RaycasterEventArgs<RaycastHit2D>> OnRaycast2D;

        #endregion
        
        public static RaycastHit2D[] GetAll2DHits(string key) => !Raycast2DHitDict.ContainsKey(key) ? null : Raycast2DHitDict[key];

        public static RaycastHit2D? GetFirst2DHit(string key)
        {
            var hits = GetAll2DHits(key);
            return hits is not { Length: > 0 } ? hits[0] : null;
        }

        public static void AddRaycast2DChanged(EventHandler<RaycasterEventArgs<RaycastHit2D>> e)
        {
            OnRaycast2DChanged += e;
        }

        public static void RemoveRaycast2DChanged(EventHandler<RaycasterEventArgs<RaycastHit2D>> e)
        {
            OnRaycast2DChanged -= e;
        }

        public static void AddRaycast2DChanged(string key, EventHandler<RaycasterEventArgs<RaycastHit2D>> e)
        {
            if (!Raycast2DChangedDict.ContainsKey(key))
            {
                Raycast2DChangedDict.Add(key, e);
            }
            else
            {
                Raycast2DChangedDict[key] += e;
            }
        }

        public static void RemoveRaycast2DChanged(string key, EventHandler<RaycasterEventArgs<RaycastHit2D>> e)
        {
            if (!Raycast2DChangedDict.ContainsKey(key))
                return;

            Raycast2DChangedDict[key] -= e;
        }

        public static void AddRaycast2D(EventHandler<RaycasterEventArgs<RaycastHit2D>> e)
        {
            OnRaycast2D += e;
        }

        public static void RemoveRaycast3D(EventHandler<RaycasterEventArgs<RaycastHit2D>> e)
        {
            OnRaycast2D -= e;
        }

        public static void AddRaycast2D(string key, EventHandler<RaycasterEventArgs<RaycastHit2D>> e)
        {
            if (!Raycast2DDict.ContainsKey(key))
            {
                Raycast2DDict.Add(key, e);
            }
            else
            {
                Raycast2DDict[key] += e;
            }
        }

        public static void RemoveRaycast2D(string key, EventHandler<RaycasterEventArgs<RaycastHit2D>> e)
        {
            if (!Raycast2DDict.ContainsKey(key))
                return;

            Raycast2DDict[key] -= e;
        }

        internal static void RaiseRaycast2DChanged(object sender, string key, RaycastHit2D[] hits)
        {
#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER 2D] Raise Raycast Change for " + key);
#endif
            Raycast2DHitDict.AddOrOverwrite(key, hits);

            if (Raycast2DChangedDict.ContainsKey(key))
            {
                Raycast2DChangedDict[key].Invoke(sender, new RaycasterEventArgs<RaycastHit2D>(key, hits));
            }

            OnRaycast2DChanged?.Invoke(sender, new RaycasterEventArgs<RaycastHit2D>(key, hits));
        }

        internal static void RaiseRaycast2D(object sender, string key, RaycastHit2D[] hits)
        {
#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER 2D] Raise Raycast for " + key);
#endif
            if (Raycast2DDict.ContainsKey(key))
            {
                Raycast2DDict[key].Invoke(sender, new RaycasterEventArgs<RaycastHit2D>(key, hits));
            }

            OnRaycast2D?.Invoke(sender, new RaycasterEventArgs<RaycastHit2D>(key, hits));
        }
    }
}