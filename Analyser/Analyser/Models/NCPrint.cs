using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCFileCompare.Models
{
    public class NCPrint
    {
        // Results
        public Dictionary<(int Left, int Right),
               Dictionary<(int? Left, int? Right), (NCDiff Left, NCDiff Right)>> Results
        { get; set; }

        // Error Messages
        public Dictionary<DiffSeverity, List<string>> DiffMessages { get; set; }

        public NCPrint(Dictionary<(int Left, int Right), /*key*/
               /*value*/Dictionary<(int? Left, int? Right), (NCDiff Left, NCDiff Right)>> results,
                        Dictionary<DiffSeverity, List<string>> diffMessages)
        {
            Results = results;
            DiffMessages = diffMessages;
        }

        
        public void Print()
        {
            Console.WriteLine("=== NC Compare Report ===");

            // AI generated, checks for critical messages and prints them
            if (DiffMessages != null &&
            DiffMessages.TryGetValue(DiffSeverity.Critical, out var criticalList) &&
            criticalList != null &&
            criticalList.Count > 0)
            {
                foreach (var message in criticalList)
                {
                    Console.WriteLine(message);
                }
                return;
            }
            // Copied from above, checks for non critical messages and prints them
            if(DiffMessages != null && 
            DiffMessages.TryGetValue(DiffSeverity.NonCritical, out var nonCriticalList)&&
            nonCriticalList != null &&
            nonCriticalList.Count > 0)
            {
                foreach(var message in nonCriticalList)
                {
                    Console.WriteLine(message);
                }
            }


            // Prints axis differences
            foreach (var result in Results)
            {
                Console.WriteLine($"COURSE NUMBER {result.Key}");

                foreach(var r in result.Value)
                {
                    Console.Write($"Seq:{r.Key}, Differences: ");

                    // Gets union from left and right axes and parameters
                    var combined = r.Value.Left.Values.Keys.
                        Union(r.Value.Right.Values.Keys).
                        Union(r.Value.Left.Commands).
                        Union(r.Value.Right.Commands);


                    // Gets union from left and right external values
                    var extCombined = r.Value.Left.ExternalValues.Keys.
                        Union(r.Value.Right.ExternalValues.Keys);

                    // Loop trough external values
                    foreach (var key in extCombined)
                    {
                        bool has1 = r.Value.Left.ExternalValues.TryGetValue(key, out object v1);
                        bool has2 = r.Value.Right.ExternalValues.TryGetValue(key, out object v2);

                        if (has1 && !has2)
                        {
                            Console.Write($"{key} missing File2, ");
                        }
                        else if (!has1 && has2)
                        {
                            Console.Write($"{key} missing File1, ");
                        }
                        else
                        {
                            Console.Write($"{key} = {v1}/{v2}, ");
                        }
                    }

                    // Loop trough axes and parameters
                    foreach (var key in combined)
                    {
                        bool has1 = r.Value.Left.Values.TryGetValue(key, out double v1);
                        bool has2 = r.Value.Right.Values.TryGetValue(key, out double v2);

                        if (has1 && !has2)
                        {
                            Console.Write($"{key} missing File2, ");
                        }
                        else if (!has1 && has2)
                        {
                            Console.Write($"{key} missing File1, ");
                        }
                        else
                        {
                            Console.Write($"{key} = {v1:F4}/{v2:F4}, ");
                        }
                    }
                    // Prints layup status
                    Console.WriteLine($"InLayup:{(r.Value.Left.InLayup ? "Yes" : "No")}/{(r.Value.Left.InLayup ? "Yes" : "No")}");
                }// End foreach
            }// End foreach
        }// End Print()
    }
}
