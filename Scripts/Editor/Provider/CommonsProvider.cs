using System.Collections.Generic;
using UnityEditor;
using UnityEditorEx.Editor.Projects.unity_editor_ex.Scripts.Editor.Utils;
using UnityEngine;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.Provider
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

        public CommonsProvider() : base("Project/Commons", SettingsScope.Project, new List<string>() { "Commons", "Singleton" })
        {
        }

        public override void OnGUI(string searchContext)
        {
            var isSingletonLogging = PlayerSettingsEx.IsScriptingSymbolDefined(UnityExtensionEditorConstants.Building.Symbol.SingletonLogging);
            var newSingletonLogging = GUILayout.Toggle(isSingletonLogging, "Activate Singleton Logging");
            if (isSingletonLogging != newSingletonLogging)
            {
                if (newSingletonLogging)
                {
                    PlayerSettingsEx.AddScriptingSymbol(UnityExtensionEditorConstants.Building.Symbol.SingletonLogging);
                }
                else
                {
                    PlayerSettingsEx.RemoveScriptingSymbol(UnityExtensionEditorConstants.Building.Symbol.SingletonLogging);
                }
            }
        }
    }
}