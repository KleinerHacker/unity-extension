using System;
using System.Collections.Generic;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
#if PCSOFT_RAYCASTER
    public static partial class Raycaster
    {
    }

    public sealed class RaycasterEventArgs<T> : EventArgs
    {
        public string Key { get; }
        public T[] Hits { get; }

        internal RaycasterEventArgs(string key, T[] hits)
        {
            Key = key;
            Hits = hits;
        }
    }
#endif
}