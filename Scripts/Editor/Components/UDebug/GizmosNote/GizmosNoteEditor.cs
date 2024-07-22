using UnityEngine;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.Components.UDebug.GizmosNote
{
    public abstract class GizmosNoteEditor : UnityEditor.Editor
    {
        public bool HasFrameBounds() { return true; }
 
        public Bounds OnGetFrameBounds() {
            return new Bounds(((MonoBehaviour)serializedObject.targetObject).transform.position, Vector3.one * serializedObject.FindProperty("noteSize").floatValue);
        }
    }
}