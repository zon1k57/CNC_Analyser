using System;
using System.IO;

namespace NCFileCompare.Models
{
    public static class PathHelper
    {
        public static string ToRelative(string absolutePath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            // Normalize absolute path in case it's a file:// URI
            if (absolutePath.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
            {
                var absUri = new Uri(absolutePath);
                absolutePath = absUri.LocalPath;
            }

            Uri baseUri = new Uri(basePath, UriKind.Absolute);
            Uri fullUri = new Uri(absolutePath, UriKind.Absolute);

            string relUri = baseUri.MakeRelativeUri(fullUri).ToString();

            // Return as normal OS path
            return Uri.UnescapeDataString(relUri)
                      .Replace('/', Path.DirectorySeparatorChar);
        }

        public static string ToAbsolute(string relativePath)
        {
            // Handle inputs that come as file:// URIs
            if (relativePath.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
            {
                var uri = new Uri(relativePath);
                return uri.LocalPath;
            }

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(basePath, relativePath));
        }
    }
}
