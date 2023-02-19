#if DEMO
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils;

namespace UnityExtension.Demo.extension.Scripts.Demo
{
    public sealed class DragDropUISource : UIBehaviour, IPointerDragSource
    {
        [SerializeField]
        private Image image;

        private bool _isDrag;

        protected override void Awake()
        {
            image.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (!_isDrag)
                return;

            image.transform.position = InputUtils.GetValueFromDevice(Pointer.current, pointer => pointer.position.ReadValue() + Vector2.up * 200f);
        }

        public bool Accept(string dragDropName) => dragDropName == "ui";

        public void OnStartDrag(out DragDropData data)
        {
            data = new DragDropDemoData(this);
            image.gameObject.SetActive(true);
            _isDrag = true;
        }

        public void OnDropCanceled(DragDropData data)
        {
            image.gameObject.SetActive(false);
            _isDrag = false;
        }

        public void OnDropSuccessfully(IPointerDropTarget target, DragDropData data)
        {
            image.gameObject.SetActive(false);
            _isDrag = false;
        }
    }
}
#endif