using System.Linq;
using UnityExtension.Runtime.extension.Scripts.Runtime.Assets;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Utils.Extensions
{
    internal static class DragDropSettingsExtensions
    {
        public static string GetSecondaryRaycasterReference(this DragDropItem item) =>
            item.UseAlternativeRaycasterForMove ? item.RaycasterMoveReference : item.RaycasterReference;

        public static RaycastItem GetPrimaryRaycasterInfo(this DragDropItem item) =>
            RaycastSettings.Singleton.Items.FirstOrDefault(x => x.Key == item.RaycasterReference);

        public static RaycastItem GetSecondaryRaycasterInfo(this DragDropItem item) =>
            RaycastSettings.Singleton.Items.FirstOrDefault(x => x.Key == item.GetSecondaryRaycasterReference());
    }
}