using System;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Types
{
    public interface IPointerDropTarget
    {
        bool Accept(string dragDropName, Type type);
        void OnDrop(DragDropData data);

        void OnDropEnter();
        void OnDropExit();
    }
}