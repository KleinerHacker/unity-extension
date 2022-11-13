using UnityEditor;
using UnityExtension.Runtime.extension.Scripts.Runtime.Components.UDebug.GizmosNote;

namespace UnityExtension.Editor.extension.Scripts.Editor.Components.UDebug.GizmosNote
{
    [CustomEditor(typeof(GizmosAreaNote))]
    [CanEditMultipleObjects]
    public sealed class GizmosAreaNoteEditor : GizmosNoteEditor
    {
    }
}