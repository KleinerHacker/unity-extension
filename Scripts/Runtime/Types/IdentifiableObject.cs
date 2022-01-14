using System;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Extra;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Types
{
    [Serializable]
    public abstract class IdentifiableObject
    {
        #region Inspector Data

        [SerializeField]
        [ReadOnly]
        private string guid;

        #endregion

        #region Properties

        public string Guid
        {
            get => guid;
#if UNITY_EDITOR
            set => guid = value;
#endif
        }

        #endregion
    }
}