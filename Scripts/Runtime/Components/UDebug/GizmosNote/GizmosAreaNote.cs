
using System;
using UnityEditor;
using UnityEngine;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.UDebug.GizmosNote
{
    [DisallowMultipleComponent]
    public sealed class GizmosAreaNote : GizmosNote
    {
        #region Inspector Data

        [Header("Area")]
        [SerializeField]
        [Min(0)]
        private float textOverArea = 125f;

        [Space]
        [SerializeField]
        private bool showArea = true;

        [SerializeField]
        private Color areaColor = new Color(1f, 1f, 1f, 0.25f);

        [SerializeField]
        [Min(0)]
        private float areaSize = 100f;

        [SerializeField]
        private GizmosNoteAreaType areaType = GizmosNoteAreaType.Sphere;

        [SerializeField]
        private bool solidArea = true;

        [Space]
        [SerializeField]
        private AnimationCurve areaFadingCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

        [SerializeField]
        private float areaStartDistance = 10f;

        [SerializeField]
        private float areaEndDistance = 500f;

        [Header("Arrow")]
        [SerializeField]
        private bool showArrow = true;

        [SerializeField]
        private Color arrowColor = new Color(1f, 1f, 0.75f, 0.25f);

        [SerializeField]
        [Min(0.001f)]
        private float arrowWidth = 10f;

        [SerializeField]
        private float startOffset = 2f;

        [SerializeField]
        private float endOffset = 10f;

        [Space]
        [SerializeField]
        private AnimationCurve arrowFadingCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));

        [Tooltip("Maximum (faraway) distance when starting fading")]
        [SerializeField]
        private float arrowStartDistance = 10f;

        [Tooltip("Minimum (nearest) distance when finish fading")]
        [SerializeField]
        private float arrowEndDistance = 100f;

        #endregion

#if UNITY_EDITOR
        protected override void DrawText(string text, Vector3 position, float cameraDistance, GUIStyle guiStyle, Color calculatedTextColor)
        {
            Handles.Label(position + Vector3.up * textOverArea, text, new GUIStyle(guiStyle)
            {
                normal =
                {
                    textColor = calculatedTextColor
                }
            });
        }

        protected override void Draw(Vector3 position, float cameraDistance)
        {
            if (showArea)
            {
                var calculatedAreaColor = CalculateColorByDistance(cameraDistance, areaStartDistance, areaEndDistance, areaFadingCurve, areaColor);
                switch (areaType)
                {
                    case GizmosNoteAreaType.Sphere:
                        Gizmos.color = calculatedAreaColor;
                        if (solidArea)
                        {
                            Gizmos.DrawSphere(position, areaSize);
                        }
                        else
                        {
                            Gizmos.DrawWireSphere(position, areaSize);
                        }

                        break;
                    case GizmosNoteAreaType.Cube:
                        Gizmos.color = calculatedAreaColor;
                        if (solidArea)
                        {
                            Gizmos.DrawCube(position, Vector3.one * areaSize);
                        }
                        else
                        {
                            Gizmos.DrawWireCube(position, Vector3.one * areaSize);
                        }

                        break;
                    case GizmosNoteAreaType.Circle:
                        Handles.color = calculatedAreaColor;
                        if (solidArea)
                        {
                            Handles.DrawSolidDisc(position, Vector3.up, areaSize);
                        }
                        else
                        {
                            Handles.DrawWireDisc(position, Vector3.up, areaSize);
                        }

                        break;
                    default:
                        throw new NotImplementedException(areaType.ToString());
                }
            }

            if (showArrow)
            {
                var calculatedArrowColor = CalculateColorByDistance(cameraDistance, arrowStartDistance, arrowEndDistance, arrowFadingCurve, arrowColor);
                
                var multiplier = cameraDistance * 0.1f;

                var cameraTransform = Camera.current.transform;
                var cameraForward = cameraTransform.forward;
                var cameraUp = cameraTransform.up;
                
                Handles.color = calculatedArrowColor;
                Handles.DrawAAConvexPolygon(
                    position + Vector3.up * (textOverArea - startOffset) + Quaternion.LookRotation(cameraForward, cameraUp) * Vector3.left * (arrowWidth + multiplier),
                    position + Vector3.up * (textOverArea - startOffset) + Quaternion.LookRotation(cameraForward, cameraUp) * Vector3.right * (arrowWidth + multiplier),
                    position + Vector3.up * endOffset
                );
            }
        }
#endif
    }

    public enum GizmosNoteAreaType
    {
        Sphere,
        Cube,
        Circle
    }
}