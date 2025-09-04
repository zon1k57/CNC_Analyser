namespace NCFileCompare
{
    partial class CompareForm
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
            this.ExternalCheckbox = new System.Windows.Forms.CheckBox();
            this.FeedRateChekbox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFile1 = new System.Windows.Forms.TextBox();
            this.btnBrowse1 = new System.Windows.Forms.Button();
            this.txtFile2 = new System.Windows.Forms.TextBox();
            this.btnBrowse2 = new System.Windows.Forms.Button();
            this.txtTolerance = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // ExternalCheckbox
            // 
            this.ExternalCheckbox.AccessibleName = "ExternalCheckbox";
            this.ExternalCheckbox.AutoSize = true;
            this.ExternalCheckbox.Location = new System.Drawing.Point(309, 121);
            this.ExternalCheckbox.Name = "ExternalCheckbox";
            this.ExternalCheckbox.Size = new System.Drawing.Size(77, 20);
            this.ExternalCheckbox.TabIndex = 25;
            this.ExternalCheckbox.Text = "External";
            this.ExternalCheckbox.UseVisualStyleBackColor = true;
            this.ExternalCheckbox.CheckedChanged += new System.EventHandler(this.ExternalCheckbox_CheckedChanged);
            // 
            // FeedRateChekbox
            // 
            this.FeedRateChekbox.AutoSize = true;
            this.FeedRateChekbox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FeedRateChekbox.Location = new System.Drawing.Point(213, 121);
            this.FeedRateChekbox.Name = "FeedRateChekbox";
            this.FeedRateChekbox.Size = new System.Drawing.Size(90, 20);
            this.FeedRateChekbox.TabIndex = 24;
            this.FeedRateChekbox.Text = "FeedRate";
            this.FeedRateChekbox.UseVisualStyleBackColor = true;
            this.FeedRateChekbox.CheckedChanged += new System.EventHandler(this.FeedRateChekbox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 23;
            this.label2.Text = "Tolerance:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 16);
            this.label1.TabIndex = 22;
            this.label1.Text = "Select two NC Files to Compare";
            // 
            // txtFile1
            // 
            this.txtFile1.Location = new System.Drawing.Point(12, 41);
            this.txtFile1.Name = "txtFile1";
            this.txtFile1.Size = new System.Drawing.Size(300, 22);
            this.txtFile1.TabIndex = 16;
            // 
            // btnBrowse1
            // 
            this.btnBrowse1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse1.Location = new System.Drawing.Point(318, 41);
            this.btnBrowse1.Name = "btnBrowse1";
            this.btnBrowse1.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse1.TabIndex = 17;
            this.btnBrowse1.Text = "Browse...";
            this.btnBrowse1.Click += new System.EventHandler(this.btnBrowse1_Click);
            // 
            // txtFile2
            // 
            this.txtFile2.Location = new System.Drawing.Point(12, 69);
            this.txtFile2.Name = "txtFile2";
            this.txtFile2.Size = new System.Drawing.Size(300, 22);
            this.txtFile2.TabIndex = 18;
            // 
            // btnBrowse2
            // 
            this.btnBrowse2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse2.Location = new System.Drawing.Point(318, 70);
            this.btnBrowse2.Name = "btnBrowse2";
            this.btnBrowse2.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse2.TabIndex = 19;
            this.btnBrowse2.Text = "Browse...";
            this.btnBrowse2.Click += new System.EventHandler(this.btnBrowse2_Click);
            // 
            // txtTolerance
            // 
            this.txtTolerance.Location = new System.Drawing.Point(12, 119);
            this.txtTolerance.Name = "txtTolerance";
            this.txtTolerance.Size = new System.Drawing.Size(100, 22);
            this.txtTolerance.TabIndex = 20;
            this.txtTolerance.Text = "0.003";
            // 
            // btnCompare
            // 
            this.btnCompare.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCompare.Location = new System.Drawing.Point(118, 119);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(89, 23);
            this.btnCompare.TabIndex = 21;
            this.btnCompare.Text = "Compare";
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            this.fileSystemWatcher1.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Changed);
            // 
            // CompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 159);
            this.Controls.Add(this.ExternalCheckbox);
            this.Controls.Add(this.FeedRateChekbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFile1);
            this.Controls.Add(this.btnBrowse1);
            this.Controls.Add(this.txtFile2);
            this.Controls.Add(this.btnBrowse2);
            this.Controls.Add(this.txtTolerance);
            this.Controls.Add(this.btnCompare);
            this.Name = "CompareForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CompareForm";
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox ExternalCheckbox;
        private System.Windows.Forms.CheckBox FeedRateChekbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFile1;
        private System.Windows.Forms.Button btnBrowse1;
        private System.Windows.Forms.TextBox txtFile2;
        private System.Windows.Forms.Button btnBrowse2;
        private System.Windows.Forms.TextBox txtTolerance;
        private System.Windows.Forms.Button btnCompare;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}