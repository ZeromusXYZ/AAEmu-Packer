namespace AAPakEditor
{
    partial class MainForm
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
            this.MM = new System.Windows.Forms.MenuStrip();
            this.MMFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileS1 = new System.Windows.Forms.ToolStripSeparator();
            this.MMFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MMImport = new System.Windows.Forms.ToolStripMenuItem();
            this.MMImportFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExport = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExportSelectedFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExportSelectedFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExportS1 = new System.Windows.Forms.ToolStripSeparator();
            this.MMExportAll = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExtra = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExtraMD5 = new System.Windows.Forms.ToolStripMenuItem();
            this.openGamePakDialog = new System.Windows.Forms.OpenFileDialog();
            this.lbFolders = new System.Windows.Forms.ListBox();
            this.lFileCount = new System.Windows.Forms.Label();
            this.lFolderList = new System.Windows.Forms.Label();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.lFiles = new System.Windows.Forms.Label();
            this.pFileInfo = new System.Windows.Forms.Panel();
            this.lfiExtras = new System.Windows.Forms.Label();
            this.lfiModifyTime = new System.Windows.Forms.Label();
            this.lfiStartOffset = new System.Windows.Forms.Label();
            this.lfiCreateTime = new System.Windows.Forms.Label();
            this.lfiHash = new System.Windows.Forms.Label();
            this.lfiSize = new System.Windows.Forms.Label();
            this.lfiName = new System.Windows.Forms.Label();
            this.exportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.exportFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.MMExtraExportData = new System.Windows.Forms.ToolStripMenuItem();
            this.MM.SuspendLayout();
            this.pFileInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // MM
            // 
            this.MM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMFile,
            this.MMImport,
            this.MMExport,
            this.MMExtra});
            this.MM.Location = new System.Drawing.Point(0, 0);
            this.MM.Name = "MM";
            this.MM.Size = new System.Drawing.Size(661, 24);
            this.MM.TabIndex = 0;
            this.MM.Text = "menuStrip1";
            // 
            // MMFile
            // 
            this.MMFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMFileOpen,
            this.MMFileSave,
            this.MMFileS1,
            this.MMFileExit});
            this.MMFile.Name = "MMFile";
            this.MMFile.Size = new System.Drawing.Size(37, 20);
            this.MMFile.Text = "&File";
            this.MMFile.DropDownOpening += new System.EventHandler(this.MMFile_DropDownOpening);
            // 
            // MMFileOpen
            // 
            this.MMFileOpen.Name = "MMFileOpen";
            this.MMFileOpen.Size = new System.Drawing.Size(115, 22);
            this.MMFileOpen.Text = "&Open ...";
            this.MMFileOpen.Click += new System.EventHandler(this.MMFileOpen_Click);
            // 
            // MMFileSave
            // 
            this.MMFileSave.Name = "MMFileSave";
            this.MMFileSave.Size = new System.Drawing.Size(115, 22);
            this.MMFileSave.Text = "&Save";
            this.MMFileSave.Click += new System.EventHandler(this.MMFileSave_Click);
            // 
            // MMFileS1
            // 
            this.MMFileS1.Name = "MMFileS1";
            this.MMFileS1.Size = new System.Drawing.Size(112, 6);
            // 
            // MMFileExit
            // 
            this.MMFileExit.Name = "MMFileExit";
            this.MMFileExit.Size = new System.Drawing.Size(115, 22);
            this.MMFileExit.Text = "E&xit";
            this.MMFileExit.Click += new System.EventHandler(this.MMFileExit_Click);
            // 
            // MMImport
            // 
            this.MMImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMImportFiles});
            this.MMImport.Enabled = false;
            this.MMImport.Name = "MMImport";
            this.MMImport.Size = new System.Drawing.Size(55, 20);
            this.MMImport.Text = "&Import";
            // 
            // MMImportFiles
            // 
            this.MMImportFiles.Name = "MMImportFiles";
            this.MMImportFiles.Size = new System.Drawing.Size(109, 22);
            this.MMImportFiles.Text = "&Files ...";
            // 
            // MMExport
            // 
            this.MMExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMExportSelectedFile,
            this.MMExportSelectedFolder,
            this.MMExportS1,
            this.MMExportAll});
            this.MMExport.Name = "MMExport";
            this.MMExport.Size = new System.Drawing.Size(52, 20);
            this.MMExport.Text = "&Export";
            this.MMExport.DropDownOpening += new System.EventHandler(this.MMExport_DropDownOpening);
            // 
            // MMExportSelectedFile
            // 
            this.MMExportSelectedFile.Name = "MMExportSelectedFile";
            this.MMExportSelectedFile.Size = new System.Drawing.Size(154, 22);
            this.MMExportSelectedFile.Text = "Selected &File";
            this.MMExportSelectedFile.Click += new System.EventHandler(this.MMExportSelectedFile_Click);
            // 
            // MMExportSelectedFolder
            // 
            this.MMExportSelectedFolder.Enabled = false;
            this.MMExportSelectedFolder.Name = "MMExportSelectedFolder";
            this.MMExportSelectedFolder.Size = new System.Drawing.Size(154, 22);
            this.MMExportSelectedFolder.Text = "Selected F&older";
            // 
            // MMExportS1
            // 
            this.MMExportS1.Name = "MMExportS1";
            this.MMExportS1.Size = new System.Drawing.Size(151, 6);
            // 
            // MMExportAll
            // 
            this.MMExportAll.Name = "MMExportAll";
            this.MMExportAll.Size = new System.Drawing.Size(154, 22);
            this.MMExportAll.Text = "&All Files";
            this.MMExportAll.Click += new System.EventHandler(this.MMExportAll_Click);
            // 
            // MMExtra
            // 
            this.MMExtra.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMExtraMD5,
            this.MMExtraExportData});
            this.MMExtra.Name = "MMExtra";
            this.MMExtra.Size = new System.Drawing.Size(44, 20);
            this.MMExtra.Text = "E&xtra";
            this.MMExtra.DropDownOpening += new System.EventHandler(this.MMExtra_DropDownOpening);
            // 
            // MMExtraMD5
            // 
            this.MMExtraMD5.Name = "MMExtraMD5";
            this.MMExtraMD5.Size = new System.Drawing.Size(180, 22);
            this.MMExtraMD5.Text = "Re-Calculate MD5";
            this.MMExtraMD5.Click += new System.EventHandler(this.MMEXtraMD5_Click);
            // 
            // openGamePakDialog
            // 
            this.openGamePakDialog.FileName = "game_pak";
            this.openGamePakDialog.Filter = "ArcheAge Game Pak|game_pak|All Files|*.*";
            this.openGamePakDialog.RestoreDirectory = true;
            // 
            // lbFolders
            // 
            this.lbFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFolders.FormattingEnabled = true;
            this.lbFolders.Location = new System.Drawing.Point(12, 53);
            this.lbFolders.Name = "lbFolders";
            this.lbFolders.Size = new System.Drawing.Size(336, 381);
            this.lbFolders.TabIndex = 1;
            this.lbFolders.SelectedIndexChanged += new System.EventHandler(this.lbFolders_SelectedIndexChanged);
            // 
            // lFileCount
            // 
            this.lFileCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lFileCount.AutoSize = true;
            this.lFileCount.Location = new System.Drawing.Point(12, 437);
            this.lFileCount.Name = "lFileCount";
            this.lFileCount.Size = new System.Drawing.Size(40, 13);
            this.lFileCount.TabIndex = 2;
            this.lFileCount.Text = "no files";
            // 
            // lFolderList
            // 
            this.lFolderList.AutoSize = true;
            this.lFolderList.Location = new System.Drawing.Point(9, 35);
            this.lFolderList.Name = "lFolderList";
            this.lFolderList.Size = new System.Drawing.Size(41, 13);
            this.lFolderList.TabIndex = 3;
            this.lFolderList.Text = "Folders";
            // 
            // lbFiles
            // 
            this.lbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(354, 53);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(295, 225);
            this.lbFiles.TabIndex = 4;
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            // 
            // lFiles
            // 
            this.lFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lFiles.AutoSize = true;
            this.lFiles.Location = new System.Drawing.Point(351, 35);
            this.lFiles.Name = "lFiles";
            this.lFiles.Size = new System.Drawing.Size(28, 13);
            this.lFiles.TabIndex = 5;
            this.lFiles.Text = "Files";
            // 
            // pFileInfo
            // 
            this.pFileInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pFileInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pFileInfo.Controls.Add(this.lfiExtras);
            this.pFileInfo.Controls.Add(this.lfiModifyTime);
            this.pFileInfo.Controls.Add(this.lfiStartOffset);
            this.pFileInfo.Controls.Add(this.lfiCreateTime);
            this.pFileInfo.Controls.Add(this.lfiHash);
            this.pFileInfo.Controls.Add(this.lfiSize);
            this.pFileInfo.Controls.Add(this.lfiName);
            this.pFileInfo.Location = new System.Drawing.Point(354, 284);
            this.pFileInfo.Name = "pFileInfo";
            this.pFileInfo.Size = new System.Drawing.Size(295, 150);
            this.pFileInfo.TabIndex = 6;
            // 
            // lfiExtras
            // 
            this.lfiExtras.AutoSize = true;
            this.lfiExtras.Location = new System.Drawing.Point(3, 117);
            this.lfiExtras.Name = "lfiExtras";
            this.lfiExtras.Size = new System.Drawing.Size(51, 13);
            this.lfiExtras.TabIndex = 6;
            this.lfiExtras.Text = "unknown";
            // 
            // lfiModifyTime
            // 
            this.lfiModifyTime.AutoSize = true;
            this.lfiModifyTime.Location = new System.Drawing.Point(3, 70);
            this.lfiModifyTime.Name = "lfiModifyTime";
            this.lfiModifyTime.Size = new System.Drawing.Size(46, 13);
            this.lfiModifyTime.TabIndex = 5;
            this.lfiModifyTime.Text = "modified";
            // 
            // lfiStartOffset
            // 
            this.lfiStartOffset.AutoSize = true;
            this.lfiStartOffset.Location = new System.Drawing.Point(3, 93);
            this.lfiStartOffset.Name = "lfiStartOffset";
            this.lfiStartOffset.Size = new System.Drawing.Size(33, 13);
            this.lfiStartOffset.TabIndex = 4;
            this.lfiStartOffset.Text = "offset";
            // 
            // lfiCreateTime
            // 
            this.lfiCreateTime.AutoSize = true;
            this.lfiCreateTime.Location = new System.Drawing.Point(3, 54);
            this.lfiCreateTime.Name = "lfiCreateTime";
            this.lfiCreateTime.Size = new System.Drawing.Size(37, 13);
            this.lfiCreateTime.TabIndex = 3;
            this.lfiCreateTime.Text = "create";
            // 
            // lfiHash
            // 
            this.lfiHash.AutoSize = true;
            this.lfiHash.Location = new System.Drawing.Point(3, 38);
            this.lfiHash.Name = "lfiHash";
            this.lfiHash.Size = new System.Drawing.Size(30, 13);
            this.lfiHash.TabIndex = 2;
            this.lfiHash.Text = "hash";
            // 
            // lfiSize
            // 
            this.lfiSize.AutoSize = true;
            this.lfiSize.Location = new System.Drawing.Point(3, 22);
            this.lfiSize.Name = "lfiSize";
            this.lfiSize.Size = new System.Drawing.Size(25, 13);
            this.lfiSize.TabIndex = 1;
            this.lfiSize.Text = "size";
            // 
            // lfiName
            // 
            this.lfiName.AutoSize = true;
            this.lfiName.Location = new System.Drawing.Point(3, 0);
            this.lfiName.Name = "lfiName";
            this.lfiName.Size = new System.Drawing.Size(33, 13);
            this.lfiName.TabIndex = 0;
            this.lfiName.Text = "name";
            // 
            // exportFolderDialog
            // 
            this.exportFolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // MMExtraExportData
            // 
            this.MMExtraExportData.Name = "MMExtraExportData";
            this.MMExtraExportData.Size = new System.Drawing.Size(193, 22);
            this.MMExtraExportData.Text = "Export Debug File Data";
            this.MMExtraExportData.Click += new System.EventHandler(this.MMExtraExportData_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 476);
            this.Controls.Add(this.pFileInfo);
            this.Controls.Add(this.lFiles);
            this.Controls.Add(this.lbFiles);
            this.Controls.Add(this.lFolderList);
            this.Controls.Add(this.lFileCount);
            this.Controls.Add(this.lbFolders);
            this.Controls.Add(this.MM);
            this.MainMenuStrip = this.MM;
            this.Name = "MainForm";
            this.Text = "AAPakEditor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MM.ResumeLayout(false);
            this.MM.PerformLayout();
            this.pFileInfo.ResumeLayout(false);
            this.pFileInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MM;
        private System.Windows.Forms.ToolStripMenuItem MMFile;
        private System.Windows.Forms.ToolStripMenuItem MMFileExit;
        private System.Windows.Forms.ToolStripMenuItem MMFileOpen;
        private System.Windows.Forms.OpenFileDialog openGamePakDialog;
        private System.Windows.Forms.ListBox lbFolders;
        private System.Windows.Forms.Label lFileCount;
        private System.Windows.Forms.Label lFolderList;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Label lFiles;
        private System.Windows.Forms.Panel pFileInfo;
        private System.Windows.Forms.Label lfiStartOffset;
        private System.Windows.Forms.Label lfiCreateTime;
        private System.Windows.Forms.Label lfiHash;
        private System.Windows.Forms.Label lfiSize;
        private System.Windows.Forms.Label lfiName;
        private System.Windows.Forms.Label lfiModifyTime;
        private System.Windows.Forms.ToolStripMenuItem MMImport;
        private System.Windows.Forms.ToolStripMenuItem MMImportFiles;
        private System.Windows.Forms.ToolStripMenuItem MMExport;
        private System.Windows.Forms.ToolStripMenuItem MMExportSelectedFile;
        private System.Windows.Forms.SaveFileDialog exportFileDialog;
        private System.Windows.Forms.ToolStripMenuItem MMExportSelectedFolder;
        private System.Windows.Forms.ToolStripMenuItem MMExportAll;
        private System.Windows.Forms.FolderBrowserDialog exportFolderDialog;
        private System.Windows.Forms.ToolStripMenuItem MMExtra;
        private System.Windows.Forms.ToolStripMenuItem MMExtraMD5;
        private System.Windows.Forms.ToolStripMenuItem MMFileSave;
        private System.Windows.Forms.ToolStripSeparator MMFileS1;
        private System.Windows.Forms.ToolStripSeparator MMExportS1;
        private System.Windows.Forms.Label lfiExtras;
        private System.Windows.Forms.ToolStripMenuItem MMExtraExportData;
    }
}

