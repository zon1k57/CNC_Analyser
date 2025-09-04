// NCFile.cs
using NCFileCompare.Models;
using NCFileCompare.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NCFileCompare.Models
{
    public class NCFile : INCFile
    {
        public string GlobalFolderName { get; set; }

        public string Filename { get; }
        public List<NCCourse> Courses { get; set; } 
        string INCFile.Filename { get => Filename; set => throw new NotImplementedException(); }
        

        private readonly Dictionary<string, double> _axes; // Biblioteka za oski
        private readonly Dictionary<string, double> _parameters; // Biblioteka za parametri
        private readonly Dictionary<string, object> _extVars; // Biblioteka za eksterni vrednosti
        private readonly Dictionary<string, double> _feedRate; // Библиотека за feedrate
        

        private static readonly Regex _seqRx = new Regex
            (@"\bN(?<n>\d+)\b", // Sequence Regex
            RegexOptions.Compiled);
        private static readonly Regex _cmdRx = new Regex
            (@"\b(?<cmd>[GM]\d{1,3})\b", // Command Regex
            RegexOptions.Compiled);
        private static readonly Regex _cmtRx = new Regex(
            @"\((?<c>[^)]*)\)|;(?<c>[^;\n\r]*)(?=\r?\n|$)|;(?<c>[^;]+);", // Comment Regex
            RegexOptions.Compiled); 
        private static readonly Regex _feedRtRx = new Regex
            (@"\b(?<name>F)\s*=?\s*(?<expr>\d+(?:\.\d+)?(?:\s*\*\s*P\d+)?)", // FeedRate Regex
            RegexOptions.Compiled);

        // assignRx фаќа name=(expr) каде expr е:
        //   - аритметика од броеви и променливи
        //   - или string во двојни наводници
        private static readonly Regex _assignRx = new Regex(
            @"\b
                (?<name>(?:V\.E\.)?[A-Za-z]\w*)      # име, можеби со V.E. префикс
                \s*=\s*
                (?<expr>
                   [+\-]?(?:\d+(?:\.\d+)?|[A-Za-z]\w*)    # број или променлива
                   (?:[+\-*/](?:\d+(?:\.\d+)?|[A-Za-z]\w*))*  # оператор+број/променлива, нула или повеќе
                   |                                        # или
                   "".*?""                                  # string во наводници
                )
            (?=\s*\(|\s|$)                              # стопи пред ( или space или крај
            ",
            RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        private static readonly List<string> AxisPrefixes = new List<string>(); // Prefiks od Axes.txt za oski
        private static readonly List<string> ParamPrefixes = new List<string>();
        private static readonly List<string> CourseSeqPrefixes = new List<string>();
        private static string LayupStart = "";// Prefisk od Axes.txt za LayupStart
        private static string LayupEnd = "";// Prefisk od Axes.txt za LayupEnd

        private static void LoadAxesConfig(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Missing axes config file: " + path);

            var section = "";
            foreach (var line in File.ReadAllLines(path))   
            {
                var trimmed = line.Trim();
                // Section check
                if (trimmed.StartsWith("[")) section = trimmed.ToUpperInvariant();
                else if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    switch (section)
                    {
                        case "[AXES]":
                            AxisPrefixes.Add(trimmed.ToUpperInvariant());
                            break;
                        case "[PARAMS]":
                            ParamPrefixes.Add(trimmed.ToUpperInvariant());
                            break;
                        case "[COURSESEQUENCE]":
                            CourseSeqPrefixes.Add(trimmed);
                            break;
                        case "[LAYUPSTART]":
                            LayupStart = trimmed.ToUpperInvariant();
                            break;
                        case "[LAYUPEND]":
                            LayupEnd = trimmed.ToUpperInvariant();
                            break;
                    }
                } // else-if end
            } // foreach end
        }

        // Checks if variable is Course Sequence
        private static bool IsNewCourse(string name,string expr, int CourseCounter)
        {
            return CourseSeqPrefixes.Contains(name) &&
                    int.TryParse(expr, out int num) &&
                    num-1 == CourseCounter;
        }

        // Checks if variable is axis
        private static bool IsAxis(string name)
        {
            foreach (var prefix in AxisPrefixes)
            {
                var pattern = $"^{prefix}[0-9.+-]*$"; // Starts with prefix, followed by optional number
                if (Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase))
                    return true;
            }
            return false;
        }

        // Checks if variable is parameter
        private static bool IsParameter(string name)
        {
            foreach (var prefix in ParamPrefixes)
            {
                var pattern = $"^{prefix}[0-9.+-]*$";
                if (Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase))
                    return true;
            }
            return false;
        }

        // Checks LayupStart and LayupEnd
        private static bool LayupCheck(List<string> name, bool isLayup)
        {
            foreach(var n in name)
            {
                if(n == LayupStart)
                {
                    isLayup = true;
                    break;
                }
                else if(n == LayupEnd)
                {
                    isLayup = false;
                    break;
                }
            }
            return isLayup;
        }

        public NCFile(string filename, string axesFileName)
        {
            AxisPrefixes.Clear();
            ParamPrefixes.Clear();
            CourseSeqPrefixes.Clear();

            Filename = filename;
            Courses = new List<NCCourse>();
            _axes = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            _parameters = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            _extVars = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            _feedRate = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

            // Вчитај конфигурација еднаш по инстанца
            if (AxisPrefixes.Count == 0 && ParamPrefixes.Count == 0)
            {
                // ..\NCFileCompare\NCFileCompare\bin\Debug\Axes.txt
                string configPath = Path.Combine(axesFileName, "Axes.txt");
                LoadAxesConfig(configPath);
                GlobalFolderName = Path.Combine(axesFileName, "CourseAliases.txt");
            }
        }

        public void Parse()
        {
            if (!File.Exists(Filename))
                throw new FileNotFoundException("File not found: " + Filename);

            int CourseCounter = 0;
            bool CourseWrite = false;
            bool isLayup = false;

            using (StreamReader reader = new StreamReader(Filename))
            {
                string raw;
                while ((raw = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(raw))
                        continue;

                    Dictionary<string, double> Axes = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
                    Dictionary<string, double> Parameters = new Dictionary<string, double>(_parameters, StringComparer.OrdinalIgnoreCase);
                    Dictionary<string, object> ExternalValues = new Dictionary<string, object>(_extVars, StringComparer.OrdinalIgnoreCase);
                    Dictionary<string, double> FeedRate = new Dictionary<string, double>(getFeedRate(raw), StringComparer.OrdinalIgnoreCase);

                    NCLine line = new NCLine
                    {
                        RawLine = raw,
                        SeqNo = GetSeqNo(raw),
                        Command = GetCommands(raw),
                        Comment = GetComment(raw),
                        Variables = new NCVariables(Axes, Parameters, ExternalValues, FeedRate)
                    };
                    isLayup = LayupCheck(line.Command, isLayup);
                    line.IsLayup = isLayup;

                    foreach (Match m in _assignRx.Matches(raw))
                    {
                        string name = m.Groups["name"].Value;
                        string expr = m.Groups["expr"].Value;

                        if (IsNewCourse(name, expr, CourseCounter) && int.TryParse(expr, out int num))
                        {
                            CourseWrite = true;
                            CourseCounter++;
                            isLayup = false;
                            line.IsLayup = isLayup;
                            Courses.Add(new NCCourse());
                        }

                        if (IsParameter(name))
                        {
                            string repl = Regex.Replace(expr, @"\b\w+\b", id =>
                            {
                                string k = id.Value;
                                if (_parameters.ContainsKey(k)) return _parameters[k].ToString();
                                if (_axes.ContainsKey(k)) return _axes[k].ToString();
                                return k;
                            });
                            double dv = EvaluateExpression(repl);
                            _parameters[name] = dv;
                            line.Variables.Parameters[name] = dv;
                        }
                        else if (IsAxis(name))
                        {
                            string repl = Regex.Replace(expr, @"\b\w+\b", id =>
                            {
                                string k = id.Value;
                                if (_axes.ContainsKey(k)) return _axes[k].ToString();
                                if (_parameters.ContainsKey(k)) return _parameters[k].ToString();
                                return k;
                            });
                            double dv = EvaluateExpression(repl);
                            _axes[name] = dv;
                            line.Variables.Axes[name] = dv;
                        }
                        else
                        {
                            object val;

                            if (expr[0] == '"' && expr[expr.Length - 1] == '"')
                            {
                                val = expr.Substring(1, expr.Length - 2);
                            }
                            else if (double.TryParse(expr, out double dv))
                            {
                                val = dv;
                            }
                            else
                            {
                                val = expr;
                            }

                            _extVars[name] = val;

                            if (CourseWrite) Courses[CourseCounter - 1].CourseNameChecker(name, val, CourseSeqPrefixes);
                            line.Variables.ExternalValues[name] = val;
                        }
                    }

                    if (CourseWrite) Courses[CourseCounter - 1].Lines.Add(line);
                }
            }
        }


        private Dictionary<string, double> getFeedRate(string raw)
        {
            Dictionary<string, double> feedRate = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            Match m = _feedRtRx.Match(raw);
            if (m.Success)
            {
                string name = m.Groups["name"].Value;
                string expr = m.Groups["expr"].Value;

                // Substitute variables (e.g., P51) with their values
                string repl = Regex.Replace(expr, @"\b\w+\b", id =>
                {
                    string k = id.Value;
                    if (_parameters.ContainsKey(k)) return _parameters[k].ToString();
                    if (_axes.ContainsKey(k)) return _axes[k].ToString();
                    return k;
                });

                feedRate[name] = EvaluateExpression(repl);
            }
            return feedRate;
        }

        // Get Sequence number
        private int? GetSeqNo(string raw)
        {
            Match m = _seqRx.Match(raw);
            if (m.Success)
            {
                return int.Parse(m.Groups["n"].Value);
            }
            return null;
        }

        // Get Commands
        private List<string> GetCommands(string raw)
        {
            MatchCollection mc = _cmdRx.Matches(raw);
            List<string> cmds = new List<string>();
            foreach (Match m in mc)
            {
                //LayupCheck(m.Groups["cmd"].Value);
                cmds.Add(m.Groups["cmd"].Value);
            }
            return cmds;
        }

        // Get comments
        private string GetComment(string raw)
        {
            Match m = _cmtRx.Match(raw);
            if (m.Success) return m.Groups["c"].Value.Trim();
            return null;
        }

        // Evaluacija na "15 + 150" ekspresii
        private double EvaluateExpression(string expr)
        {
            if (double.TryParse(expr, out double s))
                return s;
            try
            {
                // Ovde samo se sobiraat vekje presmetanite ekspresii kako string pa se konvertiraat vo double.
                // No vo sluchaj Course = b (Course bez V.E.), pagja.
                object res = new DataTable().Compute(expr, null);
                return Convert.ToDouble(res);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to evaluate expression '{expr}': {ex.Message}");
            }
        }

        // I don't now why it sits here, but I won't remove it because I'm scared
        internal void WriteAllText(object userSelectedPath, string v)
        {
            throw new NotImplementedException();
        }
    }
}
