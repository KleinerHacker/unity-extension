using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public sealed partial class RaycasterController
    {
        internal abstract class RaycastInstance
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

        internal abstract class RaycastInstance<T> : RaycastInstance, IEqualityComparer<T>
        {
            public T[] Hits { get; internal set; }

            private T[] _lastHits = Array.Empty<T>();
            private int _lastHitCount = 0;

            public bool IsDirty => Hits.Length != _lastHits.Length || HitCount != _lastHitCount || !Hits.SequenceEqual(_lastHits, this);

            protected RaycastInstance(RaycastItem item) : base(item)
            {
                Hits = new T[item.CountOfHits];
            }

            public void SubmitHits()
            {
                _lastHits = Hits.ToArray();
                _lastHitCount = HitCount;
            }

            public abstract bool Equals(T x, T y);
            public abstract int GetHashCode(T obj);
        }

        internal sealed class RaycastInstancePhysics3D : RaycastInstance<RaycastHit>
        {
            public RaycastInstancePhysics3D(RaycastItem item) : base(item)
            {
            }

            public override bool Equals(RaycastHit x, RaycastHit y) =>
                (x.collider != null ? x.collider.gameObject : null) == (y.collider != null ? y.collider.gameObject : null);

            public override int GetHashCode(RaycastHit obj) =>
                obj.collider != null ? obj.collider.gameObject.GetHashCode() : 0;
        }

        internal sealed class RaycastInstancePhysics2D : RaycastInstance<RaycastHit2D>
        {
            public RaycastInstancePhysics2D(RaycastItem item) : base(item)
            {
            }

            public override bool Equals(RaycastHit2D x, RaycastHit2D y) =>
                (x.collider != null ? x.collider.gameObject : null) == (y.collider != null ? y.collider.gameObject : null);

            public override int GetHashCode(RaycastHit2D obj) =>
                obj.collider != null ? obj.collider.gameObject.GetHashCode() : 0;
        }

        internal sealed class RaycastInstanceUI : RaycastInstance<RaycastResult>
        {
            public RaycastInstanceUI(RaycastItem item) : base(item)
            {
            }

            public override bool Equals(RaycastResult x, RaycastResult y) =>
                x.gameObject == y.gameObject;

            public override int GetHashCode(RaycastResult obj) =>
                obj.gameObject.GetHashCode();
        }
    }
}