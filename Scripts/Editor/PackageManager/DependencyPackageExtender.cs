using System;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.PackageManager
{
    public sealed class DependencyPackageExtender : IPackageManagerExtension
    {
        [InitializeOnLoadMethod]
        public static void Init()
        {
            PackageManagerExtensions.RegisterExtension(new DependencyPackageExtender());
        }
        
        private Label _label;
        
        public VisualElement CreateExtensionUI()
        {
            var template = EditorGUIUtility.Load(AssetDatabase.GUIDToAssetPath("acb9f625d137110439c22a4b49247c7c")) as VisualTreeAsset;
            var element = template.CloneTree();

            _label = element.Query<Label>("lblDependencies").First();
            
            return element;
        }

        public void OnPackageSelectionChange(PackageInfo packageInfo)
        {
            if (packageInfo == null)
            {
                _label.text = "<no package info>";
            }
            else
            {
                _label.text = packageInfo.dependencies == null ? "" : string.Join(Environment.NewLine, packageInfo.dependencies.Select(x => x.name));
            }
        }

        public void OnPackageAddedOrUpdated(PackageInfo packageInfo)
        {
        }

        public void OnPackageRemoved(PackageInfo packageInfo)
        {
        }
    }
}