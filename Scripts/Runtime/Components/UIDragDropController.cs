#if PCSOFT_DRAGDROP && PCSOFT_RAYCASTER
using System;
using System.Collections.Generic;
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

        public bool IsDragDrop => _dragStartList.Count > 0;

        #endregion

        #region Events

        public event EventHandler<DragEventArgs> DragStarted;
        public event EventHandler<DropEventArgs> DropCanceled;
        public event EventHandler<DropEventArgs> DropSuccessfully;

        #endregion

        //Stores all drag start / data info
        private readonly IDictionary<string, List<DragDropInfo>> _dragStartList = new Dictionary<string, List<DragDropInfo>>();

        //Stores current potential drop targets
        private readonly List<IPointerDropTarget> _currentDropTargets = new List<IPointerDropTarget>();

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

        private record DragDropInfo(IPointerDragSource DragSource, DragDropData Data)
        {
            public IPointerDragSource DragSource { get; } = DragSource;
            public DragDropData Data { get; } = Data;
        }
    }

    public class DragEventArgs : EventArgs
    {
        public string Name { get; }
        public IPointerDragSource DragSource { get; }
        public DragDropData Data { get; }

        public DragEventArgs(string name, IPointerDragSource dragSource, DragDropData data)
        {
            Name = name;
            DragSource = dragSource;
            Data = data;
        }
    }

    public class DropEventArgs : EventArgs
    {
        public string Name { get; }
        public IPointerDropTarget DropTarget { get; }
        public DragDropData Data { get; }

        public DropEventArgs(string name, IPointerDropTarget dropTarget, DragDropData data)
        {
            Name = name;
            DropTarget = dropTarget;
            Data = data;
        }
    }
}
#endif