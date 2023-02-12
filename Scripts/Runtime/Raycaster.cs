using System;

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
        public int Count { get; }

        internal RaycasterEventArgs(string key, T[] hits, int count)
        {
            Key = key;
            Hits = hits;
            Count = count;
        }
    }
#endif
}