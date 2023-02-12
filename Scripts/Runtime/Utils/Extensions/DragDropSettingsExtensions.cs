using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions
{
    internal static class DragDropSettingsExtensions
    {
        public static string GetMoveRaycasterReference(this DragDropSettings settings) => 
            settings.UseAlternativeRaycasterForMove ? settings.RaycasterMoveReference : settings.RaycasterReference;
    }
}