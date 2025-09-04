using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCFileCompare
{
    public partial class ExternalForm : Form
    {
        private List<string> VariablesByLine = new List<string>();
        private string VariableFilePath { get; set; }

        public ExternalForm(string variableFilePath)
        {
            VariableFilePath = variableFilePath;
            InitializeComponent();
        }

        // Load exception variables from start if there are any.
        private void ExternalForm_Load(object sender, EventArgs e)
        {
            LoadVariables();
            DisplayVariables();
        }

        private void LoadVariables()
        {
            VariablesByLine.Clear();
            string[] lines = File.ReadAllLines(VariableFilePath);

            foreach (string line in lines)
            {
                VariablesByLine.Add(line.Trim());
            }
        }

        private void DisplayVariables()
        {
            ExternalValues.Items.Clear();
            foreach (string line in VariablesByLine)
            {
                ExternalValues.Items.Add(line);
            }
        }

        // Put a variable in ExternalExceptions.txt
        private void SaveVariable()
        {
            string extVar = textBox1.Text.ToString();

            if (string.IsNullOrEmpty(extVar))
            {
                MessageBox.Show("Input cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (IsCyrillic(extVar))
            {
                MessageBox.Show("The text must be latin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!VariablesByLine.Contains(extVar)) VariablesByLine.Add(extVar);
            textBox1.Clear();
        }

        // Saves the new changes
        private void SaveFile()
        {
            var sb = new StringBuilder();
            foreach (var item in VariablesByLine)
            {
                sb.AppendLine(item);
            }
            File.WriteAllText(VariableFilePath, sb.ToString());
        }

        private void AddExtBtn_Click(object sender, EventArgs e)
        {
            SaveVariable();
            DisplayVariables();
        }

        private void DeleteExtBtn_Click(object sender, EventArgs e)
        {
            int index = ExternalValues.SelectedIndex;
            if (index >= 0)
            {
                VariablesByLine.RemoveAt(index);
                DisplayVariables();
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveFile();
            MessageBox.Show("File Saved");
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Adding variable by pressing ENTER
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.AddExtBtn_Click(sender, EventArgs.Empty);
                e.SuppressKeyPress = true;
                textBox1.Clear();
            }
        }

        // Delete specific variable by pressing DELETE key
        private void ExternalValues_KeyDown(object sender, KeyEventArgs e)
        {
            int index = ExternalValues.SelectedIndex;
            if (e.KeyCode == Keys.Delete)
            {
                VariablesByLine.RemoveAt(index);
                DisplayVariables();
            }
        }

        // Checks if text is cyrillic
        bool IsCyrillic(string input)
        {
            return Regex.IsMatch(input, @"^\p{IsCyrillic}+$");
        }

    }
}
