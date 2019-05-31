namespace AAPakEditor
{
    partial class ImportFolderDlg
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
            this.pbImport = new System.Windows.Forms.ProgressBar();
            this.lInfo = new System.Windows.Forms.Label();
            this.bgwImport = new System.ComponentModel.BackgroundWorker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lPakFolder = new System.Windows.Forms.Label();
            this.lDiskFolder = new System.Windows.Forms.Label();
            this.ePakFolder = new System.Windows.Forms.TextBox();
            this.eDiskFolder = new System.Windows.Forms.TextBox();
            this.btnSearchFolder = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.sourceFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // pbImport
            // 
            this.pbImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbImport.Location = new System.Drawing.Point(12, 109);
            this.pbImport.Name = "pbImport";
            this.pbImport.Size = new System.Drawing.Size(453, 23);
            this.pbImport.TabIndex = 0;
            // 
            // lInfo
            // 
            this.lInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lInfo.AutoSize = true;
            this.lInfo.Location = new System.Drawing.Point(12, 138);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(281, 13);
            this.lInfo.TabIndex = 1;
            this.lInfo.Text = "Select Target and Source Folders, when done, press Start";
            // 
            // bgwImport
            // 
            this.bgwImport.WorkerReportsProgress = true;
            this.bgwImport.WorkerSupportsCancellation = true;
            this.bgwImport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwImport_DoWork);
            this.bgwImport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwImport_ProgressChanged);
            this.bgwImport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwImport_RunWorkerCompleted);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(472, 109);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lPakFolder
            // 
            this.lPakFolder.AutoSize = true;
            this.lPakFolder.Location = new System.Drawing.Point(9, 9);
            this.lPakFolder.Name = "lPakFolder";
            this.lPakFolder.Size = new System.Drawing.Size(70, 13);
            this.lPakFolder.TabIndex = 3;
            this.lPakFolder.Text = "Target Folder";
            // 
            // lDiskFolder
            // 
            this.lDiskFolder.AutoSize = true;
            this.lDiskFolder.Location = new System.Drawing.Point(9, 48);
            this.lDiskFolder.Name = "lDiskFolder";
            this.lDiskFolder.Size = new System.Drawing.Size(73, 13);
            this.lDiskFolder.TabIndex = 4;
            this.lDiskFolder.Text = "Source Folder";
            // 
            // ePakFolder
            // 
            this.ePakFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ePakFolder.Location = new System.Drawing.Point(15, 25);
            this.ePakFolder.Name = "ePakFolder";
            this.ePakFolder.Size = new System.Drawing.Size(450, 20);
            this.ePakFolder.TabIndex = 5;
            // 
            // eDiskFolder
            // 
            this.eDiskFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eDiskFolder.Location = new System.Drawing.Point(15, 64);
            this.eDiskFolder.Name = "eDiskFolder";
            this.eDiskFolder.Size = new System.Drawing.Size(405, 20);
            this.eDiskFolder.TabIndex = 6;
            // 
            // btnSearchFolder
            // 
            this.btnSearchFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchFolder.Location = new System.Drawing.Point(427, 64);
            this.btnSearchFolder.Name = "btnSearchFolder";
            this.btnSearchFolder.Size = new System.Drawing.Size(39, 20);
            this.btnSearchFolder.TabIndex = 7;
            this.btnSearchFolder.Text = "...";
            this.btnSearchFolder.UseVisualStyleBackColor = true;
            this.btnSearchFolder.Click += new System.EventHandler(this.btnSearchFolder_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(472, 22);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(80, 65);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // ImportFolderDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 161);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSearchFolder);
            this.Controls.Add(this.eDiskFolder);
            this.Controls.Add(this.ePakFolder);
            this.Controls.Add(this.lDiskFolder);
            this.Controls.Add(this.lPakFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lInfo);
            this.Controls.Add(this.pbImport);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ImportFolderDlg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import from folder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportFolderDlg_FormClosing);
            this.Load += new System.EventHandler(this.ImportFolderDlg_Load);
            this.Shown += new System.EventHandler(this.ImportFolderDlg_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbImport;
        private System.Windows.Forms.Label lInfo;
        private System.ComponentModel.BackgroundWorker bgwImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lPakFolder;
        private System.Windows.Forms.Label lDiskFolder;
        public System.Windows.Forms.TextBox ePakFolder;
        public System.Windows.Forms.TextBox eDiskFolder;
        private System.Windows.Forms.Button btnSearchFolder;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.FolderBrowserDialog sourceFolderDialog;
    }
}