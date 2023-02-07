using System;
using System.Collections.Generic;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
#if PCSOFT_RAYCASTER
    public static class Raycaster
    {
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs>> RaycastChangedDict = new Dictionary<string, EventHandler<RaycasterEventArgs>>();
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs>> RaycastDict = new Dictionary<string, EventHandler<RaycasterEventArgs>>();
        private static readonly IDictionary<string, RaycastHit?> RaycastHitDict = new Dictionary<string, RaycastHit?>();

        #region Events

        private static event EventHandler<RaycasterEventArgs> OnRaycastChanged;
        private static event EventHandler<RaycasterEventArgs> OnRaycast;

        #endregion

        public static RaycastHit? GetHit(string key) => !RaycastHitDict.ContainsKey(key) ? null : RaycastHitDict[key];

        public static void AddRaycastChanged(EventHandler<RaycasterEventArgs> e)
        {
            OnRaycastChanged += e;
        }

        public static void RemoveRaycastChanged(EventHandler<RaycasterEventArgs> e)
        {
            OnRaycastChanged -= e;
        }

        public static void AddRaycastChanged(string key, EventHandler<RaycasterEventArgs> e)
        {
            if (!RaycastChangedDict.ContainsKey(key))
            {
                RaycastChangedDict.Add(key, e);
            }
            else
            {
                RaycastChangedDict[key] += e;
            }
        }

        public static void RemoveRaycastChanged(string key, EventHandler<RaycasterEventArgs> e)
        {
            if (!RaycastChangedDict.ContainsKey(key))
                return;

            RaycastChangedDict[key] -= e;
        }

        public static void AddRaycast(EventHandler<RaycasterEventArgs> e)
        {
            OnRaycast += e;
        }

        public static void RemoveRaycast(EventHandler<RaycasterEventArgs> e)
        {
            OnRaycast -= e;
        }

        public static void AddRaycast(string key, EventHandler<RaycasterEventArgs> e)
        {
            if (!RaycastDict.ContainsKey(key))
            {
                RaycastDict.Add(key, e);
            }
            else
            {
                RaycastDict[key] += e;
            }
        }

        public static void RemoveRaycast(string key, EventHandler<RaycasterEventArgs> e)
        {
            if (!RaycastDict.ContainsKey(key))
                return;

            RaycastDict[key] -= e;
        }

        internal static void RaiseRaycastChanged(object sender, string key, RaycastHit? hit)
        {
#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER] Raise Raycast Change for " + key);
#endif
            RaycastHitDict.AddOrOverwrite(key, hit);

            if (RaycastChangedDict.ContainsKey(key))
            {
                RaycastChangedDict[key].Invoke(sender, new RaycasterEventArgs(key, hit));
            }

            OnRaycastChanged?.Invoke(sender, new RaycasterEventArgs(key, hit));
        }

        internal static void RaiseRaycast(object sender, string key, RaycastHit? hit)
        {
#if PCSOFT_RAYCASTER_LOGGING
            Debug.Log("[RAYCASTER] Raise Raycast for " + key);
#endif
            if (RaycastDict.ContainsKey(key))
            {
                RaycastDict[key].Invoke(sender, new RaycasterEventArgs(key, hit));
            }

            OnRaycast?.Invoke(sender, new RaycasterEventArgs(key, hit));
        }
    }

    public sealed class RaycasterEventArgs : EventArgs
    {
        public string Key { get; }
        public RaycastHit? Hit { get; }

        internal RaycasterEventArgs(string key, RaycastHit? hit)
        {
            Key = key;
            Hit = hit;
        }
    }
#endif
}