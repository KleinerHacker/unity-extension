using System.Collections.Generic;

namespace UnityExtension.Editor.extension.Scripts.Editor.Utils
{
    internal static class PackageManagerCache
    {
        private static readonly IDictionary<string, PackageManagerCacheObject> Cache = new Dictionary<string, PackageManagerCacheObject>();

        public static string GetReadme(string package)
        {
            if (!Cache.ContainsKey(package))
                return null;

            return Cache[package].Readme;
        }

        public static void SetReadme(string package, string readme)
        {
            if (!Cache.ContainsKey(package))
            {
                Cache.Add(package, new PackageManagerCacheObject());
            }

            Cache[package].Readme = readme;
        }

        public static (string version, string notes)[] GetReleaseNotes(string package)
        {
            if (!Cache.ContainsKey(package))
                return null;

            return Cache[package].ReleaseNotes;
        }
        
        public static void SetReleaseNotes(string package, (string version, string notes)[] releaseNotes)
        {
            if (!Cache.ContainsKey(package))
            {
                Cache.Add(package, new PackageManagerCacheObject());
            }

            Cache[package].ReleaseNotes = releaseNotes;
        }
    }

    internal sealed class PackageManagerCacheObject
    {
        public string Readme { get; set; }
        public (string version, string notes)[] ReleaseNotes { get; set; }
    }
}