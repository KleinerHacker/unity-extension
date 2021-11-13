using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    internal sealed class UnityDispatcherController : ObserverSingletonBehavior<UnityDispatcherController>
    {
        #region Static Area

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateDispatcher()
        {
            var gameObject = new GameObject("Dispatcher");
            gameObject.AddComponent<UnityDispatcherController>();
            DontDestroyOnLoad(gameObject);
        }

        #endregion
        
        private readonly IList<Action> _runList = new List<Action>();

        public void RunLater(Action action)
        {
            AddAction(action, _runList);
        }

        #region Builtin Methods

        private void LateUpdate()
        {
            RunActions(_runList);
        }

        #endregion

        private void AddAction(Action action, IList<Action> actions)
        {
            lock (actions)
            {
                actions.Add(action);
            }
        }

        private void RunActions(IList<Action> actions)
        {
            lock (actions)
            {
                foreach (var action in actions)
                {
                    action();
                }
                
                actions.Clear();
            }
        }
    }
}