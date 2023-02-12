using System;
using UnityBase.Runtime.@base.Scripts.Runtime.Components.Singleton.Attributes;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    [Singleton(Instance = SingletonInstance.RequiresNewInstance, Scope = SingletonScope.Application, CreationTime = SingletonCreationTime.Loading, ObjectName = "Drag & Drop")]
    public sealed partial class UIDragDropController : UIBehaviour
    {
        #region Properties

        public bool IsDragDrop => _dragDrop != null;

        #endregion

        #region Events

        public event EventHandler<DragEventArgs> DragStarted;
        public event EventHandler<DropEventArgs> DropCanceled;
        public event EventHandler<DropEventArgs> DropSuccessfully;

        #endregion

        private (IPointerDragStart dragStart, DragDropData data)? _dragDrop;
        private IPointerDropTarget _currentDropTarget;

        private int counter = 0;

        #region Builtin Methods

        private void Update()
        {
            if (!IsDragDrop)
            {
                if (InputUtils.GetValueFromDevice(Pointer.current, pointer => pointer.press.wasPressedThisFrame))
                {
                    HandleDragStart();
                }
            }
            else
            {
                if (InputUtils.GetValueFromDevice(Pointer.current, pointer => pointer.press.wasReleasedThisFrame))
                {
                    HandleDrop();
                }
                else
                {
                    counter++;
                    if (counter > 10)
                    {
                        try
                        {
                            HandleDragDropMove();
                        }
                        finally
                        {
                            counter = 0;   
                        }
                    }
                }
            }
        }

        #endregion
    }

    public class DragEventArgs : EventArgs
    {
        public IPointerDragStart DragStart { get; }
        public DragDropData Data { get; }

        public DragEventArgs(IPointerDragStart dragStart, DragDropData data)
        {
            DragStart = dragStart;
            Data = data;
        }
    }

    public class DropEventArgs : EventArgs
    {
        public IPointerDropTarget DropTarget { get; }
        public DragDropData Data { get; }

        public DropEventArgs(IPointerDropTarget dropTarget, DragDropData data)
        {
            DropTarget = dropTarget;
            Data = data;
        }
    }
}