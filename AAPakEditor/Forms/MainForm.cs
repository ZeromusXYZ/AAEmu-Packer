using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using AAPacker;
using AAPakEditor.Properties;
using AAPakEditor.Helpers;
using FastColoredTextBoxNS;
using MethodInvoker = System.Windows.Forms.MethodInvoker;
using System.Text.RegularExpressions;

namespace AAPakEditor.Forms;

public partial class MainForm : Form
{
    private string _baseTitle = "";
    private string _currentFileViewFolder = "";

    private readonly byte[] _customKey = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

    private readonly byte[] _dbKey = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

    private readonly List<FileListEntry> _fileListEntries = new();
    public AAPak Pak;
    private readonly string _urlDiscord = "https://discord.gg/GhVfDtK";
    private readonly string _urlGitHub = "https://github.com/ZeromusXYZ/AAEmu-Packer";
    private readonly string _urlGitHubLatestRelease = "https://github.com/ZeromusXYZ/AAEmu-Packer/releases/latest";
    private bool _useCustomKey;
    private bool _useDbKey;

    public MainForm()
    {
        InitializeComponent();
    }

    private static void OpenUrl(string url)
    {
        try
        {
            var ps = new ProcessStartInfo(url)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error opening URL:\n{url}\n{e.Message}");
        }
    }

