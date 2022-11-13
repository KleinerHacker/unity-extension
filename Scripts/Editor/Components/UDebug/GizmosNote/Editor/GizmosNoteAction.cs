using UnityEditor;
using UnityEngine;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.UDebug.GizmosNote;

namespace UnityExtension.Editor.extension.Scripts.Editor.Components.UDebug.GizmosNote.Editor
{
    public static class GizmosNoteAction
    {
        [MenuItem("GameObject/Debug/Gizmos Area Note", priority = -10000)]
        public static void CreateGizmosAreaNote()
        {
            var go = new GameObject("Gizmos Area Note");
            go.AddComponent<GizmosAreaNote>();
        }
    }
}