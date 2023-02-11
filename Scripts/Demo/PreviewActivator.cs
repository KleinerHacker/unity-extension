#if DEMO
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime;

namespace UnityExtension.Demo.extension.Scripts.Demo
{
    public sealed class PreviewActivator : MonoBehaviour
    {
        private void Start()
        {
            PreviewSystem.Show("cube");
        }
    }
}
#endif