    private void UpdateMm()
    {
        MMFileSave.Enabled = Pak?.IsOpen == true && Pak.ReadOnly == false && Pak.IsDirty;
        MMFileClose.Enabled = Pak?.IsOpen == true;

        MMEditAddFile.Enabled = Pak is { IsOpen: true, ReadOnly: false } && !Pak.IsVirtual;
        MMEditImportFiles.Enabled = Pak?.IsOpen == true && Pak.ReadOnly == false && !Pak.IsVirtual;
        MMEditDeleteSelected.Enabled = Pak?.IsOpen == true && Pak.ReadOnly == false && lbFiles.SelectedIndex >= 0 && !Pak.IsVirtual;
        MMEditReplace.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual && Pak.ReadOnly == false && lbFiles.SelectedIndex >= 0;
        MMEditFileProp.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual && Pak.ReadOnly == false && lbFiles.SelectedIndex >= 0;
        MMEdit.Visible = Pak?.IsOpen == true && !Pak.IsVirtual && Pak.ReadOnly == false;

        MMExportSelectedFile.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual && lbFiles.SelectedIndex >= 0;
        MMExportSelectedFolder.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual && _currentFileViewFolder != "";
        MMExportAll.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual;
        MMExportDB.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual && lbFiles.SelectedIndex >= 0 && _useDbKey && Path.GetExtension(lbFiles.SelectedItem.ToString()).StartsWith(".sql");
        MMExportDB.Visible = Pak?.IsOpen == true && !Pak.IsVirtual && _useDbKey;
        MMExportS2.Visible = MMExportDB.Visible;
        MMExport.Visible = Pak?.IsOpen == true && !Pak.IsVirtual;
        MMExportAsCsv.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual;

        MMToolsMakeMod.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual;
        MMToolsCreatePatch.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual;
        MMToolsApplyPatch.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual && Pak.ReadOnly == false;
        MMToolsMD5.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual && Pak.ReadOnly == false && lbFiles.SelectedIndex >= 0;
        MMToolsMD5All.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual && Pak.ReadOnly == false;
        MMToolsConvertMenu.Enabled = Pak?.IsOpen == true && !Pak.IsVirtual && !Pak.ReadOnly;
        MMTools.Visible = Pak?.IsOpen == true;

        if (Pak?.IsOpen == true)
        {
            if (Pak.IsDirty)
                Text = _baseTitle + @" - *" + Pak.GpFilePath;
            else
                Text = _baseTitle + @" - " + Pak.GpFilePath;
            lPakExtraInfo.Text = Pak.NewestFileDate.ToString("yyyy-MM-dd HH:mm:ss");

            if (Pak.PakType == PakFileType.Reader)
                lTypePak.Text = Pak.Reader?.ReaderName ?? "Invalid Reader";
            else
                lTypePak.Text = Pak.PakType.ToString();
        }
        else
        {
            Text = _baseTitle;
            lPakExtraInfo.Text = @"...";
            lTypePak.Text = "";
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
        openGamePakDialog.ReadOnlyChecked = Properties.Settings.Default.OpenDefaultReadOnly;
        openGamePakDialog.Title = "Open existing Pak file";

        var overrideWarning = Control.ModifierKeys.HasFlag(Keys.Shift);
        if (openGamePakDialog.ShowDialog() == DialogResult.OK)
        {
            if (overrideWarning)
            {
                Properties.Settings.Default.SkipEditWarning = false;
                Properties.Settings.Default.Save();
            }
            Application.DoEvents();
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            LoadPakFile(openGamePakDialog.FileName, openGamePakDialog.ReadOnlyChecked);
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        UpdateMm();
    }

    private void LoadCustomReaders(string userFolder)
    {
        AAPak.ReaderPool.Clear();

        var jsonSettings = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture,
            Formatting = Formatting.Indented,
        };
        jsonSettings.Converters.Add(new ByteArrayHexConverter());

        var readerSettingsFileName = Path.Combine(userFolder, "readers.json");
        try
        {
            if (File.Exists(readerSettingsFileName))
            {
                var data = JsonConvert.DeserializeObject<List<AAPakFileFormatReader>>(File.ReadAllText(readerSettingsFileName), jsonSettings);
                if (data?.Count > 0)
                    foreach (var r in data)
                        AAPak.ReaderPool.Add(r);
            }
        }
        catch
        {
            // Ignore
        }

        // Add only default in case of errors
        if (AAPak.ReaderPool.Count <= 0)
        {
            AAPak.ReaderPool.Add(new AAPakFileFormatReader(true));
            // Write default file to user's settings
            try
            {
                Directory.CreateDirectory(userFolder);
                File.WriteAllText(readerSettingsFileName, JsonConvert.SerializeObject(AAPak.ReaderPool, jsonSettings));
            }
            catch
            {
                // Ignore
            }
        }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        // Update settings if needed
        if (!Properties.Settings.Default.IsUpdated)
        {
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            Properties.Settings.Default.IsUpdated = true;
            Properties.Settings.Default.Save();
        }

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

        // Initialize Pak Readers
        AAPak.ReaderPool.Clear();

        LoadCustomReaders(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ZeromusXYZ", "AAPakEditor"));

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
        Pak.OnProgress += AAPakNotify;

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
                MessageBox.Show("Reader  game_pak.key  does not seem valid for " + filename, "OpenPak Key Error");
            else
                MessageBox.Show("Failed to open " + filename + "\r\n" + Pak?.LastError, "OpenPak Error");
        }
        else
        {
            Text = _baseTitle + " - " + Pak.GpFilePath;


            if (!quickLoad)
                GenerateFolderViews();

            // Only show this waring if this is not a new pak file
            if ((openAsReadOnly == false) && (Pak.Files.Count > 0) && showWriteWarning && (Properties.Settings.Default.SkipEditWarning == false))
            {
                // Reset cursor to look nicer here
                Cursor.Current = Cursors.Default;
                Application.UseWaitCursor = false;

                using var warningDlg = new EditWarningDialog();
                warningDlg.ShowDialog();
            }
        }

        UpdateMm();
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

            ShowPreview(pfi);
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
            ShowPreview(null);
        }
    }

