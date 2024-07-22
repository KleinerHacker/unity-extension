using System;
using UnityEngine;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Utils.Extensions
{
    public static class CameraExtensions
    {
        public static Bounds GetOrthographicBounds(this Camera camera)
        {
            if (!camera.orthographic)
                throw new InvalidOperationException("Camera is not orthographic: " + camera.gameObject.name);
            
            var screenAspect = (float)Screen.width / (float)Screen.height;
            var cameraHeight = camera.orthographicSize * 2;
            var bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            
            return bounds;
        }
    }
}