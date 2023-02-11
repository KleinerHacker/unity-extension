using System.Collections.Generic;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
using UnityEngine;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class HoverProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new HoverProvider();
        }

        #endregion
        
        private bool _hoverGroup = true;

        public HoverProvider() : base("Project/Tooling/Hover System", SettingsScope.Project, new[] { "tooling", "hover" })
        {
        }

        public override void OnTitleBarGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                ExtendedEditorGUILayout.SymbolFieldLeft("Activate System", "PCSOFT_HOVER");
                EditorGUI.BeginDisabledGroup(
#if PCSOFT_HOVER
                    false
#else
                    true
#endif
                );
                {
                    ExtendedEditorGUILayout.SymbolFieldLeft("Verbose Logging", "PCSOFT_HOVER_LOGGING");
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }

        public override void OnGUI(string searchContext)
        {
            GUILayout.Space(15f);
        }
    }
}