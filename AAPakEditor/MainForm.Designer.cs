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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MM = new System.Windows.Forms.MenuStrip();
            this.MMFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileS1 = new System.Windows.Forms.ToolStripSeparator();
            this.MMFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileS2 = new System.Windows.Forms.ToolStripSeparator();
            this.MMFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MMEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.MMEditAddFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MMEditReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.MMEditS2 = new System.Windows.Forms.ToolStripSeparator();
            this.MMEditDeleteSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.MMEditS1 = new System.Windows.Forms.ToolStripSeparator();
            this.MMEditImportFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExport = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExportSelectedFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExportSelectedFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExportS1 = new System.Windows.Forms.ToolStripSeparator();
            this.MMExportAll = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExportS2 = new System.Windows.Forms.ToolStripSeparator();
            this.MMExportDB = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExtra = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExtraMD5 = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExtraExportData = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExtraDebugTest = new System.Windows.Forms.ToolStripMenuItem();
            this.MMExtraMakeMod = new System.Windows.Forms.ToolStripMenuItem();
            this.MMVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.MMVersionSourceCode = new System.Windows.Forms.ToolStripMenuItem();
            this.MMVersionDiscord = new System.Windows.Forms.ToolStripMenuItem();
            this.openGamePakDialog = new System.Windows.Forms.OpenFileDialog();
            this.lbFolders = new System.Windows.Forms.ListBox();
            this.lFileCount = new System.Windows.Forms.Label();
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
            this.importFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tvFolders = new System.Windows.Forms.TreeView();
            this.tcDirectoryViews = new System.Windows.Forms.TabControl();
            this.tpTreeView = new System.Windows.Forms.TabPage();
            this.tpFlatDirView = new System.Windows.Forms.TabPage();
            this.MM.SuspendLayout();
            this.pFileInfo.SuspendLayout();
            this.tcDirectoryViews.SuspendLayout();
            this.tpTreeView.SuspendLayout();
            this.tpFlatDirView.SuspendLayout();
            this.SuspendLayout();
            // 
            // MM
            // 
            this.MM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMFile,
            this.MMEdit,
            this.MMExport,
            this.MMExtra,
            this.MMVersion});
            this.MM.Location = new System.Drawing.Point(0, 0);
            this.MM.Name = "MM";
            this.MM.Size = new System.Drawing.Size(584, 24);
            this.MM.TabIndex = 0;
            this.MM.Text = "menuStrip1";
            // 
            // MMFile
            // 
            this.MMFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMFileOpen,
            this.MMFileSave,
            this.MMFileS1,
            this.MMFileNew,
            this.MMFileClose,
            this.MMFileS2,
            this.MMFileExit});
            this.MMFile.Name = "MMFile";
            this.MMFile.Size = new System.Drawing.Size(37, 20);
            this.MMFile.Text = "&File";
            // 
            // MMFileOpen
            // 
            this.MMFileOpen.Name = "MMFileOpen";
            this.MMFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MMFileOpen.Size = new System.Drawing.Size(164, 22);
            this.MMFileOpen.Text = "&Open ...";
            this.MMFileOpen.Click += new System.EventHandler(this.MMFileOpen_Click);
            // 
            // MMFileSave
            // 
            this.MMFileSave.Name = "MMFileSave";
            this.MMFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MMFileSave.Size = new System.Drawing.Size(164, 22);
            this.MMFileSave.Text = "&Save now";
            this.MMFileSave.Click += new System.EventHandler(this.MMFileSave_Click);
            // 
            // MMFileS1
            // 
            this.MMFileS1.Name = "MMFileS1";
            this.MMFileS1.Size = new System.Drawing.Size(161, 6);
            // 
            // MMFileNew
            // 
            this.MMFileNew.Name = "MMFileNew";
            this.MMFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.MMFileNew.Size = new System.Drawing.Size(164, 22);
            this.MMFileNew.Text = "New ...";
            this.MMFileNew.Click += new System.EventHandler(this.MMFileNew_Click);
            // 
            // MMFileClose
            // 
            this.MMFileClose.Name = "MMFileClose";
            this.MMFileClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.MMFileClose.Size = new System.Drawing.Size(164, 22);
            this.MMFileClose.Text = "&Close";
            this.MMFileClose.Click += new System.EventHandler(this.MMFileClose_Click);
            // 
            // MMFileS2
            // 
            this.MMFileS2.Name = "MMFileS2";
            this.MMFileS2.Size = new System.Drawing.Size(161, 6);
            // 
            // MMFileExit
            // 
            this.MMFileExit.Name = "MMFileExit";
            this.MMFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.MMFileExit.Size = new System.Drawing.Size(164, 22);
            this.MMFileExit.Text = "E&xit";
            this.MMFileExit.Click += new System.EventHandler(this.MMFileExit_Click);
            // 
            // MMEdit
            // 
            this.MMEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMEditAddFile,
            this.MMEditReplace,
            this.MMEditS2,
            this.MMEditDeleteSelected,
            this.MMEditS1,
            this.MMEditImportFiles});
            this.MMEdit.Name = "MMEdit";
            this.MMEdit.Size = new System.Drawing.Size(39, 20);
            this.MMEdit.Text = "&Edit";
            // 
            // MMEditAddFile
            // 
            this.MMEditAddFile.Name = "MMEditAddFile";
            this.MMEditAddFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.MMEditAddFile.Size = new System.Drawing.Size(222, 22);
            this.MMEditAddFile.Text = "Add File ...";
            this.MMEditAddFile.Click += new System.EventHandler(this.MMEditAddFile_Click);
            // 
            // MMEditReplace
            // 
            this.MMEditReplace.Name = "MMEditReplace";
            this.MMEditReplace.Size = new System.Drawing.Size(222, 22);
            this.MMEditReplace.Text = "Replace selected file ...";
            this.MMEditReplace.Click += new System.EventHandler(this.MMEditReplace_Click);
            // 
            // MMEditS2
            // 
            this.MMEditS2.Name = "MMEditS2";
            this.MMEditS2.Size = new System.Drawing.Size(219, 6);
            // 
            // MMEditDeleteSelected
            // 
            this.MMEditDeleteSelected.Name = "MMEditDeleteSelected";
            this.MMEditDeleteSelected.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.MMEditDeleteSelected.Size = new System.Drawing.Size(222, 22);
            this.MMEditDeleteSelected.Text = "Delete selected file ...";
            this.MMEditDeleteSelected.Click += new System.EventHandler(this.MMEditDeleteSelected_Click);
            // 
            // MMEditS1
            // 
            this.MMEditS1.Name = "MMEditS1";
            this.MMEditS1.Size = new System.Drawing.Size(219, 6);
            // 
            // MMEditImportFiles
            // 
            this.MMEditImportFiles.Name = "MMEditImportFiles";
            this.MMEditImportFiles.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.MMEditImportFiles.Size = new System.Drawing.Size(222, 22);
            this.MMEditImportFiles.Text = "&Import Files ...";
            this.MMEditImportFiles.Click += new System.EventHandler(this.MMEditImportFiles_Click);
            // 
            // MMExport
            // 
            this.MMExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMExportSelectedFile,
            this.MMExportSelectedFolder,
            this.MMExportS1,
            this.MMExportAll,
            this.MMExportS2,
            this.MMExportDB});
            this.MMExport.Name = "MMExport";
            this.MMExport.Size = new System.Drawing.Size(52, 20);
            this.MMExport.Text = "&Export";
            // 
            // MMExportSelectedFile
            // 
            this.MMExportSelectedFile.Name = "MMExportSelectedFile";
            this.MMExportSelectedFile.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.MMExportSelectedFile.Size = new System.Drawing.Size(160, 22);
            this.MMExportSelectedFile.Text = "Selected &File";
            this.MMExportSelectedFile.Click += new System.EventHandler(this.MMExportSelectedFile_Click);
            // 
            // MMExportSelectedFolder
            // 
            this.MMExportSelectedFolder.Enabled = false;
            this.MMExportSelectedFolder.Name = "MMExportSelectedFolder";
            this.MMExportSelectedFolder.Size = new System.Drawing.Size(160, 22);
            this.MMExportSelectedFolder.Text = "Selected F&older";
            this.MMExportSelectedFolder.Click += new System.EventHandler(this.MMExportSelectedFolder_Click);
            // 
            // MMExportS1
            // 
            this.MMExportS1.Name = "MMExportS1";
            this.MMExportS1.Size = new System.Drawing.Size(157, 6);
            // 
            // MMExportAll
            // 
            this.MMExportAll.Name = "MMExportAll";
            this.MMExportAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.MMExportAll.Size = new System.Drawing.Size(160, 22);
            this.MMExportAll.Text = "&All Files";
            this.MMExportAll.Click += new System.EventHandler(this.MMExportAll_Click);
            // 
            // MMExportS2
            // 
            this.MMExportS2.Name = "MMExportS2";
            this.MMExportS2.Size = new System.Drawing.Size(157, 6);
            // 
            // MMExportDB
            // 
            this.MMExportDB.Name = "MMExportDB";
            this.MMExportDB.Size = new System.Drawing.Size(160, 22);
            this.MMExportDB.Text = "Export DB";
            this.MMExportDB.Click += new System.EventHandler(this.MMExportDB_Click);
            // 
            // MMExtra
            // 
            this.MMExtra.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMExtraMD5,
            this.MMExtraExportData,
            this.MMExtraDebugTest,
            this.MMExtraMakeMod});
            this.MMExtra.Name = "MMExtra";
            this.MMExtra.Size = new System.Drawing.Size(44, 20);
            this.MMExtra.Text = "E&xtra";
            // 
            // MMExtraMD5
            // 
            this.MMExtraMD5.Name = "MMExtraMD5";
            this.MMExtraMD5.Size = new System.Drawing.Size(205, 22);
            this.MMExtraMD5.Text = "Re-Calculate MD5";
            this.MMExtraMD5.Click += new System.EventHandler(this.MMEXtraMD5_Click);
            // 
            // MMExtraExportData
            // 
            this.MMExtraExportData.Name = "MMExtraExportData";
            this.MMExtraExportData.Size = new System.Drawing.Size(205, 22);
            this.MMExtraExportData.Text = "Export File Data as CSV ...";
            this.MMExtraExportData.Click += new System.EventHandler(this.MMExtraExportData_Click);
            // 
            // MMExtraDebugTest
            // 
            this.MMExtraDebugTest.Name = "MMExtraDebugTest";
            this.MMExtraDebugTest.Size = new System.Drawing.Size(205, 22);
            this.MMExtraDebugTest.Text = "DebugTest";
            this.MMExtraDebugTest.Visible = false;
            this.MMExtraDebugTest.Click += new System.EventHandler(this.MMExtraDebugTest_Click);
            // 
            // MMExtraMakeMod
            // 
            this.MMExtraMakeMod.Name = "MMExtraMakeMod";
            this.MMExtraMakeMod.Size = new System.Drawing.Size(205, 22);
            this.MMExtraMakeMod.Text = "Export pak as mod";
            this.MMExtraMakeMod.Click += new System.EventHandler(this.MMExtraMakeMod_Click);
            // 
            // MMVersion
            // 
            this.MMVersion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMVersionSourceCode,
            this.MMVersionDiscord});
            this.MMVersion.Name = "MMVersion";
            this.MMVersion.Size = new System.Drawing.Size(57, 20);
            this.MMVersion.Text = "&Version";
            // 
            // MMVersionSourceCode
            // 
            this.MMVersionSourceCode.Name = "MMVersionSourceCode";
            this.MMVersionSourceCode.Size = new System.Drawing.Size(141, 22);
            this.MMVersionSourceCode.Text = "Source Code";
            this.MMVersionSourceCode.Click += new System.EventHandler(this.MMVersionSourceCode_Click);
            // 
            // MMVersionDiscord
            // 
            this.MMVersionDiscord.Name = "MMVersionDiscord";
            this.MMVersionDiscord.Size = new System.Drawing.Size(141, 22);
            this.MMVersionDiscord.Text = "Visit Discord";
            this.MMVersionDiscord.Click += new System.EventHandler(this.VisitDiscordToolStripMenuItem_Click);
            // 
            // openGamePakDialog
            // 
            this.openGamePakDialog.Filter = "Known pak file types|*_pak;*_pak.*;*.aamod|ArcheAge Game Pak|*_pak;*_pak.*|All Fi" +
    "les|*.*";
            this.openGamePakDialog.ReadOnlyChecked = true;
            this.openGamePakDialog.RestoreDirectory = true;
            this.openGamePakDialog.ShowReadOnly = true;
            // 
            // lbFolders
            // 
            this.lbFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFolders.FormattingEnabled = true;
            this.lbFolders.Location = new System.Drawing.Point(3, 6);
            this.lbFolders.Name = "lbFolders";
            this.lbFolders.Size = new System.Drawing.Size(319, 381);
            this.lbFolders.TabIndex = 1;
            this.lbFolders.SelectedIndexChanged += new System.EventHandler(this.lbFolders_SelectedIndexChanged);
            // 
            // lFileCount
            // 
            this.lFileCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lFileCount.AutoSize = true;
            this.lFileCount.Location = new System.Drawing.Point(13, 454);
            this.lFileCount.Name = "lFileCount";
            this.lFileCount.Size = new System.Drawing.Size(40, 13);
            this.lFileCount.TabIndex = 2;
            this.lFileCount.Text = "no files";
            // 
            // lbFiles
            // 
            this.lbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(277, 53);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(295, 238);
            this.lbFiles.TabIndex = 4;
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            // 
            // lFiles
            // 
            this.lFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lFiles.AutoSize = true;
            this.lFiles.Location = new System.Drawing.Point(274, 35);
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
            this.pFileInfo.Location = new System.Drawing.Point(277, 301);
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
            this.exportFolderDialog.Description = "Select the destination folder to export all files to.";
            this.exportFolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // importFileDialog
            // 
            this.importFileDialog.AddExtension = false;
            this.importFileDialog.Filter = "Al Files|*.*";
            this.importFileDialog.RestoreDirectory = true;
            // 
            // tvFolders
            // 
            this.tvFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvFolders.Location = new System.Drawing.Point(6, 6);
            this.tvFolders.Name = "tvFolders";
            this.tvFolders.Size = new System.Drawing.Size(239, 378);
            this.tvFolders.TabIndex = 7;
            this.tvFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFolders_AfterSelect);
            // 
            // tcDirectoryViews
            // 
            this.tcDirectoryViews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcDirectoryViews.Controls.Add(this.tpTreeView);
            this.tcDirectoryViews.Controls.Add(this.tpFlatDirView);
            this.tcDirectoryViews.Location = new System.Drawing.Point(12, 35);
            this.tcDirectoryViews.Name = "tcDirectoryViews";
            this.tcDirectoryViews.SelectedIndex = 0;
            this.tcDirectoryViews.Size = new System.Drawing.Size(259, 416);
            this.tcDirectoryViews.TabIndex = 8;
            // 
            // tpTreeView
            // 
            this.tpTreeView.Controls.Add(this.tvFolders);
            this.tpTreeView.Location = new System.Drawing.Point(4, 22);
            this.tpTreeView.Name = "tpTreeView";
            this.tpTreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tpTreeView.Size = new System.Drawing.Size(251, 390);
            this.tpTreeView.TabIndex = 0;
            this.tpTreeView.Text = "Tree View";
            this.tpTreeView.UseVisualStyleBackColor = true;
            // 
            // tpFlatDirView
            // 
            this.tpFlatDirView.Controls.Add(this.lbFolders);
            this.tpFlatDirView.Location = new System.Drawing.Point(4, 22);
            this.tpFlatDirView.Name = "tpFlatDirView";
            this.tpFlatDirView.Padding = new System.Windows.Forms.Padding(3);
            this.tpFlatDirView.Size = new System.Drawing.Size(251, 390);
            this.tpFlatDirView.TabIndex = 1;
            this.tpFlatDirView.Text = "Flat Folder View";
            this.tpFlatDirView.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 476);
            this.Controls.Add(this.tcDirectoryViews);
            this.Controls.Add(this.pFileInfo);
            this.Controls.Add(this.lFiles);
            this.Controls.Add(this.lbFiles);
            this.Controls.Add(this.lFileCount);
            this.Controls.Add(this.MM);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MM;
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AAPakEditor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.MM.ResumeLayout(false);
            this.MM.PerformLayout();
            this.pFileInfo.ResumeLayout(false);
            this.pFileInfo.PerformLayout();
            this.tcDirectoryViews.ResumeLayout(false);
            this.tpTreeView.ResumeLayout(false);
            this.tpFlatDirView.ResumeLayout(false);
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
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Label lFiles;
        private System.Windows.Forms.Panel pFileInfo;
        private System.Windows.Forms.Label lfiStartOffset;
        private System.Windows.Forms.Label lfiCreateTime;
        private System.Windows.Forms.Label lfiHash;
        private System.Windows.Forms.Label lfiSize;
        private System.Windows.Forms.Label lfiName;
        private System.Windows.Forms.Label lfiModifyTime;
        private System.Windows.Forms.ToolStripMenuItem MMEdit;
        private System.Windows.Forms.ToolStripMenuItem MMEditImportFiles;
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
        private System.Windows.Forms.OpenFileDialog importFileDialog;
        private System.Windows.Forms.ToolStripMenuItem MMEditReplace;
        private System.Windows.Forms.TreeView tvFolders;
        private System.Windows.Forms.TabControl tcDirectoryViews;
        private System.Windows.Forms.TabPage tpTreeView;
        private System.Windows.Forms.TabPage tpFlatDirView;
        private System.Windows.Forms.ToolStripMenuItem MMExtraDebugTest;
        private System.Windows.Forms.ToolStripMenuItem MMEditDeleteSelected;
        private System.Windows.Forms.ToolStripSeparator MMEditS1;
        private System.Windows.Forms.ToolStripMenuItem MMEditAddFile;
        private System.Windows.Forms.ToolStripSeparator MMEditS2;
        private System.Windows.Forms.ToolStripMenuItem MMFileNew;
        private System.Windows.Forms.ToolStripSeparator MMFileS2;
        private System.Windows.Forms.ToolStripMenuItem MMFileClose;
        private System.Windows.Forms.ToolStripMenuItem MMVersion;
        private System.Windows.Forms.ToolStripMenuItem MMVersionSourceCode;
        private System.Windows.Forms.ToolStripSeparator MMExportS2;
        private System.Windows.Forms.ToolStripMenuItem MMExportDB;
        private System.Windows.Forms.ToolStripMenuItem MMVersionDiscord;
        private System.Windows.Forms.ToolStripMenuItem MMExtraMakeMod;
    }
}

