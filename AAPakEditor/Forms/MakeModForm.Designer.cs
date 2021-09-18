namespace AAPakEditor
{
    partial class MakeModForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MakeModForm));
            this.btnCreateMod = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbCreateSFX = new System.Windows.Forms.CheckBox();
            this.tDescription = new System.Windows.Forms.TextBox();
            this.modIcon = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            this.openImageDlg = new System.Windows.Forms.OpenFileDialog();
            this.btnClearIcon = new System.Windows.Forms.Button();
            this.btnDefaultIcon = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.modIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreateMod
            // 
            this.btnCreateMod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateMod.Location = new System.Drawing.Point(12, 240);
            this.btnCreateMod.Name = "btnCreateMod";
            this.btnCreateMod.Size = new System.Drawing.Size(75, 23);
            this.btnCreateMod.TabIndex = 0;
            this.btnCreateMod.Text = "Create Mod";
            this.btnCreateMod.UseVisualStyleBackColor = true;
            this.btnCreateMod.Click += new System.EventHandler(this.BtnCreateMod_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(416, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbCreateSFX
            // 
            this.cbCreateSFX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbCreateSFX.AutoSize = true;
            this.cbCreateSFX.Checked = true;
            this.cbCreateSFX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCreateSFX.Location = new System.Drawing.Point(93, 244);
            this.cbCreateSFX.Name = "cbCreateSFX";
            this.cbCreateSFX.Size = new System.Drawing.Size(213, 17);
            this.cbCreateSFX.TabIndex = 2;
            this.cbCreateSFX.Text = "Create self-installing executable mod file";
            this.cbCreateSFX.UseVisualStyleBackColor = true;
            // 
            // tDescription
            // 
            this.tDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.tDescription.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tDescription.Location = new System.Drawing.Point(146, 28);
            this.tDescription.Multiline = true;
            this.tDescription.Name = "tDescription";
            this.tDescription.Size = new System.Drawing.Size(345, 206);
            this.tDescription.TabIndex = 5;
            this.tDescription.Text = "AAMod file\r\n-----------\r\n\r\nFile generated by AAPakEditor.\r\n";
            // 
            // modIcon
            // 
            this.modIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.modIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.modIcon.Location = new System.Drawing.Point(12, 28);
            this.modIcon.Name = "modIcon";
            this.modIcon.Size = new System.Drawing.Size(128, 128);
            this.modIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.modIcon.TabIndex = 7;
            this.modIcon.TabStop = false;
            this.modIcon.Click += new System.EventHandler(this.ModIcon_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Mod Icon (click to change)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Mod description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 26);
            this.label3.TabIndex = 10;
            this.label3.Text = "image expects\r\n128 x128 pixels";
            // 
            // saveFileDlg
            // 
            this.saveFileDlg.Filter = "Executeable Files|*.exe|AAMod files|*.aamod|All files|*.*";
            this.saveFileDlg.RestoreDirectory = true;
            // 
            // openImageDlg
            // 
            this.openImageDlg.Filter = "Supported Image Files|*.png;*.jpg|All files|*.*";
            this.openImageDlg.RestoreDirectory = true;
            // 
            // btnClearIcon
            // 
            this.btnClearIcon.Location = new System.Drawing.Point(12, 162);
            this.btnClearIcon.Name = "btnClearIcon";
            this.btnClearIcon.Size = new System.Drawing.Size(59, 23);
            this.btnClearIcon.TabIndex = 11;
            this.btnClearIcon.Text = "Clear";
            this.btnClearIcon.UseVisualStyleBackColor = true;
            this.btnClearIcon.Click += new System.EventHandler(this.BtnClearIcon_Click);
            // 
            // btnDefaultIcon
            // 
            this.btnDefaultIcon.Location = new System.Drawing.Point(77, 162);
            this.btnDefaultIcon.Name = "btnDefaultIcon";
            this.btnDefaultIcon.Size = new System.Drawing.Size(63, 23);
            this.btnDefaultIcon.TabIndex = 12;
            this.btnDefaultIcon.Text = "Default";
            this.btnDefaultIcon.UseVisualStyleBackColor = true;
            this.btnDefaultIcon.Click += new System.EventHandler(this.ButtonDefaultIcon_Click);
            // 
            // MakeModForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 275);
            this.Controls.Add(this.btnDefaultIcon);
            this.Controls.Add(this.btnClearIcon);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.modIcon);
            this.Controls.Add(this.tDescription);
            this.Controls.Add(this.cbCreateSFX);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreateMod);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MakeModForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create AAMod file or installer";
            this.Load += new System.EventHandler(this.MakeModForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.modIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateMod;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbCreateSFX;
        private System.Windows.Forms.PictureBox modIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SaveFileDialog saveFileDlg;
        private System.Windows.Forms.OpenFileDialog openImageDlg;
        private System.Windows.Forms.Button btnClearIcon;
        private System.Windows.Forms.Button btnDefaultIcon;
    }
}