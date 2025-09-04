using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCFileCompare
{
    public partial class AxesLimitForm : Form
    {
        private List<string> Axes { get; set; }
        private Dictionary<string, (TextBox minBox, TextBox maxBox)> axisControls =
            new Dictionary<string, (TextBox, TextBox)>();

        private string FilePath { get; set; }

        public AxesLimitForm(List<string> axes, string filePath)
        {
            InitializeComponent();
            FilePath = filePath;
            Axes = axes ?? new List<string>();
            this.Load += AxesLimitForm_Load;  // Changed from Load to Shown for reload on each display
        }

        private void AxesLimitForm_Load(object sender, EventArgs e)
        {
            axisControls.Clear();
            panelAxes.Controls.Clear();  // Clear any existing controls to prevent duplicates

            // Load existing limits into a dictionary
            var existingLimits = new Dictionary<string, (string min, string max)>(StringComparer.OrdinalIgnoreCase);
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadAllLines(FilePath))
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        existingLimits[parts[0]] = (parts[1], parts[2]);
                    }
                }
            }

            // Dynamically displaying available axes
            int yOffset = 10;
            foreach (var axis in Axes)
            {
                Label lbl = new Label
                {
                    Text = axis + ":",
                    Location = new Point(5, yOffset + 3),
                    AutoSize = true
                };
                panelAxes.Controls.Add(lbl);

                TextBox minBox = new TextBox
                {
                    Location = new Point(50, yOffset),
                    Width = 60,
                    Text = existingLimits.ContainsKey(axis) ? existingLimits[axis].min : "0"
                };
                panelAxes.Controls.Add(minBox);

                TextBox maxBox = new TextBox
                {
                    Location = new Point(120, yOffset),
                    Width = 60,
                    Text = existingLimits.ContainsKey(axis) ? existingLimits[axis].max : "0"
                };
                panelAxes.Controls.Add(maxBox);

                axisControls[axis] = (minBox, maxBox);

                yOffset += 30;
            }
        }

        public void SaveLimits(string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    foreach (var kvp in axisControls)
                    {
                        string axis = kvp.Key;
                        string min = kvp.Value.minBox.Text;
                        string max = kvp.Value.maxBox.Text;
                        sw.WriteLine($"{axis},{min},{max}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving limits: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Debug: Build a string of current values to show what will be saved
            StringBuilder debugContent = new StringBuilder("Values to be saved:\n");
            foreach (var kvp in axisControls)
            {
                debugContent.AppendLine($"{kvp.Key}: Min={kvp.Value.minBox.Text}, Max={kvp.Value.maxBox.Text}");
            }
            MessageBox.Show(debugContent.ToString(), "Debug: Pre-Save Values", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Saving in .txt file
            SaveLimits(FilePath);
            MessageBox.Show("Limits saved successfully! Check file at: " + FilePath, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}