using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Types
{
    public interface IPrefabItem
    {
        #region Properties

        public abstract Vector3 Position { get; }

        public abstract Quaternion Rotation { get; }

        public abstract float Scaling { get; }

        public GameObject Prefab { get; }

        #endregion
    }

    [Serializable]
    public abstract class PrefabItemBase : IPrefabItem
    {
        #region Inspector Data

        [SerializeField]
        private GameObject prefab;

        #endregion

        #region Properties

        public abstract Vector3 Position { get; }

        public abstract Quaternion Rotation { get; }

        public abstract float Scaling { get; }

        public GameObject Prefab => prefab;

        #endregion
    }
    
    [Serializable]
    public abstract class PrefabItemBase<T> : IPrefabItem where T : Component
    {
        #region Inspector Data

        [SerializeField]
        private T prefab;

        #endregion

        #region Properties

        public abstract Vector3 Position { get; }

        public abstract Quaternion Rotation { get; }

        public abstract float Scaling { get; }

        public GameObject Prefab => prefab.gameObject;

        #endregion
    }

    [Serializable]
    public sealed class FixedPrefabItem : PrefabItemBase
    {
        #region Inspector Data

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        private Vector3 rotation;

        [SerializeField]
        private float scaling = 1f;

        #endregion

        #region Properties

        public override Vector3 Position => position;

        public override Quaternion Rotation => Quaternion.Euler(rotation);

        public override float Scaling => scaling;

        #endregion
    }
    
    [Serializable]
    public class FixedPrefabItem<T> : PrefabItemBase<T> where T : Component
    {
        #region Inspector Data

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        private Vector3 rotation;

        [SerializeField]
        private float scaling = 1f;

        #endregion

        #region Properties

        public override Vector3 Position => position;

        public override Quaternion Rotation => Quaternion.Euler(rotation);

        public override float Scaling => scaling;

        #endregion
    }

    [Serializable]
    public sealed class ScalingPrefabItem : PrefabItemBase
    {
        #region Inspector Data

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        private Vector3 rotation;

        [SerializeField]
        private float minScaling = 0.7f;

        [SerializeField]
        private float maxScaling = 1f;

        #endregion

        #region Properties

        public override Vector3 Position => position;
        
        public override Quaternion Rotation => Quaternion.Euler(rotation);
        
        public override float Scaling => Random.Range(minScaling, maxScaling);

        #endregion
    }
    
    [Serializable]
    public class ScalingPrefabItem<T> : PrefabItemBase<T> where T : Component
    {
        #region Inspector Data

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        private Vector3 rotation;

        [SerializeField]
        private float minScaling = 0.7f;

        [SerializeField]
        private float maxScaling = 1f;

        #endregion

        #region Properties

        public override Vector3 Position => position;
        
        public override Quaternion Rotation => Quaternion.Euler(rotation);
        
        public override float Scaling => Random.Range(minScaling, maxScaling);

        #endregion
    }
}