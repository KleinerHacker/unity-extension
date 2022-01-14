using System;
using System.Linq;
using System.Reflection;
#if PLATFORM_ANDROID
using UnityAndroidEx.Runtime.android_ex.Scripts.Runtime.Utils;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
    public static class RuntimeEnvironment
    {
        private static Guid? _guid;
        private static bool _initialized;
        
        public static Guid? GetDetectedEnvironment()
        {
            if (!_initialized)
            {
                var guid = EnvironmentDetectionSettings.Singleton.Items.FirstOrDefault(x =>
                {
                    var inputsAvailable = x.Inputs.All(y =>
                    {
                        var type = Type.GetType(y);
                        if (type == null)
                            throw new InvalidOperationException("Type " + y + " is unknown");

                        var propertyInfo = type.GetProperty("current", BindingFlags.Public | BindingFlags.Static);
                        if (propertyInfo == null)
                            throw new InvalidOperationException("Property (static) 'current' is unknown");

                        var value = (InputDevice)propertyInfo.GetValue(null);
                        return value != null && value.deviceId != InputDevice.InvalidDeviceId && value.enabled;
                    });
                    if (!inputsAvailable)
                        return false;

                    var runtimeSystemAvailable = x.RuntimeSystemItems.Any(y =>
                    {
                        if (Application.platform != y.Platform)
                            return false;

#if PLATFORM_ANDROID
                    if (!AndroidUtils.IsOnTV)
                        return false;
#endif

                        return true;
                    });
                    if (!runtimeSystemAvailable)
                        return false;

                    return true;
                })?.Guid;

                if (string.IsNullOrWhiteSpace(guid))
                {
                    _guid = null;
                }
                else
                {
                    _guid = Guid.Parse(guid);
                }

                _initialized = true;
            }

            return _guid;
        }
    }
}