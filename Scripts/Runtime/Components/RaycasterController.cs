using System;
using System.Linq;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
using UnityExtension.Runtime.extension.Scripts.Runtime.Utils;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
#if PCSOFT_RAYCASTER
    [AddComponentMenu(UnityExtensionConstants.Root + "/Raycaster")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    [DefaultExecutionOrder(-99999)]
    public sealed partial class RaycasterController : MonoBehaviour
    {
        #region Inspector Data

        [SerializeField]
        private string[] raycasters;

        #endregion

        private Camera _camera;
        private RaycastInstance[] _regularInstances;
        private RaycastInstance[] _offsetInstances;
        private RaycastInstance[] _touchInstances;
        private RaycastInstance[] _touchOffsetInstances;

        private bool _alreadyTouched;

        #region Builtin Methods

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            enabled = raycasters is { Length: > 0 };

            if (enabled)
            {
                var instances = raycasters.Select(x =>
                {
                    var raycastItem = RaycastSettings.Singleton.Items.FirstOrDefault(y => string.Equals(y.Key, x, StringComparison.Ordinal));
                    if (raycastItem == null)
                        throw new InvalidOperationException("Key '" + x + "' not found as raycaster!");

                    return raycastItem.Type switch
                    {
                        RaycastType.Physics3D => (RaycastInstance)new RaycastInstancePhysics3D(raycastItem),
                        RaycastType.Physics2D => (RaycastInstance)new RaycastInstancePhysics2D(raycastItem),
                        RaycastType.UI => (RaycastInstance)new RaycastInstanceUI(raycastItem),
                        _ => throw new NotImplementedException(raycastItem.Type.ToString())
                    };
                }).ToArray();
                _regularInstances = instances.Where(x => !x.Item.Touch && x.Item.Offset == Vector2.zero).ToArray();
                _offsetInstances = instances.Where(x => !x.Item.Touch && x.Item.Offset != Vector2.zero).ToArray();
                _touchInstances = instances.Where(x => x.Item.Touch && x.Item.Offset == Vector2.zero).ToArray();
                _touchOffsetInstances = instances.Where(x => x.Item.Touch && x.Item.Offset != Vector2.zero).ToArray();
            }
        }

        private void Update()
        {
            if (!PointerUtils.IsPressed())
            {
                _alreadyTouched = false;
                return;
            }

            RaycastInstance[] touchInstances;
            RaycastInstance[] touchOffsetInstances;
            if (_alreadyTouched)
            {
                touchInstances = _touchInstances.Where(x => x.Next()).ToArray();
                touchOffsetInstances = _touchOffsetInstances.Where(x => x.Next()).ToArray();
                if (touchInstances.Length <= 0 && touchOffsetInstances.Length <= 0)
                    return;
            }
            else
            {
                touchInstances = _touchInstances;
                touchOffsetInstances = _touchOffsetInstances;
                _alreadyTouched = true;
            }

            var pos = PointerUtils.GetPosition();
            var ray = _camera.ScreenPointToRay(pos);

            foreach (var instance in touchInstances)
            {
                RunRaycast(pos, ray, instance);
            }

            foreach (var instance in touchOffsetInstances)
            {
                var posOffset = PointerUtils.GetPosition() + instance.Item.Offset;
                var rayOffset = _camera.ScreenPointToRay(posOffset);

                RunRaycast(posOffset, rayOffset, instance);
            }
        }

        private void FixedUpdate()
        {
            var regularInstances = _regularInstances.Where(x => x.Next()).ToArray();
            var offsetInstances = _offsetInstances.Where(x => x.Next()).ToArray();
            if (regularInstances.Length <= 0 && offsetInstances.Length <= 0)
                return;

            var pos = PointerUtils.GetPosition();
            var ray = _camera.ScreenPointToRay(pos);
            foreach (var instance in regularInstances)
            {
                RunRaycast(pos, ray, instance);
            }

            foreach (var instance in offsetInstances)
            {
                var posOffset = PointerUtils.GetPosition() + instance.Item.Offset;
                var rayOffset = _camera.ScreenPointToRay(posOffset);

                RunRaycast(posOffset, rayOffset, instance);
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
    }
#endif
}