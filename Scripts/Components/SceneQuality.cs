using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    [AddComponentMenu(UnityExtensionConstants.Root + "/Scene Quality")]
    [DisallowMultipleComponent]
    public sealed class SceneQuality : MonoBehaviour
    {
        #region Inspector Data

        [SerializeField]
        private bool autoDetectQualityChange;

        [SerializeField]
        private QualityItem[] qualityItems;

        #endregion

        private int _lastQualityLevel;

        #region Builtin Methods

        private void Awake()
        {
            UpdateQualitySettings();
            _lastQualityLevel = QualitySettings.GetQualityLevel();
        }

        private void FixedUpdate()
        {
            if (autoDetectQualityChange && _lastQualityLevel != QualitySettings.GetQualityLevel())
            {
                Debug.Log("Detect quality level change from " + _lastQualityLevel + " to " + QualitySettings.GetQualityLevel());
                
                UpdateQualitySettings();
                _lastQualityLevel = QualitySettings.GetQualityLevel();
            }
        }

        #endregion

        private void UpdateQualitySettings()
        {
            foreach (var qualityItem in qualityItems)
            {
                var qualityLevel = QualitySettings.GetQualityLevel();
                var qualityMatch = qualityLevel >= qualityItem.MinQualityLevel && qualityLevel <= qualityItem.MaxQualityLevel;

                Debug.Log("Setup Scene Quality for quality level " + qualityLevel + ", match " + qualityMatch);
                UpdateExcludedObjects(qualityItem, qualityMatch);
                UpdateIncludedObjects(qualityItem, qualityMatch);
                UpdateEvents(qualityItem, qualityMatch);
            }
        }

        private static void UpdateEvents(QualityItem qualityItem, bool qualityMatch)
        {
            if (qualityMatch)
            {
                qualityItem.OnActivate.Invoke();
            }
            else
            {
                qualityItem.OnDeactivate.Invoke();
            }
        }

        private static void UpdateIncludedObjects(QualityItem qualityItem, bool qualityMatch)
        {
            foreach (var includeObject in qualityItem.IncludeObjects)
            {
                switch (includeObject.Behavior)
                {
                    case GameObjectInclusionBehavior.Show:
                        if (qualityMatch)
                        {
                            includeObject.GameObject.SetActive(true);
                        }
                        else
                        {
                            includeObject.GameObject.SetActive(false);
                        }

                        break;
                    case GameObjectInclusionBehavior.Instantiate:
                        if (qualityMatch)
                        {
                            Instantiate(includeObject.GameObject, includeObject.Translation, includeObject.Rotation);
                        }
                        else
                        {
                            Debug.LogError("Not supported: Dynamic Quality Change");
                        }

                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void UpdateExcludedObjects(QualityItem qualityItem, bool qualityMatch)
        {
            foreach (var excludeObject in qualityItem.ExcludeObjects)
            {
                switch (excludeObject.Behavior)
                {
                    case GameObjectExclusionBehavior.Hide:
                        if (qualityMatch)
                        {
                            excludeObject.GameObject.SetActive(false);
                        }
                        else
                        {
                            excludeObject.GameObject.SetActive(true);
                        }

                        break;
                    case GameObjectExclusionBehavior.Remove:
                        if (qualityMatch)
                        {
                            Destroy(excludeObject.GameObject);
                        }
                        else
                        {
                            Debug.LogError("Not supported: Dynamic Quality Change");
                        }

                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }

    [Serializable]
    public sealed class QualityItem
    {
        #region Inspector Data

        [SerializeField]
        private int minQualityLevel;
        
        [SerializeField]
        private int maxQualityLevel;

        [Space]
        [SerializeField]
        private GameObjectExclusionItem[] excludeObjects;

        [SerializeField]
        private GameObjectInclusionItem[] includeObjects;

        [Space]
        [SerializeField]
        private UnityEvent onActivate;
        
        [SerializeField]
        private UnityEvent onDeactivate;

        #endregion

        #region Properties

        public int MinQualityLevel => minQualityLevel;

        public int MaxQualityLevel => maxQualityLevel;

        public GameObjectExclusionItem[] ExcludeObjects => excludeObjects;

        public GameObjectInclusionItem[] IncludeObjects => includeObjects;

        public UnityEvent OnActivate => onActivate;

        public UnityEvent OnDeactivate => onDeactivate;

        #endregion
    }

    [Serializable]
    public sealed class GameObjectExclusionItem
    {
        #region Inspector Data

        [SerializeField]
        private GameObject gameObject;

        [SerializeField]
        private GameObjectExclusionBehavior behavior = GameObjectExclusionBehavior.Hide;

        #endregion

        #region Properties

        public GameObject GameObject => gameObject;

        public GameObjectExclusionBehavior Behavior => behavior;

        #endregion
    }

    [Serializable]
    public sealed class GameObjectInclusionItem
    {
        #region Inspector Data

        [SerializeField]
        private GameObject gameObject;

        [SerializeField]
        private GameObjectInclusionBehavior behavior = GameObjectInclusionBehavior.Show;

        [SerializeField]
        private Vector3 translation = Vector3.zero;
        
        [SerializeField]
        private Quaternion rotation = Quaternion.identity;

        #endregion

        #region Properties

        public GameObject GameObject => gameObject;

        public GameObjectInclusionBehavior Behavior => behavior;

        public Vector3 Translation => translation;

        public Quaternion Rotation => rotation;

        #endregion
    }

    public enum GameObjectExclusionBehavior
    {
        Hide,
        Remove
    }

    public enum GameObjectInclusionBehavior
    {
        Show,
        Instantiate
    }
}