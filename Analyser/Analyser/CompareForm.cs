using NCFileCompare.Models;
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
    public partial class CompareForm : Form
    {

        public string File1 { get; private set; }
        public string File2 { get; private set; }
        public double Tolerance { get; set; }

        public CompareForm()
        {
            InitializeComponent();
        }

        private void FeedRateChekbox_CheckedChanged(object sender, EventArgs e)
        {
            GlobalFlags.EnableFeedRateComparison = FeedRateChekbox.Checked;
        }

        private void ExternalCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            GlobalFlags.EnableExternalComparison = ExternalCheckbox.Checked;
        }

        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
            {
                Filter =
               "CNC Files " +
               "(*.nc;*.mnc;*.tap;*.cnc;*.gcode;*.spf;*.src;*.dnc;*.plt;*.pgm;*.iso;*.eia;*.apt;*.cam;*.ngc)" +
               "|*.nc;*.mnc;*.tap;*.cnc;*.gcode;*.spf;*.src;*.dnc;*.plt;*.pgm;*.iso;*.eia;*.apt;*.cam;*.ngc|" + //CNC Files
               "G-code Files (*.nc;*.gcode;*.ngc;*.tap)|*.nc;*.gcode;*.ngc;*.tap|" + //G-Code Files
               "Siemens Files (*.spf;*.mpf)|*.spf;*.mpf|" + //Siemens Files
               "Fanuc/Heidenhain Files (*.pgm;*.h)|*.pgm;*.h|" + //Fanuc/Heidenhain Files
               "All Files (*.*)|*.*"
            }) //All files

                if (dlg.ShowDialog() == DialogResult.OK) txtFile1.Text = dlg.FileName;
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
            {
                Filter =
               "CNC Files " +
               "(*.nc;*.mnc;*.tap;*.cnc;*.gcode;*.spf;*.src;*.dnc;*.plt;*.pgm;*.iso;*.eia;*.apt;*.cam;*.ngc)" +
               "|*.nc;*.mnc;*.tap;*.cnc;*.gcode;*.spf;*.src;*.dnc;*.plt;*.pgm;*.iso;*.eia;*.apt;*.cam;*.ngc|" + //CNC Files
               "G-code Files (*.nc;*.gcode;*.ngc;*.tap)|*.nc;*.gcode;*.ngc;*.tap|" + //G-Code Files
               "Siemens Files (*.spf;*.mpf)|*.spf;*.mpf|" + //Siemens Files
               "Fanuc/Heidenhain Files (*.pgm;*.h)|*.pgm;*.h|" + //Fanuc/Heidenhain Files
               "All Files (*.*)|*.*"
            }) //All files

                if (dlg.ShowDialog() == DialogResult.OK) txtFile2.Text = dlg.FileName;
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            // Error handling tolerance
            if (!double.TryParse(txtTolerance.Text, out double tol))
            {
                MessageBox.Show("Invalid tolerance");
                return;
            }

            // Step 2: Validate file paths
            if (!File.Exists(txtFile1.Text) || !File.Exists(txtFile2.Text))
            {
                MessageBox.Show("Invalid path");
                return;
            }

            File1 = txtFile1.Text;
            File2 = txtFile2.Text;
            Tolerance = tol;

            // Return dialog result so it can continue with comparing
            DialogResult = DialogResult.OK;
            Close();
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {

        }
    }
}
