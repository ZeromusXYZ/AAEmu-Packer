namespace AAPakEditor.Forms
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
            this.MMFileTryOpenUsingKeyList = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileS1 = new System.Windows.Forms.ToolStripSeparator();
            this.MMFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileS2 = new System.Windows.Forms.ToolStripSeparator();
            this.MMFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MMEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.MMEditAddFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MMEditReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.MMEditFileProp = new System.Windows.Forms.ToolStripMenuItem();
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
            this.MMExportAsCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.MMTools = new System.Windows.Forms.ToolStripMenuItem();
            this.MMToolsMakeMod = new System.Windows.Forms.ToolStripMenuItem();
            this.MMToolsS1 = new System.Windows.Forms.ToolStripSeparator();
            this.MMToolsCreatePatch = new System.Windows.Forms.ToolStripMenuItem();
            this.MMToolsApplyPatch = new System.Windows.Forms.ToolStripMenuItem();
            this.MMToolsS2 = new System.Windows.Forms.ToolStripSeparator();
            this.MMToolsMD5 = new System.Windows.Forms.ToolStripMenuItem();
            this.MMToolsMD5All = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MMToolsConvertMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MMToolsConvertPak = new System.Windows.Forms.ToolStripMenuItem();
            this.MMVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.MMVersionGetLatest = new System.Windows.Forms.ToolStripMenuItem();
            this.MMVersionS1 = new System.Windows.Forms.ToolStripSeparator();
            this.MMVersionSourceCode = new System.Windows.Forms.ToolStripMenuItem();
            this.MMVersionDiscord = new System.Windows.Forms.ToolStripMenuItem();
            this.openGamePakDialog = new System.Windows.Forms.OpenFileDialog();
            this.lbFolders = new System.Windows.Forms.ListBox();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.lFiles = new System.Windows.Forms.Label();
            this.pFileInfo = new System.Windows.Forms.Panel();
            this.lfiCreateTime = new System.Windows.Forms.Label();
            this.lfiModifyTime = new System.Windows.Forms.Label();
            this.lModifiedRaw = new System.Windows.Forms.Label();
            this.lCreateRaw = new System.Windows.Forms.Label();
            this.lfiIndex = new System.Windows.Forms.Label();
            this.lfiExtras = new System.Windows.Forms.Label();
            this.lfiStartOffset = new System.Windows.Forms.Label();
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
            this.tpExtraFiles = new System.Windows.Forms.TabPage();
            this.lbExtraFiles = new System.Windows.Forms.ListBox();
            this.openKeyListDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.pbGeneric = new System.Windows.Forms.ToolStripProgressBar();
            this.lFileCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lPakExtraInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lTypePak = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainFormSplitter = new System.Windows.Forms.SplitContainer();
            this.MMToolsPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.MM.SuspendLayout();
            this.pFileInfo.SuspendLayout();
            this.tcDirectoryViews.SuspendLayout();
            this.tpTreeView.SuspendLayout();
            this.tpFlatDirView.SuspendLayout();
            this.tpExtraFiles.SuspendLayout();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainFormSplitter)).BeginInit();
            this.MainFormSplitter.Panel1.SuspendLayout();
            this.MainFormSplitter.Panel2.SuspendLayout();
            this.MainFormSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // MM
            // 
            this.MM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMFile,
            this.MMEdit,
            this.MMExport,
            this.MMTools,
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
            this.MMFileTryOpenUsingKeyList,
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
            this.MMFileOpen.Size = new System.Drawing.Size(202, 22);
            this.MMFileOpen.Text = "&Open ...";
            this.MMFileOpen.Click += new System.EventHandler(this.MMFileOpen_Click);
            // 
            // MMFileTryOpenUsingKeyList
            // 
            this.MMFileTryOpenUsingKeyList.Name = "MMFileTryOpenUsingKeyList";
            this.MMFileTryOpenUsingKeyList.Size = new System.Drawing.Size(202, 22);
            this.MMFileTryOpenUsingKeyList.Text = "Try open using key list ...";
            this.MMFileTryOpenUsingKeyList.Visible = false;
            this.MMFileTryOpenUsingKeyList.Click += new System.EventHandler(this.MMExtraTryOpenUsingKeyList_Click);
            // 
            // MMFileSave
            // 
            this.MMFileSave.Name = "MMFileSave";
            this.MMFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MMFileSave.Size = new System.Drawing.Size(202, 22);
            this.MMFileSave.Text = "&Save now";
            this.MMFileSave.Click += new System.EventHandler(this.MMFileSave_Click);
            // 
            // MMFileS1
            // 
            this.MMFileS1.Name = "MMFileS1";
            this.MMFileS1.Size = new System.Drawing.Size(199, 6);
            // 
            // MMFileNew
            // 
            this.MMFileNew.Name = "MMFileNew";
            this.MMFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.MMFileNew.Size = new System.Drawing.Size(202, 22);
            this.MMFileNew.Text = "New ...";
            this.MMFileNew.Click += new System.EventHandler(this.MMFileNew_Click);
            // 
            // MMFileClose
            // 
            this.MMFileClose.Name = "MMFileClose";
            this.MMFileClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.MMFileClose.Size = new System.Drawing.Size(202, 22);
            this.MMFileClose.Text = "&Close";
            this.MMFileClose.Click += new System.EventHandler(this.MMFileClose_Click);
            // 
            // MMFileS2
            // 
            this.MMFileS2.Name = "MMFileS2";
            this.MMFileS2.Size = new System.Drawing.Size(199, 6);
            this.MMFileS2.Click += new System.EventHandler(this.MMFileS2_Click);
            // 
            // MMFileExit
            // 
            this.MMFileExit.Name = "MMFileExit";
            this.MMFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.MMFileExit.Size = new System.Drawing.Size(202, 22);
            this.MMFileExit.Text = "E&xit";
            this.MMFileExit.Click += new System.EventHandler(this.MMFileExit_Click);
            // 
            // MMEdit
            // 
            this.MMEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMEditAddFile,
            this.MMEditReplace,
            this.MMEditFileProp,
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
            this.MMEditAddFile.Text = "Add/Replace File ...";
            this.MMEditAddFile.Click += new System.EventHandler(this.MMEditAddFile_Click);
            // 
            // MMEditReplace
            // 
            this.MMEditReplace.Name = "MMEditReplace";
            this.MMEditReplace.Size = new System.Drawing.Size(222, 22);
            this.MMEditReplace.Text = "Replace selected file ...";
            this.MMEditReplace.Visible = false;
            this.MMEditReplace.Click += new System.EventHandler(this.MMEditReplace_Click);
            // 
            // MMEditFileProp
            // 
            this.MMEditFileProp.Name = "MMEditFileProp";
            this.MMEditFileProp.Size = new System.Drawing.Size(222, 22);
            this.MMEditFileProp.Text = "Edit File Properties ...";
            this.MMEditFileProp.Click += new System.EventHandler(this.manualEditFileMD5ToolStripMenuItem_Click);
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
            this.MMExportDB,
            this.MMExportAsCsv});
            this.MMExport.Name = "MMExport";
            this.MMExport.Size = new System.Drawing.Size(53, 20);
            this.MMExport.Text = "&Export";
            // 
            // MMExportSelectedFile
            // 
            this.MMExportSelectedFile.Name = "MMExportSelectedFile";
            this.MMExportSelectedFile.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.MMExportSelectedFile.Size = new System.Drawing.Size(206, 22);
            this.MMExportSelectedFile.Text = "Selected &File ...";
            this.MMExportSelectedFile.Click += new System.EventHandler(this.MMExportSelectedFile_Click);
            // 
            // MMExportSelectedFolder
            // 
            this.MMExportSelectedFolder.Enabled = false;
            this.MMExportSelectedFolder.Name = "MMExportSelectedFolder";
            this.MMExportSelectedFolder.Size = new System.Drawing.Size(206, 22);
            this.MMExportSelectedFolder.Text = "Selected F&older ...";
            this.MMExportSelectedFolder.Click += new System.EventHandler(this.MMExportSelectedFolder_Click);
            // 
            // MMExportS1
            // 
            this.MMExportS1.Name = "MMExportS1";
            this.MMExportS1.Size = new System.Drawing.Size(203, 6);
            // 
            // MMExportAll
            // 
            this.MMExportAll.Name = "MMExportAll";
            this.MMExportAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.MMExportAll.Size = new System.Drawing.Size(206, 22);
            this.MMExportAll.Text = "&All Files ...";
            this.MMExportAll.Click += new System.EventHandler(this.MMExportAll_Click);
            // 
            // MMExportS2
            // 
            this.MMExportS2.Name = "MMExportS2";
            this.MMExportS2.Size = new System.Drawing.Size(203, 6);
            // 
            // MMExportDB
            // 
            this.MMExportDB.Name = "MMExportDB";
            this.MMExportDB.Size = new System.Drawing.Size(206, 22);
            this.MMExportDB.Text = "Export DB ...";
            this.MMExportDB.Click += new System.EventHandler(this.MMExportDB_Click);
            // 
            // MMExportAsCsv
            // 
            this.MMExportAsCsv.Name = "MMExportAsCsv";
            this.MMExportAsCsv.Size = new System.Drawing.Size(206, 22);
            this.MMExportAsCsv.Text = "Export File Data as CSV ...";
            this.MMExportAsCsv.Click += new System.EventHandler(this.MMExportAsCsv_Click);
            // 
            // MMTools
            // 
            this.MMTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMToolsMakeMod,
            this.MMToolsS1,
            this.MMToolsCreatePatch,
            this.MMToolsApplyPatch,
            this.MMToolsS2,
            this.MMToolsMD5,
            this.MMToolsMD5All,
            this.toolStripMenuItem1,
            this.MMToolsConvertMenu,
            this.MMToolsPreview});
            this.MMTools.Name = "MMTools";
            this.MMTools.Size = new System.Drawing.Size(46, 20);
            this.MMTools.Text = "&Tools";
            this.MMTools.DropDownOpening += new System.EventHandler(this.MMTools_DropDownOpening);
            // 
            // MMToolsMakeMod
            // 
            this.MMToolsMakeMod.Name = "MMToolsMakeMod";
            this.MMToolsMakeMod.Size = new System.Drawing.Size(234, 22);
            this.MMToolsMakeMod.Text = "Export pak as mod ...";
            this.MMToolsMakeMod.Click += new System.EventHandler(this.MMExtraMakeMod_Click);
            // 
            // MMToolsS1
            // 
            this.MMToolsS1.Name = "MMToolsS1";
            this.MMToolsS1.Size = new System.Drawing.Size(231, 6);
            // 
            // MMToolsCreatePatch
            // 
            this.MMToolsCreatePatch.Name = "MMToolsCreatePatch";
            this.MMToolsCreatePatch.Size = new System.Drawing.Size(234, 22);
            this.MMToolsCreatePatch.Text = "Create patch file ...";
            this.MMToolsCreatePatch.Visible = false;
            // 
            // MMToolsApplyPatch
            // 
            this.MMToolsApplyPatch.Name = "MMToolsApplyPatch";
            this.MMToolsApplyPatch.Size = new System.Drawing.Size(234, 22);
            this.MMToolsApplyPatch.Text = "Apply patch file ...";
            this.MMToolsApplyPatch.Visible = false;
            // 
            // MMToolsS2
            // 
            this.MMToolsS2.Name = "MMToolsS2";
            this.MMToolsS2.Size = new System.Drawing.Size(231, 6);
            this.MMToolsS2.Visible = false;
            // 
            // MMToolsMD5
            // 
            this.MMToolsMD5.Name = "MMToolsMD5";
            this.MMToolsMD5.Size = new System.Drawing.Size(234, 22);
            this.MMToolsMD5.Text = "Re-Calculate MD5";
            this.MMToolsMD5.Click += new System.EventHandler(this.MMEXtraMD5_Click);
            // 
            // MMToolsMD5All
            // 
            this.MMToolsMD5All.Name = "MMToolsMD5All";
            this.MMToolsMD5All.Size = new System.Drawing.Size(234, 22);
            this.MMToolsMD5All.Text = "Re-Calculate MD5 of all files ...";
            this.MMToolsMD5All.Click += new System.EventHandler(this.MMExtraMD5All_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(231, 6);
            // 
            // MMToolsConvertMenu
            // 
            this.MMToolsConvertMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMToolsConvertPak});
            this.MMToolsConvertMenu.Name = "MMToolsConvertMenu";
            this.MMToolsConvertMenu.Size = new System.Drawing.Size(234, 22);
            this.MMToolsConvertMenu.Text = "Convert Pak Type";
            this.MMToolsConvertMenu.DropDownOpening += new System.EventHandler(this.MMToolsConvertMenu_DropDownOpening);
            // 
            // MMToolsConvertPak
            // 
            this.MMToolsConvertPak.Name = "MMToolsConvertPak";
            this.MMToolsConvertPak.Size = new System.Drawing.Size(110, 22);
            this.MMToolsConvertPak.Tag = "1";
            this.MMToolsConvertPak.Text = "Classic";
            this.MMToolsConvertPak.Click += new System.EventHandler(this.MMToolsConvertPak_Click);
            // 
            // MMVersion
            // 
            this.MMVersion.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.MMVersion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMVersionGetLatest,
            this.MMVersionS1,
            this.MMVersionSourceCode,
            this.MMVersionDiscord});
            this.MMVersion.Name = "MMVersion";
            this.MMVersion.Size = new System.Drawing.Size(57, 20);
            this.MMVersion.Text = "&Version";
            // 
            // MMVersionGetLatest
            // 
            this.MMVersionGetLatest.Name = "MMVersionGetLatest";
            this.MMVersionGetLatest.Size = new System.Drawing.Size(236, 22);
            this.MMVersionGetLatest.Text = "Check for Latest version online";
            this.MMVersionGetLatest.Click += new System.EventHandler(this.MMVersionGetLatest_Click);
            // 
            // MMVersionS1
            // 
            this.MMVersionS1.Name = "MMVersionS1";
            this.MMVersionS1.Size = new System.Drawing.Size(233, 6);
            // 
            // MMVersionSourceCode
            // 
            this.MMVersionSourceCode.Name = "MMVersionSourceCode";
            this.MMVersionSourceCode.Size = new System.Drawing.Size(236, 22);
            this.MMVersionSourceCode.Text = "Source Code";
            this.MMVersionSourceCode.Click += new System.EventHandler(this.MMVersionSourceCode_Click);
            // 
            // MMVersionDiscord
            // 
            this.MMVersionDiscord.Name = "MMVersionDiscord";
            this.MMVersionDiscord.Size = new System.Drawing.Size(236, 22);
            this.MMVersionDiscord.Text = "Visit Discord";
            this.MMVersionDiscord.Click += new System.EventHandler(this.VisitDiscordToolStripMenuItem_Click);
            // 
            // openGamePakDialog
            // 
            this.openGamePakDialog.Filter = "Known pak file types|*_pak;*_pak.*;*.aamod;*.csv|ArcheAge Game Pak|*_pak;*_pak.*|" +
    "CSV Files|*.csv|All Files|*.*";
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
            this.lbFolders.Size = new System.Drawing.Size(236, 329);
            this.lbFolders.TabIndex = 1;
            this.lbFolders.SelectedIndexChanged += new System.EventHandler(this.lbFolders_SelectedIndexChanged);
            // 
            // lbFiles
            // 
            this.lbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(6, 28);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(316, 173);
            this.lbFiles.TabIndex = 4;
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            // 
            // lFiles
            // 
            this.lFiles.AutoSize = true;
            this.lFiles.Location = new System.Drawing.Point(5, 6);
            this.lFiles.Name = "lFiles";
            this.lFiles.Size = new System.Drawing.Size(28, 13);
            this.lFiles.TabIndex = 5;
            this.lFiles.Text = "Files";
            // 
            // pFileInfo
            // 
            this.pFileInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pFileInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pFileInfo.Controls.Add(this.lfiCreateTime);
            this.pFileInfo.Controls.Add(this.lfiModifyTime);
            this.pFileInfo.Controls.Add(this.lModifiedRaw);
            this.pFileInfo.Controls.Add(this.lCreateRaw);
            this.pFileInfo.Controls.Add(this.lfiIndex);
            this.pFileInfo.Controls.Add(this.lfiExtras);
            this.pFileInfo.Controls.Add(this.lfiStartOffset);
            this.pFileInfo.Controls.Add(this.lfiHash);
            this.pFileInfo.Controls.Add(this.lfiSize);
            this.pFileInfo.Controls.Add(this.lfiName);
            this.pFileInfo.Location = new System.Drawing.Point(6, 205);
            this.pFileInfo.Name = "pFileInfo";
            this.pFileInfo.Size = new System.Drawing.Size(316, 150);
            this.pFileInfo.TabIndex = 6;
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
            // lfiModifyTime
            // 
            this.lfiModifyTime.AutoSize = true;
            this.lfiModifyTime.Location = new System.Drawing.Point(3, 70);
            this.lfiModifyTime.Name = "lfiModifyTime";
            this.lfiModifyTime.Size = new System.Drawing.Size(46, 13);
            this.lfiModifyTime.TabIndex = 5;
            this.lfiModifyTime.Text = "modified";
            // 
            // lModifiedRaw
            // 
            this.lModifiedRaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lModifiedRaw.Location = new System.Drawing.Point(145, 70);
            this.lModifiedRaw.Name = "lModifiedRaw";
            this.lModifiedRaw.Size = new System.Drawing.Size(166, 13);
            this.lModifiedRaw.TabIndex = 9;
            this.lModifiedRaw.Text = "( )";
            this.lModifiedRaw.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lCreateRaw
            // 
            this.lCreateRaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lCreateRaw.Location = new System.Drawing.Point(145, 54);
            this.lCreateRaw.Name = "lCreateRaw";
            this.lCreateRaw.Size = new System.Drawing.Size(166, 13);
            this.lCreateRaw.TabIndex = 8;
            this.lCreateRaw.Text = "( )";
            this.lCreateRaw.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lfiIndex
            // 
            this.lfiIndex.AutoSize = true;
            this.lfiIndex.Location = new System.Drawing.Point(3, 130);
            this.lfiIndex.Name = "lfiIndex";
            this.lfiIndex.Size = new System.Drawing.Size(32, 13);
            this.lfiIndex.TabIndex = 7;
            this.lfiIndex.Text = "index";
            // 
            // lfiExtras
            // 
            this.lfiExtras.AutoSize = true;
            this.lfiExtras.Location = new System.Drawing.Point(3, 115);
            this.lfiExtras.Name = "lfiExtras";
            this.lfiExtras.Size = new System.Drawing.Size(51, 13);
            this.lfiExtras.TabIndex = 6;
            this.lfiExtras.Text = "unknown";
            // 
            // lfiStartOffset
            // 
            this.lfiStartOffset.AutoSize = true;
            this.lfiStartOffset.Location = new System.Drawing.Point(3, 93);
            this.lfiStartOffset.Name = "lfiStartOffset";
            this.lfiStartOffset.Size = new System.Drawing.Size(35, 13);
            this.lfiStartOffset.TabIndex = 4;
            this.lfiStartOffset.Text = "Offset";
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
            this.lfiSize.Size = new System.Drawing.Size(27, 13);
            this.lfiSize.TabIndex = 1;
            this.lfiSize.Text = "Size";
            // 
            // lfiName
            // 
            this.lfiName.AutoSize = true;
            this.lfiName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lfiName.Location = new System.Drawing.Point(3, 0);
            this.lfiName.Name = "lfiName";
            this.lfiName.Size = new System.Drawing.Size(35, 13);
            this.lfiName.TabIndex = 0;
            this.lfiName.Text = "Name";
            this.lfiName.Click += new System.EventHandler(this.lfiName_Click);
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
            this.tvFolders.Size = new System.Drawing.Size(230, 327);
            this.tvFolders.TabIndex = 7;
            this.tvFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFolders_AfterSelect);
            // 
            // tcDirectoryViews
            // 
            this.tcDirectoryViews.Controls.Add(this.tpTreeView);
            this.tcDirectoryViews.Controls.Add(this.tpFlatDirView);
            this.tcDirectoryViews.Controls.Add(this.tpExtraFiles);
            this.tcDirectoryViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcDirectoryViews.Location = new System.Drawing.Point(0, 0);
            this.tcDirectoryViews.Name = "tcDirectoryViews";
            this.tcDirectoryViews.SelectedIndex = 0;
            this.tcDirectoryViews.Size = new System.Drawing.Size(250, 365);
            this.tcDirectoryViews.TabIndex = 8;
            // 
            // tpTreeView
            // 
            this.tpTreeView.Controls.Add(this.tvFolders);
            this.tpTreeView.Location = new System.Drawing.Point(4, 22);
            this.tpTreeView.Name = "tpTreeView";
            this.tpTreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tpTreeView.Size = new System.Drawing.Size(242, 339);
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
            this.tpFlatDirView.Size = new System.Drawing.Size(242, 339);
            this.tpFlatDirView.TabIndex = 1;
            this.tpFlatDirView.Text = "Flat Folder View";
            this.tpFlatDirView.UseVisualStyleBackColor = true;
            // 
            // tpExtraFiles
            // 
            this.tpExtraFiles.Controls.Add(this.lbExtraFiles);
            this.tpExtraFiles.Location = new System.Drawing.Point(4, 22);
            this.tpExtraFiles.Name = "tpExtraFiles";
            this.tpExtraFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpExtraFiles.Size = new System.Drawing.Size(242, 339);
            this.tpExtraFiles.TabIndex = 2;
            this.tpExtraFiles.Text = "Deleted Files";
            this.tpExtraFiles.UseVisualStyleBackColor = true;
            // 
            // lbExtraFiles
            // 
            this.lbExtraFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbExtraFiles.FormattingEnabled = true;
            this.lbExtraFiles.Location = new System.Drawing.Point(3, 5);
            this.lbExtraFiles.Name = "lbExtraFiles";
            this.lbExtraFiles.Size = new System.Drawing.Size(236, 329);
            this.lbExtraFiles.TabIndex = 2;
            this.lbExtraFiles.SelectedIndexChanged += new System.EventHandler(this.LbExtraFiles_SelectedIndexChanged);
            // 
            // openKeyListDialog
            // 
            this.openKeyListDialog.DefaultExt = "csv";
            this.openKeyListDialog.Filter = "CSV files|*.csv|All files|*.*";
            this.openKeyListDialog.RestoreDirectory = true;
            this.openKeyListDialog.Title = "Open Key List";
            // 
            // statusBar
            // 
            this.statusBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbGeneric,
            this.lFileCount,
            this.lPakExtraInfo,
            this.lTypePak});
            this.statusBar.Location = new System.Drawing.Point(0, 389);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(584, 22);
            this.statusBar.TabIndex = 9;
            this.statusBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusBar_ItemClicked);
            // 
            // pbGeneric
            // 
            this.pbGeneric.Name = "pbGeneric";
            this.pbGeneric.Size = new System.Drawing.Size(125, 16);
            this.pbGeneric.Visible = false;
            // 
            // lFileCount
            // 
            this.lFileCount.Name = "lFileCount";
            this.lFileCount.Size = new System.Drawing.Size(45, 17);
            this.lFileCount.Text = "no files";
            this.lFileCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lPakExtraInfo
            // 
            this.lPakExtraInfo.Name = "lPakExtraInfo";
            this.lPakExtraInfo.Size = new System.Drawing.Size(508, 17);
            this.lPakExtraInfo.Spring = true;
            this.lPakExtraInfo.Text = "...";
            this.lPakExtraInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lTypePak
            // 
            this.lTypePak.Name = "lTypePak";
            this.lTypePak.Size = new System.Drawing.Size(16, 17);
            this.lTypePak.Text = "...";
            this.lTypePak.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainFormSplitter
            // 
            this.MainFormSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainFormSplitter.Location = new System.Drawing.Point(0, 24);
            this.MainFormSplitter.Name = "MainFormSplitter";
            // 
            // MainFormSplitter.Panel1
            // 
            this.MainFormSplitter.Panel1.Controls.Add(this.tcDirectoryViews);
            this.MainFormSplitter.Panel1MinSize = 250;
            // 
            // MainFormSplitter.Panel2
            // 
            this.MainFormSplitter.Panel2.Controls.Add(this.lFiles);
            this.MainFormSplitter.Panel2.Controls.Add(this.lbFiles);
            this.MainFormSplitter.Panel2.Controls.Add(this.pFileInfo);
            this.MainFormSplitter.Panel2MinSize = 300;
            this.MainFormSplitter.Size = new System.Drawing.Size(584, 365);
            this.MainFormSplitter.SplitterDistance = 250;
            this.MainFormSplitter.TabIndex = 10;
            // 
            // MMToolsPreview
            // 
            this.MMToolsPreview.Name = "MMToolsPreview";
            this.MMToolsPreview.Size = new System.Drawing.Size(234, 22);
            this.MMToolsPreview.Text = "Show Preview";
            this.MMToolsPreview.Click += new System.EventHandler(this.MMToolsPreview_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.MainFormSplitter);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.MM);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MM;
            this.Name = "MainForm";
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
            this.tpExtraFiles.ResumeLayout(false);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.MainFormSplitter.Panel1.ResumeLayout(false);
            this.MainFormSplitter.Panel2.ResumeLayout(false);
            this.MainFormSplitter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainFormSplitter)).EndInit();
            this.MainFormSplitter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.SplitContainer MainFormSplitter;

        #endregion

        private System.Windows.Forms.MenuStrip MM;
        private System.Windows.Forms.ToolStripMenuItem MMFile;
        private System.Windows.Forms.ToolStripMenuItem MMFileExit;
        private System.Windows.Forms.ToolStripMenuItem MMFileOpen;
        private System.Windows.Forms.OpenFileDialog openGamePakDialog;
        private System.Windows.Forms.ListBox lbFolders;
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
        private System.Windows.Forms.ToolStripMenuItem MMTools;
        private System.Windows.Forms.ToolStripMenuItem MMToolsMD5;
        private System.Windows.Forms.ToolStripMenuItem MMFileSave;
        private System.Windows.Forms.ToolStripSeparator MMFileS1;
        private System.Windows.Forms.ToolStripSeparator MMExportS1;
        private System.Windows.Forms.Label lfiExtras;
        private System.Windows.Forms.OpenFileDialog importFileDialog;
        private System.Windows.Forms.ToolStripMenuItem MMEditReplace;
        private System.Windows.Forms.TreeView tvFolders;
        private System.Windows.Forms.TabControl tcDirectoryViews;
        private System.Windows.Forms.TabPage tpTreeView;
        private System.Windows.Forms.TabPage tpFlatDirView;
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
        private System.Windows.Forms.ToolStripMenuItem MMToolsMakeMod;
        private System.Windows.Forms.TabPage tpExtraFiles;
        private System.Windows.Forms.ListBox lbExtraFiles;
        private System.Windows.Forms.Label lfiIndex;
        private System.Windows.Forms.ToolStripMenuItem MMEditFileProp;
        private System.Windows.Forms.ToolStripMenuItem MMFileTryOpenUsingKeyList;
        private System.Windows.Forms.OpenFileDialog openKeyListDialog;
        private System.Windows.Forms.Label lModifiedRaw;
        private System.Windows.Forms.Label lCreateRaw;
        private System.Windows.Forms.ToolStripMenuItem MMToolsMD5All;
        private System.Windows.Forms.ToolStripSeparator MMToolsS1;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel lFileCount;
        private System.Windows.Forms.ToolStripStatusLabel lTypePak;
        private System.Windows.Forms.ToolStripStatusLabel lPakExtraInfo;
        private System.Windows.Forms.ToolStripProgressBar pbGeneric;
        private System.Windows.Forms.ToolStripMenuItem MMExportAsCsv;
        private System.Windows.Forms.ToolStripMenuItem MMVersionGetLatest;
        private System.Windows.Forms.ToolStripSeparator MMVersionS1;
        private System.Windows.Forms.ToolStripMenuItem MMToolsCreatePatch;
        private System.Windows.Forms.ToolStripMenuItem MMToolsApplyPatch;
        private System.Windows.Forms.ToolStripSeparator MMToolsS2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MMToolsConvertMenu;
        private System.Windows.Forms.ToolStripMenuItem MMToolsConvertPak;
        private System.Windows.Forms.ToolStripMenuItem MMToolsPreview;
    }
}

