#if DEMO
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils;

namespace UnityExtension.Demo.extension.Scripts.Demo
{
    public sealed class DragDrop3DSource : MonoBehaviour, IPointerDragSource
    {
        [SerializeField]
        private Image image;

        private bool _isDrag;

        private void Awake()
        {
            image.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (!_isDrag)
                return;

            image.transform.position = InputUtils.GetValueFromDevice(Pointer.current, pointer => pointer.position.ReadValue() + Vector2.up * 200f);
        }

        public bool Accept(string dragDropName) => dragDropName == "3d";

        public void OnStartDrag(out DragDropData data)
        {
            data = new DragDropDemoData(this);
            _isDrag = true;
            image.gameObject.SetActive(true);
        }

        public void OnDropCanceled(DragDropData data)
        {
            _isDrag = false;
            image.gameObject.SetActive(false);
        }

        public void OnDropSuccessfully(IPointerDropTarget target, DragDropData data)
        {
            _isDrag = false;
            image.gameObject.SetActive(false);
        }
    }
}
#endif