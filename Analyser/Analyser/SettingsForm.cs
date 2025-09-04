using NCFileCompare.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCFileCompare
{
    public partial class SettingsForm : Form
    {

        public string variableFilePath;

        public SettingsForm(string variableFilePath)
        {
            this.variableFilePath = variableFilePath;
            InitializeComponent();
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new OpenFileDialog())
            {
                folderDialog.CheckFileExists = false;      // allow entering folder only
                folderDialog.ValidateNames = false;        // let user paste folder path
                folderDialog.FileName = "Select Folder";   // dummy filename

                // Load the last saved folder path if it exists and is valid
                MessageBox.Show(Properties.Settings.Default.LastFolderPath);
                string lastFolderPath = Properties.Settings.Default.LastFolderPath;
                if (!string.IsNullOrEmpty(lastFolderPath) && Directory.Exists(lastFolderPath))
                {
                    folderDialog.InitialDirectory = lastFolderPath;
                }

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.FileName;

                    // If the user pasted a folder and pressed Enter, FileName will be the folder itself
                    // Otherwise, FileName will be "Select Folder" -> get its directory
                    string selectedFolder;
                    if (Directory.Exists(selectedPath))
                        selectedFolder = selectedPath;
                    else
                        selectedFolder = Path.GetDirectoryName(selectedPath);

                    if (!File.Exists(Path.Combine(selectedFolder, "Axes.txt")))
                    {
                        MessageBox.Show("Axes.txt is missing");
                        return;
                    }
                    else if (!File.Exists(Path.Combine(selectedFolder, "ExternalExceptions.txt")))
                    {
                        MessageBox.Show("ExternalExceptions.txt is missing");
                        return;
                    }
                    else if (!File.Exists(Path.Combine(selectedFolder, "Limits.txt")))
                    {
                        MessageBox.Show("Limits.txt is missing");
                        return;
                    }

                    if (Directory.Exists(selectedFolder))
                    {
                        // Save main folder for use
                        variableFilePath = selectedFolder;

                        // Save folder for next time
                        Properties.Settings.Default.LastFolderPath = PathHelper.ToRelative(selectedFolder);
                        try
                        {
                            Properties.Settings.Default.Save();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error saving settings: " + ex.Message);
                        }
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ExternalSettingsBtn_Click(object sender, EventArgs e)
        {
            ExternalForm externalForm = new ExternalForm(Path.Combine(variableFilePath, "ExternalExceptions.txt"));

            externalForm.Show();
        }

        private void AxesLimit_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(variableFilePath, "Limits.txt");
            AxesLimitForm axesLimitForm = new AxesLimitForm(ReadAxesFromFile(variableFilePath), filePath);

            axesLimitForm.Show();
        }

        private List<string> ReadAxesFromFile(string filePath)
        {
            filePath = Path.Combine(filePath, "Axes.txt");
            List<string> axes = new List<string>();
            bool inAxesSection = false;

            foreach (string line in File.ReadAllLines(filePath))
            {
                string trimmed = line.Trim();

                if (string.IsNullOrEmpty(trimmed))
                    continue;

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    inAxesSection = trimmed.Equals("[Axes]", StringComparison.OrdinalIgnoreCase);
                    continue;
                }

                if (inAxesSection)
                {
                    if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                        break;

                    axes.Add(trimmed);
                }
            }

            return axes;
        }

        private void AnPBtn_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(variableFilePath, "Axes.txt");

            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Axes.txt not found in " + variableFilePath, "File Not Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CourseAliasBtn_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(variableFilePath, "CourseAliases.txt");

            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Axes.txt not found in " + variableFilePath, "File Not Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
