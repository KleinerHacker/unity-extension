using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
#if PCSOFT_RAYCASTER
    [AddComponentMenu(UnityExtensionConstants.Root + "/Raycaster")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public sealed partial class RaycasterController : MonoBehaviour
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

                    return raycastItem.Type switch
                    {
                        RaycastType.Physics3D => (RaycastInstance) new RaycastInstancePhysics3D(raycastItem),
                        RaycastType.Physics2D => (RaycastInstance) new RaycastInstancePhysics2D(raycastItem),
                        RaycastType.UI => (RaycastInstance) new RaycastInstanceUI(raycastItem),
                        _ => throw new NotImplementedException(raycastItem.Type.ToString())
                    };
                }).ToArray();
            }
        }

        private void FixedUpdate()
        {
            var instances = _instances.Where(x => x.Next()).ToArray();
            if (instances.Length <= 0)
                return;

            var pos = Pointer.current.position.ReadValue();
            var ray = _camera.ScreenPointToRay(pos);
            foreach (var instance in instances.Where(x => x.Item.Offset == Vector2.zero))
            {
                RunRaycast(pos, ray, instance);
            }

            foreach (var instance in instances.Where(x => x.Item.Offset != Vector2.zero))
            {
                var posOffset = Pointer.current.position.ReadValue() + instance.Item.Offset;
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