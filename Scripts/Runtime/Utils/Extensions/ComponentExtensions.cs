using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions
{
    public static class ComponentExtensions
    {
        public static T[] FindComponents<T>(this Component component, TreeSearchDirection direction = TreeSearchDirection.All) => 
            component.gameObject.FindComponents<T>(direction);

        public static T FindComponent<T>(this Component component, TreeSearchDirection direction = TreeSearchDirection.All) => 
            component.gameObject.FindComponent<T>(direction);
    }
}