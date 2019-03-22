namespace AAPakEditor
{
    partial class AddFileDialog
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
            this.lPakFileName = new System.Windows.Forms.Label();
            this.ePakFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.eDiskFileName = new System.Windows.Forms.TextBox();
            this.btnSearchFile = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // lPakFileName
            // 
            this.lPakFileName.AutoSize = true;
            this.lPakFileName.Location = new System.Drawing.Point(12, 9);
            this.lPakFileName.Name = "lPakFileName";
            this.lPakFileName.Size = new System.Drawing.Size(120, 13);
            this.lPakFileName.TabIndex = 0;
            this.lPakFileName.Text = "FileName inside the pak";
            // 
            // ePakFileName
            // 
            this.ePakFileName.Location = new System.Drawing.Point(15, 25);
            this.ePakFileName.Name = "ePakFileName";
            this.ePakFileName.Size = new System.Drawing.Size(493, 20);
            this.ePakFileName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File on disk to be added";
            // 
            // eDiskFileName
            // 
            this.eDiskFileName.Location = new System.Drawing.Point(15, 64);
            this.eDiskFileName.Name = "eDiskFileName";
            this.eDiskFileName.Size = new System.Drawing.Size(451, 20);
            this.eDiskFileName.TabIndex = 3;
            // 
            // btnSearchFile
            // 
            this.btnSearchFile.Location = new System.Drawing.Point(472, 64);
            this.btnSearchFile.Name = "btnSearchFile";
            this.btnSearchFile.Size = new System.Drawing.Size(36, 23);
            this.btnSearchFile.TabIndex = 4;
            this.btnSearchFile.Text = "...";
            this.btnSearchFile.UseVisualStyleBackColor = true;
            this.btnSearchFile.Click += new System.EventHandler(this.btnSearchFile_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(15, 101);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "Add";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(433, 101);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDlg
            // 
            this.openFileDlg.Filter = "All Files|*.*";
            this.openFileDlg.Title = "Open file for adding";
            // 
            // AddFileDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(520, 136);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSearchFile);
            this.Controls.Add(this.eDiskFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ePakFileName);
            this.Controls.Add(this.lPakFileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AddFileDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add File";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddFileDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lPakFileName;
        public System.Windows.Forms.TextBox ePakFileName;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox eDiskFileName;
        private System.Windows.Forms.Button btnSearchFile;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDlg;
    }
}