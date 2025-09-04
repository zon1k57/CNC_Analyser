using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace NCFileCompare
{
    partial class MainForm
    {
        private Button btnCompare;
        private DataGridView dgvResults;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnCompare = new System.Windows.Forms.Button();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Settings = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.LimitCheckerBtn = new System.Windows.Forms.Button();
            this.AxesExtremaBtn = new System.Windows.Forms.Button();
            this.SimulatorBtn = new System.Windows.Forms.Button();
            this.GraphBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCompare
            // 
            this.btnCompare.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCompare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCompare.Location = new System.Drawing.Point(5, 5);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(5);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(115, 40);
            this.btnCompare.TabIndex = 5;
            this.btnCompare.Text = "Compare";
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.ColumnHeadersHeight = 29;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.Location = new System.Drawing.Point(0, 0);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.RowHeadersWidth = 51;
            this.dgvResults.Size = new System.Drawing.Size(640, 330);
            this.dgvResults.TabIndex = 6;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // Settings
            // 
            this.Settings.AccessibleName = "ChangeVariables";
            this.Settings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Settings.Location = new System.Drawing.Point(506, 5);
            this.Settings.Margin = new System.Windows.Forms.Padding(5);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(129, 40);
            this.Settings.TabIndex = 11;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.button2_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // LimitCheckerBtn
            // 
            this.LimitCheckerBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LimitCheckerBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LimitCheckerBtn.Location = new System.Drawing.Point(130, 55);
            this.LimitCheckerBtn.Margin = new System.Windows.Forms.Padding(5);
            this.LimitCheckerBtn.Name = "LimitCheckerBtn";
            this.LimitCheckerBtn.Size = new System.Drawing.Size(115, 40);
            this.LimitCheckerBtn.TabIndex = 13;
            this.LimitCheckerBtn.Text = "Limit Checker";
            this.LimitCheckerBtn.UseVisualStyleBackColor = true;
            this.LimitCheckerBtn.Click += new System.EventHandler(this.LimitCheckerBtn_Click);
            // 
            // AxesExtremaBtn
            // 
            this.AxesExtremaBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AxesExtremaBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AxesExtremaBtn.Location = new System.Drawing.Point(5, 55);
            this.AxesExtremaBtn.Margin = new System.Windows.Forms.Padding(5);
            this.AxesExtremaBtn.Name = "AxesExtremaBtn";
            this.AxesExtremaBtn.Size = new System.Drawing.Size(115, 40);
            this.AxesExtremaBtn.TabIndex = 14;
            this.AxesExtremaBtn.Text = "Axes Extrema";
            this.AxesExtremaBtn.UseVisualStyleBackColor = true;
            this.AxesExtremaBtn.Click += new System.EventHandler(this.AxesExtremaBtn_Click);
            // 
            // SimulatorBtn
            // 
            this.SimulatorBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SimulatorBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SimulatorBtn.Location = new System.Drawing.Point(255, 5);
            this.SimulatorBtn.Margin = new System.Windows.Forms.Padding(5);
            this.SimulatorBtn.Name = "SimulatorBtn";
            this.SimulatorBtn.Size = new System.Drawing.Size(115, 40);
            this.SimulatorBtn.TabIndex = 15;
            this.SimulatorBtn.Text = "Simulator";
            this.SimulatorBtn.UseVisualStyleBackColor = true;
            this.SimulatorBtn.Click += new System.EventHandler(this.SimulatorBtn_Click);
            // 
            // GraphBtn
            // 
            this.GraphBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GraphBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GraphBtn.Location = new System.Drawing.Point(130, 5);
            this.GraphBtn.Margin = new System.Windows.Forms.Padding(5);
            this.GraphBtn.Name = "GraphBtn";
            this.GraphBtn.Size = new System.Drawing.Size(115, 40);
            this.GraphBtn.TabIndex = 16;
            this.GraphBtn.Text = "Graph";
            this.GraphBtn.UseVisualStyleBackColor = true;
            this.GraphBtn.Click += new System.EventHandler(this.GraphBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvResults);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(640, 330);
            this.panel1.TabIndex = 17;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonPanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(640, 100);
            this.panel2.TabIndex = 18;
            // 
            // buttonPanel
            // 
            this.buttonPanel.ColumnCount = 5;
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            this.buttonPanel.Controls.Add(this.btnCompare, 0, 0);
            this.buttonPanel.Controls.Add(this.GraphBtn, 1, 0);
            this.buttonPanel.Controls.Add(this.SimulatorBtn, 2, 0);
            this.buttonPanel.Controls.Add(this.AxesExtremaBtn, 0, 1);
            this.buttonPanel.Controls.Add(this.LimitCheckerBtn, 1, 1);
            this.buttonPanel.Controls.Add(this.Settings, 4, 0);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPanel.Location = new System.Drawing.Point(0, 0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.RowCount = 2;
            this.buttonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonPanel.Size = new System.Drawing.Size(640, 100);
            this.buttonPanel.TabIndex = 0;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(640, 430);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NC File Analyse";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        // Add this field to your class
        private TableLayoutPanel buttonPanel;
        private Button Settings;
        private ContextMenuStrip contextMenuStrip1;
        private IContainer components;
        private Button LimitCheckerBtn;
        private Button AxesExtremaBtn;
        private Button SimulatorBtn;
        private Button GraphBtn;
        private Panel panel1;
        private Panel panel2;
    }
}
