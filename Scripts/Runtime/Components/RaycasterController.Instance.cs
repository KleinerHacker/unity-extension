using UnityEngine;
using UnityEngine.EventSystems;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public sealed partial class RaycasterController
    {
        private abstract class RaycastInstance
        {
            public RaycastItem Item { get; }
            public byte HitCount { get; set; }

            private byte _counter;

            protected RaycastInstance(RaycastItem item)
            {
                Item = item;
            }

            public bool Next()
            {
                _counter++;
                if (_counter >= Item.FixedCheckCount)
                {
                    _counter = 0;
                    return true;
                }

                return false;
            }
        }

        private abstract class RaycastInstance<T> : RaycastInstance
        {
            public T[] Hits { get; internal set; }

            protected RaycastInstance(RaycastItem item) : base(item)
            {
                Hits = new T[item.CountOfHits];
            }
        }

        private sealed class RaycastInstancePhysics3D : RaycastInstance<RaycastHit>
        {
            public RaycastInstancePhysics3D(RaycastItem item) : base(item)
            {
            }
        }

        private sealed class RaycastInstancePhysics2D : RaycastInstance<RaycastHit2D>
        {
            public RaycastInstancePhysics2D(RaycastItem item) : base(item)
            {
            }
        }

        private sealed class RaycastInstanceUI : RaycastInstance<RaycastResult>
        {
            public RaycastInstanceUI(RaycastItem item) : base(item)
            {
            }
        }
    }
}