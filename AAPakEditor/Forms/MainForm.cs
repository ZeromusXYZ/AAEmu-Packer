using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using AAPakEditor.Properties;
using AAPacker;

namespace AAPakEditor;

public partial class MainForm : Form
{
    private string baseTitle = "";
    private string currentFileViewFolder = "";

    private readonly byte[] customKey = new byte[16]
        { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

    private readonly byte[] dbKey = new byte[16]
        { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

    private readonly List<fileListEntry> fileListEntries = new();
    public AAPak pak;
    private readonly string urlDiscord = "https://discord.gg/GhVfDtK";
    private readonly string urlGitHub = "https://github.com/ZeromusXYZ/AAEmu-Packer";
    private bool useCustomKey;
    private bool useDBKey;

    public MainForm()
    {
        InitializeComponent();
    }

    private void UpdateMM()
    {
        MMFileSave.Enabled = pak != null && pak.isOpen && !pak.readOnly && pak.isDirty;
        MMFileClose.Enabled = pak != null && pak.isOpen;

        MMEditAddFile.Enabled = pak != null && pak.isOpen && pak.readOnly == false && pak.PakType != PakFileType.CSV;
        MMEditImportFiles.Enabled =
            pak != null && pak.isOpen && pak.readOnly == false && pak.PakType != PakFileType.CSV;
        MMEditDeleteSelected.Enabled = pak != null && pak.isOpen && pak.readOnly == false &&
                                       lbFiles.SelectedIndex >= 0 && pak.PakType != PakFileType.CSV;
        MMEditReplace.Enabled = pak != null && pak.isOpen && pak.PakType != PakFileType.CSV && pak.readOnly == false &&
                                lbFiles.SelectedIndex >= 0;
        MMEditFileProp.Enabled = pak != null && pak.isOpen && pak.PakType != PakFileType.CSV && pak.readOnly == false &&
                                 lbFiles.SelectedIndex >= 0;
        MMEdit.Visible = pak != null && pak.isOpen && pak.PakType != PakFileType.CSV && pak.readOnly == false;

        MMExportSelectedFile.Enabled =
            pak != null && pak.isOpen && pak.PakType != PakFileType.CSV && lbFiles.SelectedIndex >= 0;
        MMExportSelectedFolder.Enabled =
            pak != null && pak.isOpen && pak.PakType != PakFileType.CSV && currentFileViewFolder != "";
        MMExportAll.Enabled = pak != null && pak.isOpen && pak.PakType != PakFileType.CSV;
        MMExportDB.Enabled = pak != null && pak.isOpen && pak.PakType != PakFileType.CSV &&
                             lbFiles.SelectedIndex >= 0 && useDBKey &&
                             Path.GetExtension(lbFiles.SelectedItem.ToString()).StartsWith(".sql");
        MMExportDB.Visible = pak != null && pak.isOpen && pak.PakType != PakFileType.CSV && useDBKey;
        MMExportS2.Visible = MMExportDB.Visible;
        MMExport.Visible = pak != null && pak.isOpen && pak.PakType != PakFileType.CSV;

        MMExtraMD5.Enabled = pak != null && pak.isOpen && pak.PakType != PakFileType.CSV && pak.readOnly == false &&
                             lbFiles.SelectedIndex >= 0;
        MMExtraExportData.Enabled = pak != null && pak.isOpen && pak.PakType != PakFileType.CSV;
        MMExtraMakeMod.Enabled = pak != null && pak.isOpen && pak.PakType != PakFileType.CSV;
        MMExtra.Visible = pak != null && pak.isOpen;

        if (pak != null && pak.isOpen)
        {
            if (pak.isDirty)
                Text = baseTitle + " - *" + pak._gpFilePath;
            else
                Text = baseTitle + " - " + pak._gpFilePath;
            lPakExtraInfo.Text = pak.NewestFileDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
        else
        {
            Text = baseTitle;
            lPakExtraInfo.Text = "...";
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

        UpdateMM();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        baseTitle = Text;
        var AppVer = Assembly.GetExecutingAssembly().GetName().Version;
        var v = "Version ";
        v += AppVer.Major.ToString();
        v += "." + AppVer.Minor;
        if (AppVer.Build > 0 || AppVer.MinorRevision > 0)
            v += "." + AppVer.Build;
        if (AppVer.MinorRevision > 0)
            v += "." + AppVer.MinorRevision;
        MMVersion.Text = v;
        UpdateMM();
        ShowFileInfo(null);
    }

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        if (pak != null && pak.isOpen)
        {
            pak.ClosePak();
            pak = null;
        }

        UpdateMM();
    }

    private void GenerateFolderViews()
    {
        lFileCount.Text = "Analyzing folder structure ... ";
        //lFileCount.Refresh();

        pak.GenerateFolderList();

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
        foreach (var s in pak.folders)
        {
            lbFolders.Items.Add(s);
            c++;
            if (c % 250 == 0)
                lFileCount.Text = "Loading folders ... " + c + " / " + pak.folders.Count;
            //lFileCount.Refresh();
            //Thread.Sleep(1);


            if (s != "")
            {
                var dirwalk = s.Split('/');
                var dd = "";
                var lastNode = rootNode;
                foreach (var ds in dirwalk)
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
        lFileCount.Text = pak.files.Count + " files in " + pak.folders.Count + " folders";
        foreach (var pfi in pak.extraFiles) lbExtraFiles.Items.Add(pfi.name);
        UpdateMM();
    }

    private void LoadPakKeys(string fromDirectory)
    {
        useDBKey = false;
        useCustomKey = false;
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

            fs.Read(dbKey, 0, 16);
            fs.Dispose();
            useDBKey = true;
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

            fs.Read(customKey, 0, 16);
            fs.Dispose();
            useCustomKey = true;
            pak.SetCustomKey(customKey);
        }
    }

    private void LoadPakFile(string filename, bool openAsReadOnly, bool showWriteWarning = true, bool quickLoad = false)
    {
        lTypePak.Text = string.Empty;
        if (pak == null) pak = new AAPak("");
        if (pak.isOpen)
        {
            lFileCount.Text = "Closing pak ... ";
            //lFileCount.Refresh();
            pak.ClosePak();
        }

        lFileCount.Text = "Opening Pak ... ";
        //lFileCount.Refresh();
        try
        {
            LoadPakKeys(Path.GetDirectoryName(filename).TrimEnd(Path.DirectorySeparatorChar) +
                        Path.DirectorySeparatorChar);
        }
        catch
        {
            MessageBox.Show("Error loading custom keys");
        }

        var res = pak.OpenPak(filename, openAsReadOnly);
        if (!res)
        {
            Text = baseTitle;
            lFileCount.Text = "no files";
            lbFolders.Items.Clear();
            lbFiles.Items.Clear();
            UpdateMM();
            if (useCustomKey)
                MessageBox.Show("Custom  game_pak.key  does not seem valid for " + filename, "OpenPak Key Error");
            else
                MessageBox.Show("Failed to open " + filename, "OpenPak Error");
        }
        else
        {
            Text = baseTitle + " - " + pak._gpFilePath;


            if (!quickLoad)
                GenerateFolderViews();

            // Only show this waring if this is not a new pak file
            if (openAsReadOnly == false && pak.files.Count > 0 && showWriteWarning)
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

        if (pak.PakType != PakFileType.TypeA)
            lTypePak.Text = pak.PakType.ToString();
        else
            lTypePak.Text = string.Empty;
    }

    private void lbFolders_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbFiles.Items.Clear();
        if (pak == null || !pak.isOpen)
            return;

        var d = (sender as ListBox).SelectedItem.ToString();
        if (d == "<root>")
            d = string.Empty;
        PopulateFilesList(d);
        UpdateMM();
    }

    private void ShowFileInfo(AAPakFileInfo pfi)
    {
        if (pfi != null)
        {
            lfiName.Text = pfi.name;
            lfiSize.Text = "Size: " + pfi.size + " byte(s)";
            if (pfi.paddingSize > 0)
                lfiSize.Text += "  + " + pfi.paddingSize + " padding";

            if (pfi.sizeDuplicate != pfi.size)
                lfiSize.Text += "  size mismatch " + pfi.sizeDuplicate + " byte(s)";

            //var h = BitConverter.ToString(pfi.md5).ToUpper().Replace("-", "");
            //if (h == pak._header.nullHashString)
            if (pfi.md5.SequenceEqual(AAPakFileHeader.NullHash))
                lfiHash.Text = "MD5: Invalid or not calculated !";
            else
                lfiHash.Text = "MD5: " + BitConverter.ToString(pfi.md5).ToUpper().Replace("-", "");
            try
            {
                lCreateRaw.Text = "(" + pfi.createTime + ")";
                if (pfi.createTime != 0)
                    lfiCreateTime.Text = "Created: " + DateTime.FromFileTime(pfi.createTime);
                else
                    lfiCreateTime.Text = "<Created time not used>";
            }
            catch
            {
                lfiCreateTime.Text = "CreateTime Invalid";
            }

            try
            {
                lModifiedRaw.Text = "(" + pfi.modifyTime + ")";
                if (pfi.modifyTime != 0)
                    lfiModifyTime.Text = "Modified: " + DateTime.FromFileTime(pfi.modifyTime);
                else
                    lfiModifyTime.Text = "<Modified time not used>";
            }
            catch
            {
                lfiModifyTime.Text = "ModifiedTime Invalid (" + pfi.modifyTime + ")";
            }

            lfiStartOffset.Text = "Start Offset: 0x" + pfi.offset.ToString("X16");
            lfiExtras.Text = "D1 0x" + pfi.dummy1.ToString("X") + "  D2 0x" + pfi.dummy2.ToString("X");
            if (pfi.entryIndexNumber >= 0)
                lfiIndex.Text = "index: " + pfi.entryIndexNumber;
            else if (pfi.deletedIndexNumber >= 0) lfiIndex.Text = "extra-index: " + pfi.deletedIndexNumber;
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
        if (pak == null || !pak.isOpen)
            return;

        if (lbFiles.SelectedIndex < 0)
            return;

        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;

        var d = currentFileViewFolder;
        if (d != "") d += "/";
        d += lbFiles.SelectedItem.ToString();
        ref var pfi = ref pak.nullAAPakFileInfo;

        try
        {
            var fle = lbFiles.SelectedItem as fileListEntry;
            ShowFileInfo(fle.pfi);
        }
        catch
        {
            if (pak.GetFileByName(d, ref pfi)) ShowFileInfo(pfi);
        }

        UpdateMM();

        Cursor.Current = Cursors.Default;
        Application.UseWaitCursor = false;
    }

    private void MMExportSelectedFile_Click(object sender, EventArgs e)
    {
        if (pak == null || !pak.isOpen)
            return;

        if (lbFiles.SelectedIndex < 0)
        {
            MessageBox.Show("No file selected");
            return;
        }

        var d = currentFileViewFolder;
        if (d != "") d += "/";
        fileListEntry fle = null;
        try
        {
            fle = lbFiles.SelectedItem as fileListEntry;
            d += fle.DisplayName;
        }
        catch
        {
            fle = null;
            d += lbFiles.SelectedItem.ToString();
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
            if (!ExportFile(fle.pfi, exportFileDialog.FileName))
                //  if (!ExportFile(d, exportFileDialog.FileName))
                MessageBox.Show("Failed to export " + d);
    }

    public bool ExportFile(AAPakFileInfo pfi, string destName)
    {
        try
        {
            // Save file stream
            var filePakStream = pak.ExportFileAsStream(pfi);
            var fs = new FileStream(destName, FileMode.Create);
            filePakStream.Position = 0;

            filePakStream.CopyTo(fs);

            filePakStream.Dispose();
            fs.Dispose();

            // Update file details
            if (pfi.createTime != 0)
                File.SetCreationTime(destName, DateTime.FromFileTimeUtc(pfi.createTime));
            if (pfi.modifyTime != 0)
                File.SetLastWriteTime(destName, DateTime.FromFileTimeUtc(pfi.modifyTime));
        }
        catch
        {
            return false;
        }

        return true;
    }

    public bool ExportFile(string sourceName, string destName)
    {
        ref var pfi = ref pak.nullAAPakFileInfo;
        if (pak.GetFileByName(sourceName, ref pfi))
            return ExportFile(pfi, destName);
        return false;
    }

    private void MMExportAll_Click(object sender, EventArgs e)
    {
        if (pak == null || !pak.isOpen)
            return;

        exportFolderDialog.Description = "Select a folder to where to export all files to";
        exportFolderDialog.SelectedPath = Path.GetDirectoryName(pak._gpFilePath);
        if (exportFolderDialog.ShowDialog() != DialogResult.OK)
            return;

        if (MessageBox.Show(
                "Are you sure you want to export all the files ?\r\nAll files in destination will be overwritten !",
                "Export All", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;

        var export = new ExportAllDlg();
        export.pak = pak;
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
        if (pak == null || !pak.isOpen)
            return;

        if (lbFiles.SelectedIndex < 0)
        {
            MessageBox.Show("No file selected");
            return;
        }

        var d = currentFileViewFolder;
        if (d != "") d += "/";
        d += lbFiles.SelectedItem.ToString();

        ref var pfi = ref pak.nullAAPakFileInfo;
        if (pak.GetFileByName(d, ref pfi))
            MessageBox.Show("MD5 Hash updated to " + pak.UpdateMD5(pfi));
        else
            MessageBox.Show("ERROR: No file");
        UpdateMM();
    }

    private void MMFileSave_Click(object sender, EventArgs e)
    {
        if (!pak.readOnly && pak.isDirty)
        {
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            pak.SaveHeader();
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        UpdateMM();
    }


    private void MMExtraExportData_Click(object sender, EventArgs e)
    {
        if (pak == null || !pak.isOpen)
            return;
        CreateCSVFile();
    }

    private void CreateCSVFile(string filename = "")
    {
        var newest = new DateTime(1600, 1, 1);

        var sl = new List<string>();
        var s = "";
        s = "name";
        s += ";size";
        s += ";offset";
        s += ";md5";
        s += ";createTime";
        s += ";modifyTime";
        s += ";sizeDuplicate";
        s += ";paddingSize";
        s += ";dummy1";
        s += ";dummy2";
        sl.Add(s);
        foreach (var pfi in pak.files)
        {
            var modTime = DateTime.FromFileTimeUtc(pfi.modifyTime);
            if (modTime > newest)
                newest = modTime;

            s = pfi.name;
            s += ";" + pfi.size;
            s += ";" + pfi.offset;
            s += ";" + BitConverter.ToString(pfi.md5).Replace("-", "").ToUpper();
            s += ";" + AAPak.DateTimeToDateTimeStr(
                DateTime.FromFileTimeUtc(pfi.createTime)); // .ToString("yyyy-MM-dd HH:mm:ss");
            s += ";" + AAPak.DateTimeToDateTimeStr(modTime); // .ToString("yyyy-MM-dd HH:mm:ss");
            s += ";" + pfi.sizeDuplicate;
            s += ";" + pfi.paddingSize;
            s += ";" + pfi.dummy1;
            s += ";" + pfi.dummy2;
            sl.Add(s);
        }

        exportFileDialog.FileName =
            Path.GetFileName(pak._gpFilePath) + "_files_" + newest.ToString("yyyyMMdd") + ".csv";
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
        if (pak == null || !pak.isOpen || lbFiles.SelectedIndex < 0)
            return;

        if (pak.readOnly)
        {
            MessageBox.Show("Pak is opened in Read-Only mode, cannot add/replace files.");
            return;
        }

        var filename = currentFileViewFolder;
        if (filename != "") filename += "/";
        filename += lbFiles.SelectedItem.ToString();

        fileListEntry fle = null;
        try
        {
            fle = lbFiles.SelectedItem as fileListEntry;
        }
        catch
        {
            MessageBox.Show("It looks like there was no file selected to replace !");
            return;
        }

        var pfi = fle.pfi;

        /*
        ref AAPakFileInfo pfi = ref pak.nullAAPakFileInfo;

        if (!pak.GetFileByName(filename, ref pfi))
            return;
        */

        var maxSize = pfi.size + pfi.paddingSize;

        importFileDialog.FileName = lbFiles.SelectedItem.ToString();
        if (importFileDialog.ShowDialog() != DialogResult.OK)
            return;

        // DateTime createTime = File.GetCreationTime(importFileDialog.FileName);
        var modifyTime = File.GetLastWriteTime(importFileDialog.FileName);

        try
        {
            var fs = new FileStream(importFileDialog.FileName, FileMode.Open, FileAccess.Read);
            if (fs.Length > maxSize)
            {
                fs.Dispose();
                MessageBox.Show(string.Format(
                    "File is too big!\r\n{0}\r\nCan only be replaced with a file with the \r\nmaximum size of {1} bytes",
                    filename, maxSize));
            }

            fs.Position = 0;
            if (pak.ReplaceFile(ref pfi, fs, modifyTime) == false)
                MessageBox.Show(string.Format("Failed to replace file !\r\n{0}\r\nPak or File might be damaged !!",
                    filename));
            fs.Dispose();
        }
        catch (Exception ex)
        {
            MessageBox.Show("ERROR: " + ex.Message);
        }

        UpdateMM();
    }

    private void PrepareFileListView()
    {
        fileListEntries.Clear();
        lbFiles.Items.Clear();
    }

    private void AddFileListEntry(AAPakFileInfo pfi, string displayName, bool isDeletedList)
    {
        var fle = new fileListEntry();
        fle.DisplayName = displayName;
        fle.pfi = pfi;
        fle.isDeletedFile = isDeletedList;
        fileListEntries.Add(fle);
    }

    private void FinalizeFileListView()
    {
        lbFiles.Items.Clear();
        fileListEntries.Sort();
        foreach (var fle in fileListEntries)
            lbFiles.Items.Add(fle);
    }

    private void PopulateFilesList(string withdir)
    {
        currentFileViewFolder = withdir;

        PrepareFileListView();
        //lbFiles.Items.Clear();
        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;

        var list = pak.GetFilesInDirectory(currentFileViewFolder);
        if (currentFileViewFolder == "")
            lFiles.Text = list.Count + " files in <root>";
        else
            lFiles.Text = list.Count + " files in \"" + currentFileViewFolder + "\"";
        //List<string> sl = new List<string>();
        foreach (var pfi in list)
        {
            var f = string.Empty;
            try
            {
                f = Path.GetFileName(pfi.name);
            }
            catch
            {
                f = "__invalid_name_" + pfi.entryIndexNumber + "__";
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
        UpdateMM();
    }

    private void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (pak == null || !pak.isOpen)
            return;


        if (e == null || e.Node == null)
            return;

        PopulateFilesList(e.Node.Name);
        e.Node.Expand();
        UpdateMM();
    }

    private void MMExtraDebugTest_Click(object sender, EventArgs e)
    {
        if (pak == null || !pak.isOpen)
            return;


        var d = currentFileViewFolder;
        if (d != "") d += "/";
        d += lbFiles.SelectedItem.ToString();
        exportFileDialog.FileName = Path.GetFileName(d.Replace('/', Path.DirectorySeparatorChar));

        if (exportFileDialog.ShowDialog() != DialogResult.OK)
            return;

        var pfraw = pak.ExportFileAsStream(d);

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
            if (AAPakFileHeader.EncryptStreamAes(pf, fsraw, dbKey, false, true))
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
        // ms.Dispose();

        UpdateMM();
    }

    private void MMEditDeleteSelected_Click(object sender, EventArgs e)
    {
        if (pak == null || !pak.isOpen || lbFiles.SelectedIndex < 0)
            return;

        if (pak.readOnly)
        {
            MessageBox.Show("Pak is opened in Read-Only mode, cannot delete files.");
            return;
        }

        if (lbFiles.SelectedIndex < 0)
        {
            MessageBox.Show("Nothing selected to delete.");
            return;
        }

        var filename = currentFileViewFolder;
        if (filename != "") filename += "/";
        filename += lbFiles.SelectedItem.ToString();

        var fle = lbFiles.SelectedItem as fileListEntry;

        /*
        ref AAPakFileInfo pfi = ref pak.nullAAPakFileInfo;
        if (!pak.GetFileByName(filename, ref pfi))
            return;
        */

        if (MessageBox.Show("Are you sure you want to delete this file ?\r\n" + filename, "Delete",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        if (pak.DeleteFile(fle.pfi)) MessageBox.Show("Reference to " + filename + " has been removed from the pak.");

        if (lbFiles.Items.Count <= 1)
            // If this was the last file listed in the directory listing, we will need to re-populate the folder views to update this change
            GenerateFolderViews();
        lbFiles.Items.Clear();
        PopulateFilesList(currentFileViewFolder);

        UpdateMM();
    }

    private void MMEditAddFile_Click(object sender, EventArgs e)
    {
        // open add dialog
        var addDlg = new AddFileDialog();
        if (currentFileViewFolder != "")
            addDlg.suggestedDir = currentFileViewFolder + '/';
        if (addDlg.ShowDialog(this) == DialogResult.OK)
        {
            var diskfn = addDlg.eDiskFileName.Text;
            var pakfn = addDlg.ePakFileName.Text.ToLower()
                .Replace("\\", "/"); // You just know people will put this wrong, so pre-emptive replace
            addDlg.Dispose();

            // virual directory to the new file
            var newpakfilepath = string.Empty;
            try
            {
                newpakfilepath = Path.GetDirectoryName(pakfn.Replace("/", "\\")).Replace("\\", "/");
            }
            catch
            {
                newpakfilepath = string.Empty;
            }

            if (File.Exists(diskfn))
            {
                var sourceIsDBFile = Path.GetExtension(diskfn).ToLower() == ".sqlite3";

                var doEncrypt = false;
                if (sourceIsDBFile && useDBKey)
                    if (MessageBox.Show(
                            "Import of DB detected, do you want to encrypt before importing using the provided key ?",
                            "Import as DB", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        doEncrypt = true;


                var cTime = File.GetCreationTime(diskfn);
                var mTime = File.GetLastWriteTime(diskfn);
                var pfi = pak.nullAAPakFileInfo;
                Stream fs;
                var fStream = new FileStream(diskfn, FileMode.Open, FileAccess.Read);
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
                        var buf = new byte[rest];
                        var decBuf = new byte[rest];
                        fStream.Read(buf, 0, rest);
                        decBuf = AAPakFileHeader.EncryptAes(buf, dbKey, true);
                        mStream.Write(decBuf, 0, decBuf.Length);
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

                var res = pak.AddFileFromStream(pakfn, fs, cTime, mTime, true, out pfi);
                fs.Dispose();
                if (res)
                {
                    MessageBox.Show("File:\r\n" + diskfn + "\r\n\r\nadded as:\r\n" + pfi.name);

                    if (pak.folders.IndexOf(newpakfilepath) < 0)
                        // We added to a new folder
                        GenerateFolderViews();

                    lbFolders.SelectedIndex = lbFolders.Items.IndexOf(newpakfilepath);
                    tvFolders.SelectedNode = tvFolders.Nodes.Find(newpakfilepath, true)[0];

                    PopulateFilesList(newpakfilepath);
                }
                else
                {
                    MessageBox.Show("Failed to add file: " + diskfn);
                }
            }
            else
            {
                MessageBox.Show("File not found: " + diskfn);
            }
        }
        else
        {
            addDlg.Dispose();
        }

        PopulateFilesList(currentFileViewFolder);
        UpdateMM();
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
        pak.ClosePak();
        lFileCount.Text = "Pak Closed";
        Cursor.Current = Cursors.Default;
        Application.UseWaitCursor = false;
        UpdateMM();
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
        if (pak != null) pak.ClosePak();
        // Create and a new pakfile
        pak = new AAPak(openGamePakDialog.FileName, false, true);
        // TODO: Add Pak-type selector (specialized dialog box)
        pak.ClosePak();
        // Re-open it in read/write mode
        LoadPakFile(openGamePakDialog.FileName, false);

        Cursor.Current = Cursors.Default;
        Application.UseWaitCursor = false;
        UpdateMM();
    }

    private void MMExportSelectedFolder_Click(object sender, EventArgs e)
    {
        if (pak == null || !pak.isOpen)
            return;

        exportFolderDialog.Description = "Select a folder to where to export \r\n" + currentFileViewFolder;
        exportFolderDialog.SelectedPath = Path.GetDirectoryName(pak._gpFilePath);
        if (exportFolderDialog.ShowDialog() != DialogResult.OK)
            return;

        if (MessageBox.Show(
                "Are you sure you want to export \"" + currentFileViewFolder +
                "\" and all of it's sub-folders ?\r\nAll files in destination will be overwritten !", "Export Folder",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;

        var export = new ExportAllDlg();
        export.pak = pak;
        export.TargetDir = exportFolderDialog.SelectedPath;
        export.masterRoot = currentFileViewFolder + "/";
        export.Text = "Export " + currentFileViewFolder;

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
        UpdateMM();
    }

    private void MMEditImportFiles_Click(object sender, EventArgs e)
    {
        using (var importFolder = new ImportFolderDlg())
        {
            importFolder.ePakFolder.Text = currentFileViewFolder;
            importFolder.eDiskFolder.Text = Path.GetDirectoryName(pak._gpFilePath);
            importFolder.pak = pak;
            try
            {
                if (importFolder.ShowDialog() == DialogResult.OK)
                {
                    GenerateFolderViews();
                    PopulateFilesList(currentFileViewFolder);
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

        UpdateMM();
    }

    private void MMVersionSourceCode_Click(object sender, EventArgs e)
    {
        Process.Start(urlGitHub);
    }

    private void MMExportDB_Click(object sender, EventArgs e)
    {
        if (pak == null || !pak.isOpen)
            return;

        var d = currentFileViewFolder;
        if (d != "") d += "/";
        d += lbFiles.SelectedItem.ToString();
        exportFileDialog.FileName = Path.GetFileName(d.Replace('/', Path.DirectorySeparatorChar));

        if (exportFileDialog.ShowDialog() != DialogResult.OK)
            return;

        var pf = pak.ExportFileAsStream(d);

        var fs = new FileStream(exportFileDialog.FileName, FileMode.Create);
        try
        {
            AAPakFileHeader.LastAesError = string.Empty;
            if (AAPakFileHeader.EncryptStreamAes(pf, fs, dbKey, false))
                MessageBox.Show("ExportDB: Done");
            else
                MessageBox.Show("Decryption failed:\r\n" + AAPakFileHeader.LastAesError, "Error");
        }
        catch (Exception x)
        {
            MessageBox.Show("Exception: " + x.Message);
        }

        fs.Dispose();
        // ms.Dispose();
        UpdateMM();
    }

    private void VisitDiscordToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Process.Start(urlDiscord);
    }

    private void MMExtraMakeMod_Click(object sender, EventArgs e)
    {
        // Basically, what we'll make here, is just a simple pak file, but the first file added is going to be our modding tool's executable
        // Because of the way that XL's pak file format works, it's not encrypting or compressing the files.
        // Adding a executable as first file, and renaming that pak to .exe, would effectively create a executeable with the rest of the
        // Pak's data attached to it. If you also mark that executeable with a special tag that the executable knows to ignore, you can effectively
        // make a "self-extracting pak file" in the same way that the old Self Extractiong Zip files work, except, that this time, the executeable
        // is INSIDE it's own pak and not just in front of it, so you don't even need to account for the offset where the data starts.
        // Pretty sure this was originally made by design, neat ^_^

        using (var MakeModDlg = new MakeModForm())
        {
            MakeModDlg.mainPak = pak;
            MakeModDlg.ShowDialog();
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
        if (pak == null || !pak.isOpen)
            return;

        lbFiles.Items.Clear();
        lFiles.Text = "";
        var d = (sender as ListBox).SelectedIndex;
        if (d >= 0 && d < pak.extraFiles.Count) ShowFileInfo(pak.extraFiles[d]);
        UpdateMM();
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
                if (pak != null)
                    pak.ClosePak();
                LoadPakFile(arg1, false, false, true);
                if (pak == null || !pak.isOpen) cmdErrors += "Failed to open for r/w: " + arg1 + "\r\n";
            }
            else if (arg == "+c")
            {
                i++; // take one arg

                if (pak != null)
                    pak.ClosePak();
                // Create and a new pakfile
                pak = new AAPak(arg1, false, true);
                if (pak == null || !pak.isOpen)
                {
                    cmdErrors += "Failed to created file: " + arg1 + "\r\n";
                    continue;
                }

                pak.ClosePak();
                // Re-open it in read/write mode
                LoadPakFile(arg1, false, false, true);

                if (pak == null || !pak.isOpen) cmdErrors += "Failed to re-open created file: " + arg1 + "\r\n";
            }
            else if (arg == "+sfx")
            {
                i++;
                if (pak == null || !pak.isOpen || pak.readOnly)
                {
                    cmdErrors +=
                        "Pak file needs to be opened in read/write mode to be able to add a mod installer !\r\n";
                }
                else
                {
                    // add MODSFX
                    var sfxStream = new MemoryStream(Resources.AAModSFX);
                    // We will be possibly be editing the icon, so it's a good idea to have some spare space here
                    if (!pak.AddFileFromStream(MakeModForm.SFXInfoFileName, sfxStream, DateTime.Now, DateTime.Now, true,
                            out _)) cmdErrors += "Failed to add SFX executable\r\n";

                    if (File.Exists(arg1))
                    {
                        if (!pak.AddFileFromFile(arg1, MakeModForm.ModInfoFileName, false))
                            cmdErrors += "Failed to add SFX description file: \r\n" + arg1;
                    }
                    else
                    {
                        // Consider the provided arg as a name
                        var modDescStream = new MemoryStream();
                        var descBytes = Encoding.UTF8.GetBytes(arg1);
                        modDescStream.Write(descBytes, 0, descBytes.Length);
                        modDescStream.Position = 0;

                        if (!pak.AddFileFromStream(MakeModForm.ModInfoFileName, modDescStream, DateTime.Now,
                                DateTime.Now, false, out _))
                            cmdErrors += "Failed to add SFX description text: \r\n" + arg1;
                    }
                }
            }
            else if (arg == "+f")
            {
                i += 2; // take two args
                if (pak == null || !pak.isOpen || pak.readOnly)
                {
                    cmdErrors += "Pak file needs to be opened in read/write mode to be able to add a file !\r\n";
                }
                else
                {
                    if (!pak.AddFileFromFile(arg1, arg2, false))
                        cmdErrors += "Failed to add file:\r\n" + arg1 + "\r\n=>" + arg2 + "\r\n";
                }
            }
            else if (arg == "-f")
            {
                i++; // take one arg
                if (pak == null || !pak.isOpen || pak.readOnly)
                {
                    cmdErrors += "Pak file needs to be opened in read/write mode to be able to delete a file !\r\n";
                }
                else
                {
                    if (!pak.DeleteFile(arg1))
                        // Technically, this could never fail as it only can return false if it's in read-only
                        cmdErrors += "Failed to delete file:\r\n" + arg1;
                }
            }
            else if (arg == "-s" || arg == "+s")
            {
                if (pak == null || !pak.isOpen || pak.readOnly)
                    cmdErrors += "Pak file needs to be opened in read/write mode to be able save it !\r\n";
                else
                    pak.SaveHeader();
            }
            else if (arg == "+d")
            {
                i += 2; // take two args
                if (pak == null || !pak.isOpen || pak.readOnly)
                    cmdErrors += "Pak file needs to be opened in read/write mode to be able to add a file !\r\n";
                else
                    using (var importFolder = new ImportFolderDlg())
                    {
                        importFolder.pak = pak;
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
                if (pak == null || !pak.isOpen || pak.readOnly)
                    cmdErrors += "Pak file needs to be opened in read/write mode to be able to delete a file !\r\n";
                else
                    // Delete the files
                    try
                    {
                        var filesDeleted = 0;
                        var delDir = arg1.ToLower();
                        if (delDir.Last() != '/')
                            delDir += '/';
                        for (var n = pak.files.Count - 1; n >= 0; n--)
                            //foreach(AAPakFileInfo pfi in pak.files)
                        {
                            var pfi = pak.files[n];
                            if (pfi.name.ToLower().StartsWith(delDir))
                                if (pak.DeleteFile(pfi))
                                    filesDeleted++;
                        }
                    }
                    catch (Exception x)
                    {
                        cmdErrors += "Exception: " + x.Message + " \r\nPossible file corruption !";
                    }
            }
            else if (arg == "-x" || arg == "+x")
            {
                if (pak == null || !pak.isOpen)
                    cmdErrors += "Pak file needs to be opened before you can close it !\r\n";
                else
                    pak.ClosePak();
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
                if (pak == null || !pak.isOpen)
                    cmdErrors += "Pak file needs to be opened to be able generate a CSV file !\r\n";
                else
                    CreateCSVFile(arg1);
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
                if (pak != null)
                    pak.ClosePak();
                LoadPakFile(arg, true, true, true);
                if (pak == null || !pak.isOpen) cmdErrors += "Failed to open: " + arg + "\r\n";
            }
            else
            {
                cmdErrors += "Unknown command or filename: " + arg + "\r\n";
            }
        }

        if (cmdErrors != string.Empty)
            MessageBox.Show(cmdErrors, "Command-Line Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        if (pak != null && pak.isOpen) GenerateFolderViews();

        Application.UseWaitCursor = false;
        Cursor.Current = Cursors.Default;
        return closeWhenDone;
    }

    private void lfiName_Click(object sender, EventArgs e)
    {
        // copy to clipboard
        Clipboard.SetText((sender as Label).Text);
    }

    private void manualEditFileMD5ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (pak == null || !pak.isOpen)
            return;

        if (pak.readOnly)
            return;

        if (lbFiles.SelectedIndex < 0)
        {
            MessageBox.Show("No file selected");
            return;
        }

        var d = currentFileViewFolder;
        if (d != "") d += "/";
        d += lbFiles.SelectedItem.ToString();

        ref var pfi = ref pak.nullAAPakFileInfo;
        if (pak.GetFileByName(d, ref pfi))
            using (var fp = new FilePropForm())
            {
                fp.pfi = pfi;
                fp.ResetFileInfo();
                if (fp.ShowDialog() == DialogResult.OK)
                {
                    pfi.name = fp.newInfo.name;
                    pfi.size = fp.newInfo.size;
                    pfi.sizeDuplicate = fp.newInfo.sizeDuplicate;
                    pfi.paddingSize = fp.newInfo.paddingSize;
                    fp.newInfo.md5.CopyTo(pfi.md5, 0);
                    pfi.createTime = fp.newInfo.createTime;
                    pfi.modifyTime = fp.newInfo.modifyTime;
                    pfi.offset = fp.newInfo.offset;
                    pfi.dummy1 = fp.newInfo.dummy1;
                    pfi.dummy2 = fp.newInfo.dummy2;
                    pak.isDirty = true;
                    ShowFileInfo(pfi);
                }
            }
        else
            MessageBox.Show("ERROR: No file");

        UpdateMM();
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
                    var keyfile = Path.ChangeExtension(pak._gpFilePath, ".key");
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
        if (pak == null || !pak.isOpen)
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
            rehashDlg.pak = pak;
            rehashDlg.allFiles = res == DialogResult.Yes;
            rehashDlg.ShowDialog();
        }
    }

    private void statusBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
    }

    public class fileListEntry : object, IEquatable<fileListEntry>, IComparable<fileListEntry>
    {
        public string DisplayName;
        public bool isDeletedFile;
        public AAPakFileInfo pfi;

        public int CompareTo(fileListEntry comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
                return 1;

            return pfi.name.CompareTo(comparePart.pfi.name);
        }

        public bool Equals(fileListEntry other)
        {
            if (other == null) return false;
            return pfi.name.Equals(other.pfi.name);
        }

        public override string ToString()
        {
            if (DisplayName != string.Empty)
                return DisplayName;
            if (pfi != null)
                return pfi.name;
            return "<noname>";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var objAsPart = obj as fileListEntry;
            if (objAsPart == null) return false;
            return Equals(objAsPart);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}