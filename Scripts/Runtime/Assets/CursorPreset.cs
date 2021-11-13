using System;
using System.Linq;
using Unity.Collections;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Types;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Utils;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Assets
{
    public abstract class CursorPreset<TDefault, TDefaultEnum, TSpecific, TSpecificEnum>
        where TDefault : CursorItem<TDefaultEnum>
        where TSpecific : CursorItem<TSpecificEnum>
        where TDefaultEnum : Enum
        where TSpecificEnum : Enum
    {
        #region Inspector Data

        [SerializeField]
        private TDefault[] defaultCursorItems;

        [SerializeField]
        private TSpecific[] specificCursorItems;

        #endregion

        #region Properties

        public TDefault[] DefaultCursorItems => defaultCursorItems;

        public TSpecific[] SpecificCursorItems => specificCursorItems;

        #endregion

        protected CursorPreset()
        {
            defaultCursorItems = ArrayUtils.CreateIdentifierArray<TDefault, TDefaultEnum>();
            specificCursorItems = ArrayUtils.CreateIdentifierArray<TSpecific, TSpecificEnum>();
        }

        public TDefault Find(TDefaultEnum identifier) => defaultCursorItems.First(x => Equals(x.Identifier, identifier));
        public TSpecific Find(TSpecificEnum identifier) => specificCursorItems.First(x => Equals(x.Identifier, identifier));
    }

    [Serializable]
    public class CursorItem
    {
        #region Inspector Data

        [SerializeField]
        private Texture2D cursor;

        [SerializeField]
        private Vector2 hotspot;

        #endregion

        #region Properties

        public Texture2D Cursor => cursor;

        public Vector2 Hotspot => hotspot;

        #endregion
    }

    [Serializable]
    public abstract class CursorItem<T> : CursorItem, IIdentifiedObject<T> where T : Enum
    {
        #region Inspector Data

        [ReadOnly]
        [SerializeField]
        private T identifier;

        #endregion

        #region Properties

        public T Identifier => identifier;

        #endregion

        public CursorItem(T identifier)
        {
            this.identifier = identifier;
        }
    }
}