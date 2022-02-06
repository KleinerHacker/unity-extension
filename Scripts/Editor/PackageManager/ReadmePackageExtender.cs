using System;
using System.Collections;
using System.IO;
using Unity.EditorCoroutines.Editor;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using UnityExtension.Editor.extension.Scripts.Editor.Utils;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace UnityExtension.Editor.extension.Scripts.Editor.PackageManager
{
    public sealed class ReadmePackageExtender : IPackageManagerExtension
    {
        [InitializeOnLoadMethod]
        public static void Init()
        {
            PackageManagerExtensions.RegisterExtension(new ReadmePackageExtender());
        }

        private Label _label;
        private VisualElement _readme;

        public VisualElement CreateExtensionUI()
        {
            var template = EditorGUIUtility.Load(AssetDatabase.GUIDToAssetPath("813a751f9510ef74fb25322cceadfa39")) as VisualTreeAsset;
            var element = template.CloneTree();

            _label = element.Query<Label>("lblReadme").First();
            _readme = element.Query<VisualElement>("pnlReadme").First();

            return element;
        }

        public void OnPackageSelectionChange(PackageInfo packageInfo)
        {
            _readme.Clear();
            if (packageInfo == null)
            {
                _label.text = "<no package info>";
                return;
            }

            if (packageInfo.source != PackageSource.Registry || !string.Equals(packageInfo.registry.url, "https://package.openupm.com", StringComparison.OrdinalIgnoreCase))
            {
                _label.text = "";
                return;
            }

            _label.text = "Loading...";
            EditorCoroutineUtility.StartCoroutine(LoadReadme(packageInfo), this);
        }

        private IEnumerator LoadReadme(PackageInfo packageInfo)
        {
            var packageId = packageInfo.packageId;
            var package = packageId[..packageId.LastIndexOf("@", StringComparison.Ordinal)];
            var readme = PackageManagerCache.GetReadme(package);
            if (readme == null)
            {
                var request = UnityWebRequest.Get(packageInfo.registry.url + "/" + package);
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    _label.text = "! Failure: " + request.result + " - " + request.error + " !";
                }
                else
                {
                    readme = JsonParsingUtils.FindReadme(request.downloadHandler.text);
                    if (readme == null)
                    {
                        _label.text = "! Failure: Unable to find readme !";
                    }
                    else
                    {
                        PackageManagerCache.SetReadme(package, readme);
                    }
                }
            }

            if (readme != null)
            {
                _label.text = "";
                EditorCoroutineUtility.StartCoroutine(MarkupUtils.BuildReadmeWithStylesAsync(readme, _readme), this);
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