using UnityEngine;
using UnityEngine.InputSystem;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Utils
{
    internal static class PointerUtils
    {
        public static bool IsPressed() => InputUtils.GetValueFromDevice(Pointer.current, pointer => pointer.press.isPressed);

        public static Vector2 GetPosition() => InputUtils.GetValueFromDevice(Pointer.current, pointer => pointer.position.ReadValue());
    }
}