using System.Collections.Generic;
using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
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