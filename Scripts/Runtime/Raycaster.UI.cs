#if PCSOFT_RAYCASTER
using System;
using System.Collections.Generic;
#if PCSOFT_RAYCASTER_LOGGING
using UnityEngine;
#endif
using UnityEngine.EventSystems;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
    public static partial class Raycaster
    {
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs<RaycastResult>>> RaycastUIChangedDict = new Dictionary<string, EventHandler<RaycasterEventArgs<RaycastResult>>>();
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs<RaycastResult>>> RaycastUIDict = new Dictionary<string, EventHandler<RaycasterEventArgs<RaycastResult>>>();
        private static readonly IDictionary<string, DynamicRaycastList<RaycastResult>> RaycastUIHitDict = new Dictionary<string, DynamicRaycastList<RaycastResult>>();

        #region Events

        private static event EventHandler<RaycasterEventArgs<RaycastResult>> OnRaycastUIChanged;
        private static event EventHandler<RaycasterEventArgs<RaycastResult>> OnRaycastUI;

        #endregion

        public static RaycastResult[] GetAllUIHits(string key) => !RaycastUIHitDict.ContainsKey(key) ? Array.Empty<RaycastResult>() : RaycastUIHitDict[key].Array;

        public static RaycastResult? GetFirstUIHit(string key)
        {
            var hits = GetAllUIHits(key);
            return hits.Length > 0 ? hits[0] : null;
        }

        public static void AddRaycastUIChanged(EventHandler<RaycasterEventArgs<RaycastResult>> e)
        {
            OnRaycastUIChanged += e;
        }

        public static void RemoveRaycastUIChanged(EventHandler<RaycasterEventArgs<RaycastResult>> e)
        {
            OnRaycastUIChanged -= e;
        }

        public static void AddRaycastUIChanged(string key, EventHandler<RaycasterEventArgs<RaycastResult>> e)
        {
            if (!RaycastUIChangedDict.ContainsKey(key))
            {
                RaycastUIChangedDict.Add(key, e);
            }
            else
            {
                RaycastUIChangedDict[key] += e;
            }
        }

        public static void RemoveRaycastUIChanged(string key, EventHandler<RaycasterEventArgs<RaycastResult>> e)
        {
            if (!RaycastUIChangedDict.ContainsKey(key))
                return;

            RaycastUIChangedDict[key] -= e;
        }

        public static void AddRaycastUI(EventHandler<RaycasterEventArgs<RaycastResult>> e)
        {
            OnRaycastUI += e;
        }

        public static void RemoveRaycastUI(EventHandler<RaycasterEventArgs<RaycastResult>> e)
        {
            OnRaycastUI -= e;
        }

        public static void AddRaycastUI(string key, EventHandler<RaycasterEventArgs<RaycastResult>> e)
        {
            if (!RaycastUIDict.ContainsKey(key))
            {
                RaycastUIDict.Add(key, e);
            }
            else
            {
                RaycastUIDict[key] += e;
            }
        }

        public static void RemoveRaycastUI(string key, EventHandler<RaycasterEventArgs<RaycastResult>> e)
        {
            if (!RaycastUIDict.ContainsKey(key))
                return;

            RaycastUIDict[key] -= e;
        }

        internal static void RaiseRaycastUIChanged(object sender, string key, RaycastResult[] hits)
        {
#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER UI] Raise Raycast Change for " + key);
#endif

            if (!RaycastUIHitDict.ContainsKey(key))
            {
                RaycastUIHitDict.Add(key, new DynamicRaycastList<RaycastResult>());
            }

            RaycastUIHitDict[key].SetContent(hits);

            if (RaycastUIChangedDict.ContainsKey(key))
            {
                RaycastUIChangedDict[key].Invoke(sender, new RaycasterEventArgs<RaycastResult>(key, hits, hits.Length));
            }

            OnRaycastUIChanged?.Invoke(sender, new RaycasterEventArgs<RaycastResult>(key, hits, hits.Length));
        }

        internal static void RaiseRaycastUI(object sender, string key, RaycastResult[] hits)
        {
#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER UI] Raise Raycast for " + key);
#endif
            if (RaycastUIDict.ContainsKey(key))
            {
                RaycastUIDict[key].Invoke(sender, new RaycasterEventArgs<RaycastResult>(key, hits, hits.Length));
            }

            OnRaycastUI?.Invoke(sender, new RaycasterEventArgs<RaycastResult>(key, hits, hits.Length));
        }
    }
}
#endif