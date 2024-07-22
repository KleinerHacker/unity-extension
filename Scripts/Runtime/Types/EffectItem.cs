using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Types
{
    [Serializable]
    public abstract class EffectItemBase
    {
        #region Inspector Data

        [SerializeField]
        private GameObject effectPrefab;

        [Space]
        [SerializeField]
        private Vector3 position;

        [SerializeField]
        private Vector3 rotation;

        [Space]
        [SerializeField]
        private float postDelay;

        #endregion

        #region Properties

        public GameObject EffectPrefab => effectPrefab;

        public Vector3 Position => position;

        public Quaternion Rotation => Quaternion.Euler(rotation);

        public float PostDelay => postDelay;

        #endregion
    }

    [Serializable]
    public sealed class LoopEffectItem : EffectItemBase
    {
    }

    [Serializable]
    public sealed class SimpleEffectItem : EffectItemBase
    {
        #region Inspector Data

        [SerializeField]
        private float minPreDelay;
        
        [SerializeField]
        private float maxPreDelay;

        #endregion

        #region Properties

        public float PreDelay => Random.Range(minPreDelay, maxPreDelay);

        #endregion
    }

    [Serializable]
    public sealed class ActionEffectItem : EffectItemBase
    {
        #region Inspector Data
        
        [SerializeField]
        private float minPreDelay;
        
        [SerializeField]
        private float maxPreDelay;

        [SerializeField]
        private float actionDelay;

        #endregion

        #region Properties
        
        public float PreDelay => Random.Range(minPreDelay, maxPreDelay);

        public float ActionDelay => actionDelay;

        #endregion
    }
}