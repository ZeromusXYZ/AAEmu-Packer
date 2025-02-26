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
            MM = new System.Windows.Forms.MenuStrip();
            MMFile = new System.Windows.Forms.ToolStripMenuItem();
            MMFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            MMFileTryOpenUsingKeyList = new System.Windows.Forms.ToolStripMenuItem();
            MMFileSave = new System.Windows.Forms.ToolStripMenuItem();
            MMFileS1 = new System.Windows.Forms.ToolStripSeparator();
            MMFileNew = new System.Windows.Forms.ToolStripMenuItem();
            MMFileClose = new System.Windows.Forms.ToolStripMenuItem();
            MMFileS2 = new System.Windows.Forms.ToolStripSeparator();
            MMFileExit = new System.Windows.Forms.ToolStripMenuItem();
            MMEdit = new System.Windows.Forms.ToolStripMenuItem();
            MMEditAddFile = new System.Windows.Forms.ToolStripMenuItem();
            MMEditReplace = new System.Windows.Forms.ToolStripMenuItem();
            MMEditFileProp = new System.Windows.Forms.ToolStripMenuItem();
            MMEditS2 = new System.Windows.Forms.ToolStripSeparator();
            MMEditDeleteSelected = new System.Windows.Forms.ToolStripMenuItem();
            MMEditS1 = new System.Windows.Forms.ToolStripSeparator();
            MMEditImportFiles = new System.Windows.Forms.ToolStripMenuItem();
            MMExport = new System.Windows.Forms.ToolStripMenuItem();
            MMExportSelectedFile = new System.Windows.Forms.ToolStripMenuItem();
            MMExportSelectedFolder = new System.Windows.Forms.ToolStripMenuItem();
            MMExportS1 = new System.Windows.Forms.ToolStripSeparator();
            MMExportAll = new System.Windows.Forms.ToolStripMenuItem();
            MMExportS2 = new System.Windows.Forms.ToolStripSeparator();
            MMExportDB = new System.Windows.Forms.ToolStripMenuItem();
            MMExportAsCsv = new System.Windows.Forms.ToolStripMenuItem();
            MMTools = new System.Windows.Forms.ToolStripMenuItem();
            MMToolsMakeMod = new System.Windows.Forms.ToolStripMenuItem();
            MMToolsS1 = new System.Windows.Forms.ToolStripSeparator();
            MMToolsCreatePatch = new System.Windows.Forms.ToolStripMenuItem();
            MMToolsApplyPatch = new System.Windows.Forms.ToolStripMenuItem();
            MMToolsS2 = new System.Windows.Forms.ToolStripSeparator();
            MMToolsMD5 = new System.Windows.Forms.ToolStripMenuItem();
            MMToolsMD5All = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            MMToolsConvertMenu = new System.Windows.Forms.ToolStripMenuItem();
            MMToolsConvertPak = new System.Windows.Forms.ToolStripMenuItem();
            MMToolsPreview = new System.Windows.Forms.ToolStripMenuItem();
            MMVersion = new System.Windows.Forms.ToolStripMenuItem();
            MMVersionGetLatest = new System.Windows.Forms.ToolStripMenuItem();
            MMVersionS1 = new System.Windows.Forms.ToolStripSeparator();
            MMVersionSourceCode = new System.Windows.Forms.ToolStripMenuItem();
            MMVersionDiscord = new System.Windows.Forms.ToolStripMenuItem();
            openGamePakDialog = new System.Windows.Forms.OpenFileDialog();
            lbFolders = new System.Windows.Forms.ListBox();
            lbFiles = new System.Windows.Forms.ListBox();
            lFiles = new System.Windows.Forms.Label();
            pFileInfo = new System.Windows.Forms.Panel();
            lfiCreateTime = new System.Windows.Forms.Label();
            lfiModifyTime = new System.Windows.Forms.Label();
            lModifiedRaw = new System.Windows.Forms.Label();
            lCreateRaw = new System.Windows.Forms.Label();
            lfiIndex = new System.Windows.Forms.Label();
            lfiExtras = new System.Windows.Forms.Label();
            lfiStartOffset = new System.Windows.Forms.Label();
            lfiHash = new System.Windows.Forms.Label();
            lfiSize = new System.Windows.Forms.Label();
            lfiName = new System.Windows.Forms.Label();
            exportFileDialog = new System.Windows.Forms.SaveFileDialog();
            exportFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            importFileDialog = new System.Windows.Forms.OpenFileDialog();
            tvFolders = new System.Windows.Forms.TreeView();
            tcDirectoryViews = new System.Windows.Forms.TabControl();
            tpTreeView = new System.Windows.Forms.TabPage();
            tpFlatDirView = new System.Windows.Forms.TabPage();
            tpExtraFiles = new System.Windows.Forms.TabPage();
            lbExtraFiles = new System.Windows.Forms.ListBox();
            openKeyListDialog = new System.Windows.Forms.OpenFileDialog();
            statusBar = new System.Windows.Forms.StatusStrip();
            pbGeneric = new System.Windows.Forms.ToolStripProgressBar();
            lFileCount = new System.Windows.Forms.ToolStripStatusLabel();
            lPakExtraInfo = new System.Windows.Forms.ToolStripStatusLabel();
            lTypePak = new System.Windows.Forms.ToolStripStatusLabel();
            MainFormSplitter = new System.Windows.Forms.SplitContainer();
            saveGamePakDialog = new System.Windows.Forms.SaveFileDialog();
            MM.SuspendLayout();
            pFileInfo.SuspendLayout();
            tcDirectoryViews.SuspendLayout();
            tpTreeView.SuspendLayout();
            tpFlatDirView.SuspendLayout();
            tpExtraFiles.SuspendLayout();
            statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainFormSplitter).BeginInit();
            MainFormSplitter.Panel1.SuspendLayout();
            MainFormSplitter.Panel2.SuspendLayout();
            MainFormSplitter.SuspendLayout();
            SuspendLayout();
            // 
            // MM
            // 
            MM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { MMFile, MMEdit, MMExport, MMTools, MMVersion });
            MM.Location = new System.Drawing.Point(0, 0);
            MM.Name = "MM";
            MM.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            MM.Size = new System.Drawing.Size(681, 24);
            MM.TabIndex = 0;
            MM.Text = "menuStrip1";
            // 
            // MMFile
            // 
            MMFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MMFileOpen, MMFileTryOpenUsingKeyList, MMFileSave, MMFileS1, MMFileNew, MMFileClose, MMFileS2, MMFileExit });
            MMFile.Name = "MMFile";
            MMFile.Size = new System.Drawing.Size(37, 20);
            MMFile.Text = "&File";
            // 
            // MMFileOpen
            // 
            MMFileOpen.Name = "MMFileOpen";
            MMFileOpen.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            MMFileOpen.Size = new System.Drawing.Size(203, 22);
            MMFileOpen.Text = "&Open ...";
            MMFileOpen.Click += MMFileOpen_Click;
            // 
            // MMFileTryOpenUsingKeyList
            // 
            MMFileTryOpenUsingKeyList.Name = "MMFileTryOpenUsingKeyList";
            MMFileTryOpenUsingKeyList.Size = new System.Drawing.Size(203, 22);
            MMFileTryOpenUsingKeyList.Text = "Try open using key list ...";
            MMFileTryOpenUsingKeyList.Visible = false;
            MMFileTryOpenUsingKeyList.Click += MMExtraTryOpenUsingKeyList_Click;
            // 
            // MMFileSave
            // 
            MMFileSave.Name = "MMFileSave";
            MMFileSave.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            MMFileSave.Size = new System.Drawing.Size(203, 22);
            MMFileSave.Text = "&Save now";
            MMFileSave.Click += MMFileSave_Click;
            // 
            // MMFileS1
            // 
            MMFileS1.Name = "MMFileS1";
            MMFileS1.Size = new System.Drawing.Size(200, 6);
            // 
            // MMFileNew
            // 
            MMFileNew.Name = "MMFileNew";
            MMFileNew.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
            MMFileNew.Size = new System.Drawing.Size(203, 22);
            MMFileNew.Text = "New ...";
            MMFileNew.Click += MMFileNew_Click;
            // 
            // MMFileClose
            // 
            MMFileClose.Name = "MMFileClose";
            MMFileClose.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W;
            MMFileClose.Size = new System.Drawing.Size(203, 22);
            MMFileClose.Text = "&Close";
            MMFileClose.Click += MMFileClose_Click;
            // 
            // MMFileS2
            // 
            MMFileS2.Name = "MMFileS2";
            MMFileS2.Size = new System.Drawing.Size(200, 6);
            MMFileS2.Click += MMFileS2_Click;
            // 
            // MMFileExit
            // 
            MMFileExit.Name = "MMFileExit";
            MMFileExit.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X;
            MMFileExit.Size = new System.Drawing.Size(203, 22);
            MMFileExit.Text = "E&xit";
            MMFileExit.Click += MMFileExit_Click;
            // 
            // MMEdit
            // 
            MMEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MMEditAddFile, MMEditReplace, MMEditFileProp, MMEditS2, MMEditDeleteSelected, MMEditS1, MMEditImportFiles });
            MMEdit.Name = "MMEdit";
            MMEdit.Size = new System.Drawing.Size(39, 20);
            MMEdit.Text = "&Edit";
            // 
            // MMEditAddFile
            // 
            MMEditAddFile.Name = "MMEditAddFile";
            MMEditAddFile.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A;
            MMEditAddFile.Size = new System.Drawing.Size(222, 22);
            MMEditAddFile.Text = "Add/Replace File ...";
            MMEditAddFile.Click += MMEditAddFile_Click;
            // 
            // MMEditReplace
            // 
            MMEditReplace.Name = "MMEditReplace";
            MMEditReplace.Size = new System.Drawing.Size(222, 22);
            MMEditReplace.Text = "Replace selected file ...";
            MMEditReplace.Visible = false;
            MMEditReplace.Click += MMEditReplace_Click;
            // 
            // MMEditFileProp
            // 
            MMEditFileProp.Name = "MMEditFileProp";
            MMEditFileProp.Size = new System.Drawing.Size(222, 22);
            MMEditFileProp.Text = "Edit File Properties ...";
            MMEditFileProp.Click += manualEditFileMD5ToolStripMenuItem_Click;
            // 
            // MMEditS2
            // 
            MMEditS2.Name = "MMEditS2";
            MMEditS2.Size = new System.Drawing.Size(219, 6);
            // 
            // MMEditDeleteSelected
            // 
            MMEditDeleteSelected.Name = "MMEditDeleteSelected";
            MMEditDeleteSelected.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            MMEditDeleteSelected.Size = new System.Drawing.Size(222, 22);
            MMEditDeleteSelected.Text = "Delete selected file ...";
            MMEditDeleteSelected.Click += MMEditDeleteSelected_Click;
            // 
            // MMEditS1
            // 
            MMEditS1.Name = "MMEditS1";
            MMEditS1.Size = new System.Drawing.Size(219, 6);
            // 
            // MMEditImportFiles
            // 
            MMEditImportFiles.Name = "MMEditImportFiles";
            MMEditImportFiles.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.A;
            MMEditImportFiles.Size = new System.Drawing.Size(222, 22);
            MMEditImportFiles.Text = "&Import Files ...";
            MMEditImportFiles.Click += MMEditImportFiles_Click;
            // 
            // MMExport
            // 
            MMExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MMExportSelectedFile, MMExportSelectedFolder, MMExportS1, MMExportAll, MMExportS2, MMExportDB, MMExportAsCsv });
            MMExport.Name = "MMExport";
            MMExport.Size = new System.Drawing.Size(52, 20);
            MMExport.Text = "&Export";
            // 
            // MMExportSelectedFile
            // 
            MMExportSelectedFile.Name = "MMExportSelectedFile";
            MMExportSelectedFile.ShortcutKeys = System.Windows.Forms.Keys.F5;
            MMExportSelectedFile.Size = new System.Drawing.Size(205, 22);
            MMExportSelectedFile.Text = "Selected &File ...";
            MMExportSelectedFile.Click += MMExportSelectedFile_Click;
            // 
            // MMExportSelectedFolder
            // 
            MMExportSelectedFolder.Enabled = false;
            MMExportSelectedFolder.Name = "MMExportSelectedFolder";
            MMExportSelectedFolder.Size = new System.Drawing.Size(205, 22);
            MMExportSelectedFolder.Text = "Selected F&older ...";
            MMExportSelectedFolder.Click += MMExportSelectedFolder_Click;
            // 
            // MMExportS1
            // 
            MMExportS1.Name = "MMExportS1";
            MMExportS1.Size = new System.Drawing.Size(202, 6);
            // 
            // MMExportAll
            // 
            MMExportAll.Name = "MMExportAll";
            MMExportAll.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5;
            MMExportAll.Size = new System.Drawing.Size(205, 22);
            MMExportAll.Text = "&All Files ...";
            MMExportAll.Click += MMExportAll_Click;
            // 
            // MMExportS2
            // 
            MMExportS2.Name = "MMExportS2";
            MMExportS2.Size = new System.Drawing.Size(202, 6);
            // 
            // MMExportDB
            // 
            MMExportDB.Name = "MMExportDB";
            MMExportDB.Size = new System.Drawing.Size(205, 22);
            MMExportDB.Text = "Export DB ...";
            MMExportDB.Click += MMExportDB_Click;
            // 
            // MMExportAsCsv
            // 
            MMExportAsCsv.Name = "MMExportAsCsv";
            MMExportAsCsv.Size = new System.Drawing.Size(205, 22);
            MMExportAsCsv.Text = "Export File Data as CSV ...";
            MMExportAsCsv.Click += MMExportAsCsv_Click;
            // 
            // MMTools
            // 
            MMTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MMToolsMakeMod, MMToolsS1, MMToolsCreatePatch, MMToolsApplyPatch, MMToolsS2, MMToolsMD5, MMToolsMD5All, toolStripMenuItem1, MMToolsConvertMenu, MMToolsPreview });
            MMTools.Name = "MMTools";
            MMTools.Size = new System.Drawing.Size(47, 20);
            MMTools.Text = "&Tools";
            MMTools.DropDownOpening += MMTools_DropDownOpening;
            // 
            // MMToolsMakeMod
            // 
            MMToolsMakeMod.Name = "MMToolsMakeMod";
            MMToolsMakeMod.Size = new System.Drawing.Size(234, 22);
            MMToolsMakeMod.Text = "Export pak as mod ...";
            MMToolsMakeMod.Click += MMExtraMakeMod_Click;
            // 
            // MMToolsS1
            // 
            MMToolsS1.Name = "MMToolsS1";
            MMToolsS1.Size = new System.Drawing.Size(231, 6);
            // 
            // MMToolsCreatePatch
            // 
            MMToolsCreatePatch.Name = "MMToolsCreatePatch";
            MMToolsCreatePatch.Size = new System.Drawing.Size(234, 22);
            MMToolsCreatePatch.Text = "Create patch file ...";
            MMToolsCreatePatch.Click += MMToolsCreatePatch_Click;
            // 
            // MMToolsApplyPatch
            // 
            MMToolsApplyPatch.Name = "MMToolsApplyPatch";
            MMToolsApplyPatch.Size = new System.Drawing.Size(234, 22);
            MMToolsApplyPatch.Text = "Apply patch file ...";
            MMToolsApplyPatch.Visible = false;
            // 
            // MMToolsS2
            // 
            MMToolsS2.Name = "MMToolsS2";
            MMToolsS2.Size = new System.Drawing.Size(231, 6);
            // 
            // MMToolsMD5
            // 
            MMToolsMD5.Name = "MMToolsMD5";
            MMToolsMD5.Size = new System.Drawing.Size(234, 22);
            MMToolsMD5.Text = "Re-Calculate MD5";
            MMToolsMD5.Click += MMEXtraMD5_Click;
            // 
            // MMToolsMD5All
            // 
            MMToolsMD5All.Name = "MMToolsMD5All";
            MMToolsMD5All.Size = new System.Drawing.Size(234, 22);
            MMToolsMD5All.Text = "Re-Calculate MD5 of all files ...";
            MMToolsMD5All.Click += MMExtraMD5All_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(231, 6);
            // 
            // MMToolsConvertMenu
            // 
            MMToolsConvertMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MMToolsConvertPak });
            MMToolsConvertMenu.Name = "MMToolsConvertMenu";
            MMToolsConvertMenu.Size = new System.Drawing.Size(234, 22);
            MMToolsConvertMenu.Text = "Convert Pak Type";
            MMToolsConvertMenu.DropDownOpening += MMToolsConvertMenu_DropDownOpening;
            // 
            // MMToolsConvertPak
            // 
            MMToolsConvertPak.Name = "MMToolsConvertPak";
            MMToolsConvertPak.Size = new System.Drawing.Size(110, 22);
            MMToolsConvertPak.Tag = "1";
            MMToolsConvertPak.Text = "Classic";
            MMToolsConvertPak.Click += MMToolsConvertPak_Click;
            // 
            // MMToolsPreview
            // 
            MMToolsPreview.Name = "MMToolsPreview";
            MMToolsPreview.Size = new System.Drawing.Size(234, 22);
            MMToolsPreview.Text = "Show Preview";
            MMToolsPreview.Click += MMToolsPreview_Click;
            // 
            // MMVersion
            // 
            MMVersion.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            MMVersion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MMVersionGetLatest, MMVersionS1, MMVersionSourceCode, MMVersionDiscord });
            MMVersion.Name = "MMVersion";
            MMVersion.Size = new System.Drawing.Size(57, 20);
            MMVersion.Text = "&Version";
            // 
            // MMVersionGetLatest
            // 
            MMVersionGetLatest.Name = "MMVersionGetLatest";
            MMVersionGetLatest.Size = new System.Drawing.Size(236, 22);
            MMVersionGetLatest.Text = "Check for Latest version online";
            MMVersionGetLatest.Click += MMVersionGetLatest_Click;
            // 
            // MMVersionS1
            // 
            MMVersionS1.Name = "MMVersionS1";
            MMVersionS1.Size = new System.Drawing.Size(233, 6);
            // 
            // MMVersionSourceCode
            // 
            MMVersionSourceCode.Name = "MMVersionSourceCode";
            MMVersionSourceCode.Size = new System.Drawing.Size(236, 22);
            MMVersionSourceCode.Text = "Source Code";
            MMVersionSourceCode.Click += MMVersionSourceCode_Click;
            // 
            // MMVersionDiscord
            // 
            MMVersionDiscord.Name = "MMVersionDiscord";
            MMVersionDiscord.Size = new System.Drawing.Size(236, 22);
            MMVersionDiscord.Text = "Visit Discord";
            MMVersionDiscord.Click += VisitDiscordToolStripMenuItem_Click;
            // 
            // openGamePakDialog
            // 
            openGamePakDialog.Filter = "Known pak file types|*_pak*;*_pak*.*;*.aamod;*.csv|ArcheAge Game Pak|*_pak;*_pak.*|CSV Files|*.csv|All Files|*.*";
            openGamePakDialog.ReadOnlyChecked = true;
            openGamePakDialog.RestoreDirectory = true;
            openGamePakDialog.ShowReadOnly = true;
            // 
            // lbFolders
            // 
            lbFolders.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lbFolders.FormattingEnabled = true;
            lbFolders.ItemHeight = 15;
            lbFolders.Location = new System.Drawing.Point(4, 7);
            lbFolders.Margin = new System.Windows.Forms.Padding(4);
            lbFolders.Name = "lbFolders";
            lbFolders.Size = new System.Drawing.Size(274, 349);
            lbFolders.TabIndex = 1;
            lbFolders.SelectedIndexChanged += lbFolders_SelectedIndexChanged;
            // 
            // lbFiles
            // 
            lbFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lbFiles.FormattingEnabled = true;
            lbFiles.ItemHeight = 15;
            lbFiles.Location = new System.Drawing.Point(7, 32);
            lbFiles.Margin = new System.Windows.Forms.Padding(4);
            lbFiles.Name = "lbFiles";
            lbFiles.Size = new System.Drawing.Size(363, 184);
            lbFiles.TabIndex = 4;
            lbFiles.SelectedIndexChanged += lbFiles_SelectedIndexChanged;
            // 
            // lFiles
            // 
            lFiles.AutoSize = true;
            lFiles.Location = new System.Drawing.Point(6, 7);
            lFiles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lFiles.Name = "lFiles";
            lFiles.Size = new System.Drawing.Size(30, 15);
            lFiles.TabIndex = 5;
            lFiles.Text = "Files";
            // 
            // pFileInfo
            // 
            pFileInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pFileInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pFileInfo.Controls.Add(lfiCreateTime);
            pFileInfo.Controls.Add(lfiModifyTime);
            pFileInfo.Controls.Add(lModifiedRaw);
            pFileInfo.Controls.Add(lCreateRaw);
            pFileInfo.Controls.Add(lfiIndex);
            pFileInfo.Controls.Add(lfiExtras);
            pFileInfo.Controls.Add(lfiStartOffset);
            pFileInfo.Controls.Add(lfiHash);
            pFileInfo.Controls.Add(lfiSize);
            pFileInfo.Controls.Add(lfiName);
            pFileInfo.Location = new System.Drawing.Point(7, 244);
            pFileInfo.Margin = new System.Windows.Forms.Padding(4);
            pFileInfo.Name = "pFileInfo";
            pFileInfo.Size = new System.Drawing.Size(363, 173);
            pFileInfo.TabIndex = 6;
            // 
            // lfiCreateTime
            // 
            lfiCreateTime.AutoSize = true;
            lfiCreateTime.Location = new System.Drawing.Point(4, 62);
            lfiCreateTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lfiCreateTime.Name = "lfiCreateTime";
            lfiCreateTime.Size = new System.Drawing.Size(39, 15);
            lfiCreateTime.TabIndex = 3;
            lfiCreateTime.Text = "create";
            // 
            // lfiModifyTime
            // 
            lfiModifyTime.AutoSize = true;
            lfiModifyTime.Location = new System.Drawing.Point(4, 81);
            lfiModifyTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lfiModifyTime.Name = "lfiModifyTime";
            lfiModifyTime.Size = new System.Drawing.Size(55, 15);
            lfiModifyTime.TabIndex = 5;
            lfiModifyTime.Text = "modified";
            // 
            // lModifiedRaw
            // 
            lModifiedRaw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lModifiedRaw.Location = new System.Drawing.Point(164, 81);
            lModifiedRaw.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lModifiedRaw.Name = "lModifiedRaw";
            lModifiedRaw.Size = new System.Drawing.Size(194, 15);
            lModifiedRaw.TabIndex = 9;
            lModifiedRaw.Text = "( )";
            lModifiedRaw.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lCreateRaw
            // 
            lCreateRaw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lCreateRaw.Location = new System.Drawing.Point(164, 62);
            lCreateRaw.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lCreateRaw.Name = "lCreateRaw";
            lCreateRaw.Size = new System.Drawing.Size(194, 15);
            lCreateRaw.TabIndex = 8;
            lCreateRaw.Text = "( )";
            lCreateRaw.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lfiIndex
            // 
            lfiIndex.AutoSize = true;
            lfiIndex.Location = new System.Drawing.Point(4, 150);
            lfiIndex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lfiIndex.Name = "lfiIndex";
            lfiIndex.Size = new System.Drawing.Size(35, 15);
            lfiIndex.TabIndex = 7;
            lfiIndex.Text = "index";
            // 
            // lfiExtras
            // 
            lfiExtras.AutoSize = true;
            lfiExtras.Location = new System.Drawing.Point(4, 133);
            lfiExtras.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lfiExtras.Name = "lfiExtras";
            lfiExtras.Size = new System.Drawing.Size(57, 15);
            lfiExtras.TabIndex = 6;
            lfiExtras.Text = "unknown";
            // 
            // lfiStartOffset
            // 
            lfiStartOffset.AutoSize = true;
            lfiStartOffset.Location = new System.Drawing.Point(4, 107);
            lfiStartOffset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lfiStartOffset.Name = "lfiStartOffset";
            lfiStartOffset.Size = new System.Drawing.Size(39, 15);
            lfiStartOffset.TabIndex = 4;
            lfiStartOffset.Text = "Offset";
            // 
            // lfiHash
            // 
            lfiHash.AutoSize = true;
            lfiHash.Location = new System.Drawing.Point(4, 44);
            lfiHash.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lfiHash.Name = "lfiHash";
            lfiHash.Size = new System.Drawing.Size(32, 15);
            lfiHash.TabIndex = 2;
            lfiHash.Text = "hash";
            // 
            // lfiSize
            // 
            lfiSize.AutoSize = true;
            lfiSize.Location = new System.Drawing.Point(4, 25);
            lfiSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lfiSize.Name = "lfiSize";
            lfiSize.Size = new System.Drawing.Size(27, 15);
            lfiSize.TabIndex = 1;
            lfiSize.Text = "Size";
            // 
            // lfiName
            // 
            lfiName.AutoSize = true;
            lfiName.Cursor = System.Windows.Forms.Cursors.Hand;
            lfiName.Location = new System.Drawing.Point(4, 0);
            lfiName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lfiName.Name = "lfiName";
            lfiName.Size = new System.Drawing.Size(39, 15);
            lfiName.TabIndex = 0;
            lfiName.Text = "Name";
            lfiName.Click += lfiName_Click;
            // 
            // exportFolderDialog
            // 
            exportFolderDialog.Description = "Select the destination folder to export all files to.";
            exportFolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // importFileDialog
            // 
            importFileDialog.AddExtension = false;
            importFileDialog.Filter = "Al Files|*.*";
            importFileDialog.RestoreDirectory = true;
            // 
            // tvFolders
            // 
            tvFolders.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tvFolders.Location = new System.Drawing.Point(7, 7);
            tvFolders.Margin = new System.Windows.Forms.Padding(4);
            tvFolders.Name = "tvFolders";
            tvFolders.Size = new System.Drawing.Size(267, 382);
            tvFolders.TabIndex = 7;
            tvFolders.AfterSelect += tvFolders_AfterSelect;
            // 
            // tcDirectoryViews
            // 
            tcDirectoryViews.Controls.Add(tpTreeView);
            tcDirectoryViews.Controls.Add(tpFlatDirView);
            tcDirectoryViews.Controls.Add(tpExtraFiles);
            tcDirectoryViews.Dock = System.Windows.Forms.DockStyle.Fill;
            tcDirectoryViews.Location = new System.Drawing.Point(0, 0);
            tcDirectoryViews.Margin = new System.Windows.Forms.Padding(4);
            tcDirectoryViews.Name = "tcDirectoryViews";
            tcDirectoryViews.SelectedIndex = 0;
            tcDirectoryViews.Size = new System.Drawing.Size(291, 428);
            tcDirectoryViews.TabIndex = 8;
            // 
            // tpTreeView
            // 
            tpTreeView.Controls.Add(tvFolders);
            tpTreeView.Location = new System.Drawing.Point(4, 24);
            tpTreeView.Margin = new System.Windows.Forms.Padding(4);
            tpTreeView.Name = "tpTreeView";
            tpTreeView.Padding = new System.Windows.Forms.Padding(4);
            tpTreeView.Size = new System.Drawing.Size(283, 400);
            tpTreeView.TabIndex = 0;
            tpTreeView.Text = "Tree View";
            tpTreeView.UseVisualStyleBackColor = true;
            // 
            // tpFlatDirView
            // 
            tpFlatDirView.Controls.Add(lbFolders);
            tpFlatDirView.Location = new System.Drawing.Point(4, 24);
            tpFlatDirView.Margin = new System.Windows.Forms.Padding(4);
            tpFlatDirView.Name = "tpFlatDirView";
            tpFlatDirView.Padding = new System.Windows.Forms.Padding(4);
            tpFlatDirView.Size = new System.Drawing.Size(283, 400);
            tpFlatDirView.TabIndex = 1;
            tpFlatDirView.Text = "Flat Folder View";
            tpFlatDirView.UseVisualStyleBackColor = true;
            // 
            // tpExtraFiles
            // 
            tpExtraFiles.Controls.Add(lbExtraFiles);
            tpExtraFiles.Location = new System.Drawing.Point(4, 24);
            tpExtraFiles.Margin = new System.Windows.Forms.Padding(4);
            tpExtraFiles.Name = "tpExtraFiles";
            tpExtraFiles.Padding = new System.Windows.Forms.Padding(4);
            tpExtraFiles.Size = new System.Drawing.Size(283, 400);
            tpExtraFiles.TabIndex = 2;
            tpExtraFiles.Text = "Deleted Files";
            tpExtraFiles.UseVisualStyleBackColor = true;
            // 
            // lbExtraFiles
            // 
            lbExtraFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lbExtraFiles.FormattingEnabled = true;
            lbExtraFiles.ItemHeight = 15;
            lbExtraFiles.Location = new System.Drawing.Point(4, 6);
            lbExtraFiles.Margin = new System.Windows.Forms.Padding(4);
            lbExtraFiles.Name = "lbExtraFiles";
            lbExtraFiles.Size = new System.Drawing.Size(274, 349);
            lbExtraFiles.TabIndex = 2;
            lbExtraFiles.SelectedIndexChanged += LbExtraFiles_SelectedIndexChanged;
            // 
            // openKeyListDialog
            // 
            openKeyListDialog.DefaultExt = "csv";
            openKeyListDialog.Filter = "CSV files|*.csv|All files|*.*";
            openKeyListDialog.RestoreDirectory = true;
            openKeyListDialog.Title = "Open Key List";
            // 
            // statusBar
            // 
            statusBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { pbGeneric, lFileCount, lPakExtraInfo, lTypePak });
            statusBar.Location = new System.Drawing.Point(0, 452);
            statusBar.Name = "statusBar";
            statusBar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusBar.Size = new System.Drawing.Size(681, 22);
            statusBar.TabIndex = 9;
            statusBar.ItemClicked += statusBar_ItemClicked;
            // 
            // pbGeneric
            // 
            pbGeneric.Name = "pbGeneric";
            pbGeneric.Size = new System.Drawing.Size(146, 16);
            pbGeneric.Visible = false;
            // 
            // lFileCount
            // 
            lFileCount.Name = "lFileCount";
            lFileCount.Size = new System.Drawing.Size(45, 17);
            lFileCount.Text = "no files";
            lFileCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lPakExtraInfo
            // 
            lPakExtraInfo.Name = "lPakExtraInfo";
            lPakExtraInfo.Size = new System.Drawing.Size(603, 17);
            lPakExtraInfo.Spring = true;
            lPakExtraInfo.Text = "...";
            lPakExtraInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lTypePak
            // 
            lTypePak.Name = "lTypePak";
            lTypePak.Size = new System.Drawing.Size(16, 17);
            lTypePak.Text = "...";
            lTypePak.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainFormSplitter
            // 
            MainFormSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            MainFormSplitter.Location = new System.Drawing.Point(0, 24);
            MainFormSplitter.Margin = new System.Windows.Forms.Padding(4);
            MainFormSplitter.Name = "MainFormSplitter";
            // 
            // MainFormSplitter.Panel1
            // 
            MainFormSplitter.Panel1.Controls.Add(tcDirectoryViews);
            MainFormSplitter.Panel1MinSize = 250;
            // 
            // MainFormSplitter.Panel2
            // 
            MainFormSplitter.Panel2.Controls.Add(lFiles);
            MainFormSplitter.Panel2.Controls.Add(lbFiles);
            MainFormSplitter.Panel2.Controls.Add(pFileInfo);
            MainFormSplitter.Panel2MinSize = 300;
            MainFormSplitter.Size = new System.Drawing.Size(681, 428);
            MainFormSplitter.SplitterDistance = 291;
            MainFormSplitter.SplitterWidth = 5;
            MainFormSplitter.TabIndex = 10;
            // 
            // saveGamePakDialog
            // 
            saveGamePakDialog.AddToRecent = false;
            saveGamePakDialog.Filter = "Known pak file types|*_pak;*_pak.*;*.aamod;*.csv|ArcheAge Game Pak|*_pak;*_pak.*|All Files|*.*";
            saveGamePakDialog.RestoreDirectory = true;
            saveGamePakDialog.Title = "Save pak file";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(681, 474);
            Controls.Add(MainFormSplitter);
            Controls.Add(statusBar);
            Controls.Add(MM);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = MM;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "MainForm";
            Text = "AAPakEditor";
            FormClosed += MainForm_FormClosed;
            Load += MainForm_Load;
            Shown += MainForm_Shown;
            MM.ResumeLayout(false);
            MM.PerformLayout();
            pFileInfo.ResumeLayout(false);
            pFileInfo.PerformLayout();
            tcDirectoryViews.ResumeLayout(false);
            tpTreeView.ResumeLayout(false);
            tpFlatDirView.ResumeLayout(false);
            tpExtraFiles.ResumeLayout(false);
            statusBar.ResumeLayout(false);
            statusBar.PerformLayout();
            MainFormSplitter.Panel1.ResumeLayout(false);
            MainFormSplitter.Panel2.ResumeLayout(false);
            MainFormSplitter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MainFormSplitter).EndInit();
            MainFormSplitter.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.SaveFileDialog saveGamePakDialog;
    }
}

