#if DEMO
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Demo.extension.Scripts.Demo
{
    [RequireComponent(typeof(Image))]
    public sealed class DragDropUITarget : UIBehaviour, IPointerDropTarget
    {
        [SerializeField]
        private Sprite normal;

        [SerializeField]
        private Sprite hover;

        [SerializeField]
        private Sprite dropped;

        private Image _image;

        protected override void Awake()
        {
            _image = GetComponent<Image>();
            _image.sprite = normal;
        }

        public bool Accept(string name, Type type) => name == "ui" && type == typeof(DragDropDemoData);

        public void OnDrop(DragDropData data)
        {
            _image.sprite = dropped;
        }

        public void OnDropEnter()
        {
            _image.sprite = hover;
        }

        public void OnDropExit()
        {
            if (_image.sprite == dropped)
                return;

            _image.sprite = normal;
        }
    }
}
#endif