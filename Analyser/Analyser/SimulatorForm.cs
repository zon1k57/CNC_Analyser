using NCFileCompare.Models;
using System;
using System.Windows.Forms.Integration;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace NCFileCompare
{
    public partial class SimulatorForm : Form
    {
        private HelixViewport3D helixView;
        private List<NCCourse> Courses { get; set; }

        // Animation state
        private readonly List<NCVisualization_Move> _movesFlat = new List<NCVisualization_Move>();
        private readonly List<LinesVisual3D> _drawnSegments = new List<LinesVisual3D>();
        private int _moveIndex = 0;
        private Timer _animTimer;

        // Visuals
        private SphereVisual3D _tool;

        // UI toolbar
        private FlowLayoutPanel _toolbar;
        private Button _btnPlay, _btnPause, _btnStep, _btnReset;
        private NumericUpDown _nudSpeedMs, _nudSteps;

        public SimulatorForm(List<NCCourse> courses)
        {
            InitializeComponent();
            Courses = courses;

            // --- WPF viewport inside WinForms
            helixView = new HelixViewport3D
            {
                ShowCoordinateSystem = true,
                ShowViewCube = true
            };

            // Camera
            var Position = new Point3D(0, 0, 100);
            var lookDirection = new Vector3D(0, 0, -1);
            var upDirection = new Vector3D(0, 1, 0);
            var FoV = 45;

            helixView.Camera = new PerspectiveCamera(
                Position,
                lookDirection,
                upDirection,
                FoV);

            // Light
            helixView.Children.Add(new SunLight());

            // Grid (XY plane)
            var grid = new GridLinesVisual3D
            {
                Width = 150,
                Length = 150,
                MajorDistance = 10,
                Thickness = 0.1,
                Center = new Point3D(0, 0, 0),
                Normal = new Vector3D(0, 0, 1)
            };
            helixView.Children.Add(grid);

            // Tool sphere (current tool position)
            _tool = new SphereVisual3D
            {
                Radius = 0.8,
                Center = new Point3D(0, 0, 0),
                Material = Materials.Orange
            };
            helixView.Children.Add(_tool);

            // Host WPF in WinForms
            var elementHost = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = helixView
            };

            // Toolbar (WinForms)
            BuildToolbar();

            // Layout: toolbar on top, viewport fills the rest
            Controls.Add(elementHost);
            Controls.Add(_toolbar);

            // Prepare moves
            var nestedMoves = new List<List<NCVisualization_Move>>();
            foreach (var course in Courses)
                nestedMoves.Add(NCUtils.ExtractMoves(course));

            _movesFlat.AddRange(nestedMoves.SelectMany(m => m));

            // Timer
            _animTimer = new Timer { Interval = (int)_nudSpeedMs.Value };
            _animTimer.Tick += (s, e) => Advance((int)_nudSteps.Value);
        }

        private void BuildToolbar()
        {
            _toolbar = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 42,
                Padding = new Padding(6, 6, 6, 6),
                AutoSize = false
            };

            _btnPlay = new Button { Text = "Play" };
            _btnPause = new Button { Text = "Pause" };
            _btnStep = new Button { Text = "Step" };
            _btnReset = new Button { Text = "Reset" };

            _nudSpeedMs = new NumericUpDown
            {
                Minimum = 10,
                Maximum = 2000,
                Value = 80,     // ms per tick
                Increment = 10,
                Width = 70
            };
            var lblSpeed = new Label { Text = "ms/step:", AutoSize = true, Padding = new Padding(10, 8, 5, 0) };

            _nudSteps = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 1000,
                Value = 1,      // moves per tick
                Width = 60
            };
            var lblSteps = new Label { Text = "steps/tick:", AutoSize = true, Padding = new Padding(10, 8, 5, 0) };

            _btnPlay.Click += (s, e) => { _animTimer.Interval = (int)_nudSpeedMs.Value; _animTimer.Start(); };
            _btnPause.Click += (s, e) => _animTimer.Stop();
            _btnStep.Click += (s, e) => Advance((int)_nudSteps.Value);
            _btnReset.Click += (s, e) => ResetAnimation();
            _nudSpeedMs.ValueChanged += (s, e) => _animTimer.Interval = (int)_nudSpeedMs.Value;

            _toolbar.Controls.Add(_btnPlay);
            _toolbar.Controls.Add(_btnPause);
            _toolbar.Controls.Add(_btnStep);
            _toolbar.Controls.Add(_btnReset);
            _toolbar.Controls.Add(lblSpeed);
            _toolbar.Controls.Add(_nudSpeedMs);
            _toolbar.Controls.Add(lblSteps);
            _toolbar.Controls.Add(_nudSteps);
        }

        private void Advance(int steps)
        {
            // Draw N segments per tick
            for (int i = 0; i < steps; i++)
            {
                if (_moveIndex >= _movesFlat.Count)
                {
                    _animTimer.Stop();
                    return;
                }

                var move = _movesFlat[_moveIndex++];

                // Extract coordinates (default 0 if missing)
                Point3D start = ToPoint3D(move.StartPosition);
                Point3D end = ToPoint3D(move.EndPosition);

                // Draw the segment
                var seg = new LinesVisual3D
                {
                    Thickness = 2,
                    Color = string.Equals(move.GCommand, "G00", StringComparison.OrdinalIgnoreCase)
                            ? Colors.Yellow   // rapid
                            : Colors.Red      // feed
                };
                seg.Points.Add(start);
                seg.Points.Add(end);
                helixView.Children.Add(seg);
                _drawnSegments.Add(seg);

                // Move the tool sphere to the end
                _tool.Center = end;
            }
        }

        private void ResetAnimation()
        {
            _animTimer.Stop();
            _moveIndex = 0;

            // Remove only the segments we added; keep grid, light, tool, etc.
            foreach (var seg in _drawnSegments)
                helixView.Children.Remove(seg);
            _drawnSegments.Clear();

            _tool.Center = new Point3D(0, 0, 0);
        }

        private static Point3D ToPoint3D(Dictionary<string, double> axes)
        {
            if (axes == null) return new Point3D(0, 0, 0);

            double x = axes.TryGetValue("X", out var xv) ? xv : 0.0;
            double y = axes.TryGetValue("Y", out var yv) ? yv : 0.0;
            double z = axes.TryGetValue("Z", out var zv) ? zv : 0.0;
            return new Point3D(x, y, z);
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
            // Optional: auto-start
            // _animTimer.Start();
        }
    }
}
