namespace UnityExtension.Runtime.extension.Scripts.Runtime.Types
{
    public interface IPointerDragSource
    {
        bool Accept(string dragDropName);

        void OnStartDrag(out DragDropData data);
        void OnDropCanceled(DragDropData data);
        void OnDropSuccessfully(IPointerDropTarget target, DragDropData data);
    }
}