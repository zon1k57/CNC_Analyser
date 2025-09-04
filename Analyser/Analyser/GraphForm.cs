using NCFileCompare.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Shapes;

namespace NCFileCompare
{
    public partial class GraphForm : Form
    {
        public List<NCCourse> Courses { get; set; }
        public GraphForm(List<NCCourse> courses)
        {
            Courses = courses;
            InitializeComponent();

            AxesGraph.Series.Clear();
            AxesGraph.ChartAreas.Clear();

            NCUtils.InitGraphs(AxesGraph);

            // Enable mouse wheel zoom
            AxesGraph.MouseWheel += AxesGraph_MouseWheel;
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
            var allAxes = Courses
                .SelectMany(c => c.Lines)
                .SelectMany(l => l.Variables.Axes.Keys)
                .Distinct()
                .ToList();

            AxisList.Items.Clear();

            
            foreach(var axis in allAxes)
            {
                AxisList.Items.Add(axis);
            }

            // Attach double-click handler once
            AxisList.DoubleClick += AxisList_DoubleClick;

            // Enable drag-to-zoom (can also be set in Properties window)
            var ca = AxesGraph.ChartAreas[0];
            ca.AxisX.ScaleView.Zoomable = true;
            ca.AxisY.ScaleView.Zoomable = true;
            ca.CursorX.IsUserEnabled = true;
            ca.CursorX.IsUserSelectionEnabled = true;
            ca.CursorY.IsUserEnabled = true;
            ca.CursorY.IsUserSelectionEnabled = true;
        }

        private void AxisList_DoubleClick(object sender, EventArgs e)
        {
            if (AxisList.SelectedItem != null)
            {
                string selectedAxis = AxisList.SelectedItem.ToString();

                // Call your function directly with the axis
                PlotAxis(selectedAxis);
            }
        }

        // Refactor your plotting logic into a reusable method
        private void PlotAxis(string axisToCheck)
        {
            AxesGraph.Series.Clear();
            AxesGraph.ChartAreas.Clear();
            

            NCUtils.InitGraphs(AxesGraph);

            double? lastValue = null;
            int? lastSeq = null;
            bool? lastLayup = null;

            Series series = null;

            
            int seriesCounter = 0;

            foreach (var course in Courses)
            {
                foreach (var line in course.Lines)
                {
                    if (line.Variables.Axes.TryGetValue(axisToCheck, out double vAxis))
                    {
                        bool isLayup = line.IsLayup;


                        // if state changes OR no series yet → start a new segment
                        if (series == null || lastLayup != isLayup)
                        {
                            // finalize previous
                            if (series != null && series.Points.Count > 0)
                                AxesGraph.Series.Add(series);

                            // create new series with unique name
                            string seriesName = (isLayup ? "Layup" : "Normal") + "_" + seriesCounter++;
                            series = new Series(seriesName)
                            {
                                ChartType = SeriesChartType.Line,
                                BorderWidth = 2,
                                Color = isLayup ? Color.Red : Color.Yellow,
                                IsVisibleInLegend =
                                    (isLayup && AxesGraph.Series.All(s => !s.Name.StartsWith("Layup"))) ||
                                    (!isLayup && AxesGraph.Series.All(s => !s.Name.StartsWith("Normal")))
                            };

                            // connect continuity
                            if (lastValue.HasValue && lastSeq.HasValue)
                                series.Points.AddXY(lastSeq.Value, lastValue.Value);
                        }

                        // add this point
                        series.Points.AddXY(line.SeqNo, vAxis);

                        // remember state
                        lastValue = vAxis;
                        lastSeq = line.SeqNo;
                        lastLayup = isLayup;
                    }
                }
            }

            // add last segment
            if (series != null && series.Points.Count > 0)
                AxesGraph.Series.Add(series);
        }

        /// <summary>
        /// Handles zooming with the mouse wheel
        /// </summary>
        private void AxesGraph_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                var chart = (Chart)sender;
                var ca = chart.ChartAreas[0];
                var xAxis = ca.AxisX;
                var yAxis = ca.AxisY;

                if (e.Delta < 0) // Scroll down = zoom out
                {
                    xAxis.ScaleView.ZoomReset();
                    yAxis.ScaleView.ZoomReset();
                }
                else if (e.Delta > 0) // Scroll up = zoom in
                {
                    double xMin = xAxis.ScaleView.ViewMinimum;
                    double xMax = xAxis.ScaleView.ViewMaximum;
                    double yMin = yAxis.ScaleView.ViewMinimum;
                    double yMax = yAxis.ScaleView.ViewMaximum;

                    double posX = xAxis.PixelPositionToValue(e.Location.X);
                    double posY = yAxis.PixelPositionToValue(e.Location.Y);

                    double zoomFactor = 0.8; // 80% zoom
                    double newXMin = posX - (posX - xMin) * zoomFactor;
                    double newXMax = posX + (xMax - posX) * zoomFactor;
                    double newYMin = posY - (posY - yMin) * zoomFactor;
                    double newYMax = posY + (yMax - posY) * zoomFactor;

                    xAxis.ScaleView.Zoom(newXMin, newXMax);
                    yAxis.ScaleView.Zoom(newYMin, newYMax);
                }
            }
            catch
            {
                // ignore zoom errors
            }
        }

    }
}
