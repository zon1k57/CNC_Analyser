using System;
using System.Collections.Generic;
using System.IO;

namespace NCFileCompare.Models
{
    public static class CourseAliasLoader
    {
        public static Dictionary<string, string> Load(string filePath)
        {
            var aliasMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string currentProperty = null;

            foreach (var rawLine in File.ReadLines(filePath))
            {
                var line = rawLine.Trim();

                // skip comments and empty lines
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    // start of a new property section
                    currentProperty = line.Trim('[', ']');
                }
                else if (currentProperty != null)
                {
                    // this is an alias for the current property
                    aliasMap[line] = currentProperty;
                }
            }

            return aliasMap;
        }
    }
}
