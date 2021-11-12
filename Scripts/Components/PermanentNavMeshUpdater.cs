using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Components
{
    [AddComponentMenu(UnityExtensionConstants.Root + "/Permanent NavMesh Updater")]
    [DisallowMultipleComponent]
    public class PermanentNavMeshUpdater : NavMeshUpdater
    {
        #region Inspector Data

        [Header("Behavior")]
        [SerializeField]
        private float permanentlyUpdateTime = 1f;

        [SerializeField]
        private Bounds bounds;

        #endregion

        private float _timeCounter = 0f;

        #region Builtin Methods

        protected virtual void Update()
        {
            _timeCounter += Time.deltaTime;
            if (_timeCounter >= permanentlyUpdateTime)
            {
                UpdateNavMesh(bounds);
            }
        }

        #endregion
    }
}