    /// <summary>
    /// Convert a stream into a string
    /// </summary>
    /// <param name="stream">Source stream</param>
    /// <returns>String value of the data isnide the stream</returns>
    public static string StreamToString(Stream stream)
    {
        stream.Position = 0;
        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    private void ShowPreview(AAPakFileInfo pfi)
    {
        if ((pfi == null) || (Properties.Settings.Default.AllowPreview == false))
        {
            if (PreviewForm.IsActive)
                PreviewForm.Instance?.Close();
            return;
        }

        var fileExt = Path.GetExtension(pfi.Name)?.ToLower() ?? string.Empty;

        if (fileExt == string.Empty)
        {
            if (PreviewForm.IsActive)
                PreviewForm.Instance?.Close();
            return;
        }

        if ((fileExt == ".xml") || (fileExt == ".fdp") || (fileExt == ".lyr") || (fileExt == ".fsq") ||
            (fileExt == ".joy") || (fileExt == ".fxl") || (fileExt == ".cba") || (fileExt == ".animevents") ||
            (fileExt == ".lmg") || (fileExt == ".ent") || (fileExt == ".mtl") || (fileExt == ".ccc"))
        {
            PreviewForm.Instance.tcViewer.SelectedTab = PreviewForm.Instance.tpBasicText;
            PreviewForm.Instance.tPreview.Language = Language.XML;
            var s = StreamToString(Pak.ExportFileAsStream(pfi));
            PreviewForm.Instance.tPreview.Text = s;
        }
        else
        if ((fileExt == ".html") || (fileExt == ".htm"))
        {
            PreviewForm.Instance.tcViewer.SelectedTab = PreviewForm.Instance.tpBasicText;
            PreviewForm.Instance.tPreview.Language = Language.HTML;
            var s = StreamToString(Pak.ExportFileAsStream(pfi));
            PreviewForm.Instance.tPreview.Text = s;
        }
        else
        if ((fileExt == ".txt") || (fileExt == ".g") || (fileExt == ".cfg") || (fileExt == ".cal") || (fileExt == ".ini"))
        {
            PreviewForm.Instance.tcViewer.SelectedTab = PreviewForm.Instance.tpBasicText;
            PreviewForm.Instance.tPreview.Language = Language.Custom;
            var s = StreamToString(Pak.ExportFileAsStream(pfi));
            PreviewForm.Instance.tPreview.Text = s;
        }
        else
        if ((fileExt == ".lua"))// || (fileExt == ".alb"))
        {
            PreviewForm.Instance.tcViewer.SelectedTab = PreviewForm.Instance.tpBasicText;
            // TODO: convert Alb to readable Lua
            PreviewForm.Instance.tPreview.Language = Language.Lua;
            var s = StreamToString(Pak.ExportFileAsStream(pfi));
            PreviewForm.Instance.tPreview.Text = s;
        }
        else
        if ((fileExt == ".jpg") || (fileExt == ".png") || (fileExt == ".bmp"))
        {
            PreviewForm.Instance.tcViewer.SelectedTab = PreviewForm.Instance.tpImage;
            try
            {
                var imgStream = Pak.ExportFileAsStream(pfi);
                var img = Image.FromStream(imgStream);
                PreviewForm.Instance.pbPreview.Image = img;
                PreviewForm.Instance.tcViewer.SelectedTab = PreviewForm.Instance.tpImage;
                PreviewForm.Instance.tPreview.Text = pfi.Name + "\n" + img.Width + " x " + img.Height;
                PreviewForm.Instance.pbPreview.Size = new Size(img.Width, img.Height);
            }
            catch (Exception e)
            {
                PreviewForm.Instance.tcViewer.SelectedTab = PreviewForm.Instance.tpBasicText;
                var s = $"Failed to load {pfi.Name}\n{e.Message}";
                PreviewForm.Instance.tPreview.Text = s;
            }
        }
        else
        if ((fileExt == ".dds") || (fileExt == ".tga"))
        {
            // Load using Pfim
            PreviewForm.Instance.tcViewer.SelectedTab = PreviewForm.Instance.tpImage;
            try
            {
                var imgStream = Pak.ExportFileAsStream(pfi);
                var img = ImageHelpers.ReadDdsFromStream(imgStream);
                PreviewForm.Instance.pbPreview.Image = img;
                PreviewForm.Instance.tcViewer.SelectedTab = PreviewForm.Instance.tpImage;
                PreviewForm.Instance.tPreview.Text = pfi.Name + "\n" + img.Width + " x " + img.Height;
                PreviewForm.Instance.pbPreview.Size = new Size(img.Width, img.Height);
            }
            catch (Exception e)
            {
                PreviewForm.Instance.tcViewer.SelectedTab = PreviewForm.Instance.tpBasicText;
                var s = $"Failed to load {pfi.Name}\n{e.Message}";
                PreviewForm.Instance.tPreview.Text = s;
            }
        }
        else
        {
            if (PreviewForm.IsActive)
                PreviewForm.Instance?.Close();
            return;
        }

        PreviewForm.Instance.Show();
        PreviewForm.Instance.BringToFront();
        PreviewForm.Instance.Location = new Point(this.Location.X + this.Width, this.Location.Y);
        Application.DoEvents();
        this.Focus();
        lbFiles.Focus();
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
        if (Pak == null)
        {
            MessageBox.Show("No Pak assigned.");
            return;
        }

        if (Pak.ReadOnly)
        {
            MessageBox.Show("Pak is opened in Read-Only mode, cannot add/replace files.");
            return;
        }

        var replaceFileName = string.Empty;
        if (lbFiles.SelectedIndex >= 0)
        {
            replaceFileName = _currentFileViewFolder;
            if (replaceFileName != "")
                replaceFileName += "/";
            replaceFileName += lbFiles.SelectedItem.ToString();
        }

        // Open add file dialog
        using var addDlg = new AddFileDialog();
        addDlg.Pak = Pak;
        addDlg.SuggestedFile = replaceFileName;

        if (_currentFileViewFolder != "")
            addDlg.SuggestedDir = _currentFileViewFolder + '/';

        if (addDlg.ShowDialog(this) == DialogResult.OK)
        {
            var diskFileName = addDlg.eDiskFileName.Text;
            var pakFileName = addDlg.ePakFileName.Text.ToLower().Replace("\\", "/"); // You just know people will put this wrong, so preemptive replace

            var isReplacing = Pak.FileExists(pakFileName);
            Pak.GetFileByName(pakFileName, out var oldPakFileInfo);
            // We need to copy the pfi values here, as they can possibly be overriden in case a on-location replace happens
            var oldPakFileInfoCreateTime = oldPakFileInfo.CreateTime;
            var oldPakFileInfoModifyTime = oldPakFileInfo.ModifyTime;
            var oldPakFileInfoMd5 = oldPakFileInfo.Md5;
            var oldPakFileInfoDummy1 = oldPakFileInfo.Dummy1;
            var oldPakFileInfoDummy2 = oldPakFileInfo.Dummy2;

            // addDlg.Dispose();

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


                var cTime = File.GetCreationTimeUtc(diskFileName);
                var mTime = File.GetLastWriteTimeUtc(diskFileName);

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

                // Skip MD5 calculations if we are keeping the old value when replacing, or if we are specifying one
                var oldMd5ReCalc = Pak.AutoUpdateMd5WhenAdding;
                if ((isReplacing && addDlg.cbMD5KeepExisting.Checked) || (addDlg.rbMD5Specified.Checked))
                    Pak.AutoUpdateMd5WhenAdding = false;
                else
                    Pak.AutoUpdateMd5WhenAdding = true;

                var res = Pak.AddFileFromStream(pakFileName, fs, cTime, mTime, addDlg.cbReserveSpareSpace.Checked, out var pfi);
                fs.Dispose();

                // Reset the flag
                Pak.AutoUpdateMd5WhenAdding = oldMd5ReCalc;

                if (res)
                {
                    // Update values as needed

                    // Create time
                    if (isReplacing && addDlg.cbCreateTimeKeepExisting.Checked)
                    {
                        pfi.CreateTime = oldPakFileInfoCreateTime;
                    }
                    else
                    {
                        //if (addDlg.rbCreateTimeSourceCreateTime.Checked)
                        //    pfi.CreateTime = File.GetCreationTimeUtc(diskFileName).ToFileTime();
                        if (addDlg.rbCreateTimeSourceModifiedTime.Checked)
                            pfi.CreateTime = File.GetLastWriteTimeUtc(diskFileName).ToFileTime();
                        if (addDlg.rbCreateTimePakCreateTime.Checked)
                            pfi.CreateTime = File.GetCreationTimeUtc(Pak.GpFilePath).ToFileTime();
                        if (addDlg.rbCreateTimeUtcNow.Checked)
                            pfi.CreateTime = DateTime.UtcNow.ToFileTime();
                        if (addDlg.rbCreateTimeSpecifiedTime.Checked)
                            pfi.CreateTime = addDlg.dtCreateTime.Value.ToFileTime();
                        if (addDlg.rbCreateTimeSpecifiedValue.Checked)
                            pfi.CreateTime = addDlg.CreateTimeAsNumber;
                    }

                    // Modify Time
                    if (isReplacing && addDlg.cbModifyTimeKeepExisting.Checked)
                    {
                        pfi.ModifyTime = oldPakFileInfoModifyTime;
                    }
                    else
                    {
                        if (addDlg.rbCreateTimeSourceCreateTime.Checked)
                            pfi.CreateTime = File.GetCreationTimeUtc(diskFileName).ToFileTime();
                        //if (addDlg.rbModifyTimeSourceModifiedTime.Checked)
                        //    pfi.ModifyTime = File.GetLastWriteTimeUtc(diskFileName).ToFileTime();
                        if (addDlg.rbModifyTimePakCreateTime.Checked)
                            pfi.ModifyTime = File.GetCreationTimeUtc(Pak.GpFilePath).ToFileTime();
                        if (addDlg.rbModifyTimeUtcNow.Checked)
                            pfi.ModifyTime = DateTime.UtcNow.ToFileTime();
                        if (addDlg.rbModifyTimeSpecifiedTime.Checked)
                            pfi.ModifyTime = addDlg.dtModifyTime.Value.ToFileTime();
                        if (addDlg.rbModifyTimeSpecifiedValue.Checked)
                            pfi.ModifyTime = addDlg.ModifyTimeAsNumber;
                    }

                    // MD5
                    if (isReplacing && addDlg.cbMD5KeepExisting.Checked)
                    {
                        pfi.Md5 = oldPakFileInfoMd5;
                    }
                    else
                    if (addDlg.rbMD5Specified.Checked)
                    {
                        pfi.Md5 = addDlg.Md5Value;
                    }

                    // Dummy 1
                    if (isReplacing && addDlg.cbDummy1KeepExisting.Checked)
                    {
                        pfi.Dummy1 = oldPakFileInfoDummy1;
                    }
                    else
                    if (addDlg.rbDummy1Specified.Checked)
                    {
                        pfi.Dummy1 = addDlg.Dummy1AsNumber;
                    }

                    // Dummy 2
                    if (isReplacing && addDlg.cbDummy2KeepExisting.Checked)
                    {
                        pfi.Dummy2 = oldPakFileInfoDummy2;
                    }
                    else
                    if (addDlg.rbDummy2Specified.Checked)
                    {
                        pfi.Dummy2 = addDlg.Dummy2AsNumber;
                    }

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
        Pak.OnProgress += AAPakNotify;
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
        OpenUrl(_urlGitHub);
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
        OpenUrl(_urlDiscord);
    }

    private void MMExtraMakeMod_Click(object sender, EventArgs e)
    {
        // Basically, what we'll make here, is just a simple pak file, but the first file added is going to be our modding tool's executable
        // Because of the way that XL's pak file format works, it's not encrypting or compressing the files.
        // Adding a executable as first file, and renaming that pak to .exe, would effectively create a executable with the rest of the
        // Pak's data attached to it. If you also mark that executable with a special tag that the executable knows to ignore, you can effectively
        // make a "self-extracting pak file" in the same way that the old Self Extracting Zip files work, except, that this time, the executable
        // is INSIDE it's own pak and not just in front of it, so you don't even need to account for the Offset where the data starts.
        // Pretty sure this was originally made by design, neat ^_^

        if ((Pak == null) || (Pak.IsOpen == false) || (Pak.IsVirtual == true))
        {
            MessageBox.Show("You must have a pak file open to be able to create a Mod or Patch", "Create Mod or Patch",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Warn user if not using a default pak type
        if (!((Pak.PakType == PakFileType.Classic) || (Pak.Reader?.IsDefault == true)))
        {
            if (MessageBox.Show(
                    "Your currently opened a pak file that is not in the default format!\r\n" +
                    "\r\n" +
                    "If you want to make a SFX Mod Installer, then take note that it only supports the default format.\r\n" +
                    "Using a custom reader is not possible with this installer.\r\n" +
                    "The resulting mod might not work on it's intended target pak file!\r\n" +
                    "If you are only making a manual patch file, this should not be a issue.\r\n" +
                    "\r\n" +
                    "Are you sure you want to continue?",
                    "Create Mod or Patch",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
        }

        using var makeModDlg = new MakeModForm();
        makeModDlg.mainPak = Pak;
        makeModDlg.ShowDialog();
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
                Pak.OnProgress += AAPakNotify;
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
            pak.OnProgress += AAPakNotify;
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

        using var rehashDlg = new ReMD5Dlg();
        rehashDlg.pak = Pak;
        rehashDlg.allFiles = (res == DialogResult.Yes);
        rehashDlg.ShowDialog();
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

    public void AAPakNotify(AAPak sender, AAPakLoadingProgressType progressType, int step, int maximum)
    {
        Invoke((MethodInvoker)delegate
        {
            // Running on the UI thread
            pbGeneric.Minimum = 0;
            pbGeneric.Maximum = maximum;
            pbGeneric.Value = step;
            pbGeneric.Visible = (step != maximum);

            statusBar.Refresh();
        });
    }

    private void MMFileS2_Click(object sender, EventArgs e)
    {
        if (Control.ModifierKeys.HasFlag(Keys.Shift))
            MMFileTryOpenUsingKeyList.Visible = true;
    }

    private void MMExportAsCsv_Click(object sender, EventArgs e)
    {
        if (Pak == null || !Pak.IsOpen)
            return;
        CreateCsvFile();
    }

    private void MMVersionGetLatest_Click(object sender, EventArgs e)
    {
        OpenUrl(_urlGitHubLatestRelease);
    }

    private void MMToolsConvertPak_Click(object sender, EventArgs e)
    {
        // Warning way too many if's coming
        if ((Pak?.IsOpen == true) && (Pak.ReadOnly == false))
        {
            var newType = Pak.PakType;
            var newReader = Pak.Reader;

            // Convert it
            if (sender is ToolStripMenuItem toolStripMenuItem)
            {
                if (toolStripMenuItem.Tag is AAPakFileFormatReader ffr)
                {
                    newType = PakFileType.Reader;
                    newReader = ffr;
                }
                else
                if (toolStripMenuItem.Tag is string val)
                {
                    if (val == "1")
                    {
                        newType = PakFileType.Classic;
                        newReader = null;
                    }
                }
            }

            if ((newType != Pak.PakType) || (newReader != Pak.Reader))
            {
                Pak.PakType = newType;
                Pak.Reader = newReader;
                if (newType == PakFileType.Reader)
                    Pak.SetCustomKey(newReader.HeaderEncryptionKey);
                else
                    Pak.SetDefaultKey();
                Pak.IsDirty = true;
                UpdateMm();
            }
        }
    }

    private void MMToolsConvertMenu_DropDownOpening(object sender, EventArgs e)
    {
        MMToolsConvertMenu.DropDownItems.Clear();
        var classicItem = new ToolStripMenuItem();
        classicItem.Text = "Classic";
        classicItem.Tag = "1";
        classicItem.Checked = (Pak?.PakType == PakFileType.Classic);
        classicItem.Click += MMToolsConvertPak_Click;
        MMToolsConvertMenu.DropDownItems.Add(classicItem);

        foreach (var aaPakFileFormatReader in AAPak.ReaderPool)
        {
            var newItem = new ToolStripMenuItem();
            newItem.Text = aaPakFileFormatReader.ReaderName;
            newItem.Tag = aaPakFileFormatReader;
            newItem.Checked = ((Pak?.PakType == PakFileType.Reader) && (Pak?.Reader == aaPakFileFormatReader));
            newItem.Click += MMToolsConvertPak_Click;
            MMToolsConvertMenu.DropDownItems.Add(newItem);
        }
    }

    private void MMTools_DropDownOpening(object sender, EventArgs e)
    {
        MMToolsPreview.Checked = Settings.Default.AllowPreview;
        MMToolsCreatePatch.Enabled = Pak.IsOpen && !Pak.IsDirty;
    }

    private void MMToolsPreview_Click(object sender, EventArgs e)
    {
        MMToolsPreview.Checked = !MMToolsPreview.Checked;
        Settings.Default.AllowPreview = MMToolsPreview.Checked;
        Settings.Default.Save();
    }

    private void MMToolsCreatePatch_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(
                $"Do you want to create a patch file for a older pak file to make it up to date to the currently opened one?",
                "Create Patch", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            return;
        }

        if (openGamePakDialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        lFileCount.Text = $"Opening old pak {openGamePakDialog.FileName} ...";

        var oldPak = new AAPak();
        oldPak.OnProgress += AAPakNotify;
        try
        {
            if (!oldPak.OpenPak(openGamePakDialog.FileName, true))
            {
                lFileCount.Text = $"Failed to open {openGamePakDialog.FileName}";
                MessageBox.Show($"Source file must be a pak file", "Not a pak", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
        }
        catch (Exception exception)
        {
            lFileCount.Text = $"Failed to open {openGamePakDialog.FileName}";
            MessageBox.Show(exception.Message, @"Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var filesToAdd = new List<string>();
        var filesToDelete = new List<string>();

        oldPak.OnProgress -= AAPakNotify;

        pbGeneric.Visible = true;
        pbGeneric.Minimum = 0;
        pbGeneric.Maximum = Pak.Files.Count;
        pbGeneric.Step = 1;

        lFileCount.Text = $"Sorting ...";
        statusBar.Refresh();

        var pakFiles = new Dictionary<string, AAPakFileInfo>();
        var oldFiles = new Dictionary<string, AAPakFileInfo>();
        var sortedPakFiles = Pak.Files.ToList();
        sortedPakFiles.Sort();
        foreach (var sortedPakFile in sortedPakFiles)
        {
            pakFiles.Add(sortedPakFile.Name, sortedPakFile);
        }

        var sortedOldPakFiles = oldPak.Files.ToList();
        sortedOldPakFiles.Sort();
        foreach (var oldSortedPakFile in sortedOldPakFiles)
        {
            oldFiles.Add(oldSortedPakFile.Name, oldSortedPakFile);
        }

        lFileCount.Text = $"Analyzing changed files ...";
        statusBar.Refresh();

        // Check changed and new files
        foreach (var (fileName, aaPakFileInfo) in pakFiles)
        {
            pbGeneric.PerformStep();
            if (!oldFiles.TryGetValue(fileName, out var oldAAPakFileInfo))
            {
                filesToAdd.Add(aaPakFileInfo.Name);
                continue;
            }

            if (
                (oldAAPakFileInfo.CreateTime != aaPakFileInfo.CreateTime) ||
                (oldAAPakFileInfo.ModifyTime != aaPakFileInfo.ModifyTime) ||
                (oldAAPakFileInfo.Dummy1 != aaPakFileInfo.Dummy1) ||
                (oldAAPakFileInfo.Dummy2 != aaPakFileInfo.Dummy2) ||
                (oldAAPakFileInfo.Size != aaPakFileInfo.Size) ||
                (oldAAPakFileInfo.SizeDuplicate != aaPakFileInfo.SizeDuplicate) ||
                (oldAAPakFileInfo.Md5.SequenceEqual(aaPakFileInfo.Md5) == false)
            )
            {
                filesToAdd.Add(aaPakFileInfo.Name);
            }
        }

        lFileCount.Text = $"Analyzing deleted files ...";
        pbGeneric.Minimum = 0;
        pbGeneric.Maximum = oldPak.Files.Count;
        pbGeneric.Step = 1;
        statusBar.Refresh();

        // Check deleted files
        foreach (var (fileName, oldAAPakFileInfo) in oldFiles)
        {
            pbGeneric.PerformStep();
            if (!pakFiles.TryGetValue(fileName, out var aaPakFileInfo))
            {
                filesToDelete.Add(fileName);
            }
        }

        lFileCount.Text = $"Analyzing files ...";

        if (MessageBox.Show($"Data changes from\r\n" +
                            $"{oldPak.GpFilePath}\r\n" +
                            $"=>\r\n" +
                            $"{Pak.GpFilePath}\r\n" +
                            $"\r\n" +
                            $"{filesToAdd.Count} file(s) added or changed\r\n" +
                            $"{filesToDelete.Count} file(s) removed\r\n" +
                            $"\r\n" +
                            $"Create patch pak file?",
                "Create Patch", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            lFileCount.Text = $"Cancelled";
            oldPak.ClosePak();
            return;
        }

        if (saveGamePakDialog.ShowDialog() != DialogResult.OK)
        {
            lFileCount.Text = $"Cancelled";
            oldPak.ClosePak();
            return;
        }

        lFileCount.Text = $"Creating {saveGamePakDialog.FileName}";

        var patchPak = new AAPak(saveGamePakDialog.FileName, false, true);
        patchPak.AutoUpdateMd5WhenAdding = true;

        lFileCount.Text = $"Adding {filesToAdd} file(s) ...";

        // Add files
        foreach (var fileName in filesToAdd)
        {
            if (!pakFiles.TryGetValue(fileName, out var aaPakFileInfo))
            {
                MessageBox.Show($"Where did {fileName} to go?");
                lFileCount.Text = $"Failed";
                patchPak.ClosePak();
                oldPak.ClosePak();
                return;
            }

            var fs = Pak.ExportFileAsStream(aaPakFileInfo);
            if (!patchPak.AddFileFromStream(fileName,
                    fs,
                    DateTime.FromFileTimeUtc(aaPakFileInfo.CreateTime),
                    DateTime.FromFileTimeUtc(aaPakFileInfo.ModifyTime),
                    false,
                    out var addedFile))
            {
                lFileCount.Text = $"Failed";
                MessageBox.Show($"Failed to add {fileName} to {patchPak.GpFilePath} !");
                patchPak.ClosePak();
                oldPak.ClosePak();
                return;
            }
        }

        // Add deleted.txt file
        if (filesToDelete.Count > 0)
        {
            var sb = new StringBuilder(filesToDelete.Count);
            sb.AppendJoin('\n', filesToDelete);
            var deletedStream = AAPak.StringToStream(sb.ToString());
            if (!patchPak.AddFileFromStream("deleted.txt", deletedStream, DateTime.UtcNow, DateTime.UtcNow, false,
                    out _))
            {
                MessageBox.Show($"Failed to add deleted.txt to {patchPak.GpFilePath} !");
            }
        }

        var patchPakFileName = patchPak.GpFilePath;

        lFileCount.Text = $"Cleaning up ...";

        oldPak.ClosePak();
        var totalAdded = patchPak.Files.Count;
        patchPak.ClosePak();

        lFileCount.Text = $"Complete!";

        if (MessageBox.Show($"Create patch pak completed !\n\r" +
                            $"Added {totalAdded} file(s)\n\r" +
                            $"\n\r" +
                            $"Do you want to open this new patch pak?",
                "Create patch", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        // Open the patch
        Application.DoEvents();
        Application.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;
        LoadPakFile(saveGamePakDialog.FileName, false, false);
        Cursor.Current = Cursors.Default;
        Application.UseWaitCursor = false;
        UpdateMm();
    }
}