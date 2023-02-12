#if PCSOFT_RAYCASTER
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
    public static partial class Raycaster
    {
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs<RaycastHit2D>>> Raycast2DChangedDict = new Dictionary<string, EventHandler<RaycasterEventArgs<RaycastHit2D>>>();
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs<RaycastHit2D>>> Raycast2DDict = new Dictionary<string, EventHandler<RaycasterEventArgs<RaycastHit2D>>>();
        private static readonly IDictionary<string, FixedRaycastList<RaycastHit2D>> Raycast2DHitDict = new Dictionary<string, FixedRaycastList<RaycastHit2D>>();

        #region Events

        private static event EventHandler<RaycasterEventArgs<RaycastHit2D>> OnRaycast2DChanged;
        private static event EventHandler<RaycasterEventArgs<RaycastHit2D>> OnRaycast2D;

        #endregion

        public static RaycastHit2D[] GetAll2DHits(string key) => !Raycast2DHitDict.ContainsKey(key) ? Array.Empty<RaycastHit2D>() : Raycast2DHitDict[key].Array;

        public static RaycastHit2D? GetFirst2DHit(string key)
        {
            var hits = GetAll2DHits(key);
            return hits.Length > 0 ? hits[0] : null;
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

        internal static void RaiseRaycast2DChanged(object sender, string key, RaycastHit2D[] hits, int count)
        {
#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER 2D] Raise Raycast Change for " + key);
#endif

            if (!Raycast2DHitDict.ContainsKey(key))
            {
                Raycast2DHitDict.Add(key, new FixedRaycastList<RaycastHit2D>(RaycastSettings.Singleton.Items.First(x => x.Key == key).CountOfHits));
            }

            Raycast2DHitDict[key].SetContent(hits, count);

            if (Raycast2DChangedDict.ContainsKey(key))
            {
                Raycast2DChangedDict[key].Invoke(sender, new RaycasterEventArgs<RaycastHit2D>(key, hits, count));
            }

            OnRaycast2DChanged?.Invoke(sender, new RaycasterEventArgs<RaycastHit2D>(key, hits, count));
        }

        internal static void RaiseRaycast2D(object sender, string key, RaycastHit2D[] hits, int count)
        {
#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER 2D] Raise Raycast for " + key);
#endif
            if (Raycast2DDict.ContainsKey(key))
            {
                Raycast2DDict[key].Invoke(sender, new RaycasterEventArgs<RaycastHit2D>(key, hits, count));
            }

            OnRaycast2D?.Invoke(sender, new RaycasterEventArgs<RaycastHit2D>(key, hits, count));
        }
    }
}
#endif