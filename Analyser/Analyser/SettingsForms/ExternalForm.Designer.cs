namespace NCFileCompare
{
    partial class ExternalForm
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
            this.ExternalValues = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AddExtBtn = new System.Windows.Forms.Button();
            this.DeleteExtBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExternalValues
            // 
            this.ExternalValues.FormattingEnabled = true;
            this.ExternalValues.ItemHeight = 16;
            this.ExternalValues.Location = new System.Drawing.Point(13, 13);
            this.ExternalValues.Name = "ExternalValues";
            this.ExternalValues.Size = new System.Drawing.Size(345, 100);
            this.ExternalValues.TabIndex = 0;
            this.ExternalValues.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExternalValues_KeyDown);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 119);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(183, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // AddExtBtn
            // 
            this.AddExtBtn.Location = new System.Drawing.Point(202, 119);
            this.AddExtBtn.Name = "AddExtBtn";
            this.AddExtBtn.Size = new System.Drawing.Size(75, 23);
            this.AddExtBtn.TabIndex = 2;
            this.AddExtBtn.Text = "Add";
            this.AddExtBtn.UseVisualStyleBackColor = true;
            this.AddExtBtn.Click += new System.EventHandler(this.AddExtBtn_Click);
            // 
            // DeleteExtBtn
            // 
            this.DeleteExtBtn.Location = new System.Drawing.Point(283, 119);
            this.DeleteExtBtn.Name = "DeleteExtBtn";
            this.DeleteExtBtn.Size = new System.Drawing.Size(75, 23);
            this.DeleteExtBtn.TabIndex = 3;
            this.DeleteExtBtn.Text = "Delete";
            this.DeleteExtBtn.UseVisualStyleBackColor = true;
            this.DeleteExtBtn.Click += new System.EventHandler(this.DeleteExtBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(121, 149);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 4;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(202, 149);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // ExternalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 184);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.DeleteExtBtn);
            this.Controls.Add(this.AddExtBtn);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ExternalValues);
            this.Name = "ExternalForm";
            this.Text = "ExternalForm";
            this.Load += new System.EventHandler(this.ExternalForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ExternalValues;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button AddExtBtn;
        private System.Windows.Forms.Button DeleteExtBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}