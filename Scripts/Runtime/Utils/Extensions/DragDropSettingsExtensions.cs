using System.Linq;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions
{
    internal static class DragDropSettingsExtensions
    {
        public static string GetSecondaryRaycasterReference(this DragDropSettings settings) =>
            settings.UseAlternativeRaycasterForMove ? settings.RaycasterMoveReference : settings.RaycasterReference;

        public static RaycastItem GetPrimaryRaycasterInfo(this DragDropSettings settings) =>
            RaycastSettings.Singleton.Items.FirstOrDefault(x => x.Key == settings.RaycasterReference);

        public static RaycastItem GetSecondaryRaycasterInfo(this DragDropSettings settings) =>
            RaycastSettings.Singleton.Items.FirstOrDefault(x => x.Key == settings.GetSecondaryRaycasterReference());
    }
}