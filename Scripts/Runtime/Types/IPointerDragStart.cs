namespace UnityExtension.Runtime.extension.Scripts.Runtime.Types
{
    public interface IPointerDragStart
    {
        void OnStartDrag(out DragDropData data);
        void OnDropCanceled();
        void OnDropSuccessfully(IPointerDropTarget target);
    }
}