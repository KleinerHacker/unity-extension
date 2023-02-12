namespace UnityExtension.Runtime.extension.Scripts.Runtime.Types
{
    public abstract class DragDropData
    {
        public IPointerDragStart Source { get; }

        protected DragDropData(IPointerDragStart source)
        {
            Source = source;
        }
    }
}