﻿#if DEMO
using System;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Demo.extension.Scripts.Demo
{
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class DragDropTarget : MonoBehaviour, IPointerDropTarget
    {
        private MeshRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _renderer.material.color = Color.blue;
        }

        public bool AcceptType(Type type) => type == typeof(DragDropDemoData);

        public void OnDrop(DragDropData data)
        {
            _renderer.material.color = Color.green;
        }

        public void OnDropEnter() => _renderer.material.color = Color.yellow;

        public void OnDropExit()
        {
            if (_renderer.material.color == Color.yellow)
            {
                _renderer.material.color = Color.blue;
            }
        }
    }
}
#endif