using NCFileCompare.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.LinkLabel;

public static class NCUtils
{
    // Fills missing sequences
    public static void FillMissingSequenceNumbers(List<NCCourse> courses)
    {
        int currentN = 0;
        int courseCounter = 1;

        foreach (var course in courses)
        {
            var lines = course.Lines;
            

            foreach (var line in lines)
            {
                var match = Regex.Match(line.RawLine, @"N(\d+)", RegexOptions.IgnoreCase); //Extract N
                if (match.Success)
                {
                    currentN = int.Parse(match.Groups[1].Value);
                    
                    // Checks if the sequence is a course sequence
                    if (courseCounter == currentN)
                    {
                        currentN -= courseCounter;
                        courseCounter++;
                    }
                    line.SeqNo = currentN;
                }
                else
                {
                    currentN += 10;
                    line.SeqNo = currentN;

                }
            } // Foreach line end
        } // Foreach course end
        
    }

    // Axis Limit Parse
    public static Dictionary<string,(int, int)> ParseAxesLimit(string filepath)
    {
        Dictionary<string, (int, int)> temp = new Dictionary<string, (int, int)>();
        foreach(var line in File.ReadAllLines(filepath))
        {
            var parts = line.Split(',');
            temp[parts[0]] = (int.Parse(parts[1]), int.Parse(parts[2]));
        }
        return temp;
    }

    // Axis Extrema find
    public static (double min, double max) findAxesExtrema(NCLine line1, string axis,
        Dictionary<string, (double min, double max)> temp)
    {
         bool has1 = line1.Variables.Axes.TryGetValue(axis, out double v1Axis);
      

        // Adds axis if it does't exist in Extrema
        if (!temp.Keys.Contains(axis))
        {
            temp[axis] = (99999, -99999);
            
        }

        // Comparing Extrema
        var current = temp[axis];

        
        if (current.min > v1Axis)
        {
            current.min = v1Axis;
        }
        if (current.max < v1Axis)
        {
            current.max = v1Axis;
        }
        
        
        return current;
    }

    public static List<NCVisualization_Move> ExtractMoves(NCCourse course)
    {
        var moves = new List<NCVisualization_Move>();

        // Initialize current position for all possible axes to 0
        Dictionary<string, double> currentPos = new Dictionary<string, double>();

        foreach (var line in course.Lines)
        {
            if (line.IsMove && line.Variables != null && line.Variables.Axes?.Any() == true)
            {
                // Create a copy of current positions to modify for this move
                Dictionary<string, double> newPos = new Dictionary<string, double>(currentPos);

                // Update all axes present in the command
                foreach (var axis in line.Variables.Axes)
                {
                    // Convert from hundredths (assuming same conversion for all axes)
                    newPos[axis.Key] = axis.Value / 100.0;
                }

                // Detect G command
                string gCmd = line.Command.FirstOrDefault(c => c.StartsWith("G")) ?? "G01";

                // Extract feedrate
                double feed = line.Variables.FeedRate?.Values.FirstOrDefault() ?? 0.0;

                moves.Add(new NCVisualization_Move(
                    new Dictionary<string, double>(currentPos), // Clone current state
                    new Dictionary<string, double>(newPos),    // Clone new state
                    gCmd,
                    feed
                ));

                // Update current position to new state
                currentPos = newPos;
            }
        }

        return moves;
    }


    public static void InitGraphs(Chart AxesGraph)
    {
        var chartArea = new ChartArea("MainArea");
        chartArea.AxisX.Title = "NC Line";
        chartArea.AxisY.Title = "Value (Axis/Param)";
        AxesGraph.ChartAreas.Add(chartArea);
    }
}
