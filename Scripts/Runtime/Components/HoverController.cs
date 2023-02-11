using System.Linq;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
#if PCSOFT_RAYCASTER && PCSOFT_HOVER
    /// <summary>
    /// Base class to handle hover controller. Is created automatically.
    /// </summary>
    public abstract class HoverController : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// Return name for raycaster ro use. See <see cref="Raycaster"/>
        /// </summary>
        protected abstract string RaycasterName { get; }

        #endregion

        private RaycastHit? _currentHit = null;

        #region Builtin Methods

        protected virtual void OnEnable()
        {
            Raycaster.AddRaycast3DChanged(RaycasterName, OnRaycast);
        }

        protected virtual void OnDisable()
        {
            Raycaster.RemoveRaycast3DChanged(RaycasterName, OnRaycast);
        }

        #endregion

        private void OnRaycast(object sender, RaycasterEventArgs<RaycastHit> e)
        {
            if (e.Hits is not { Length: > 0 } && _currentHit != null)
            {
#if PCSOFT_HOVER_LOGGING
                Debug.Log("Last game objects exit: " + _currentHit.Value.collider.gameObject.name, _currentHit.Value.collider);
#endif
                OnExit(_currentHit.Value.collider.gameObject);
                _currentHit = null;
            }
            else if (e.Hits is { Length: > 0 })
            {
                if (_currentHit != null)
                {
#if PCSOFT_HOVER_LOGGING
                    Debug.Log("Last game object exit: " + _currentHit.Value.collider.gameObject.name, _currentHit.Value.collider);
#endif
                    OnExit(_currentHit.Value.collider.gameObject);
                    _currentHit = null;
                }

                if (OnFilter(e.Hits[0]))
                {
#if PCSOFT_HOVER_LOGGING
                    Debug.Log("New game object(s) entered: " + string.Join(',', e.Hits.Select(x => x.collider.gameObject.name)));
#endif
                    OnEnter(e.Hits[0].collider.gameObject);
                    _currentHit = e.Hits[0];
                }
            }
        }

        /// <summary>
        /// Is called if hit is not <c>null</c> and <see cref="OnFilter"/> returns true.
        /// </summary>
        /// <param name="gameObject">Game object of collision</param>
        protected abstract void OnEnter(GameObject gameObject);

        /// <summary>
        /// Is called if hit is <c>null</c> or changed to another game objects.
        /// </summary>
        /// <param name="gameObject">Game object that is leaved now</param>
        protected abstract void OnExit(GameObject gameObject);

        /// <summary>
        /// Override this method to filter raycast hits before <see cref="OnEnter"/> is called.
        /// </summary>
        /// <param name="hit">Hit object of raycast (only if raycast hits any object)</param>
        /// <returns><c>true</c> to call <see cref="OnEnter"/>, <c>false</c> to ignore hit</returns>
        protected virtual bool OnFilter(RaycastHit hit) => true;
    }
#endif
}