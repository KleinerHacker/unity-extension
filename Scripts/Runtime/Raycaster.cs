using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
    public static class Raycaster
    {
        private static readonly IDictionary<string, EventHandler<RaycasterEventArgs>> EventHandlers = new Dictionary<string, EventHandler<RaycasterEventArgs>>();

        #region Events

        private static event EventHandler<RaycasterEventArgs> OnRaycastReceived;

        #endregion

        public static void AddRaycastReceiver(EventHandler<RaycasterEventArgs> e)
        {
            OnRaycastReceived += e;
        }

        public static void RemoveRaycastReceiver(EventHandler<RaycasterEventArgs> e)
        {
            OnRaycastReceived -= e;
        }

        public static void AddRaycastReceiver(string key, EventHandler<RaycasterEventArgs> e)
        {
            if (!EventHandlers.ContainsKey(key))
            {
                EventHandlers.Add(key, e);
            }
            else
            {
                EventHandlers[key] += e;
            }
        }

        public static void RemoveRaycastReceiver(string key, EventHandler<RaycasterEventArgs> e)
        {
            if (!EventHandlers.ContainsKey(key))
                return;

            EventHandlers[key] -= e;
        }

        internal static void RaiseRaycast(object sender, string key, RaycastHit? hit)
        {
            if (EventHandlers.ContainsKey(key))
            {
                EventHandlers[key].Invoke(sender, new RaycasterEventArgs(key, hit));
            }

            OnRaycastReceived?.Invoke(sender, new RaycasterEventArgs(key, hit));
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
}