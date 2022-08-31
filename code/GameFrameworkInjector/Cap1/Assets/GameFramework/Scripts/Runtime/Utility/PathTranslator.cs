using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class PathTranslator
    {
        private const string HttpPreFix = "http://";
        private const string HttpsPreFix = "https://";

        public static string GetIndexedDBPath(string fullPath)
        {
            string targetPath = fullPath;
            if (fullPath.StartsWith("file:///idbfs/"))
            {
                targetPath = fullPath.Replace("file:///idbfs/", "/idbfs/");
            }

            string httpPrefix = null;
            if (fullPath.StartsWith(HttpPreFix))
            {
                httpPrefix = HttpPreFix;
            }
            else if (fullPath.StartsWith(HttpsPreFix))
            {
                httpPrefix = HttpsPreFix;
            }

            if (httpPrefix != null)
            {
                int firstSlashIndex = fullPath.IndexOf("/", httpPrefix.Length);
                string subPath = fullPath.Substring(firstSlashIndex + 1, fullPath.Length - firstSlashIndex - 1);
                targetPath = Application.persistentDataPath + subPath;
            }
            
            Log.Debug(" change path: {0} -> {1}", fullPath, targetPath);
            return targetPath;
        }
    }
}