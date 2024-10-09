#if PCSOFT_CURSOR
using UnityEngine;

namespace UnityExtensions.Runtime.Projects.unity_extensions.Scripts.Runtime.Components
{
    /// <summary>
    /// Component to set a custom cursor for a 3d scene object.
    /// <b>Please note that you must add a ObjectCursorRaycaster to your camera!</b>
    /// </summary>
    [AddComponentMenu(UnityExtensionsConstants.ROOT + "/Object Cursor")]
    [DisallowMultipleComponent]
    public sealed class ObjectCursor : MonoBehaviour
    {
        #region Inspector Data

        [SerializeField] private string cursorKey;

        #endregion

        #region Properties

        public string CursorKey => cursorKey;

        #endregion
    }
}
#endif