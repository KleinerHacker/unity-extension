using UnityEditor;
using UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Components.UDebug.GizmosNote;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.Components.UDebug.GizmosNote
{
    [CustomEditor(typeof(GizmosAreaNote))]
    [CanEditMultipleObjects]
    public sealed class GizmosAreaNoteEditor : GizmosNoteEditor
    {
    }
}