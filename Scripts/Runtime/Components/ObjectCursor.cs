using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Components
{
    [AddComponentMenu(UnityExtensionConstants.ROOT + "/Object Cursor")]
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