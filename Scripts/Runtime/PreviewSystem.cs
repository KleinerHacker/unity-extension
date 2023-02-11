using UnityExtension.Runtime.extension.Scripts.Runtime.Components;

namespace UnityExtension.Runtime.extension.Scripts.Runtime
{
#if PCSOFT_PREVIEW && PCSOFT_RAYCASTER
    public static class PreviewSystem
    {
        public static bool IsShown => PreviewController.Singleton.IsShown;
        public static string ShownPreview => PreviewController.Singleton.ShownPreview;
        
        public static void Show(string key) => PreviewController.Singleton.Show(key);
        public static void Hide() => PreviewController.Singleton.Hide();
    }
#endif
}