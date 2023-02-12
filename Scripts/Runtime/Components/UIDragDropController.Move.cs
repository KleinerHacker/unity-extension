using UnityEngine.EventSystems;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    public sealed partial class UIDragDropController
    {
        private void HandleDragDropMove()
        {
            var dropTarget = EventSystem.current.IsPointerOverGameObject() ? HandleDropUI() : HandleDrop3D();
            if (dropTarget != null)
            {
                if (_currentDropTarget == null && dropTarget.AcceptType(_dragDrop?.data.GetType()))
                {
                    dropTarget.OnDropEnter();
                    _currentDropTarget = dropTarget;
                }
            }
            else if (_currentDropTarget != null && _currentDropTarget.AcceptType(_dragDrop?.data.GetType()))
            {
                _currentDropTarget.OnDropExit();
                _currentDropTarget = null;
            }
        }
    }
}