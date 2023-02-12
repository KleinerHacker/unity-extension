using System.Collections.Generic;
using System.Linq;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Types
{
    public interface IRaycastList<out T>
    {
        T[] Array { get; }

        int Count { get; }
    }

    public sealed class FixedRaycastList<T> : IRaycastList<T>
    {
        private T[] _array;

        public T[] Array => _array.Take(Count).ToArray();

        public int Count { get; private set; }

        public FixedRaycastList(int capacity)
        {
            _array = new T[capacity];
            Count = 0;
        }

        public void SetContent(T[] hits, int count)
        {
            _array = hits;
            Count = count;
        }
    }

    public sealed class DynamicRaycastList<T> : IRaycastList<T>
    {
        private readonly List<T> _list = new List<T>();

        public T[] Array => _list.ToArray();

        public int Count => _list.Count;

        public void SetContent(IEnumerable<T> list)
        {
            _list.Clear();
            _list.AddRange(list);
        }
    }
}