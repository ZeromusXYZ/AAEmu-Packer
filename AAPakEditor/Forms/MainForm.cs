using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using AAPacker;
using AAPakEditor.Properties;

namespace AAPakEditor.Forms;

public partial class MainForm : Form
{
    private string _baseTitle = "";
    private string _currentFileViewFolder = "";

    private readonly byte[] _customKey = new byte[16]
        { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

    private readonly byte[] _dbKey = new byte[16]
        { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

    private readonly List<FileListEntry> _fileListEntries = new();
    public AAPak Pak;
    private readonly string _urlDiscord = "https://discord.gg/GhVfDtK";
    private readonly string _urlGitHub = "https://github.com/ZeromusXYZ/AAEmu-Packer";
    private bool _useCustomKey;
    private bool _useDbKey;

    public MainForm()
    {
        InitializeComponent();
    }

    private void UpdateMm()
    {
        MMFileSave.Enabled = Pak != null && Pak.IsOpen && !Pak.ReadOnly && Pak.IsDirty;
        MMFileClose.Enabled = Pak != null && Pak.IsOpen;

        MMEditAddFile.Enabled = Pak != null && Pak.IsOpen && Pak.ReadOnly == false && !Pak.IsVirtual;
        MMEditImportFiles.Enabled = Pak != null && Pak.IsOpen && Pak.ReadOnly == false && !Pak.IsVirtual;
        MMEditDeleteSelected.Enabled = Pak != null && Pak.IsOpen && Pak.ReadOnly == false && lbFiles.SelectedIndex >= 0 && !Pak.IsVirtual;
        MMEditReplace.Enabled = Pak != null && Pak.IsOpen && !Pak.IsVirtual && Pak.ReadOnly == false && lbFiles.SelectedIndex >= 0;
        MMEditFileProp.Enabled = Pak != null && Pak.IsOpen && !Pak.IsVirtual && Pak.ReadOnly == false && lbFiles.SelectedIndex >= 0;
        MMEdit.Visible = Pak != null && Pak.IsOpen && !Pak.IsVirtual && Pak.ReadOnly == false;

        MMExportSelectedFile.Enabled = Pak != null && Pak.IsOpen && !Pak.IsVirtual && lbFiles.SelectedIndex >= 0;
        MMExportSelectedFolder.Enabled = Pak != null && Pak.IsOpen && !Pak.IsVirtual && _currentFileViewFolder != "";
        MMExportAll.Enabled = Pak != null && Pak.IsOpen && !Pak.IsVirtual;
        MMExportDB.Enabled = Pak != null && Pak.IsOpen && !Pak.IsVirtual && lbFiles.SelectedIndex >= 0 && _useDbKey && Path.GetExtension(lbFiles.SelectedItem.ToString()).StartsWith(".sql");
        MMExportDB.Visible = Pak != null && Pak.IsOpen && !Pak.IsVirtual && _useDbKey;
        MMExportS2.Visible = MMExportDB.Visible;
        MMExport.Visible = Pak != null && Pak.IsOpen && !Pak.IsVirtual;

        MMExtraMD5.Enabled = Pak != null && Pak.IsOpen && !Pak.IsVirtual && Pak.ReadOnly == false &&
                             lbFiles.SelectedIndex >= 0;
        MMExtraExportData.Enabled = Pak != null && Pak.IsOpen && !Pak.IsVirtual;
        MMExtraMakeMod.Enabled = Pak != null && Pak.IsOpen && !Pak.IsVirtual;
        MMExtra.Visible = Pak != null && Pak.IsOpen;

        if (Pak != null && Pak.IsOpen)
        {
            if (Pak.IsDirty)
                Text = _baseTitle + @" - *" + Pak.GpFilePath;
            else
                Text = _baseTitle + @" - " + Pak.GpFilePath;
            lPakExtraInfo.Text = Pak.NewestFileDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
        else
        {
            Text = _baseTitle;
            lPakExtraInfo.Text = @"...";
        }
    }


    private void MMFileExit_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void MMFileOpen_Click(object sender, EventArgs e)
    {
        openGamePakDialog.CheckFileExists = false;
        openGamePakDialog.ShowReadOnly = true;
        openGamePakDialog.Title = "Open EXISTING Pak file";
        if (openGamePakDialog.ShowDialog() == DialogResult.OK)
        {
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            LoadPakFile(openGamePakDialog.FileName, openGamePakDialog.ReadOnlyChecked);
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        UpdateMm();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        _baseTitle = Text;
        var appVer = Assembly.GetExecutingAssembly().GetName().Version;
        var v = "Version ";
        v += appVer.Major.ToString();
        v += "." + appVer.Minor;
        if (appVer.Build > 0 || appVer.MinorRevision > 0)
            v += "." + appVer.Build;
        if (appVer.MinorRevision > 0)
            v += "." + appVer.MinorRevision;
        MMVersion.Text = v;
        UpdateMm();
        ShowFileInfo(null);
    }

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        if (Pak != null && Pak.IsOpen)
        {
            Pak.ClosePak();
            Pak = null;
        }

        UpdateMm();
    }

    private void GenerateFolderViews()
    {
        lFileCount.Text = "Analyzing folder structure ... ";
        //lFileCount.Refresh();

        Pak.GenerateFolderList();

        lFileCount.Text = "Loading ... ";
        //lFileCount.Refresh();

        lbFolders.Items.Clear();
        lbFiles.Items.Clear();
        tvFolders.Nodes.Clear();
        lbExtraFiles.Items.Clear();
        var rootNode = tvFolders.Nodes.Add("", "<root>");
        TreeNode foundNode = null;
        var c = 0;
        lbFolders.Items.Add("<root>");
        foreach (var s in Pak.Folders)
        {
            lbFolders.Items.Add(s);
            c++;
            if (c % 250 == 0)
                lFileCount.Text = "Loading folders ... " + c + " / " + Pak.Folders.Count;
            //lFileCount.Refresh();
            //Thread.Sleep(1);


            if (s != "")
            {
                var dirWalk = s.Split('/');
                var dd = "";
                var lastNode = rootNode;
                foreach (var ds in dirWalk)
                {
                    if (dd != "")
                        dd += "/";
                    dd += ds;

                    foundNode = null;
                    foreach (TreeNode n in lastNode.Nodes)
                        if (n.Name == dd)
                        {
                            foundNode = n;
                            break;
                        }

                    //TreeNode[] nsearch = lastNode.Nodes.Find(ds, false);
                    // if (nsearch.Length <= 0)
                    if (foundNode == null)
                        // No node for this yet, make one
                        lastNode = lastNode.Nodes.Add(dd, ds);
                    else
                        lastNode = foundNode;
                }
            }
        }

        rootNode.Expand();
        lFileCount.Text = Pak.Files.Count + " files in " + Pak.Folders.Count + " folders";
        foreach (var pfi in Pak.ExtraFiles) lbExtraFiles.Items.Add(pfi.Name);
        UpdateMm();
    }

    private void LoadPakKeys(string fromDirectory)
    {
        _useDbKey = false;
        _useCustomKey = false;
        string fn;

        // DB key
        fn = fromDirectory + "game_db.key";
        if (File.Exists(fn))
        {
            var fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
            if (fs.Length != 16)
            {
                fs.Dispose();
                return;
            }

            var amountRead = fs.Read(_dbKey, 0, 16);
            fs.Dispose();
            _useDbKey = amountRead == 16;
        }

        // PAK-Header Key
        fn = fromDirectory + "game_pak.key";
        if (File.Exists(fn))
        {
            var fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
            if (fs.Length != 16)
            {
                fs.Dispose();
                return;
            }

            var amountRead = fs.Read(_customKey, 0, 16);
            fs.Dispose();
            _useCustomKey = amountRead == 16;
            Pak.SetCustomKey(_customKey);
        }
    }

    private void LoadPakFile(string filename, bool openAsReadOnly, bool showWriteWarning = true, bool quickLoad = false)
    {
        lTypePak.Text = string.Empty;
        Pak ??= new AAPak("");
        if (Pak.IsOpen)
        {
            lFileCount.Text = "Closing pak ... ";
            //lFileCount.Refresh();
            Pak.ClosePak();
        }

        lFileCount.Text = "Opening Pak ... ";
        //lFileCount.Refresh();
        try
        {
            LoadPakKeys(Path.GetDirectoryName(filename)?.TrimEnd(Path.DirectorySeparatorChar) +
                        Path.DirectorySeparatorChar);
        }
        catch
        {
            MessageBox.Show("Error loading custom keys");
        }

        var res = Pak.OpenPak(filename, openAsReadOnly);
        if (!res)
        {
            Text = _baseTitle;
            lFileCount.Text = "no files";
            lbFolders.Items.Clear();
            lbFiles.Items.Clear();
            UpdateMm();
            if (_useCustomKey)
                MessageBox.Show("Custom  game_pak.key  does not seem valid for " + filename, "OpenPak Key Error");
            else
                MessageBox.Show("Failed to open " + filename + "\r\n" + Pak?.LastError, "OpenPak Error");
        }
        else
        {
            Text = _baseTitle + " - " + Pak.GpFilePath;


            if (!quickLoad)
                GenerateFolderViews();

            // Only show this waring if this is not a new pak file
            if (openAsReadOnly == false && Pak.Files.Count > 0 && showWriteWarning)
                MessageBox.Show("!!! Warning !!!\r\n" +
                                "You have opened this pak in read/write mode !\r\n" +
                                "\r\n" +
                                "This program comes with absolutly NO warranty.\r\n" +
                                "It is possible that this program will inreversably damage your game files while editing.\r\n" +
                                "Please be sure that you have a backup available of the pak file you are editing.\r\n" +
                                "That being said, I did my best as to avoid possible damage (other than odered by the user) caused by malfunctions.\r\n" +
                                "\r\n" +
                                "Also, I am in no way responsible for possible damage to the game or the account you will be playing as.\r\n" +
                                "There are systems in place on the live servers to check the validity of the game files. \r\n" +
                                "Please consider that chaning files as you can potentially get your account banned !\r\n" +
                                "\r\n" +
                                "Enjoy, and edit responsibly.\r\n" +
                                "~ ZeromusXYZ",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        if (Pak.PakType == PakFileType.Custom)
            lTypePak.Text = Pak.Reader?.ReaderName ?? "Invalid Reader";
        else
            lTypePak.Text = string.Empty;
    }

    private void lbFolders_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbFiles.Items.Clear();
        if (Pak == null || !Pak.IsOpen)
            return;

        var d = (sender as ListBox)?.SelectedItem.ToString();
        if (d == "<root>")
            d = string.Empty;
        PopulateFilesList(d);
        UpdateMm();
    }

    private void ShowFileInfo(AAPakFileInfo pfi)
    {
        if (pfi != null)
        {
            lfiName.Text = pfi.Name;
            lfiSize.Text = "Size: " + pfi.Size + " byte(s)";
            if (pfi.PaddingSize > 0)
                lfiSize.Text += "  + " + pfi.PaddingSize + " padding";

            if (pfi.SizeDuplicate != pfi.Size)
                lfiSize.Text += "  Size mismatch " + pfi.SizeDuplicate + " byte(s)";

            //var h = BitConverter.ToString(pfi.Md5).ToUpper().Replace("-", "");
            //if (h == pak._header.nullHashString)
            if (pfi.Md5.SequenceEqual(AAPakFileHeader.NullHash))
                lfiHash.Text = "MD5: Invalid or not calculated !";
            else
                lfiHash.Text = "MD5: " + BitConverter.ToString(pfi.Md5).ToUpper().Replace("-", "");
            try
            {
                lCreateRaw.Text = "(" + pfi.CreateTime + ")";
                if (pfi.CreateTime != 0)
                    lfiCreateTime.Text = "Created: " + DateTime.FromFileTime(pfi.CreateTime);
                else
                    lfiCreateTime.Text = "<Created time not used>";
            }
            catch
            {
                lfiCreateTime.Text = "CreateTime Invalid";
            }

            try
            {
                lModifiedRaw.Text = "(" + pfi.ModifyTime + ")";
                if (pfi.ModifyTime != 0)
                    lfiModifyTime.Text = "Modified: " + DateTime.FromFileTime(pfi.ModifyTime);
                else
                    lfiModifyTime.Text = "<Modified time not used>";
            }
            catch
            {
                lfiModifyTime.Text = "ModifiedTime Invalid (" + pfi.ModifyTime + ")";
            }

            lfiStartOffset.Text = "Start Offset: 0x" + pfi.Offset.ToString("X16");
            lfiExtras.Text = "D1 0x" + pfi.Dummy1.ToString("X") + "  D2 0x" + pfi.Dummy2.ToString("X");
            if (pfi.EntryIndexNumber >= 0)
                lfiIndex.Text = "index: " + pfi.EntryIndexNumber;
            else if (pfi.DeletedIndexNumber >= 0) lfiIndex.Text = "extra-index: " + pfi.DeletedIndexNumber;
        }
        else
        {
            lfiName.Text = "<no file selected>";
            lfiSize.Text = "";
            lfiHash.Text = "";
            lfiCreateTime.Text = "";
            lfiModifyTime.Text = "";
            lfiStartOffset.Text = "";
            lfiExtras.Text = "";
            lfiIndex.Text = "";
            lCreateRaw.Text = "";
            lModifiedRaw.Text = "";
        }
    }

    private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;

        if (lbFiles.SelectedIndex < 0)
            return;

        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;

        var d = _currentFileViewFolder;
        if (d != "") d += "/";
        d += lbFiles.SelectedItem.ToString();

        try
        {
            if (lbFiles.SelectedItem is FileListEntry fle) 
                ShowFileInfo(fle.Pfi);
        }
        catch
        {
            if (Pak.GetFileByName(d, out var pfi)) 
                ShowFileInfo(pfi);
        }

        UpdateMm();

        Cursor.Current = Cursors.Default;
        Application.UseWaitCursor = false;
    }

    private void MMExportSelectedFile_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;

        if (lbFiles.SelectedIndex < 0)
        {
            MessageBox.Show("No file selected");
            return;
        }

        var d = _currentFileViewFolder;
        if (d != "") d += "/";
        //fileListEntry fle = null;

        if (lbFiles.SelectedItem is FileListEntry fle)
        {
            d += fle.DisplayName;
        }
        else
        {
            fle = null;
            d += lbFiles.SelectedItem?.ToString();
        }

        if (fle == null)
        {
            MessageBox.Show("Error selecting file, please contact report this error.");
            return;
        }

        try
        {
            exportFileDialog.FileName = Path.GetFileName(d.Replace('/', Path.DirectorySeparatorChar));
        }
        catch
        {
            exportFileDialog.FileName = "__invalid_name__";
        }

        if (exportFileDialog.ShowDialog() == DialogResult.OK)
            if (!ExportFile(fle.Pfi, exportFileDialog.FileName))
                //  if (!ExportFile(d, exportFileDialog.FileName))
                MessageBox.Show("Failed to export " + d);
    }

    public bool ExportFile(AAPakFileInfo pfi, string destName)
    {
        try
        {
            // Save file stream
            var filePakStream = Pak.ExportFileAsStream(pfi);
            var fs = new FileStream(destName, FileMode.Create);
            filePakStream.Position = 0;

            filePakStream.CopyTo(fs);

            filePakStream.Dispose();
            fs.Dispose();

            // Update file details
            if (pfi.CreateTime != 0)
                File.SetCreationTime(destName, DateTime.FromFileTimeUtc(pfi.CreateTime));
            if (pfi.ModifyTime != 0)
                File.SetLastWriteTime(destName, DateTime.FromFileTimeUtc(pfi.ModifyTime));
        }
        catch
        {
            return false;
        }

        return true;
    }

    public bool ExportFile(string sourceName, string destName)
    {
        if (Pak.GetFileByName(sourceName, out var pfi))
            return ExportFile(pfi, destName);
        return false;
    }

    private void MMExportAll_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;

        exportFolderDialog.Description = "Select a folder to where to export all files to";
        exportFolderDialog.SelectedPath = Path.GetDirectoryName(Pak.GpFilePath);
        if (exportFolderDialog.ShowDialog() != DialogResult.OK)
            return;

        if (MessageBox.Show(
                "Are you sure you want to export all the files ?\r\nAll files in destination will be overwritten !",
                "Export All", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;

        var export = new ExportAllDlg();
        export.pak = Pak;
        export.TargetDir = exportFolderDialog.SelectedPath;
        export.Text = "Export All Files";
        export.masterRoot = "";

        try
        {
            export.ShowDialog(this);
        }
        catch (Exception x)
        {
            MessageBox.Show("ERROR: " + x.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        Cursor.Current = Cursors.Default;
        Application.UseWaitCursor = false;
    }

    private void MMEXtraMD5_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;

        if (lbFiles.SelectedIndex < 0)
        {
            MessageBox.Show("No file selected");
            return;
        }

        var d = _currentFileViewFolder;
        if (d != "") d += "/";
        d += lbFiles.SelectedItem.ToString();

        if (Pak.GetFileByName(d, out var pfi))
            MessageBox.Show("MD5 Hash updated to " + Pak.UpdateMd5(pfi));
        else
            MessageBox.Show("ERROR: No file");
        UpdateMm();
    }

    private void MMFileSave_Click(object sender, EventArgs e)
    {
        if (!Pak.ReadOnly && Pak.IsDirty)
        {
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            Pak.SaveHeader();
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        UpdateMm();
    }


    private void MMExtraExportData_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;
        CreateCsvFile();
    }

    private void CreateCsvFile(string filename = "")
    {
        var newest = new DateTime(1600, 1, 1);

        var sl = new List<string>();
        var s = "";
        s = "Name";
        s += ";Size";
        s += ";Offset";
        s += ";Md5";
        s += ";CreateTime";
        s += ";ModifyTime";
        s += ";SizeDuplicate";
        s += ";PaddingSize";
        s += ";Dummy1";
        s += ";Dummy2";
        sl.Add(s);
        foreach (var pfi in Pak.Files)
        {
            var modTime = DateTime.FromFileTimeUtc(pfi.ModifyTime);
            if (modTime > newest)
                newest = modTime;

            s = pfi.Name;
            s += ";" + pfi.Size;
            s += ";" + pfi.Offset;
            s += ";" + BitConverter.ToString(pfi.Md5).Replace("-", "").ToUpper();
            s += ";" + AAPak.DateTimeToDateTimeStr(
                DateTime.FromFileTimeUtc(pfi.CreateTime)); // .ToString("yyyy-MM-dd HH:mm:ss");
            s += ";" + AAPak.DateTimeToDateTimeStr(modTime); // .ToString("yyyy-MM-dd HH:mm:ss");
            s += ";" + pfi.SizeDuplicate;
            s += ";" + pfi.PaddingSize;
            s += ";" + pfi.Dummy1;
            s += ";" + pfi.Dummy2;
            sl.Add(s);
        }

        exportFileDialog.FileName =
            Path.GetFileName(Pak.GpFilePath) + "_files_" + newest.ToString("yyyyMMdd") + ".csv";
        if (filename == string.Empty)
        {
            if (exportFileDialog.ShowDialog() != DialogResult.OK)
                return;
            filename = exportFileDialog.FileName;
        }

        File.WriteAllLines(filename, sl);
    }

    private void MMEditReplace_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen || lbFiles.SelectedIndex < 0)
            return;

        if (Pak.ReadOnly)
        {
            MessageBox.Show("Pak is opened in Read-Only mode, cannot add/replace files.");
            return;
        }

        var filename = _currentFileViewFolder;
        if (filename != "") filename += "/";
        filename += lbFiles.SelectedItem.ToString();

        FileListEntry fle = null;
        try
        {
            fle = lbFiles.SelectedItem as FileListEntry;
        }
        catch
        {
            MessageBox.Show("It looks like there was no file selected to replace !");
            return;
        }

        var pfi = fle.Pfi;

        /*
        ref AAPakFileInfo pfi = ref pak.nullAAPakFileInfo;

        if (!pak.GetFileByName(filename, ref pfi))
            return;
        */

        var maxSize = pfi.Size + pfi.PaddingSize;

        importFileDialog.FileName = lbFiles.SelectedItem.ToString();
        if (importFileDialog.ShowDialog() != DialogResult.OK)
            return;

        // DateTime CreateTime = File.GetCreationTime(importFileDialog.FileName);
        var modifyTime = File.GetLastWriteTime(importFileDialog.FileName);

        try
        {
            var fs = new FileStream(importFileDialog.FileName, FileMode.Open, FileAccess.Read);
            if (fs.Length > maxSize)
            {
                fs.Dispose();
                MessageBox.Show(
                    $"File is too big!\r\n{filename}\r\nCan only be replaced with a file with the \r\nmaximum Size of {maxSize} bytes");
            }

            fs.Position = 0;
            if (Pak.ReplaceFile(ref pfi, fs, modifyTime) == false)
                MessageBox.Show($"Failed to replace file !\r\n{filename}\r\nPak or File might be damaged !!");
            fs.Dispose();
        }
        catch (Exception ex)
        {
            MessageBox.Show("ERROR: " + ex.Message);
        }

        UpdateMm();
    }

    private void PrepareFileListView()
    {
        _fileListEntries.Clear();
        lbFiles.Items.Clear();
    }

    private void AddFileListEntry(AAPakFileInfo pfi, string displayName, bool isDeletedList)
    {
        var fle = new FileListEntry();
        fle.DisplayName = displayName;
        fle.Pfi = pfi;
        fle.IsDeletedFile = isDeletedList;
        _fileListEntries.Add(fle);
    }

    private void FinalizeFileListView()
    {
        lbFiles.Items.Clear();
        _fileListEntries.Sort();
        foreach (var fle in _fileListEntries)
            lbFiles.Items.Add(fle);
    }

    private void PopulateFilesList(string withdir)
    {
        _currentFileViewFolder = withdir;

        PrepareFileListView();
        //lbFiles.Items.Clear();
        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;

        var list = Pak.GetFilesInDirectory(_currentFileViewFolder);
        if (_currentFileViewFolder == "")
            lFiles.Text = list.Count + " files in <root>";
        else
            lFiles.Text = list.Count + " files in \"" + _currentFileViewFolder + "\"";
        //List<string> sl = new List<string>();
        foreach (var pfi in list)
        {
            var f = string.Empty;
            try
            {
                f = Path.GetFileName(pfi.Name);
            }
            catch
            {
                f = "__invalid_name_" + pfi.EntryIndexNumber + "__";
            }

            AddFileListEntry(pfi, f, false);

            // lbFiles.Items.Add(f);
            //sl.Add(f);
        }

        FinalizeFileListView();
        //sl.Sort();
        //lbFiles.Items.AddRange(sl.ToArray());
        Cursor.Current = Cursors.Default;
        Application.UseWaitCursor = false;
        UpdateMm();
    }

    private void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;


        if (e == null || e.Node == null)
            return;

        PopulateFilesList(e.Node.Name);
        e.Node.Expand();
        UpdateMm();
    }

    private void MMExtraDebugTest_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;


        var d = _currentFileViewFolder;
        if (d != "") d += "/";
        d += lbFiles.SelectedItem.ToString();
        exportFileDialog.FileName = Path.GetFileName(d.Replace('/', Path.DirectorySeparatorChar));

        if (exportFileDialog.ShowDialog() != DialogResult.OK)
            return;

        var pfraw = Pak.ExportFileAsStream(d);

        var fs = new FileStream(exportFileDialog.FileName, FileMode.Create);

        var pf = new MemoryStream();
        pfraw.CopyTo(pf);

        // Padding
        while (pf.Length % 16 != 0)
            pf.WriteByte(0);

        pf.Position = 0;

        var fsraw = new MemoryStream();
        try
        {
            if (AAPakFileHeader.EncryptStreamAes(pf, fsraw, _dbKey, false, true))
            {
                fsraw.Position = 16;
                fsraw.CopyTo(fs);
                MessageBox.Show("ExportDB: Done", "Export DB");
            }
            else
            {
                MessageBox.Show("Decryption failed:\r\n" + AAPakFileHeader.LastAesError, "Error");
            }
        }
        catch (Exception x)
        {
            MessageBox.Show("Exception: " + x.Message);
        }

        fs.Dispose();
        UpdateMm();
    }

