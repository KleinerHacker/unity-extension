using System;
using System.Linq;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;
using UnityEngine.Serialization;

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

        #region Inspector Data

        [SerializeField]
        private DragDropItem[] items = Array.Empty<DragDropItem>();

        #endregion

        #region Properties

        public DragDropItem[] Items => items.Where(x => x.Active).ToArray();

        #endregion

        public DragDropItem Get(string name) => Items.FirstOrDefault(x => x.Name == name);
    }

    [Serializable]
    public sealed class DragDropItem
    {
        #region Inspector Data

        [SerializeField]
        private string name;

        [SerializeField]
        private bool active = true;

        [SerializeField]
        private string raycasterReference;

        [SerializeField]
        private bool useAlternativeRaycasterForMove;

        [SerializeField]
        private string raycasterMoveReference;

        [SerializeField]
        private DragDropHitType hitType = DragDropHitType.FirstTarget;

        #endregion

        #region Properties

        public string Name => name;

        public bool Active => active;

        public string RaycasterReference => raycasterReference;

        public bool UseAlternativeRaycasterForMove => useAlternativeRaycasterForMove;

        public string RaycasterMoveReference => raycasterMoveReference;

        public DragDropHitType HitType => hitType;

        #endregion
    }

    public enum DragDropHitType
    {
        FirstHit,
        FirstTarget,
        AllTargets
    }
}