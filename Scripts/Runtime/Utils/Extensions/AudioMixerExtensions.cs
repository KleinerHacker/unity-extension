using UnityBase.Runtime.Projects.unity_base.Scripts.Runtime.Utils;
using UnityEngine.Audio;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Utils.Extensions
{
    public static class AudioMixerExtensions
    {
        public static void SetVolume(this AudioMixer audioMixer, float value, float minValue, float maxValue = 0f, string property = "volume") => 
            audioMixer.SetFloat(property, MathfEx.Remap(value, 0f, 1f, minValue, maxValue));
    }
}