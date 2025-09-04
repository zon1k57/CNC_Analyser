using System;
using System.Collections.Generic;
using System.Reflection;

namespace NCFileCompare.Models
{
    public class NCCourse
    {
        public int CourseSequence { get; set; }
        public int CourseLayupSequence { get; set; }
        public string CourseName { get; set; }
        public string CourseID { get; set; }
        public int CourseVersion { get; set; }
        public int LeadAngle { get; set; }
        public int LeanAngle { get; set; }
        public int CourseLayupPathLength { get; set; }
        public int CourseArea { get; set; }
        public int IsWaste { get; set; }
        public Dictionary<string, object> Unknown { get; set; } = new Dictionary<string, object>();
        public List<NCLine> Lines { get; set; } = new List<NCLine>();


        public void CourseNameChecker(string extName, object extVar, List<string> coursePrefixes)
        {
            // Special case: course sequence handling
            if (coursePrefixes.Contains(extName))
            {
                if (extVar != null && int.TryParse(extVar.ToString(), out int courseVal))
                {
                    CourseSequence = courseVal;
                }
                else
                {
                    // fallback if not parsable
                    Unknown[extName] = extVar;
                }
            }

            // Look for matching property
            var prop = GetType().GetProperty(extName, BindingFlags.Public | BindingFlags.Instance);

            if (prop != null && prop.CanWrite)
            {
                try
                {
                    // Handle numeric conversion more safely
                    object converted;

                    if (prop.PropertyType == typeof(int) && int.TryParse(extVar?.ToString(), out int intVal))
                    {
                        converted = intVal;
                    }
                    else
                    {
                        converted = Convert.ChangeType(extVar, prop.PropertyType);
                    }

                    prop.SetValue(this, converted);
                }
                catch
                {
                    // fallback if type conversion fails
                    Unknown[extName] = extVar;
                }
            }
            else
            {
                Unknown[extName] = extVar;
            }
        }

    }
}
