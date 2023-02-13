#if DEMO
using System;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Demo.extension.Scripts.Demo
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class DragDrop2DTarget : MonoBehaviour, IPointerDropTarget
    {
        [SerializeField]
        private Sprite normal;

        [SerializeField]
        private Sprite hover;

        [SerializeField]
        private Sprite dropped;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.sprite = normal;
        }

        public bool Accept(string name, Type type) => name == "2d" && type == typeof(DragDropDemoData);

        public void OnDrop(DragDropData data)
        {
            _renderer.sprite = dropped;
        }

        public void OnDropEnter()
        {
            _renderer.sprite = hover;
        }

        public void OnDropExit()
        {
            if (_renderer.sprite == dropped)
                return;

            _renderer.sprite = normal;
        }
    }
}
#endif