using NCFileCompare.Models;
using NCFileCompare.Models.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace NCFileCompare.Models
{

    public enum DiffSeverity
    {
        Critical,
        NonCritical
    }

    public class NCComparer
    {
        public readonly NCFile File1, File2;
        public readonly double Tol;
        public string FilePath;


        // Dictionary for Compare Result, what the f*

        public Dictionary</*key*/(int Left, int Right),
           /*value*/Dictionary<(int? Left, int? Right), (NCDiff Left, NCDiff Right)>> Result =
                new Dictionary<(int First, int Second),
                    Dictionary<(int? Left, int? Right), (NCDiff Left, NCDiff Right)>>();

        // Error Messages
        public Dictionary<DiffSeverity, List<string>> DiffMessages =
            new Dictionary<DiffSeverity, List<string>>();


        public NCComparer(NCFile file, string filePath)
        {
            File1 = file;
            FilePath = filePath;
            DiffMessages = new Dictionary<DiffSeverity, List<string>>()
            {
                { DiffSeverity.Critical, new List<string>() },
                { DiffSeverity.NonCritical, new List<string>() }
            };
            GlobalFlags.Error = false;
        }
        public NCComparer(NCFile file1, NCFile file2, double tol, string filePath)
        {
            File1 = file1;
            File2 = file2;
            Tol = tol;
            FilePath = filePath;
            DiffMessages = new Dictionary<DiffSeverity, List<string>>()
            {
                { DiffSeverity.Critical, new List<string>() },
                { DiffSeverity.NonCritical, new List<string>() }
            };
            GlobalFlags.Error = false;
        }

        public void Compare()
        {
            var Courses1 = File1.Courses;
            var Courses2 = File2.Courses;
            GlobalFlags.Diff = false;

            // Checks course count
            if (Courses1.Count != Courses2.Count)
            {
                DiffMessages[DiffSeverity.Critical].Add
                    ($"The number of courses is not equal, File1={Courses1.Count} File2={Courses2.Count}");
                GlobalFlags.Diff = true;
                return;
            }

            int common = Math.Min(Courses1.Count, Courses2.Count);
            for (int i = 0; i < common; i++)
            {
                var Lines1 = Courses1[i].Lines;
                var Lines2 = Courses2[i].Lines;

                var CourseResults =
                    new Dictionary<(int? Left, int? Right), (NCDiff Left, NCDiff Right)>();

                // Checks line count
                if (Lines1.Count != Lines2.Count)
                {
                    DiffMessages[DiffSeverity.NonCritical].Add
                        ($"In Course {i + 1} the number of lines is different, File1={Lines1.Count} File2={Lines2.Count}");
                    GlobalFlags.Diff = true;
                }
                // Checks if lines end in layup
                if (Lines1[Lines1.Count - 1].IsLayup || Lines2[Lines2.Count - 1].IsLayup)
                {
                    DiffMessages[DiffSeverity.NonCritical].Add
                        ($"In Course {i + 1}, the layup is not closed");
                    GlobalFlags.Error = true;
                }


                int n = Math.Min(Lines1.Count, Lines2.Count);
                for (int j = 0; j < n; j++)
                {
                    var Line1 = Lines1[j];
                    var Line2 = Lines2[j];

                    bool lineDiff = false;

                    NCDiff bufferDiff1 = new NCDiff();
                    NCDiff bufferDiff2 = new NCDiff();

                    //ValueComparison
                    var axes1 = Line1.Variables.Axes.Keys.Where(k => Line1.RawLine.Contains(k)).ToList();
                    var axes2 = Line2.Variables.Axes.Keys.Where(k => Line2.RawLine.Contains(k)).ToList();
                    var param1 = Line1.Variables.Parameters.Keys.Where(k => Line1.RawLine.Contains(k)).ToList();
                    var param2 = Line2.Variables.Parameters.Keys.Where(k => Line2.RawLine.Contains(k)).ToList();

                    var allAxes = axes1.Union(axes2).ToList();

                    var allParams = param1.Union(param2).ToList();

                    // Axis loop
                    foreach (var axis in allAxes)
                    {
                        bool has1 = Line1.Variables.Axes.TryGetValue(axis, out double v1Axis);
                        bool has2 = Line2.Variables.Axes.TryGetValue(axis, out double v2Axis);

                        //AxesExtrema[axis] = NCUtils.findAxesExtrema(Line1, Line2, axis,AxesExtrema); 

                        if (!has1 && !has2)
                            continue;

                        // Missing in File 2
                        if (has1 && !has2)
                        {
                            GlobalFlags.Diff = true;
                            lineDiff = true;
                            bufferDiff1.Values[axis] = v1Axis;
                        }
                        // Missing in File 1
                        else if (!has1 && has2)
                        {
                            GlobalFlags.Diff = true;
                            lineDiff = true;
                            bufferDiff2.Values[axis] = v2Axis;
                        }
                        // Abs of both axis values 
                        else if (Math.Abs(v1Axis - v2Axis) > Tol)
                        {
                            GlobalFlags.Diff = true;
                            lineDiff = true;

                            bufferDiff1.Values[axis] = v1Axis;
                            bufferDiff2.Values[axis] = v2Axis;

                            bufferDiff1.InLayup = Line1.IsLayup;
                            bufferDiff2.InLayup = Line2.IsLayup;

                        }

                    } // End of foreach

                    // Param loop
                    foreach (var param in allParams)
                    {
                        bool has1 = Line1.Variables.Axes.TryGetValue(param, out double v1Param);
                        bool has2 = Line2.Variables.Axes.TryGetValue(param, out double v2Param);

                        if (!has1 && !has2)
                            continue;

                        // Missing in File 2
                        if (has1 && !has2)
                        {
                            GlobalFlags.Diff = true;
                            lineDiff = true;
                            bufferDiff1.Values[param] = v1Param;
                        }
                        // Missing in File 1
                        else if (!has1 && has2)
                        {
                            GlobalFlags.Diff = true;
                            lineDiff = true;
                            bufferDiff2.Values[param] = v2Param;
                        }
                        // Abs of both axis values 
                        else if (Math.Abs(v1Param - v2Param) > Tol)
                        {
                            GlobalFlags.Diff = true;
                            lineDiff = true;

                            bufferDiff1.Values[param] = v1Param;
                            bufferDiff2.Values[param] = v2Param;

                            bufferDiff1.InLayup = Line1.IsLayup;
                            bufferDiff2.InLayup = Line2.IsLayup;

                        }
                    } // End loop

                    // External Course Values
                    if (GlobalFlags.EnableExternalComparison)
                    {
                        var e1 = Line1.Variables.ExternalValues.Keys.Where(k => Line1.RawLine.Contains(k)).ToList();
                        var e2 = Line1.Variables.ExternalValues.Keys.Where(k => Line2.RawLine.Contains(k)).ToList();
                        var allE = e1.Union(e2);

                        // External variables loop
                        foreach (var extKey in allE)
                        {
                            bool has1 = Line1.Variables.ExternalValues.TryGetValue(extKey, out object extO1);
                            bool has2 = Line2.Variables.ExternalValues.TryGetValue(extKey, out object extO2);

                            var exceptionsPath = Path.Combine(FilePath, "ExternalExceptions.txt");
                            var exceptions = File.ReadAllLines(exceptionsPath);

                            if (has1 && !has2)
                            {
                                GlobalFlags.Diff = true;
                                lineDiff = true;
                                bufferDiff1.ExternalValues[extKey] = extO1;
                            }
                            else if (!has1 && has2)
                            {
                                GlobalFlags.Diff = true;
                                lineDiff = true;
                                bufferDiff2.ExternalValues[extKey] = extO2;
                            }
                            else if (!object.Equals(extO1, extO2) && !exceptions.Contains(extKey))
                            {
                                GlobalFlags.Diff = true;
                                lineDiff = true;

                                bufferDiff1.ExternalValues[extKey] = extO1;
                                bufferDiff2.ExternalValues[extKey] = extO2;
                            }
                        }// End of foreach
                    }// End of External If

                    var m1 = Line1.Command.Where(k => Line1.RawLine.Contains(k)).ToList();
                    var m2 = Line2.Command.Where(k => Line2.RawLine.Contains(k)).ToList();
                    foreach (var cmd in m1.Except(m2))
                    {
                        GlobalFlags.Diff = true;
                        bufferDiff1.Commands.Add(cmd);
                        //Console.WriteLine($"M {cmd} present only in File1");
                    }

                    foreach (var cmd in m2.Except(m1))
                    {
                        GlobalFlags.Diff = true;
                        bufferDiff2.Commands.Add(cmd);
                        //Console.WriteLine($"M {cmd} present only in File2");
                    }

                    // Compare FeedRate only if the checkbox is checked
                    if (GlobalFlags.EnableFeedRateComparison)
                    {
                        bool f1Exists = Line1.Variables.FeedRate.TryGetValue("F", out double f1);
                        bool f2Exists = Line2.Variables.FeedRate.TryGetValue("F", out double f2);

                        if (f1Exists && !f2Exists)
                        {
                            GlobalFlags.Diff = true;
                            lineDiff = true;
                            bufferDiff1.Values["F"] = f1;
                        }
                        else if (!f1Exists && f2Exists)
                        {
                            GlobalFlags.Diff = true;
                            lineDiff = true;
                            bufferDiff2.Values["F"] = f2;
                        }
                        else if (f1Exists && f2Exists && Math.Abs(f1 - f2) > Tol)
                        {
                            GlobalFlags.Diff = true;
                            lineDiff = true;

                            string s1 = Line1.SeqNo?.ToString() ?? "NaN";
                            string s2 = Line2.SeqNo?.ToString() ?? "NaN";

                            bufferDiff1.Values["F"] = f1;
                            bufferDiff2.Values["F"] = f2;

                            bufferDiff1.InLayup = Line1.IsLayup;
                            bufferDiff2.InLayup = Line2.IsLayup;
                        }
                    } // End of GlobalFlag if

                    // Print in 1 line multiple variables
                    if (lineDiff)
                    {
                        CourseResults[(Line1.SeqNo, Line2.SeqNo)] = (bufferDiff1, bufferDiff2);
                    }

                } // End of for



                Result[(Courses1[i].CourseSequence, Courses2[i].CourseSequence)] = CourseResults;

                //Console.WriteLine("\n=== M-Commands ===");


                //if (GlobalFlags.Diff) Console.WriteLine("-----------------------------------------------------------------");



            } // End of Course For loop
        } // End of Compare


        public void GetAxesExtrema()
        {
            Console.WriteLine("=== NC Axes Extrema Report ===");

            var Courses = File1.Courses;

            Dictionary<string, (double Min, double Max)> AxesExtrema =
                new Dictionary<string, (double Min, double Max)>();

            foreach (var Course in Courses)
            {
                var Lines = Course.Lines;

                foreach (var Line in Lines)
                {
                    var Axes = Line.Variables.Axes.Keys.Where(k => Line.RawLine.Contains(k)).ToList();

                    foreach (var axis in Axes)
                    {
                        bool has = Line.Variables.Axes.TryGetValue(axis, out double vAxis);

                        // Axes Extrema handling
                        AxesExtrema[axis] = NCUtils.findAxesExtrema(Line, axis, AxesExtrema);
                    }
                }
            }

            // Printing axes extrema
            foreach (var axis in AxesExtrema)
            {
                Console.WriteLine($"{axis.Key}, min:{axis.Value.Min}, max:{axis.Value.Max}");
            }
        }


        public bool CheckLimit()
        {
            Console.WriteLine("=== NC Axe Limit Test Report ===");

            
            var Courses = File1.Courses;
            int i = 0;

            bool checker = true;
            // Parse limits
            string filename = Path.Combine(FilePath, "Limits.txt");
            Dictionary<string, (int, int)> limits = new Dictionary<string, (int, int)>();
            limits = NCUtils.ParseAxesLimit(filename);

            foreach (var Course in Courses)
            {
                if (GlobalFlags.Error)
                {
                    break;
                }
                var Lines = Course.Lines;

                foreach (var Line in Lines)
                {
                    NCDiff bufferDiff = new NCDiff();

                    // Debug purpouse
                    var raw = Line.RawLine;

                    bool LineDiff = false;

                    var Axes = Line.Variables.Axes.Keys.Where(k => Line.RawLine.Contains(k)).ToList();

                    foreach (var axis in Axes)
                    {

                        bool inLine = Line.Variables.Axes.TryGetValue(axis, out double vAxis);
                        bool min;
                        bool max;

                        if (!limits.ContainsKey(axis))
                        {
                            GlobalFlags.Error = true;
                            throw new Exception($"Error, {axis} does not exist, please make sure you have selected the correct folder");
                        }

                        // If both limits are 0, continue for an axis name wihout number
                        if (limits[axis].Item1 == 0 && limits[axis].Item2 == 0)
                        {
                            continue;
                        }

                        // Limit check
                        min = vAxis < limits[axis].Item1;
                        max = vAxis > limits[axis].Item2;


                        if (!inLine)
                        {
                            continue;
                        }
                        if (min || max)
                        {
                            bufferDiff.Values[axis] = vAxis;
                            LineDiff = true;
                            checker = false;
                        }
                    }// Axis loop end
                    if (LineDiff)
                    {
                        Console.Write($"Course {i + 1}, Seq: {Line.SeqNo}, Differences: ");

                        foreach (var key in bufferDiff.Values.Keys)
                        {
                            bool has = bufferDiff.Values.TryGetValue(key, out double v1);

                            // Print if it's present
                            if (has)
                            {
                                Console.Write($"{key} = {v1:F4}, Min: ({limits[key].Item1}), Max: ({limits[key].Item2}) | ");
                            }
                        }
                        Console.WriteLine($"InLayup:{(Line.IsLayup ? "Yes" : "No")}");
                    }
                }// Line loop end
                i++;
            } // Course loop end

            return checker;
        }
    }
}