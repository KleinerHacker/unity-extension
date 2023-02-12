using System;
using System.Collections.Generic;
using UnityBase.Runtime.@base.Scripts.Runtime.Components.Singleton;
using UnityBase.Runtime.@base.Scripts.Runtime.Components.Singleton.Attributes;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components
{
    [Singleton(Scope = SingletonScope.Application, Instance = SingletonInstance.RequiresNewInstance, CreationTime = SingletonCreationTime.Loading, ObjectName = "Dispatcher")]
    internal sealed class UnityDispatcherController : SingletonBehavior<UnityDispatcherController>
    {
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