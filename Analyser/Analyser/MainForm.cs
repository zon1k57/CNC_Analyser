using NCFileCompare;
using NCFileCompare.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NCFileCompare
{
    public partial class MainForm : Form
    {
        private string VariableFolderPath { get; set; }
        public MainForm() => InitializeComponent();

        
        private async void btnCompare_Click(object s, EventArgs e)
        {
            // Open CompareForm as modal dialog
            using (var compareForm = new CompareForm())
            {
                if (string.IsNullOrWhiteSpace(Properties.Settings.Default.LastFolderPath))
                {
                    MessageBox.Show($"Please select a folder");
                    return;
                }
                if (compareForm.ShowDialog() != DialogResult.OK)
                    return; // user canceled

                // ✅ Get values back from CompareForm
                string file1Path = compareForm.File1;
                string file2Path = compareForm.File2;
                double tol = compareForm.Tolerance;

                // Ask user where to save the output (optional - or keep inside CompareForm)
                string userSelectedPath;
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "Save Results As";
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    saveFileDialog.DefaultExt = "txt";
                    saveFileDialog.FileName = "ComparisonResult.txt";

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    userSelectedPath = saveFileDialog.FileName;
                }

                // Disable spam clicking
                btnCompare.Enabled = false;
                dgvResults.Rows.Clear();
                dgvResults.Rows.Add("Working... please wait");

                try
                {
                    var resultText = await Task.Run(() =>
                    {
                        // Parse NC files using returned values
                        var f1 = new NCFile(file1Path, VariableFolderPath);
                        var f2 = new NCFile(file2Path, VariableFolderPath);
                        f1.Parse();
                        f2.Parse();
                        NCUtils.FillMissingSequenceNumbers(f1.Courses);
                        NCUtils.FillMissingSequenceNumbers(f2.Courses);

                        var cmp = new NCComparer(f1, f2, tol, VariableFolderPath);

                        var sb = new StringBuilder();
                        using (var sw = new StreamWriter(userSelectedPath, false, Encoding.UTF8))
                        {
                            // redirect Console.Out to StreamWriter
                            var originalOut = Console.Out;
                            Console.SetOut(sw);

                            cmp.Compare();
                            var printer = new NCPrint(cmp.Result, cmp.DiffMessages);
                            printer.Print();

                            Console.SetOut(originalOut);
                        }

                        // After writing, read result file back into string (for UI display)
                        return File.ReadAllText(userSelectedPath);
                    });

                    // Update UI
                    dgvResults.Rows.Clear();
                    int maxRows = 5000;

                    var lines = resultText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < Math.Min(lines.Length, maxRows); i++)
                        dgvResults.Rows.Add(lines[i]);

                    if (lines.Length > maxRows)
                        dgvResults.Rows.Add($"... (output truncated, full results in {userSelectedPath})");

                    if (GlobalFlags.Error)
                        MessageBox.Show("Error, there was a problem.");
                    else if (!GlobalFlags.Diff)
                        MessageBox.Show("There are no differences in both of the files.");
                    else
                        MessageBox.Show("Done. Results saved to:\n" + userSelectedPath);
                }
                finally
                {
                    btnCompare.Enabled = true;
                }
            }
        }


        // Pre load the last saved folder path when starting app
        private void MainForm_Load(object sender, EventArgs e)
        {
            GlobalFlags.EnableGraph = false;
            // Loads last used folder path 
            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastFolderPath))
            {
                string absPath = PathHelper.ToAbsolute(Properties.Settings.Default.LastFolderPath);
                if (Directory.Exists(absPath))
                {
                    VariableFolderPath = absPath;
                }
            }
        }

       
        // Settings button
        private void button2_Click(object sender, EventArgs e)
        {
            using (var form = new SettingsForm(VariableFolderPath))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    VariableFolderPath = form.variableFilePath;  // ✅ updated from child form
                    if(!string.IsNullOrEmpty(VariableFolderPath))
                        Properties.Settings.Default.LastFolderPath = PathHelper.ToRelative(VariableFolderPath);
                }
            }
        }



        private async void LimitCheckerBtn_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
            {
                Filter =
               "CNC Files " +
               "(*.nc;*.mnc;*.tap;*.cnc;*.gcode;*.spf;*.src;*.dnc;*.plt;*.pgm;*.iso;*.eia;*.apt;*.cam;*.ngc)" +
               "|*.nc;*.mnc;*.tap;*.cnc;*.gcode;*.spf;*.src;*.dnc;*.plt;*.pgm;*.iso;*.eia;*.apt;*.cam;*.ngc|" + // CNC Files
               "G-code Files (*.nc;*.gcode;*.ngc;*.tap)|*.nc;*.gcode;*.ngc;*.tap|" + // G-Code Files
               "Siemens Files (*.spf;*.mpf)|*.spf;*.mpf|" + // Siemens Files
               "Fanuc/Heidenhain Files (*.pgm;*.h)|*.pgm;*.h|" + // Fanuc/Heidenhain Files
               "All Files (*.*)|*.*" // All Files
            })
            {
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                LimitCheckerBtn.Enabled = false;
                dgvResults.Rows.Clear();
                dgvResults.Rows.Add("Working... please wait");

                try
                {
                    var (resultText, inRange) = await Task.Run(() =>
                    {
                        // File path to save results
                        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LimitResult.txt");
                        
                        // Get and parse file
                        var file = new NCFile(dlg.FileName, VariableFolderPath);
                        file.Parse();
                        NCUtils.FillMissingSequenceNumbers(file.Courses);

                        var cmp = new NCComparer(file, VariableFolderPath);

                        var sb = new StringBuilder();
                        bool inRangeLocal = true; // Range checker
                        
                        // Compare limits
                        using (var sw = new StringWriter(sb))
                        {
                            var originalOut = Console.Out;
                            Console.SetOut(sw);
                            try
                            {
                                inRangeLocal = cmp.CheckLimit();
                            }
                            catch(Exception exc)
                            {
                                MessageBox.Show($"{exc.Message}");
                                GlobalFlags.Error = true;                                
                            }
                            Console.SetOut(originalOut);
                        }

                        File.WriteAllText(filePath, sb.ToString());
                        return (sb.ToString(), inRangeLocal);
                    });

                    // Update UI with results (back on UI thread)
                    dgvResults.Rows.Clear();

                    if (!inRange)
                    {
                        // Limit number of displayed rows to keep UI responsive
                        int maxRows = 5000;
                        var lines = resultText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < Math.Min(lines.Length, maxRows); i++)
                        {
                            dgvResults.Rows.Add(lines[i]);
                        }
                        if (lines.Length > maxRows)
                            dgvResults.Rows.Add("... (output truncated, see LimitResult.txt)");

                        MessageBox.Show("⚠️ Limit Violation detected, check results.");
                    }
                    else if (GlobalFlags.Error)
                    {
                        // Empty because the problem is already printed on line 191
                    }
                    else
                    {
                        MessageBox.Show("✅ No Limit Violation");
                    }
                }
                finally
                {
                    LimitCheckerBtn.Enabled = true;
                }
            }
        }


        private async void AxesExtremaBtn_Click(object sender, EventArgs e)
        {
            GlobalFlags.EnableGraph = true;
            using (var dlg = new OpenFileDialog
            {
                Filter =
               "CNC Files " +
               "(*.nc;*.mnc;*.tap;*.cnc;*.gcode;*.spf;*.src;*.dnc;*.plt;*.pgm;*.iso;*.eia;*.apt;*.cam;*.ngc)" +
               "|*.nc;*.mnc;*.tap;*.cnc;*.gcode;*.spf;*.src;*.dnc;*.plt;*.pgm;*.iso;*.eia;*.apt;*.cam;*.ngc|" + //CNC Files
               "G-code Files (*.nc;*.gcode;*.ngc;*.tap)|*.nc;*.gcode;*.ngc;*.tap|" + //G-Code Files
               "Siemens Files (*.spf;*.mpf)|*.spf;*.mpf|" + //Siemens Files
               "Fanuc/Heidenhain Files (*.pgm;*.h)|*.pgm;*.h|" + //Fanuc/Heidenhain Files
               "All Files (*.*)|*.*" //All files
            }) 
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // Disable key during comparison
                    AxesExtremaBtn.Enabled = false;
                    dgvResults.Rows.Clear();
                    dgvResults.Rows.Add("Working... please wait");

                    try
                    {
                        var resultText = await Task.Run(() =>
                        {
                            // File path to save extrema
                            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AxesExtrema.txt");

                            // Get and parse file
                            NCFile File = new NCFile(dlg.FileName, VariableFolderPath);
                            File.Parse();
                            NCUtils.FillMissingSequenceNumbers(File.Courses);

                            NCComparer cmp = new NCComparer(File, VariableFolderPath);
                            
                            // Compare Extrema
                            var sb = new StringBuilder();
                            using (var sw = new StringWriter(sb))
                            {
                                var originalOut = Console.Out;
                                Console.SetOut(sw);
                                cmp.GetAxesExtrema();

                                Console.SetOut(originalOut);
                            }
                            
                            System.IO.File.WriteAllText(filePath, sb.ToString());
                            
                            return sb.ToString();
                        });

                        dgvResults.Rows.Clear();

                        // Display extrema 
                        foreach (var line in resultText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            dgvResults.Rows.Add(line);
                        }
                        MessageBox.Show("Extrema for all axes have been found, check results");
                    }
                    finally
                    {
                        AxesExtremaBtn.Enabled = true;
                    }
                    
                }
        }

        private async void SimulatorBtn_Click(object sender, EventArgs e)
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
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SimulatorBtn.Enabled = false;
                    dgvResults.Rows.Clear();
                    dgvResults.Rows.Add("Working... please wait");

                    try
                    {
                        var result = await Task.Run(()=>
                        {
                            // Get and parse file
                            NCFile File = new NCFile(dlg.FileName, VariableFolderPath);
                            File.Parse();
                            NCUtils.FillMissingSequenceNumbers(File.Courses);

                            return File.Courses;
                        });

                        // Create simulator form and pass parsed file
                        SimulatorForm simulatorForm = new SimulatorForm(result);

                        simulatorForm.Show();
                    }
                    finally
                    {
                        dgvResults.Rows.Clear();
                        SimulatorBtn.Enabled = true;
                    }
                }
                
        }

        private async void GraphBtn_Click(object sender, EventArgs e)
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
               "All Files (*.*)|*.*" //All files
            }) 
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    GraphBtn.Enabled = false;
                    dgvResults.Rows.Clear();
                    dgvResults.Rows.Add("Working... please wait");
                    try
                    {
                        var result = await Task.Run(() =>
                        {
                            // Get and parse file
                            NCFile File = new NCFile(dlg.FileName, VariableFolderPath);
                            File.Parse();
                            NCUtils.FillMissingSequenceNumbers(File.Courses);

                            return File.Courses;
                        });

                        // Create graph form and pass parsed file
                        GraphForm graphForm = new GraphForm(result);
                        graphForm.Show();
                    }
                    finally
                    {
                        dgvResults.Rows.Clear();
                        GraphBtn.Enabled = true;
                    }
                }
        }
    }
}
