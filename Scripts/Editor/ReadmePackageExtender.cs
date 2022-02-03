using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Unity.EditorCoroutines.Editor;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace UnityExtension.Editor.extension.Scripts.Editor
{
    public sealed class ReadmePackageExtender : IPackageManagerExtension
    {
        private static readonly Regex REGEX_URL = new Regex(@"\!\[([^\]]*)\]\(([^\)]*)\)");

        [InitializeOnLoadMethod]
        public static void Init()
        {
            PackageManagerExtensions.RegisterExtension(new ReadmePackageExtender());
        }

        private Label _label;
        private VisualElement _readme;

        public VisualElement CreateExtensionUI()
        {
            var template = EditorGUIUtility.Load("Assets/extension/Scripts/Editor/ReadmePackageView.uxml") as VisualTreeAsset;
            var element = template.CloneTree();

            _label = element.Query<Label>("lblReadme").First();
            _readme = element.Query<VisualElement>("pnlReadme").First();

            return element;
        }

        public void OnPackageSelectionChange(PackageInfo packageInfo)
        {
            if (packageInfo.source != PackageSource.Registry || !string.Equals(packageInfo.registry.url, "https://package.openupm.com", StringComparison.OrdinalIgnoreCase))
            {
                _label.text = "";
                return;
            }

            _readme.Clear();
            _label.text = "Loading...";
            EditorCoroutineUtility.StartCoroutine(LoadReadme(packageInfo), this);
        }

        private IEnumerator LoadReadme(PackageInfo packageInfo)
        {
            var packageId = packageInfo.packageId;
            var package = packageId[..packageId.LastIndexOf("@", StringComparison.Ordinal)];
            var request = UnityWebRequest.Get(packageInfo.registry.url + "/" + package);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                _label.text = "! Failure: " + request.result + " - " + request.error + " !";
            }
            else
            {
                _label.text = "";

                var json = request.downloadHandler.text;
                var reader = new JsonTextReader(new StringReader(json));
                while (reader.Read())
                {
                    if (string.Equals(reader.Path, "readme", StringComparison.OrdinalIgnoreCase))
                    {
                        var text = reader.ReadAsString();
                        EditorCoroutineUtility.StartCoroutine(BuildReadmeWithStyles(text), this);

                        break;
                    }
                }
            }
        }

        private IEnumerator BuildReadmeWithStyles(string text)
        {
            var reader = new StringReader(text);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (REGEX_URL.IsMatch(line))
                {
                    var match = REGEX_URL.Match(line);
                    Debug.Log("Try load image from " + match.Groups[2].Value);

                    var request = UnityWebRequest.Get(match.Groups[2].Value);
                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogWarning("Unable to load image: " + request.result + " / " + request.error + "; replace with text " + match.Groups[1].Value);
                        
                        var label = new Label(match.Groups[1].Value);
                        _readme.Add(label);
                    }
                    else
                    {
                        var data = request.downloadHandler.data;
                        
                        var texture2D = new Texture2D(1, 1, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None);
                        texture2D.LoadImage(data);

                        var width = Mathf.Min(texture2D.width, _readme.worldBound.width);
                        var height = width * texture2D.height / texture2D.width;

                        var image = new Image
                        {
                            image = texture2D,
                        };
                        var style = image.style;
                        style.position = Position.Relative;
                        style.width = width; 
                        style.height = height;
                        _readme.Add(image);
                    }
                }
                else
                {
                    var label = new Label();
                    if (line.StartsWith("###"))
                    {
                        label.text = line.Substring(3).Trim();
                        label.AddToClassList("head3");
                    }
                    else if (line.StartsWith("##"))
                    {
                        label.text = line.Substring(2).Trim();
                        label.AddToClassList("head2");
                    }
                    else if (line.StartsWith("#"))
                    {
                        label.text = line.Substring(1).Trim();
                        label.AddToClassList("head1");
                    }
                    else
                    {
                        label.text = line;
                    }
                    
                    _readme.Add(label);
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