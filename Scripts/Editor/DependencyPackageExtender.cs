using System;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace UnityExtension.Editor.extension.Scripts.Editor
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
            var template = EditorGUIUtility.Load("Assets/extension/Scripts/Editor/DependencyPackageView.uxml") as VisualTreeAsset;
            var element = template.CloneTree();

            _label = element.Query<Label>("lblDependencies").First();
            
            return element;
        }

        public void OnPackageSelectionChange(PackageInfo packageInfo)
        {
            _label.text = packageInfo.dependencies == null ? "" : string.Join(Environment.NewLine, packageInfo.dependencies.Select(x => x.name));
        }

        public void OnPackageAddedOrUpdated(PackageInfo packageInfo)
        {
        }

        public void OnPackageRemoved(PackageInfo packageInfo)
        {
        }
    }
}