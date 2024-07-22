using UnityBase.Runtime.Projects.unity_base.Scripts.Runtime.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Components.UDebug.GizmosNote
{
    public abstract class GizmosNote : MonoBehaviour
    {
        #region Inspector Data

        [SerializeField]
        protected string category = "General";

        [Tooltip("Used for game object selection zoom in")]
        [SerializeField]
        protected float noteSize = 100f;

        [Header("Text")]
        
        [SerializeField]
        protected string text = "Note";

        [FormerlySerializedAs("color")]
        [Space]
        [SerializeField]
        protected Color textColor = Color.white;

        [FormerlySerializedAs("size")]
        [SerializeField]
        [Min(1)]
        protected int textSize = 28;

        [FormerlySerializedAs("style")]
        [SerializeField]
        protected FontStyle textStyle = FontStyle.Bold;

        [Space]
        [SerializeField]
        protected AnimationCurve textFadingCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

        [SerializeField]
        protected float textStartDistance = 10f;

        [SerializeField]
        protected float textEndDistance = 10000f;

        #endregion

        #region Properties

#if UNITY_EDITOR
        public string Category => category;

        public string Text => text;
#endif

        #endregion

        private GUIStyle _guiStyle;

        #region Builtin Methods

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            var position = transform.position;
            var cameraDistance = (position - Camera.current.transform.position).magnitude;

            var calculatedTextColor = CalculateColorByDistance(cameraDistance, textStartDistance, textEndDistance, textFadingCurve, textColor);
            
            DrawText(text, position, cameraDistance, _guiStyle, calculatedTextColor);
            Draw(position, cameraDistance);
        }

        protected virtual void OnValidate()
        {
            _guiStyle = new GUIStyle
            {
                normal =
                {
                    textColor = textColor
                },
                fontSize = textSize,
                fontStyle = textStyle,
                alignment = TextAnchor.MiddleCenter
            };
        }
#endif

        #endregion

#if UNITY_EDITOR
        protected abstract void DrawText(string text, Vector3 position, float cameraDistance, GUIStyle guiStyle, Color calculatedTextColor);
        
        protected virtual void Draw(Vector3 position, float cameraDistance)
        {
            //Empty
        }

        /// <summary>
        /// Calculates the color for fading via distance to camera
        /// </summary>
        /// <param name="cameraDistance"></param>
        /// <param name="startDistance"></param>
        /// <param name="endDistance"></param>
        /// <param name="fadingCurve"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        protected Color CalculateColorByDistance(float cameraDistance, float startDistance, float endDistance, AnimationCurve fadingCurve, Color color)
        {
            var textColorValue = MathfEx.Remap01(cameraDistance, startDistance, endDistance);
            return Color.Lerp(Color.clear, color, fadingCurve.Evaluate(textColorValue));
        }
#endif
    }
}