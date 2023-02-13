namespace UnityExtension.Runtime.extension.Scripts.Runtime.Types
{
    public abstract class DragDropData
    {
        public IPointerDragSource Source { get; }

        protected DragDropData(IPointerDragSource source)
        {
            Source = source;
        }
    }
}