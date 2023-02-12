#if PCSOFT_RAYCASTER
using System;
using System.Collections.Generic;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
    public static partial class Raycaster
    {
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs<RaycastHit>>> Raycast3DChangedDict = new Dictionary<string, EventHandler<RaycasterEventArgs<RaycastHit>>>();
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs<RaycastHit>>> Raycast3DDict = new Dictionary<string, EventHandler<RaycasterEventArgs<RaycastHit>>>();
        private static readonly IDictionary<string, RaycastHit[]> Raycast3DHitDict = new Dictionary<string, RaycastHit[]>();

        #region Events

        private static event EventHandler<RaycasterEventArgs<RaycastHit>> OnRaycast3DChanged;
        private static event EventHandler<RaycasterEventArgs<RaycastHit>> OnRaycast3D;

        #endregion
        
        public static RaycastHit[] GetAll3DHits(string key) => !Raycast3DHitDict.ContainsKey(key) ? Array.Empty<RaycastHit>() : Raycast3DHitDict[key];

        public static RaycastHit? GetFirst3DHit(string key)
        {
            var hits = GetAll3DHits(key);
            return hits is { Length: > 0 } ? hits[0] : null;
        }

        public static void AddRaycast3DChanged(EventHandler<RaycasterEventArgs<RaycastHit>> e)
        {
            OnRaycast3DChanged += e;
        }

        public static void RemoveRaycast3DChanged(EventHandler<RaycasterEventArgs<RaycastHit>> e)
        {
            OnRaycast3DChanged -= e;
        }

        public static void AddRaycast3DChanged(string key, EventHandler<RaycasterEventArgs<RaycastHit>> e)
        {
            if (!Raycast3DChangedDict.ContainsKey(key))
            {
                Raycast3DChangedDict.Add(key, e);
            }
            else
            {
                Raycast3DChangedDict[key] += e;
            }
        }

        public static void RemoveRaycast3DChanged(string key, EventHandler<RaycasterEventArgs<RaycastHit>> e)
        {
            if (!Raycast3DChangedDict.ContainsKey(key))
                return;

            Raycast3DChangedDict[key] -= e;
        }

        public static void AddRaycast3D(EventHandler<RaycasterEventArgs<RaycastHit>> e)
        {
            OnRaycast3D += e;
        }

        public static void RemoveRaycast3D(EventHandler<RaycasterEventArgs<RaycastHit>> e)
        {
            OnRaycast3D -= e;
        }

        public static void AddRaycast3D(string key, EventHandler<RaycasterEventArgs<RaycastHit>> e)
        {
            if (!Raycast3DDict.ContainsKey(key))
            {
                Raycast3DDict.Add(key, e);
            }
            else
            {
                Raycast3DDict[key] += e;
            }
        }

        public static void RemoveRaycast3D(string key, EventHandler<RaycasterEventArgs<RaycastHit>> e)
        {
            if (!Raycast3DDict.ContainsKey(key))
                return;

            Raycast3DDict[key] -= e;
        }

        internal static void RaiseRaycast3DChanged(object sender, string key, RaycastHit[] hits)
        {
#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER 3D] Raise Raycast Change for " + key);
#endif
            Raycast3DHitDict.AddOrOverwrite(key, hits);

            if (Raycast3DChangedDict.ContainsKey(key))
            {
                Raycast3DChangedDict[key].Invoke(sender, new RaycasterEventArgs<RaycastHit>(key, hits));
            }

            OnRaycast3DChanged?.Invoke(sender, new RaycasterEventArgs<RaycastHit>(key, hits));
        }

        internal static void RaiseRaycast3D(object sender, string key, RaycastHit[] hits)
        {
#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER 3D] Raise Raycast for " + key);
#endif
            if (Raycast3DDict.ContainsKey(key))
            {
                Raycast3DDict[key].Invoke(sender, new RaycasterEventArgs<RaycastHit>(key, hits));
            }

            OnRaycast3D?.Invoke(sender, new RaycasterEventArgs<RaycastHit>(key, hits));
        }
    }
}
#endif