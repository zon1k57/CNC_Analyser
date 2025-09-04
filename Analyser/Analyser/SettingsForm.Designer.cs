using System;
using System.Drawing;
using System.Windows.Forms;

namespace NCFileCompare
{
    partial class SettingsForm
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
            this.butonClose = new System.Windows.Forms.Button();
            this.ExternalSettingsBtn = new System.Windows.Forms.Button();
            this.AxesLimit = new System.Windows.Forms.Button();
            this.AnPBtn = new System.Windows.Forms.Button();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // butonClose
            // 
            this.butonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.butonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.butonClose.Location = new System.Drawing.Point(180, 324);
            this.butonClose.Name = "butonClose";
            this.butonClose.Size = new System.Drawing.Size(180, 39);
            this.butonClose.TabIndex = 13;
            this.butonClose.Text = "Close";
            this.butonClose.UseVisualStyleBackColor = true;
            this.butonClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // ExternalSettingsBtn
            // 
            this.ExternalSettingsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExternalSettingsBtn.Location = new System.Drawing.Point(366, 41);
            this.ExternalSettingsBtn.Name = "ExternalSettingsBtn";
            this.ExternalSettingsBtn.Size = new System.Drawing.Size(178, 37);
            this.ExternalSettingsBtn.TabIndex = 14;
            this.ExternalSettingsBtn.Text = "External Exceptions";
            this.ExternalSettingsBtn.UseVisualStyleBackColor = true;
            this.ExternalSettingsBtn.Click += new System.EventHandler(this.ExternalSettingsBtn_Click);
            // 
            // AxesLimit
            // 
            this.AxesLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AxesLimit.Location = new System.Drawing.Point(366, 84);
            this.AxesLimit.Name = "AxesLimit";
            this.AxesLimit.Size = new System.Drawing.Size(178, 34);
            this.AxesLimit.TabIndex = 15;
            this.AxesLimit.Text = "Axes Limit";
            this.AxesLimit.UseVisualStyleBackColor = true;
            this.AxesLimit.Click += new System.EventHandler(this.AxesLimit_Click);
            // 
            // AnPBtn
            // 
            this.AnPBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnPBtn.Location = new System.Drawing.Point(366, 3);
            this.AnPBtn.Name = "AnPBtn";
            this.AnPBtn.Size = new System.Drawing.Size(178, 32);
            this.AnPBtn.TabIndex = 16;
            this.AnPBtn.Text = "Axes and Parameters";
            this.AnPBtn.UseVisualStyleBackColor = true;
            this.AnPBtn.Click += new System.EventHandler(this.AnPBtn_Click);
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseBtn.Location = new System.Drawing.Point(366, 204);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(178, 34);
            this.BrowseBtn.TabIndex = 17;
            this.BrowseBtn.Text = "Browse Folder";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.78706F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.21294F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.AnPBtn, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.ExternalSettingsBtn, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.AxesLimit, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.BrowseBtn, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.butonClose, 1, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.92308F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.07692F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(547, 366);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 366);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Button butonClose;
        private Button ExternalSettingsBtn;
        private Button AxesLimit;
        private Button AnPBtn;
        private Button BrowseBtn;
        private TableLayoutPanel tableLayoutPanel1;
    }
}