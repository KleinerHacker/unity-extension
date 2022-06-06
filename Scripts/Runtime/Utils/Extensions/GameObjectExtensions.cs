using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions
{
    public static class GameObjectExtensions
    {
        public static IEnumerable<GameObject> InstantiateAll(this IEnumerable<GameObject> list)
        {
            return list.Select(GameObject.Instantiate).ToArray();
        }

        public static IEnumerable<GameObject> InstantiateAll(this IEnumerable<GameObject> list, Vector3 pos, Quaternion rot, Transform transform = null)
        {
            return list.Select(x => GameObject.Instantiate(x, pos, rot, transform)).ToArray();
        }

        public static void DestroyAll(this IEnumerable<GameObject> list)
        {
            foreach (var o in list)
            {
                GameObject.Destroy(o);
            }
        }

        public static void SetActive(this IEnumerable<GameObject> list, bool active)
        {
            foreach (var go in list)
            {
                go.SetActive(active);
            }
        }
        
        public static T[] FindComponents<T>(this GameObject gameObject, TreeSearchDirection direction = TreeSearchDirection.All)
        {
            var list = new List<T>();

            if (direction.HasFlag(TreeSearchDirection.Children))
                list.AddRange(gameObject.GetComponentsInChildren<T>());
            if (direction.HasFlag(TreeSearchDirection.Current))
                list.AddRange(gameObject.GetComponents<T>());
            if (direction.HasFlag(TreeSearchDirection.Parent))
                list.AddRange(gameObject.GetComponentsInParent<T>());

            return list.Distinct().ToArray();
        }

        public static T FindComponent<T>(this GameObject gameObject, TreeSearchDirection direction = TreeSearchDirection.All)
        {
            if (direction.HasFlag(TreeSearchDirection.Children))
            {
                var childComponent = gameObject.GetComponentInChildren<T>();
                if (childComponent != null)
                    return childComponent;
            }

            if (direction.HasFlag(TreeSearchDirection.Current))
            {
                var myComponent = gameObject.GetComponent<T>();
                if (myComponent != null)
                    return myComponent;
            }

            if (direction.HasFlag(TreeSearchDirection.Parent))
            {
                var parentComponent = gameObject.GetComponentInParent<T>();
                if (parentComponent != null)
                    return parentComponent;
            }

            return default;
        }

#if UNITY_EDITOR
        public static void DestroyInEditor(this MonoBehaviour behaviour, GameObject gameObject)
        {
            behaviour.StartCoroutine(Destroy(gameObject));
        }

        private static IEnumerator Destroy(GameObject go)
        {
            yield return new WaitForEndOfFrame();
            GameObject.DestroyImmediate(go);
        }
#endif
    }
}