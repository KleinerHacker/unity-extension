using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.Utils
{
    internal static class MarkupUtils
    {
        private static readonly Regex RegexURL = new Regex(@"\!\[([^\]]*)\]\(([^\)]*)\)");

        public static IEnumerator BuildReadmeWithStylesAsync(string text, VisualElement target)
        {
            return BuildReadmeWithStylesAsync(text, target, MarkupStyleConfig.Default);
        }

        public static IEnumerator BuildReadmeWithStylesAsync(string text, VisualElement target, MarkupStyleConfig config)
        {
            target.Clear();
            
            var reader = new StringReader(text);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (RegexURL.IsMatch(line))
                {
                    var match = RegexURL.Match(line);
                    Debug.Log("Try load image from " + match.Groups[2].Value);

                    var request = UnityWebRequest.Get(match.Groups[2].Value);
                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogWarning("Unable to load image: " + request.result + " / " + request.error + "; replace with text " + match.Groups[1].Value);

                        var label = new Label(match.Groups[1].Value);
                        target.Add(label);
                    }
                    else
                    {
                        var data = request.downloadHandler.data;

                        var texture2D = new Texture2D(1, 1, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None);
                        texture2D.LoadImage(data);

                        var width = Mathf.Min(texture2D.width, target.worldBound.width);
                        var height = width * texture2D.height / texture2D.width;

                        var image = new Image
                        {
                            image = texture2D,
                        };
                        var style = image.style;
                        style.position = Position.Relative;
                        style.width = width;
                        style.height = height;
                        target.Add(image);
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

                    target.Add(label);
                }
            }
        }
    }

    public struct MarkupStyleConfig
    {
        public static MarkupStyleConfig Default { get; } = new MarkupStyleConfig("head1", "head2", "head3");
        
        public string ClassHeadLevel1 { get; }
        public string ClassHeadLevel2 { get; }
        public string ClassHeadLevel3 { get; }

        public MarkupStyleConfig(string classHeadLevel1, string classHeadLevel2, string classHeadLevel3)
        {
            ClassHeadLevel1 = classHeadLevel1;
            ClassHeadLevel2 = classHeadLevel2;
            ClassHeadLevel3 = classHeadLevel3;
        }
    } 
}