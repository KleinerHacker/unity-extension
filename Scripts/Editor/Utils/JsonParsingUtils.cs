using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace UnityExtension.Editor.Projects.unity_extension.Scripts.Editor.Utils
{
    internal static class JsonParsingUtils
    {
        private static readonly Regex VersionRegex = new Regex(@"versions\[([^\]]*)\]\.author\.url");
        private static readonly Regex ReleaseRegex = new Regex(@"\[([^\]]*)\]\.name");

        public static string FindReadme(string json)
        {
            var reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (string.Equals(reader.Path, "readme", StringComparison.OrdinalIgnoreCase))
                    return reader.ReadAsString();
            }

            return null;
        }

        public static (string url, string version)? FindReleaseNotesUrl(string json)
        {
            var reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (VersionRegex.IsMatch(reader.Path))
                {
                    var version = VersionRegex.Match(reader.Path).Groups[1].Value;
                    return (reader.ReadAsString(), version);
                }
            }

            return null;
        }

        public static (string version, string notes)[] FindReleaseNotes(string json, string version)
        {
            var list = new List<(string, string)>();

            var reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (ReleaseRegex.IsMatch(reader.Path))
                {
                    var number = ReleaseRegex.Match(reader.Path).Groups[1].Value;
                    if (string.Equals(reader.ReadAsString(), version, StringComparison.OrdinalIgnoreCase))
                    {
                        while (reader.Read())
                        {
                            if (reader.Path == "[" + number + "].body")
                            {
                                var text = reader.ReadAsString();

                                list.Add((version, text));
                                break;
                            }
                        }
                    }
                    break;
                }
            }

            return list.ToArray();
        }
    }
}