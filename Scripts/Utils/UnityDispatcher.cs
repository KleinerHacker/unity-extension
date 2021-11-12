using System;
using UnityExtension.Runtime.extension.Scripts.Components;

namespace UnityExtension.Runtime.extension.Scripts.Utils
{
    public static class UnityDispatcher
    {
        public static void RunLater(Action action)
        {
            var controller = UnityDispatcherController.Singleton;
            if (controller == null)
                throw new InvalidOperationException("No dispatcher found in scene!");
            
            controller.RunLater(action);
        }
    }
}