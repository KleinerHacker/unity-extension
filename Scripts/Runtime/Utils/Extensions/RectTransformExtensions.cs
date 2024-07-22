using UnityEngine;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Utils.Extensions
{
    public static class RectTransformExtensions
    {
        public static Rect CalculateAbsoluteRect(this RectTransform transform, Canvas canvas)
        {
            var position = transform.position;
            var x = position.x;
            var y = position.y;
            
            var rect = transform.rect;
            var width = rect.width;
            var height = rect.height;

            return new Rect(x, y, width, height);
        }
    }
}