#if DEMO
using System;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime;

namespace UnityExtension.Demo.extension.Scripts.Demo
{
    [RequireComponent(typeof(Renderer))]
    public sealed class Raycaster2Receiver : MonoBehaviour
    {
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            Raycaster.AddRaycastChanged("ray2", OnRay2);
        }

        private void OnDisable()
        {
            Raycaster.RemoveRaycastChanged("ray2", OnRay2);
        }

        private void OnRay2(object sender, RaycasterEventArgs e)
        {
            Debug.Log("OnRay2: " + e.Hit.HasValue);
            _renderer.material.color = e.Hit.HasValue ? Color.green : Color.white;
        }
    }
}
#endif