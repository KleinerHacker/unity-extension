using UnityEngine;
using UnityEngine.InputSystem;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Utils
{
    internal static class PointerUtils
    {
        public static bool IsPressed()
        {
            var mousePressed =
#if PCSOFT_RAYCASTER_MOUSE
                    InputUtils.GetValueFromDevice(Mouse.current, mouse => mouse.leftButton.isPressed)
#else
                    false
#endif
                ;

            var touchPressed =
#if PCSOFT_RAYCASTER_TOUCH
                    InputUtils.GetValueFromDevice(Touchscreen.current, touchscreen => touchscreen.primaryTouch.press.isPressed)
#else
                    false
#endif
                ;

            var penPressed =
#if PCSOFT_RAYCASTER_PEN
                    InputUtils.GetValueFromDevice(Pen.current, pen => pen.press.isPressed)
#else
                    false
#endif
                ;

            return mousePressed || touchPressed || penPressed;
        }

        public static Vector2 GetPosition()
        {
#if PCSOFT_RAYCASTER_MOUSE
            var mousePosition = InputUtils.GetValueFromDevice(Mouse.current, mouse => mouse.position.ReadValue());
            if (mousePosition != default)
                return mousePosition;
#endif

#if PCSOFT_RAYCASTER_TOUCH
            var touchPosition = InputUtils.GetValueFromDevice(Touchscreen.current, touchscreen => touchscreen.primaryTouch?.position.ReadValue() ?? default);
            if (touchPosition != default)
                return touchPosition;
#endif

#if PCSOFT_RAYCASTER_PEN
            var penPosition = InputUtils.GetValueFromDevice(Pen.current, pen => pen.position.ReadValue());
            if (penPosition != default)
                return penPosition;
#endif

            return Vector2.zero;
        }
    }
}