    private void MMEditDeleteSelected_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen || lbFiles.SelectedIndex < 0)
            return;

        if (Pak.ReadOnly)
        {
            MessageBox.Show("Pak is opened in Read-Only mode, cannot delete files.");
            return;
        }

        if (lbFiles.SelectedIndex < 0)
        {
            MessageBox.Show("Nothing selected to delete.");
            return;
        }

        var filename = _currentFileViewFolder;
        if (filename != "") filename += "/";
        filename += lbFiles.SelectedItem.ToString();

        if (!(lbFiles.SelectedItem is FileListEntry fle))
        {
            MessageBox.Show($"Invalid file entry for\r\n" + filename, "Delete", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        /*
        ref AAPakFileInfo pfi = ref pak.nullAAPakFileInfo;
        if (!pak.GetFileByName(filename, ref pfi))
            return;
        */

        if (MessageBox.Show("Are you sure you want to delete this file ?\r\n" + filename, "Delete",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        if (Pak.DeleteFile(fle.Pfi)) MessageBox.Show("Reference to " + filename + " has been removed from the pak.");

        if (lbFiles.Items.Count <= 1)
            // If this was the last file listed in the directory listing, we will need to re-populate the folder views to update this change
            GenerateFolderViews();
        lbFiles.Items.Clear();
        PopulateFilesList(_currentFileViewFolder);

        UpdateMm();
    }

    private void MMEditAddFile_Click(object sender, EventArgs e)
    {
        // open add dialog
        var addDlg = new AddFileDialog();
        addDlg.Pak = Pak;

        if (_currentFileViewFolder != "")
            addDlg.suggestedDir = _currentFileViewFolder + '/';

        if (addDlg.ShowDialog(this) == DialogResult.OK)
        {
            var diskFileName = addDlg.eDiskFileName.Text;
            var pakFileName = addDlg.ePakFileName.Text.ToLower()
                .Replace("\\", "/"); // You just know people will put this wrong, so preemptive replace
            addDlg.Dispose();

            // virtual directory to the new file
            string newPakFilePath;
            try
            {
                newPakFilePath = Path.GetDirectoryName(pakFileName.Replace("/", "\\"))?.Replace("\\", "/") ?? string.Empty;
            }
            catch
            {
                newPakFilePath = string.Empty;
            }

            if (File.Exists(diskFileName))
            {
                var sourceIsDbFile = Path.GetExtension(diskFileName).ToLower() == ".sqlite3";

                var doEncrypt = false;
                if (sourceIsDbFile && _useDbKey)
                    if (MessageBox.Show(
                            "Import of DB detected, do you want to encrypt before importing using the provided key ?",
                            "Import as DB", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        doEncrypt = true;


                var cTime = File.GetCreationTime(diskFileName);
                var mTime = File.GetLastWriteTime(diskFileName);
                Stream fs;
                var fStream = new FileStream(diskFileName, FileMode.Open, FileAccess.Read);
                var mStream = new MemoryStream();

                if (doEncrypt)
                {
                    while (fStream.Position < fStream.Length)
                    {
                        var rest = (int)(fStream.Length - fStream.Position);
                        // reading blocks doesn't end too well, comment for now :p
                        /*
                        if (rest > 4096)
                            rest = 4096;
                        */
                        var readBuffer = new byte[rest];
                        var readAmount = fStream.Read(readBuffer, 0, rest);
                        var decryptedBuffer = AAPakFileHeader.EncryptAes(readBuffer, _dbKey, true);
                        mStream.Write(decryptedBuffer, 0, decryptedBuffer.Length);
                    }

                    fStream.Dispose();
                    mStream.Position = 0;
                    fs = mStream;
                }
                else
                {
                    mStream.Dispose();
                    fs = fStream;
                }

                var res = Pak.AddFileFromStream(pakFileName, fs, cTime, mTime, true, out var pfi);
                fs.Dispose();
                if (res)
                {
                    MessageBox.Show("File:\r\n" + diskFileName + "\r\n\r\nadded as:\r\n" + pfi.Name);

                    if (Pak.Folders.IndexOf(newPakFilePath) < 0)
                        // We added to a new folder
                        GenerateFolderViews();

                    lbFolders.SelectedIndex = lbFolders.Items.IndexOf(newPakFilePath);
                    tvFolders.SelectedNode = tvFolders.Nodes.Find(newPakFilePath, true)[0];

                    PopulateFilesList(newPakFilePath);
                }
                else
                {
                    MessageBox.Show("Failed to add file: " + diskFileName);
                }
            }
            else
            {
                MessageBox.Show("File not found: " + diskFileName);
            }
        }
        else
        {
            addDlg.Dispose();
        }

        PopulateFilesList(_currentFileViewFolder);
        UpdateMm();
    }

    private void MMFileClose_Click(object sender, EventArgs e)
    {
        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;
        lbFiles.Items.Clear();
        lbFolders.Items.Clear();
        tvFolders.Nodes.Clear();
        lFiles.Text = "";
        lFileCount.Text = "Closing pak";
        //lFileCount.Refresh();
        Pak.ClosePak();
        lFileCount.Text = "Pak Closed";
        Cursor.Current = Cursors.Default;
        Application.UseWaitCursor = false;
        UpdateMm();
        ShowFileInfo(null);
    }

    private void MMFileNew_Click(object sender, EventArgs e)
    {
        openGamePakDialog.CheckFileExists = false;
        openGamePakDialog.ShowReadOnly = false;
        openGamePakDialog.Title = "Create NEW Pak file";
        if (openGamePakDialog.ShowDialog() != DialogResult.OK)
            return;

        if (File.Exists(openGamePakDialog.FileName))
            if (MessageBox.Show("Overwrite and erase the existing pak file ?\r\n" + openGamePakDialog.FileName,
                    "Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;
        Pak?.ClosePak();
        // Create and a new pakFile
        Pak = new AAPak(openGamePakDialog.FileName, false, true);
        // TODO: Add Pak-type selector (specialized dialog box)
        Pak.ClosePak();
        // Re-open it in read/write mode
        LoadPakFile(openGamePakDialog.FileName, false);

        Cursor.Current = Cursors.Default;
        Application.UseWaitCursor = false;
        UpdateMm();
    }

    private void MMExportSelectedFolder_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;

        exportFolderDialog.Description = "Select a folder to where to export \r\n" + _currentFileViewFolder;
        exportFolderDialog.SelectedPath = Path.GetDirectoryName(Pak.GpFilePath);
        if (exportFolderDialog.ShowDialog() != DialogResult.OK)
            return;

        if (MessageBox.Show(
                "Are you sure you want to export \"" + _currentFileViewFolder +
                "\" and all of it's sub-folders ?\r\nAll files in destination will be overwritten !", "Export Folder",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;

        var export = new ExportAllDlg();
        export.pak = Pak;
        export.TargetDir = exportFolderDialog.SelectedPath;
        export.masterRoot = _currentFileViewFolder + "/";
        export.Text = "Export " + _currentFileViewFolder;

        try
        {
            export.ShowDialog(this);
        }
        catch (Exception x)
        {
            MessageBox.Show("ERROR: " + x.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        Cursor.Current = Cursors.Default;
        Application.UseWaitCursor = false;
        UpdateMm();
    }

    private void MMEditImportFiles_Click(object sender, EventArgs e)
    {
        using (var importFolder = new ImportFolderDlg())
        {
            importFolder.ePakFolder.Text = _currentFileViewFolder;
            importFolder.eDiskFolder.Text = Path.GetDirectoryName(Pak.GpFilePath);
            importFolder.pak = Pak;
            try
            {
                if (importFolder.ShowDialog() == DialogResult.OK)
                {
                    GenerateFolderViews();
                    PopulateFilesList(_currentFileViewFolder);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(
                    "ERROR: " + x.Message + " \r\n\r\nDo not forget to save your file to prevent further corruption !",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        UpdateMm();
    }

    private void MMVersionSourceCode_Click(object sender, EventArgs e)
    {
        Process.Start(_urlGitHub);
    }

    private void MMExportDB_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;

        var d = _currentFileViewFolder;
        if (d != "") d += "/";
        d += lbFiles.SelectedItem.ToString();
        exportFileDialog.FileName = Path.GetFileName(d.Replace('/', Path.DirectorySeparatorChar));

        if (exportFileDialog.ShowDialog() != DialogResult.OK)
            return;

        var pf = Pak.ExportFileAsStream(d);

        var fs = new FileStream(exportFileDialog.FileName, FileMode.Create);
        try
        {
            AAPakFileHeader.LastAesError = string.Empty;
            if (AAPakFileHeader.EncryptStreamAes(pf, fs, _dbKey, false))
                MessageBox.Show("ExportDB: Done");
            else
                MessageBox.Show("Decryption failed:\r\n" + AAPakFileHeader.LastAesError, "Error");
        }
        catch (Exception x)
        {
            MessageBox.Show("Exception: " + x.Message);
        }

        fs.Dispose();
        UpdateMm();
    }

    private void VisitDiscordToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Process.Start(_urlDiscord);
    }

    private void MMExtraMakeMod_Click(object sender, EventArgs e)
    {
        // Basically, what we'll make here, is just a simple pak file, but the first file added is going to be our modding tool's executable
        // Because of the way that XL's pak file format works, it's not encrypting or compressing the files.
        // Adding a executable as first file, and renaming that pak to .exe, would effectively create a executeable with the rest of the
        // Pak's data attached to it. If you also mark that executeable with a special tag that the executable knows to ignore, you can effectively
        // make a "self-extracting pak file" in the same way that the old Self Extractiong Zip files work, except, that this time, the executeable
        // is INSIDE it's own pak and not just in front of it, so you don't even need to account for the Offset where the data starts.
        // Pretty sure this was originally made by design, neat ^_^

        using (var makeModDlg = new MakeModForm())
        {
            makeModDlg.mainPak = Pak;
            makeModDlg.ShowDialog();
        }
    }

    private void MainForm_Shown(object sender, EventArgs e)
    {
        // Returns true if commandline requests that we are done
        if (HandleCommandLine())
            Close();
    }

    private void LbExtraFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;

        lbFiles.Items.Clear();
        lFiles.Text = "";
        var d = (sender as ListBox)?.SelectedIndex ?? -1;
        if (d >= 0 && d < Pak.ExtraFiles.Count) ShowFileInfo(Pak.ExtraFiles[d]);
        UpdateMm();
    }

    private bool HandleCommandLine()
    {
        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;
        var closeWhenDone = false;

        var cmdErrors = string.Empty;
        var args = Environment.GetCommandLineArgs();
        for (var i = 1; i < args.Length; i++)
        {
            var arg = args[i];
            var arg1 = "";
            var arg2 = "";
            if (i + 1 < args.Length)
                arg1 = args[i + 1];
            if (i + 2 < args.Length)
                arg2 = args[i + 2];

            if (arg == "-o" || arg == "+o")
            {
                i++; // take one arg
                Pak?.ClosePak();
                LoadPakFile(arg1, false, false, true);
                if (Pak == null || !Pak.IsOpen) cmdErrors += "Failed to open for r/w: " + arg1 + "\r\n";
            }
            else if (arg == "+c")
            {
                i++; // take one arg

                if (Pak != null)
                    Pak.ClosePak();
                // Create and a new pakfile
                Pak = new AAPak(arg1, false, true);
                if (Pak == null || !Pak.IsOpen)
                {
                    cmdErrors += "Failed to created file: " + arg1 + "\r\n";
                    continue;
                }

                Pak.ClosePak();
                // Re-open it in read/write mode
                LoadPakFile(arg1, false, false, true);

                if (Pak == null || !Pak.IsOpen) cmdErrors += "Failed to re-open created file: " + arg1 + "\r\n";
            }
            else if (arg == "+sfx")
            {
                i++;
                if (Pak == null || !Pak.IsOpen || Pak.ReadOnly)
                {
                    cmdErrors +=
                        "Pak file needs to be opened in read/write mode to be able to add a mod installer !\r\n";
                }
                else
                {
                    // add MODSFX
                    var sfxStream = new MemoryStream(Resources.AAModSFX);
                    // We will be possibly be editing the icon, so it's a good idea to have some spare space here
                    if (!Pak.AddFileFromStream(MakeModForm.SfxInfoFileName, sfxStream, DateTime.Now, DateTime.Now, true,
                            out _)) cmdErrors += "Failed to add SFX executable\r\n";

                    if (File.Exists(arg1))
                    {
                        if (!Pak.AddFileFromFile(arg1, MakeModForm.ModInfoFileName, false))
                            cmdErrors += "Failed to add SFX description file: \r\n" + arg1;
                    }
                    else
                    {
                        // Consider the provided arg as a Name
                        var modDescStream = new MemoryStream();
                        var descBytes = Encoding.UTF8.GetBytes(arg1);
                        modDescStream.Write(descBytes, 0, descBytes.Length);
                        modDescStream.Position = 0;

                        if (!Pak.AddFileFromStream(MakeModForm.ModInfoFileName, modDescStream, DateTime.Now,
                                DateTime.Now, false, out _))
                            cmdErrors += "Failed to add SFX description text: \r\n" + arg1;
                    }
                }
            }
            else if (arg == "+f")
            {
                i += 2; // take two args
                if (Pak == null || !Pak.IsOpen || Pak.ReadOnly)
                {
                    cmdErrors += "Pak file needs to be opened in read/write mode to be able to add a file !\r\n";
                }
                else
                {
                    if (!Pak.AddFileFromFile(arg1, arg2, false))
                        cmdErrors += "Failed to add file:\r\n" + arg1 + "\r\n=>" + arg2 + "\r\n";
                }
            }
            else if (arg == "-f")
            {
                i++; // take one arg
                if (Pak == null || !Pak.IsOpen || Pak.ReadOnly)
                {
                    cmdErrors += "Pak file needs to be opened in read/write mode to be able to delete a file !\r\n";
                }
                else
                {
                    if (!Pak.DeleteFile(arg1))
                        // Technically, this could never fail as it only can return false if it's in read-only
                        cmdErrors += "Failed to delete file:\r\n" + arg1;
                }
            }
            else if (arg == "-s" || arg == "+s")
            {
                if (Pak == null || !Pak.IsOpen || Pak.ReadOnly)
                    cmdErrors += "Pak file needs to be opened in read/write mode to be able save it !\r\n";
                else
                    Pak.SaveHeader();
            }
            else if (arg == "+d")
            {
                i += 2; // take two args
                if (Pak == null || !Pak.IsOpen || Pak.ReadOnly)
                    cmdErrors += "Pak file needs to be opened in read/write mode to be able to add a file !\r\n";
                else
                {
                    using var importFolder = new ImportFolderDlg();
                    
                    importFolder.pak = Pak;
                    importFolder.InitAutoRun(arg1, arg2);
                    try
                    {
                        if (importFolder.ShowDialog() != DialogResult.OK)
                            cmdErrors += "Possible errors while adding directory\r\n" + arg1 + "\r\n=>\r\n" + arg2;
                    }
                    catch (Exception x)
                    {
                        cmdErrors += "EXCEPTION: " + x.Message + " \r\nPossible file corruption !";
                    }
                }
            }
            else if (arg == "-d")
            {
                i += 1; // take one arg
                if (Pak == null || !Pak.IsOpen || Pak.ReadOnly)
                    cmdErrors += "Pak file needs to be opened in read/write mode to be able to delete a file !\r\n";
                else
                    // Delete the files
                    try
                    {
                        var delDir = arg1.ToLower();
                        if (delDir.Last() != '/')
                            delDir += '/';
                        for (var n = Pak.Files.Count - 1; n >= 0; n--)
                        {
                            var pfi = Pak.Files[n];
                            if (pfi.Name.ToLower().StartsWith(delDir))
                                Pak.DeleteFile(pfi);
                        }
                    }
                    catch (Exception x)
                    {
                        cmdErrors += "Exception: " + x.Message + " \r\nPossible file corruption !";
                    }
            }
            else if (arg == "-x" || arg == "+x")
            {
                if (Pak == null || !Pak.IsOpen)
                    cmdErrors += "Pak file needs to be opened before you can close it !\r\n";
                else
                    Pak.ClosePak();
                if (arg == "+x")
                    closeWhenDone = true;
            }
            else if (arg == "-m" || arg == "+m")
            {
                i++;
                MessageBox.Show(arg1, "Command-Line Message", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if (arg == "-csv" || arg == "+csv")
            {
                i++; // take one arg
                if (Pak == null || !Pak.IsOpen)
                    cmdErrors += "Pak file needs to be opened to be able generate a CSV file !\r\n";
                else
                    CreateCsvFile(arg1);
            }
            else if (arg == "-h" || arg == "--h" || arg == "--help" || arg == "-help" || arg == "-?" || arg == "--?" ||
                     arg == "/?" || arg == "/help")
            {
                MessageBox.Show(Resources.cmdhelp, "Command-Line Help", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                closeWhenDone = true;
            }
            else if (File.Exists(arg))
            {
                // Open file in read-only mode if nothing is specified and it's a valid filename
                if (Pak != null)
                    Pak.ClosePak();
                LoadPakFile(arg, true, true, true);
                if (Pak == null || !Pak.IsOpen) cmdErrors += "Failed to open: " + arg + "\r\n";
            }
            else
            {
                cmdErrors += "Unknown command or filename: " + arg + "\r\n";
            }
        }

        if (cmdErrors != string.Empty)
            MessageBox.Show(cmdErrors, "Command-Line Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        if (Pak != null && Pak.IsOpen) GenerateFolderViews();

        Application.UseWaitCursor = false;
        Cursor.Current = Cursors.Default;
        return closeWhenDone;
    }

    private void lfiName_Click(object sender, EventArgs e)
    {
        // copy to clipboard
        if ((sender is Label label) && (label.Text != null))
            Clipboard.SetText(label.Text);
    }

    private void manualEditFileMD5ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;

        if (Pak.ReadOnly)
            return;

        if (lbFiles.SelectedIndex < 0)
        {
            MessageBox.Show("No file selected");
            return;
        }

        var d = _currentFileViewFolder;
        if (d != "") d += "/";
        d += lbFiles.SelectedItem.ToString();

        if (Pak.GetFileByName(d, out var pfi))
            using (var fp = new FilePropForm())
            {
                fp.pfi = pfi;
                fp.ResetFileInfo();
                if (fp.ShowDialog() == DialogResult.OK)
                {
                    pfi.Name = fp.newInfo.Name;
                    pfi.Size = fp.newInfo.Size;
                    pfi.SizeDuplicate = fp.newInfo.SizeDuplicate;
                    pfi.PaddingSize = fp.newInfo.PaddingSize;
                    fp.newInfo.Md5.CopyTo(pfi.Md5, 0);
                    pfi.CreateTime = fp.newInfo.CreateTime;
                    pfi.ModifyTime = fp.newInfo.ModifyTime;
                    pfi.Offset = fp.newInfo.Offset;
                    pfi.Dummy1 = fp.newInfo.Dummy1;
                    pfi.Dummy2 = fp.newInfo.Dummy2;
                    Pak.IsDirty = true;
                    ShowFileInfo(pfi);
                }
            }
        else
            MessageBox.Show("ERROR: No file");

        UpdateMm();
    }

    private void MMExtraTryOpenUsingKeyList_Click(object sender, EventArgs e)
    {
        if (openKeyListDialog.ShowDialog() != DialogResult.OK)
            return;
        if (openGamePakDialog.ShowDialog() != DialogResult.OK)
            return;
        var useDebug = MessageBox.Show("Output debug headers ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes;
        var keyListText = File.ReadAllLines(openKeyListDialog.FileName).ToList();
        var keyList = new List<byte[]>();
        foreach (var s in keyListText)
        {
            var key = AAPak.StringToByteArray(s);
            keyList.Add(key);
        }

        foreach (var key in keyList)
        {
            var pak = new AAPak("");
            pak.DebugMode = useDebug;
            pak.SetCustomKey(key);
            if (pak.OpenPak(openGamePakDialog.FileName, true))
            {
                if (MessageBox.Show(
                        "Was able to open pak with key \r\n" + AAPakFileHeader.ByteArrayToHexString(key, "", "") +
                        "\r\n\r\nDo you want to save this key at the game_pak location ?", "Valid key found",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var keyfile = Path.ChangeExtension(pak.GpFilePath, ".key");
                    File.WriteAllBytes(keyfile, key);
                    MessageBox.Show("Saved key as " + keyfile);
                }

                return;
            }
        }

        MessageBox.Show("No match found using " + keyList.Count + " keys !");
    }

    private void MMFileS2_Click(object sender, EventArgs e)
    {
        MMFileTryOpenUsingKeyList.Visible = true;
    }

    private void MM_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
    }

    private void MMExtraMD5All_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;

        var res = MessageBox.Show(
            "Do you want to re-calculate the MD5 hash for all files even those that already have a value set (takes long)\n" +
            "Yes - recalculate all files\n" +
            "No - recalculate only files that have a null-hash",
            "Recalculate all files hashes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

        if (res == DialogResult.Cancel)
            return;

        using (var rehashDlg = new ReMD5Dlg())
        {
            rehashDlg.pak = Pak;
            rehashDlg.allFiles = res == DialogResult.Yes;
            rehashDlg.ShowDialog();
        }
    }

    private void statusBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
    }

    public class FileListEntry : object, IEquatable<FileListEntry>, IComparable<FileListEntry>
    {
        public string DisplayName;
        public bool IsDeletedFile;
        public AAPakFileInfo Pfi;

        public int CompareTo(FileListEntry comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
                return 1;

            return string.Compare(Pfi.Name, comparePart.Pfi.Name, StringComparison.InvariantCulture);
        }

        public bool Equals(FileListEntry other)
        {
            return other != null && Pfi.Name.Equals(other.Pfi.Name);
        }

        public override string ToString()
        {
            if (DisplayName != string.Empty)
                return DisplayName;
            if (Pfi != null)
                return Pfi.Name;
            return "<noname>";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var objAsPart = obj as FileListEntry;
            return objAsPart != null && Equals(objAsPart);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    private void Panel1_Paint(object sender, PaintEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}