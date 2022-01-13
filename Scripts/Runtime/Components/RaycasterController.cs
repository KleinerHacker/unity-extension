using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    [AddComponentMenu(UnityExtensionConstants.Root + "/Raycaster")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public sealed class RaycasterController : MonoBehaviour
    {
        #region Inspector Data

        [SerializeField]
        private string[] raycasters;

        #endregion

        private Camera _camera;
        private RaycastInstance[] _instances;

        #region Builtin Methods

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            enabled = raycasters is { Length: > 0 };

            if (enabled)
            {
                _instances = raycasters.Select(x =>
                {
                    var raycastItem = RaycastSettings.Singleton.Items.FirstOrDefault(y => string.Equals(y.Key, x, StringComparison.Ordinal));
                    if (raycastItem == null)
                        throw new InvalidOperationException("Key '" + x + "' not found as raycaster!");

                    return new RaycastInstance(raycastItem);
                }).ToArray();
            }
        }

        private void FixedUpdate()
        {
            var instances = _instances.Where(x => x.Next()).ToArray();
            if (instances.Length <= 0)
                return;

            var ray = _camera.ScreenPointToRay(Pointer.current.position.ReadValue());
            foreach (var instance in instances)
            {
                RunRaycast(ray, instance);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            raycasters = raycasters
                .Where(x => RaycastSettings.Singleton.Items.Any(y => string.Equals(y.Key, x, StringComparison.Ordinal)))
                .ToArray();
        }
#endif

        #endregion

        private void RunRaycast(Ray ray, RaycastInstance instance)
        {
            if (Physics.Raycast(ray, out var hit, instance.Item.MaxDistance, instance.Item.LayerMask))
            {
                if (!instance.HasHit)
                {
                    instance.HasHit = true;
                    Raycaster.RaiseRaycast(this, instance.Item.Key, hit);
                }
            }
            else
            {
                if (instance.HasHit)
                {
                    instance.HasHit = false;
                    Raycaster.RaiseRaycast(this, instance.Item.Key, null);
                }
            }
        }

        private sealed class RaycastInstance
        {
            public RaycastItem Item { get; }
            public bool HasHit { get; set; }

            private byte _counter;

            public RaycastInstance(RaycastItem item)
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
    }
}