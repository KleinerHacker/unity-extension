#if PCSOFT_RAYCASTER && PCSOFT_PREVIEW
using System;
using UnityBase.Runtime.@base.Scripts.Runtime.Components.Singleton;
using UnityBase.Runtime.@base.Scripts.Runtime.Components.Singleton.Attributes;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
#if PCSOFT_PREVIEW
    [DefaultExecutionOrder(UnityExtensionConstants.Script.ExecutionOrder.PreviewController)]
    [Singleton(Scope = SingletonScope.Application, Instance = SingletonInstance.RequiresNewInstance, CreationTime = SingletonCreationTime.Loading, ObjectName = "Preview System")]
    public sealed class PreviewController : SingletonBehavior<PreviewController>
    {
        #region Properties

        public bool IsShown => !string.IsNullOrEmpty(ShownPreview);
        public string ShownPreview => _item?.Key;

        #endregion

        private GameObject _preview;
        private PreviewItem _item;

        #region Builtin Methods

        private void OnEnable()
        {
            Raycaster.AddRaycast3D(PreviewSettings.Singleton.Raycaster, OnRaycaster);
        }

        private void OnDisable()
        {
            Raycaster.RemoveRaycast3D(PreviewSettings.Singleton.Raycaster, OnRaycaster);
        }

        #endregion

        public void Show(string key)
        {
            if (IsShown)
            {
                Hide();
            }

            _item = PreviewSettings.Singleton.Items.FirstOrThrow(x => x.Key == key,
                () => new InvalidOperationException("Key '" + key + "' is unknown in previews"));

            _preview = Instantiate(_item.PreviewPrefab);
            _preview.name = "Preview";
        }

        public void Hide()
        {
            if (!IsShown)
                return;

            Destroy(_preview);

            _preview = null;
            _item = null;
        }

        private void OnRaycaster(object sender, RaycasterEventArgs<RaycastHit> e)
        {
            if (!IsShown || e.Hits is not { Length: > 0 })
                return;

            _preview.transform.position = _item.PreviewPosition + e.Hits[0].point;
            if (_item.AutoRotate)
            {
                var euler = Quaternion.FromToRotation(Vector3.up, e.Hits[0].normal).eulerAngles;
                _preview.transform.rotation = Quaternion.Euler(_item.PreviewRotation) * Quaternion.Euler(
                    Mathf.Clamp(euler.x, -_item.MaxRotation, _item.MaxRotation),
                    euler.y,
                    Mathf.Clamp(euler.y, -_item.MaxRotation, _item.MaxRotation)
                );
            }
        }
    }
#endif
}
#endif