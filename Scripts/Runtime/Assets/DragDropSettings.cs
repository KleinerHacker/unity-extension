using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public sealed class DragDropSettings : ProviderAsset<DragDropSettings>
    {
        #region Static Area

        public static DragDropSettings Singleton => GetSingleton("Drag Drop", "drag-drop.asset");

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => GetSerializedSingleton("Drag Drop", "drag-drop.asset");
#endif

        #endregion

        #region Properties

        [SerializeField]
        private string raycasterReference;

        [SerializeField]
        private bool useAlternativeRaycasterForMove;

        [SerializeField]
        private string raycasterMoveReference;

        #endregion

        #region Properties

        public string RaycasterReference => raycasterReference;

        public bool UseAlternativeRaycasterForMove => useAlternativeRaycasterForMove;

        public string RaycasterMoveReference => raycasterMoveReference;

        #endregion
    }
}