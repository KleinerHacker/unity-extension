using UnityEditorEx.Runtime.Projects.unity_editor_ex.Scripts.Runtime.Extra;
using UnityEngine;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Assets
{
    public abstract class IdentifiableAsset : ScriptableObject
    {
        #region Inspector Data

        [SerializeField]
        [ReadOnly]
        private string guid;

        #endregion

        #region Properties

        public string Guid => guid;

        #endregion

        #region Builtin Methods

#if UNITY_EDITOR
        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                guid = System.Guid.NewGuid().ToString();
            }
        }
#endif

        #endregion
    }
}