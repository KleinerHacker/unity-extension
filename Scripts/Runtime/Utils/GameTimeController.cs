using System;
using UnityAnimation.Runtime.Projects.unity_animation.Scripts.Runtime.Types;
using UnityAnimation.Runtime.Projects.unity_animation.Scripts.Runtime.Utils;
using UnityEngine;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Utils
{
    public static class GameTimeController
    {
        #region Properties

        public static bool IsPlaying { get; private set; } = true;
        public static bool IsPaused => !IsPlaying;

        public static float Scaling { get; private set; } = Time.timeScale;

        #endregion

        public static void ChangeScaling(float scaling)
        {
            Scaling = scaling;

            if (!IsPlaying)
                return;

            Time.timeScale = scaling;
        }

        public static void ChangeScaling(float scaling, MonoBehaviour mb, float speed = 1f, Action finished = null)
        {
            ChangeScaling(scaling, mb, AnimationCurve.Linear(0f, 0f, 1f, 1f), speed, finished);
        }

        public static void ChangeScaling(float scaling, MonoBehaviour mb, AnimationCurve curve, float speed = 1f, Action finished = null)
        {
            if (!IsPlaying)
            {
                Scaling = scaling;
                return;
            }

            AnimationBuilder.Create(mb)
                .Animate(curve, speed, v => Time.timeScale = Mathf.Lerp(Scaling, scaling, v))
                .WithFinisher(() =>
                {
                    Scaling = scaling;
                    finished?.Invoke();
                })
                .Start();
        }

        public static void Pause()
        {
            if (IsPaused)
            {
                Debug.LogWarning("Game time already paused");
                return;
            }

            Time.timeScale = 0f;
            IsPlaying = false;
        }

        public static void Pause(MonoBehaviour mb, float speed = 1f, Action finished = null)
        {
            Pause(mb, AnimationCurve.Linear(0f, 0f, 1f, 1f), speed, finished);
        }

        public static void Pause(MonoBehaviour mb, AnimationCurve curve, float speed = 1f, Action finished = null)
        {
            if (IsPaused)
            {
                Debug.LogWarning("Game time already paused");
                return;
            }

            IsPlaying = false;
            AnimationBuilder.Create(mb, AnimationType.Unscaled)
                .Animate(curve, speed, v => Time.timeScale = Scaling * (1f - v), finished)
                .Start();
        }

        public static void Resume()
        {
            if (IsPlaying)
            {
                Debug.LogWarning("Game time already playing");
                return;
            }

            Time.timeScale = Scaling;
            IsPlaying = true;
        }

        public static void Resume(MonoBehaviour mb, float speed = 1f, Action finished = null)
        {
            Resume(mb, AnimationCurve.Linear(0f, 0f, 1f, 1f), speed, finished);
        }

        public static void Resume(MonoBehaviour mb, AnimationCurve curve, float speed = 1f, Action finished = null)
        {
            if (IsPlaying)
            {
                Debug.LogWarning("Game time already playing");
                return;
            }

            IsPlaying = true;
            AnimationBuilder.Create(mb, AnimationType.Unscaled)
                .Animate(curve, speed, v => Time.timeScale = Scaling * v, finished)
                .Start();
        }
    }
}