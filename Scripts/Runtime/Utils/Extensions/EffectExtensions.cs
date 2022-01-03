using System;
using System.Linq;
using UnityAnimation.Runtime.animation.Scripts.Runtime.Utils;
using UnityEngine;
using UnityEngine.VFX;
using UnityExtension.Runtime.extension.Scripts.Runtime.Types;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions
{
    public static class EffectExtensions
    {
        public static void PlayEffect(this SimpleEffectItem item, MonoBehaviour behaviour, Action onFinished = null) => 
            PlayEffect(item, behaviour, Vector3.zero, onFinished);

        public static void PlayEffect(this SimpleEffectItem item, MonoBehaviour behaviour, Vector3 position, Action onFinished = null) => 
            PlayEffect(item, behaviour, position, Quaternion.identity, onFinished);

        public static void PlayEffect(this SimpleEffectItem item, MonoBehaviour behaviour, Vector3 position, Quaternion rotation, Action onFinished = null)
        {
            AnimationBuilder.Create(behaviour)
                .Wait(item.PreDelay, data => data.Set("go", Instantiate(item, position, rotation)))
                .Wait(item.PostDelay, data => GameObject.Destroy(data.Get<GameObject>("go")))
                .WithFinisher(onFinished)
                .Start();
        }
        
        public static PlayedEffect PlayEffect(this LoopEffectItem item) => 
            PlayEffect(item, Vector3.zero);

        public static PlayedEffect PlayEffect(this LoopEffectItem item, Vector3 position) => 
            PlayEffect(item, position, Quaternion.identity);

        public static PlayedEffect PlayEffect(this LoopEffectItem item, Vector3 position, Quaternion rotation)
        {
            var go = Instantiate(item, position, rotation);
            return new PlayedEffect(go, item.PostDelay);
        }
        
        public static void PlayEffect(this ActionEffectItem item, MonoBehaviour behaviour, Action action = null, Action onFinished = null) =>
            PlayEffect(item, behaviour, Vector3.zero, action, onFinished);

        public static void PlayEffect(this ActionEffectItem item, MonoBehaviour behaviour, Vector3 position, Action action = null, Action onFinished = null) => 
            PlayEffect(item, behaviour, position, Quaternion.identity, action, onFinished);

        public static void PlayEffect(this ActionEffectItem item, MonoBehaviour behaviour, Vector3 position, Quaternion rotation, Action action = null, Action onFinished = null)
        {
            AnimationBuilder.Create(behaviour)
                .Wait(item.PreDelay, data => data.Set("go", Instantiate(item, position, rotation)))
                .Wait(item.PostDelay, data => GameObject.Destroy(data.Get<GameObject>("go")))
                .WithFinisher(onFinished)
                .Start();

            if (action != null)
            {
                AnimationBuilder.Create(behaviour)
                    .Wait(item.ActionDelay, action)
                    .Start();
            }
        }

        private static GameObject Instantiate(EffectItemBase item, Vector3 position, Quaternion rotation) => 
            GameObject.Instantiate(item.EffectPrefab, position + item.Position, rotation * item.Rotation);
    }

    public sealed class PlayedEffect
    {
        private GameObject _gameObject;
        private float _delay;
        
        public bool IsDestroyed { get; private set; }

        internal PlayedEffect(GameObject gameObject, float delay)
        {
            _gameObject = gameObject;
            _delay = delay;
        }

        public void DestroyAsync(MonoBehaviour behaviour)
        {
            if (IsDestroyed)
                return;

            IsDestroyed = true;
            
            var particleSystems = _gameObject.GetComponentsInChildren<ParticleSystem>().Append(_gameObject.GetComponent<ParticleSystem>()).ToArray();
            foreach (var particleSystem in particleSystems.Where(x => x != null))
            {
                particleSystem.Stop(true);
            }

            var visualEffects = _gameObject.GetComponentsInChildren<VisualEffect>().Append(_gameObject.GetComponent<VisualEffect>()).ToArray();
            foreach (var visualEffect in visualEffects.Where(x => x != null))
            {
                visualEffect.Stop();
            }

            AnimationBuilder.Create(behaviour)
                .Wait(_delay, () => GameObject.Destroy(_gameObject))
                .Start();
        }
    }
}