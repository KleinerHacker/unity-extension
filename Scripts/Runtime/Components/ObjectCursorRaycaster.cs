#if PCSOFT_CURSOR
using UnityBase.Runtime.Projects.unity_base.Scripts.Runtime.Utils;
using UnityCommons.Runtime.Projects.unity_commons.Scripts.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UnityExtensions.Runtime.Projects.unity_extensions.Scripts.Runtime.Components
{
    /// <summary>
    /// Raycaster component for your scene camera to handle cursor update over objects.
    /// </summary>
    [AddComponentMenu(UnityExtensionsConstants.ROOT + "/Object Cursor Raycaster")]
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    public sealed class ObjectCursorRaycaster : MonoBehaviour
    {
        #region Inspector Data

        [SerializeField]
        [Min(1)]
        private int updateFrame = 10;

        #endregion
        
        private Camera camera;
        private ObjectCursor currentObjectCursor;
        private Counting frameCounter;

        #region Builtin Methods

        private void Awake()
        {
            camera = GetComponent<Camera>();
            frameCounter = new Incrementing(updateFrame);
        }

        private void FixedUpdate()
        {
            if (updateFrame > 1 && !frameCounter.Try(1f))
                return;
            
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            
            var ray = camera.ScreenPointToRay(Mouse.current.position.value);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var objectCursor = hit.collider.GetComponent<ObjectCursor>();
                if (objectCursor != currentObjectCursor)
                {
                    OnChange(currentObjectCursor, objectCursor);
                    currentObjectCursor = objectCursor;
                }
            }
            else
            {
                OnChange(currentObjectCursor, null);
                currentObjectCursor = null;
            }
        }

        #endregion

        private void OnChange(ObjectCursor oldCursor, ObjectCursor newCursor)
        {
            if (newCursor == null)
            {
                CursorSystem.ResetCursor();
            }
            else
            {
                CursorSystem.ChangeCursor(newCursor.CursorKey);
            }
        }
    }
}
#endif