using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using UnityExtension.Editor.extension.Scripts.Editor.Utils;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace UnityExtension.Editor.extension.Scripts.Editor.PackageManager
{
    public sealed class ReleaseNotesPackageExtender : IPackageManagerExtension
    {
        private static readonly IDictionary<string, (string version, string notes)> Cache = new Dictionary<string, (string version, string notes)>();

        /*[InitializeOnLoadMethod]
        public static void Init()
        {
            PackageManagerExtensions.RegisterExtension(new ReleaseNotesPackageExtender());
        } */

        private Label _label;
        private VisualElement _releaseNotes;

        public VisualElement CreateExtensionUI()
        {
            var template = EditorGUIUtility.Load(AssetDatabase.GUIDToAssetPath("a91d4a4bfff85b341b6eb7d119e58e2e")) as VisualTreeAsset;
            var element = template.CloneTree();

            _label = element.Query<Label>("lblReleaseNotes").First();
            _releaseNotes = element.Query<VisualElement>("pnlReleaseNotes").First();

            return element;
        }

        public void OnPackageSelectionChange(PackageInfo packageInfo)
        {
            _releaseNotes.Clear();
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
            EditorCoroutineUtility.StartCoroutine(LoadReleaseNotes(packageInfo), this);
        }

        private IEnumerator LoadReleaseNotes(PackageInfo packageInfo)
        {
            var packageId = packageInfo.packageId;
            var package = packageId[..packageId.LastIndexOf("@", StringComparison.Ordinal)];
            var releaseNotes = PackageManagerCache.GetReleaseNotes(package);
            if (releaseNotes == null)
            {
                var request = UnityWebRequest.Get(packageInfo.registry.url + "/" + package);
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    _label.text = "! Failure: (UPM) " + request.result + " - " + request.error + " !";
                }
                else
                {
                    var urlTuple = JsonParsingUtils.FindReleaseNotesUrl(request.downloadHandler.text);
                    if (urlTuple == null)
                    {
                        _label.text = "! Failure: Url not found !";
                    }
                    else
                    {
                        var githubUrl = urlTuple.Value.url.Replace("https://github.com", "https://api.github.com/repos") + "/releases";
                        Debug.Log("Try to connect to github API: " + githubUrl);
                        var webRequest = UnityWebRequest.Get(githubUrl);
                        yield return webRequest.SendWebRequest();

                        if (webRequest.result != UnityWebRequest.Result.Success)
                        {
                            _label.text = "! Failure: (RELEASE) " + webRequest.result + " - " + webRequest.error + " !";
                        }
                        else
                        {
                            releaseNotes = JsonParsingUtils.FindReleaseNotes(urlTuple.Value.version, webRequest.downloadHandler.text);
                            if (releaseNotes == null || releaseNotes.Length <= 0)
                            {
                                _label.text = "List is empty";
                            }
                            else
                            {
                                PackageManagerCache.SetReleaseNotes(package, releaseNotes);
                            }
                        }
                    }
                }
            }

            if (releaseNotes != null)
            {
                _label.text = "";

                foreach (var releaseNote in releaseNotes)
                {
                    var child = new VisualElement();
                    EditorCoroutineUtility.StartCoroutine(MarkupUtils.BuildReadmeWithStylesAsync(releaseNote.notes, child), this);

                    var label = new Label(releaseNote.version);
                    label.AddToClassList("header");

                    _releaseNotes.Add(label);
                    _releaseNotes.Add(child);
                }
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