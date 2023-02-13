#if DEMO
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Demo.extension.Scripts.Demo
{
    public sealed class DragDropDemoData : DragDropData
    {
        public DragDropDemoData(IPointerDragSource source) : base(source)
        {
        }
    }
}
#endif