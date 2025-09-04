namespace NCFileCompare
{
    partial class GraphForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.AxesGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.AxisPanel = new System.Windows.Forms.Panel();
            this.AxisList = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.AxesGraph)).BeginInit();
            this.AxisPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // AxesGraph
            // 
            chartArea1.Name = "ChartArea1";
            this.AxesGraph.ChartAreas.Add(chartArea1);
            this.AxesGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AxesGraph.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            legend1.Name = "Legend1";
            this.AxesGraph.Legends.Add(legend1);
            this.AxesGraph.Location = new System.Drawing.Point(0, 0);
            this.AxesGraph.Name = "AxesGraph";
            this.AxesGraph.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.AxesGraph.Series.Add(series1);
            this.AxesGraph.Size = new System.Drawing.Size(800, 450);
            this.AxesGraph.TabIndex = 0;
            this.AxesGraph.Text = "AxesGraph";
            // 
            // AxisPanel
            // 
            this.AxisPanel.Controls.Add(this.AxisList);
            this.AxisPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.AxisPanel.Location = new System.Drawing.Point(758, 0);
            this.AxisPanel.Name = "AxisPanel";
            this.AxisPanel.Size = new System.Drawing.Size(42, 450);
            this.AxisPanel.TabIndex = 4;
            // 
            // AxisList
            // 
            this.AxisList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AxisList.FormattingEnabled = true;
            this.AxisList.ItemHeight = 16;
            this.AxisList.Location = new System.Drawing.Point(0, 0);
            this.AxisList.Name = "AxisList";
            this.AxisList.Size = new System.Drawing.Size(42, 450);
            this.AxisList.TabIndex = 4;
            // 
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.AxisPanel);
            this.Controls.Add(this.AxesGraph);
            this.Name = "GraphForm";
            this.Text = "Graph";
            this.Load += new System.EventHandler(this.GraphForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AxesGraph)).EndInit();
            this.AxisPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart AxesGraph;
        private System.Windows.Forms.Panel AxisPanel;
        private System.Windows.Forms.ListBox AxisList;
    }
}