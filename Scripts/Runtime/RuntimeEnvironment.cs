#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#if PCSOFT_ENV_STEAM && STEAMWORKS_NET && !DISABLESTEAMWORKS
using Steamworks;
#endif
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;
#if PLATFORM_ANDROID
using UnityAndroidEx.Runtime.android_ex.Scripts.Runtime.Utils;
#endif

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
#if PCSOFT_ENV
    public static class RuntimeEnvironment
    {
        public static Environment CurrentEnvironment { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
            Debug.Log("[ENV] Initialize...");
#if PCSOFT_ENV_LOGGING
            Debug.Log("[ENV] Detect environment target...");
#endif
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    CurrentEnvironment = RunWindowsDetection();
                    break;
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    CurrentEnvironment = RunLinuxDetection();
                    break;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    CurrentEnvironment = RunMacDetection();
                    break;
#if PLATFORM_ANDROID
                case RuntimePlatform.Android:
                    CurrentEnvironment = RunAndroidDetection();
                    break;
#endif
                case RuntimePlatform.IPhonePlayer:
                    CurrentEnvironment = RunIOSDetection();
                    break;
                default:
                    Debug.LogWarning("[ENV] Unknown target for environment detection");
                    break;
            }
        }

        private static Environment RunWindowsDetection()
        {
#if PCSOFT_ENV_LOGGING
            Debug.Log("[ENV] > Environment Platform: Windows");
#endif
            var settings = EnvironmentDetectionSettings.Singleton;
            foreach (var item in settings.Windows)
            {
                var inputCheck = RunSimpleInputCheck(item);
                if (inputCheck)
                {
#if PCSOFT_ENV_LOGGING
                    Debug.Log("[ENV] > Find fit environment target: " + item.Name);
#endif
                    var groupName = settings.Groups
                        .Where(x => x.Items.Any(y => y.Platform == EnvironmentSupportedPlatform.Windows && y.TargetName == item.Name))
                        .Select(x => x.Name)
                        .FirstOrDefault();
#if PCSOFT_ENV_LOGGING
                    Debug.Log("[ENV] > Find fit environment group: " + groupName);
#endif

                    return new Environment(Application.platform, item.Name, groupName);
                }
            }

            Debug.LogWarning("[ENV] Unable to find any fitting environment target");
            return new Environment(Application.platform, null, null);
        }

        private static Environment RunLinuxDetection()
        {
#if PCSOFT_ENV_LOGGING
            Debug.Log("[ENV] > Environment Platform: Linux");
#endif
            var settings = EnvironmentDetectionSettings.Singleton;
            foreach (var item in settings.Linux)
            {
                var inputCheck = RunSimpleInputCheck(item);
                var steamCheck =
#if PCSOFT_ENV_STEAM && STEAMWORKS_NET && !DISABLESTEAMWORKS
                    RunSteamCheck(item);
#else
                    true;
#endif

                if (inputCheck && steamCheck)
                {
#if PCSOFT_ENV_LOGGING
                    Debug.Log("[ENV] > Find fit environment target: " + item.Name);
#endif
                    var groupName = settings.Groups
                        .Where(x => x.Items.Any(y => y.Platform == EnvironmentSupportedPlatform.Linux && y.TargetName == item.Name))
                        .Select(x => x.Name)
                        .FirstOrDefault();
#if PCSOFT_ENV_LOGGING
                    Debug.Log("[ENV] > Find fit environment groups: " + groupName);
#endif

                    return new Environment(Application.platform, item.Name, groupName);
                }
            }

            Debug.LogWarning("[ENV] Unable to find any fitting environment target");
            return new Environment(Application.platform, null, null);
        }

        private static Environment RunMacDetection()
        {
#if PCSOFT_ENV_LOGGING
            Debug.Log("[ENV] > Environment Platform: Mac");
#endif
            var settings = EnvironmentDetectionSettings.Singleton;
            foreach (var item in settings.Mac)
            {
                var inputCheck = RunSimpleInputCheck(item);
                if (inputCheck)
                {
#if PCSOFT_ENV_LOGGING
                    Debug.Log("[ENV] > Find fit environment target: " + item.Name);
#endif
                    var groupName = settings.Groups
                        .Where(x => x.Items.Any(y => y.Platform == EnvironmentSupportedPlatform.Mac && y.TargetName == item.Name))
                        .Select(x => x.Name)
                        .FirstOrDefault();
#if PCSOFT_ENV_LOGGING
                    Debug.Log("[ENV] > Find fit environment groups: " + groupName);
#endif

                    return new Environment(Application.platform, item.Name, groupName);
                }
            }

            Debug.LogWarning("[ENV] Unable to find any fitting environment target");
            return new Environment(Application.platform, null, null);
        }

