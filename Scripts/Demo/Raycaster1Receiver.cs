#if DEMO
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime;

namespace UnityExtension.Demo.extension.Scripts.Demo
{
    [RequireComponent(typeof(Renderer))]
    public sealed class Raycaster1Receiver : MonoBehaviour
    {
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            Raycaster.AddRaycast3DChanged("ray1", OnRay1);
        }

        private void OnDisable()
        {
            Raycaster.RemoveRaycast3DChanged("ray1", OnRay1);
        }

        private void OnRay1(object sender, RaycasterEventArgs<RaycastHit> e)
        {
            Debug.Log("OnRay1: " + (e.Hits.Length > 0));
            _renderer.material.color = e.Hits.Length > 0 ? Color.yellow : Color.white;
        }
    }
}
#endif