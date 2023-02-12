using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityBase.Runtime.@base.Scripts.Runtime.Utils.Extensions;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityExtension.Editor.extension.Scripts.Editor.Components.UDebug.GizmosNote.Editor
{
    public sealed class GizmosNoteWindow : EditorWindow
    {
        #region Static Area

        [MenuItem("Window/General/Debug/Gizmos Notes")]
        public static void ShowGizmosNoteWindow()
        {
            var window = CreateInstance<GizmosNoteWindow>();
            window.titleContent = new GUIContent("Gizmos Notes", EditorGUIUtility.IconContent("d_DebuggerAttached").image);
            window.Show();
        }

        #endregion

        private readonly SortedDictionary<string, IList<Runtime.extension.Scripts.Runtime.Components.UDebug.GizmosNote.GizmosNote>> _notes = new SortedDictionary<string, IList<Runtime.extension.Scripts.Runtime.Components.UDebug.GizmosNote.GizmosNote>>(StringComparer.Ordinal);
        private readonly IDictionary<string, bool> _foldouts = new Dictionary<string, bool>();

        #region Builtin Methods

        private void OnEnable()
        {
            ReloadGizmosNotes();
        }

        private void OnFocus()
        {
            ReloadGizmosNotes();
        }

        private void OnGUI()
        {
            foreach (var key in _notes.Keys)
            {
                var currentFoldoutState = _foldouts.GetOrDefault(key, false, true);
                var newFoldoutState = EditorGUILayout.BeginFoldoutHeaderGroup(currentFoldoutState, key);
                _foldouts.AddOrOverwrite(key, newFoldoutState);
                
                if (newFoldoutState)
                {
                    EditorGUI.indentLevel = 1;
                    foreach (var gizmosNote in _notes[key])
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField(gizmosNote.Text, GUILayout.ExpandWidth(true));
                            if (GUILayout.Button(EditorGUIUtility.IconContent("d_scenevis_visible_hover").image, EditorStyles.iconButton))
                            {
                                EditorGUIUtility.PingObject(gizmosNote);
                                Selection.activeGameObject = gizmosNote.gameObject;
                                EditorCoroutineUtility.StartCoroutine(SelectionCoroutine(), this);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.indentLevel = 0;
                }
                
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            
            IEnumerator SelectionCoroutine()
            {
                yield return null;
                SceneView.lastActiveSceneView.FrameSelected();
            }
        }

        #endregion

        private void ReloadGizmosNotes()
        {
            _notes.Clear();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                foreach (var rootGameObject in scene.GetRootGameObjects())
                {
                    foreach (var gizmosNote in rootGameObject.FindComponents<Runtime.extension.Scripts.Runtime.Components.UDebug.GizmosNote.GizmosNote>())
                    {
                        if (!_notes.ContainsKey(gizmosNote.Category))
                        {
                            _notes.Add(gizmosNote.Category, new List<Runtime.extension.Scripts.Runtime.Components.UDebug.GizmosNote.GizmosNote>());
                        }

                        _notes[gizmosNote.Category].Add(gizmosNote);
                    }
                }
            }
        }
    }
}