#if PLATFORM_ANDROID
        private static Environment RunAndroidDetection()
        {
#if PCSOFT_ENV_LOGGING
            Debug.Log("[ENV] > Environment Platform: Android");
#endif
            var settings = EnvironmentDetectionSettings.Singleton;
            foreach (var item in settings.Android)
            {
                var inputCheck = RunSimpleInputCheck(item);
                var tvCheck = !item.RequiresTv || AndroidUtils.IsOnTV;

                if (inputCheck && tvCheck)
                {
#if PCSOFT_ENV_LOGGING
                    Debug.Log("[ENV] > Find fit environment target: " + item.Name);
#endif
                    var groupName = settings.Groups
                        .Where(x => x.Items.Any(y => y.Platform == EnvironmentSupportedPlatform.Android && y.TargetName == item.Name))
                        .Select(x => x.Name)
                        .FirstOrDefault();
#if PCSOFT_ENV_LOGGING
                    Debug.Log("[ENV] > Find fit environment groups: " + groupName);
#endif

                    return new Environment(Application.platform, item.Name, groupName);
                }
            }

            Debug.LogWarning("[ENV] Unable to find any fitting environment target");
            return new Environment(Application.platform, null, null);
        }
#endif

        private static Environment RunIOSDetection()
        {
#if PCSOFT_ENV_LOGGING
            Debug.Log("[ENV] > Environment Platform: IOS");
#endif
            var settings = EnvironmentDetectionSettings.Singleton;
            foreach (var item in settings.IOS)
            {
                var inputCheck = RunSimpleInputCheck(item);
                if (inputCheck)
                {
#if PCSOFT_ENV_LOGGING
                    Debug.Log("[ENV] > Find fit environment target: " + item.Name);
#endif
                    var groupName = settings.Groups
                        .Where(x => x.Items.Any(y => y.Platform == EnvironmentSupportedPlatform.IOS && y.TargetName == item.Name))
                        .Select(x => x.Name)
                        .FirstOrDefault();
#if PCSOFT_ENV_LOGGING
                    Debug.Log("[ENV] > Find fit environment groups: " + groupName);
#endif

                    return new Environment(Application.platform, item.Name, groupName);
                }
            }

            Debug.LogWarning("[ENV] Unable to find any fitting environment target");
            return new Environment(Application.platform, null, null);
        }

#if PCSOFT_ENV_STEAM && STEAMWORKS_NET && !DISABLESTEAMWORKS
        private static bool RunSteamCheck(LinuxEnvironmentTarget item)
        {
            var steamDeckCheck = !item.RequiresSteamDeck || SteamUtils.IsSteamRunningOnSteamDeck();

            return steamDeckCheck;
        }
#endif

        private static bool RunSimpleInputCheck(EnvironmentTarget item)
        {
            var keyboardCheck = !item.RequiresKeyboard || (Keyboard.current != null && Keyboard.current.deviceId != InputDevice.InvalidDeviceId);
            var mouseCheck = !item.RequiresMouse || (Mouse.current != null && Mouse.current.deviceId != InputDevice.InvalidDeviceId);
            var touchCheck = !item.RequiresTouch || (Touchscreen.current != null && Touchscreen.current.deviceId != InputDevice.InvalidDeviceId);
            var gamepadCheck = !item.RequiresGamepad || (Gamepad.all.Count > 0 && Gamepad.current != null && Gamepad.current.deviceId != InputDevice.InvalidDeviceId);

            return keyboardCheck && mouseCheck && touchCheck && gamepadCheck;
        }
    }

    public record Environment(RuntimePlatform Platform, string DetectedEnvironmentName, string DetectedEnvironmentGroup)
    {
        public RuntimePlatform Platform { get; } = Platform;
        public string DetectedEnvironmentName { get; } = DetectedEnvironmentName;
        public string DetectedEnvironmentGroup { get; } = DetectedEnvironmentGroup;
    }

#endif
}