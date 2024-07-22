using System;
using UnityEditor;
using UnityEngine;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Components.UDebug.GizmosNote
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public sealed class GizmosObjectNote : GizmosNote
    {
        #region Inspector Data

        [Space]
        [SerializeField]
        private GizmosObjectNotePlace textPlace = GizmosObjectNotePlace.CenterBounds;
        
        [SerializeField]
        private Vector3 textOffset = Vector3.zero;
        
        #endregion

#if UNITY_EDITOR
        protected override void DrawText(string text, Vector3 position, float cameraDistance, GUIStyle guiStyle, Color calculatedTextColor)
        {
            var bounds = GetComponent<Renderer>().bounds;

            if (textPlace.HasFlag(GizmosObjectNotePlace.MinimumBounds))
            {
                DrawText(bounds.min + textOffset, guiStyle);
            }
            
            if (textPlace.HasFlag(GizmosObjectNotePlace.MaximumBounds))
            {
                DrawText(bounds.max - textOffset, guiStyle);
            }
            
            if (textPlace.HasFlag(GizmosObjectNotePlace.CenterBounds))
            {
                DrawText(bounds.center, guiStyle);
            }
        }

        private void DrawText(Vector3 targetPos, GUIStyle guiStyle)
        {
            var distance = (targetPos - Camera.current.transform.position).magnitude;
                
            var color = CalculateColorByDistance(distance, textStartDistance, textEndDistance, textFadingCurve, textColor);
            Handles.Label(targetPos, text, new GUIStyle(guiStyle)
            {
                normal =
                {
                    textColor = color
                }
            });
        }
#endif
    }

    [Flags]
    public enum GizmosObjectNotePlace
    {
        MinimumBounds = 0x01,
        MaximumBounds = 0x02,
        CenterBounds = 0x04
    }
}