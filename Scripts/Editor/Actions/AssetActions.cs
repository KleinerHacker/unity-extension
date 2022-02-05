using UnityEditor;
using UnityEngine;

namespace UnityExtension.Editor.extension.Scripts.Editor.Actions
{
    public static class AssetActions
    {
        [MenuItem("Assets/Copy GUID", priority = int.MaxValue)]
        public static void CopyGUID() 
        {
            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
            var guid = AssetDatabase.GUIDFromAssetPath(assetPath);

            GUIUtility.systemCopyBuffer = guid.ToString();
        }
    }
}