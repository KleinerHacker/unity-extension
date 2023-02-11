#if DEMO
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
            Raycaster.AddRaycast3DChanged("ray2", OnRay2);
        }

        private void OnDisable()
        {
            Raycaster.RemoveRaycast3DChanged("ray2", OnRay2);
        }

        private void OnRay2(object sender, RaycasterEventArgs<RaycastHit> e)
        {
            Debug.Log("OnRay2: " + (e.Hits.Length > 0));
            _renderer.material.color = e.Hits.Length > 0 ? Color.green : Color.white;
        }
    }
}
#endif