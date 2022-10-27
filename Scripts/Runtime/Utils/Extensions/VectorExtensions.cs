using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 MultiplyXY(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(
                v1.x * v2.x,
                v1.y * v2.y
            );
        }
        
        public static Vector3 MultiplyXYZ(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.x * v2.x,
                v1.y * v2.y,
                v1.z * v2.z
            );
        }
        
        public static Vector4 MultiplyXYZW(this Vector4 v1, Vector4 v2)
        {
            return new Vector4(
                v1.x * v2.x,
                v1.y * v2.y,
                v1.z * v2.z,
                v1.w * v2.w
            );
        }
        
        public static Vector2 DivideXY(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(
                v1.x / v2.x,
                v1.y / v2.y
            );
        }
        
        public static Vector3 DivideXYZ(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.x / v2.x,
                v1.y / v2.y,
                v1.z / v2.z
            );
        }
        
        public static Vector4 DivideXYZW(this Vector4 v1, Vector4 v2)
        {
            return new Vector4(
                v1.x / v2.x,
                v1.y / v2.y,
                v1.z / v2.z,
                v1.w / v2.w
            );
        }
    }
}