#if PCSOFT_DRAGDROP && PCSOFT_RAYCASTER
using System;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
    public static class DragDropSystem
    {
        public static event EventHandler<DragEventArgs> DragStarted
        {
            add => UIDragDropController.Singleton.DragStarted += value;
            remove => UIDragDropController.Singleton.DragStarted -= value;
        }

        public static event EventHandler<DropEventArgs> DropSuccessfully
        {
            add => UIDragDropController.Singleton.DropSuccessfully += value;
            remove => UIDragDropController.Singleton.DropSuccessfully -= value;
        }

        public static event EventHandler<DropEventArgs> DropCanceled
        {
            add => UIDragDropController.Singleton.DropCanceled += value;
            remove => UIDragDropController.Singleton.DropCanceled -= value;
        }

        public static event EventHandler<DropEventArgs> DropEntered
        {
            add => UIDragDropController.Singleton.DropEntered += value;
            remove => UIDragDropController.Singleton.DropEntered -= value;
        }

        public static event EventHandler<DropEventArgs> DropExited
        {
            add => UIDragDropController.Singleton.DropExited += value;
            remove => UIDragDropController.Singleton.DropExited -= value;
        }
    }
}
#endif