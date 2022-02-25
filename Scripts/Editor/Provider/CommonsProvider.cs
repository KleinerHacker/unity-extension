using System.Collections.Generic;
using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace UnityExtension.Editor.extension.Scripts.Editor.Provider
{
    public sealed class CommonsProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new CommonsProvider();
        }

        #endregion

        public CommonsProvider() : base("Project/Player/Commons", SettingsScope.Project, new List<string>() { "Commons", "Singleton" })
        {
        }

        public override void OnGUI(string searchContext)
        {
            PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup), out var symbols);
            var isSingletonLogging = symbols.Contains(UnityExtensionEditorConstants.Building.Symbol.SingletonLogging);
            var newSingletonLogging = GUILayout.Toggle(isSingletonLogging, "Activate Singleton Logging");
            if (isSingletonLogging != newSingletonLogging)
            {
                if (newSingletonLogging)
                {
                    PlayerSettings.SetScriptingDefineSymbols(
                        NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup),
                        symbols.Append(UnityExtensionEditorConstants.Building.Symbol.SingletonLogging).ToArray()
                    );
                }
                else
                {
                    PlayerSettings.SetScriptingDefineSymbols(
                        NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup),
                        symbols.Remove(UnityExtensionEditorConstants.Building.Symbol.SingletonLogging).ToArray()
                    );
                }
            }
        }
    }
}