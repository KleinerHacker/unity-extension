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
                    Debug.Log("[ENV] Check: " + x.Name);
                    var inputsAvailable = x.Inputs.All(y =>
                    {
                        var type = Type.GetType(y);
                        if (type == null)
                            throw new InvalidOperationException("Type " + y + " is unknown");

                        var propertyInfo = type.GetProperty("current", BindingFlags.Public | BindingFlags.Static);
                        if (propertyInfo == null)
                            throw new InvalidOperationException("Property (static) 'current' is unknown");

                        var value = (InputDevice)propertyInfo.GetValue(null);
                        var available = value != null && value.deviceId != InputDevice.InvalidDeviceId && value.enabled;
                        
                        Debug.Log("[ENV] Input: " + x.Name + ", available: " + available);
                        return available;
                    });
                    if (!inputsAvailable)
                        return false;

                    var runtimeSystemAvailable = x.RuntimeSystemItems.Any(y =>
                    {
                        if (Application.platform != y.Platform)
                        {
                            Debug.Log("[ENV] Platform: " + x.Name + ", wrong: " + y.Platform);
                            return false;
                        }

#if PLATFORM_ANDROID
                    if (!AndroidUtils.IsOnTV) {
                        Debug.Log("[ENV] On TV: " + x.Name + ", wrong");
                        return false;
                    }
#endif

                        Debug.Log("[ENV] Platform: " + x.Name + ", success");
                        return true;
                    });
                    if (!runtimeSystemAvailable)
                    {
                        Debug.Log("[ENV] Check: " + x.Name + ", failed");
                        return false;
                    }

                    Debug.Log("[ENV] Check: " + x.Name + ", success");
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
                
                Debug.Log("[ENV] Found guid: " + guid);
            }

            return _guid;
        }
    }
}