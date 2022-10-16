using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AAPacker;

namespace AAPakEditor;

public partial class ImportFolderDlg : Form
{
    private bool AutoRun;
    private string currentSearchDir = "";
    private int errorCount;
    private string etaTimeString = "";
    public int filesDone;
    private readonly List<string> importFileList = new();
    private string masterRoot = "";
    public AAPak pak;

    public string TargetDir = "";
    public int totalFiles;
    public long totalImportedSize;
    private long totalSecondsUsed;
    public long totalSize;
    private int updateCounter;

    private long CreateTimeAsNumber { get; set; }
    private long ModifyTimeAsNumber { get; set; }
    private byte[] Md5Value { get; set; } = new byte[16];
    private uint Dummy1AsNumber { get; set; }
    private uint Dummy2AsNumber { get; set; }


    public ImportFolderDlg()
    {
        InitializeComponent();
    }

    private void ImportFolderDlg_Load(object sender, EventArgs e)
    {
        //bgwImport.RunWorkerAsync();
        btnSearchFolder.Enabled = true;
        btnCancel.Enabled = false;
        btnCancel.Text = "Cancel";
        eDiskFolder.Text = Properties.Settings.Default.LastImportFolder;

        RevertSettings();

        ShowHideAdvanced(cbShowAdvanced.Checked);
    }

    private void AddDirectory(string path)
    {
        if (bgwImport.CancellationPending)
            return;
        currentSearchDir = path.Substring(masterRoot.Length);

        var thisDir = new DirectoryInfo(path);
        var files = thisDir.GetFiles();
        foreach (var fi in files)
        {
            // Don't add hidden files, or the pak we are adding, or anything that is called game_pak
            var canAdd = (fi.Attributes & FileAttributes.Hidden) == 0 && !fi.Name.StartsWith(".") &&
                         path + fi.Name != pak.GpFilePath && fi.Name != "game_pak";

            // if file it larger than 512MB, ask if we really want to add it, it's likely a mistake
            if (canAdd && fi.Length > 0x20000000)
            {
                var mbSize = fi.Length / 0x100000;
                canAdd = MessageBox.Show(
                    "You are about to add a very large file, are you sure you want to include this file ?\r\n\r\n" +
                    path + fi.Name + "\r\n" + fi.Length + " bytes (" + mbSize + " MB)", "Big File",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
            }

            if (canAdd)
            {
                importFileList.Add(path.Substring(masterRoot.Length) + fi.Name);
                totalSize += fi.Length;

                updateCounter++;
                if (updateCounter % 100 == 0)
                    bgwImport.ReportProgress(0);
            }
        }

        var dirs = thisDir.GetDirectories();
        foreach (var di in dirs) AddDirectory(path + di.Name + Path.DirectorySeparatorChar);
    }

    private void bgwImport_DoWork(object sender, DoWorkEventArgs e)
    {
        // Calculate Total Size
        totalSize = 0;
        totalImportedSize = 0;
        totalFiles = 0;
        filesDone = 0;
        currentSearchDir = "";
        bgwImport.ReportProgress(0);
        masterRoot = eDiskFolder.Text;
        var pakRoot = ePakFolder.Text;
        updateCounter = 0;

        AddDirectory(masterRoot);

        currentSearchDir = "";
        totalFiles = importFileList.Count;

        //File.WriteAllLines("debugimportlist.txt",importFileList.ToArray());

        bgwImport.ReportProgress(0);

        var startTime = DateTime.Now;
        long diffTime = 0;
        var exptectedEndTime = diffTime + 30; // Start with 30s minimum
        long eta = 0;

        errorCount = 0;
        totalSecondsUsed = 0;
        for (var i = 0; i < importFileList.Count; i++)
        {
            if (bgwImport.CancellationPending)
                break;

            diffTime = (long)Math.Round((DateTime.Now - startTime).TotalSeconds);
            // Only update expected time every 50 files
            if (i % 50 == 0 && totalImportedSize > 0 && totalSize > 0)
            {
                exptectedEndTime = diffTime * totalSize / totalImportedSize;
                eta = exptectedEndTime - diffTime;
            }

            if (diffTime > 15 && eta > 0)
            {
                // Only start adding eta when we are at least 15 seconds busy

                if (eta < 10)
                    etaTimeString = ", a few seconds remaining";
                else if (eta < 100) // one and a half minute
                    etaTimeString = ", about " + eta + " seconds remaining";
                else if (eta < 7200) // 2 hours
                    etaTimeString = ", about " + eta / 60 + " minutes remaining";
                else
                    etaTimeString = ", " + eta / 3600 + " hours remaining";
            }

            var fn = masterRoot + importFileList[i];
            var pfn = importFileList[i].Replace(Path.DirectorySeparatorChar, '/');
            var fi = new FileInfo(fn);

            var cTime = File.GetCreationTimeUtc(fn);
            var mTime = File.GetLastWriteTimeUtc(fn);
            var isReplacing = pak.FileExists(pakRoot + pfn);

            var fs = new FileStream(fn, FileMode.Open, FileAccess.Read);

            var oldMd5ReCalc = pak.AutoUpdateMd5WhenAdding;
            if ((isReplacing && cbMD5KeepExisting.Checked) || (rbMD5Specified.Checked))
                pak.AutoUpdateMd5WhenAdding = false;
            else
                pak.AutoUpdateMd5WhenAdding = true;

            pak.GetFileByName(pakRoot + pfn, out var oldPakFileInfo);
            // We need to copy the pfi values here, as they can possibly be overriden in case a on-location replace happens
            var oldPakFileInfoCreateTime = oldPakFileInfo.CreateTime;
            var oldPakFileInfoModifyTime = oldPakFileInfo.ModifyTime;
            var oldPakFileInfoMd5 = oldPakFileInfo.Md5;
            var oldPakFileInfoDummy1 = oldPakFileInfo.Dummy1;
            var oldPakFileInfoDummy2 = oldPakFileInfo.Dummy2;

            var res = pak.AddFileFromStream(pakRoot + pfn, fs, cTime, mTime, cbReserveSpareSpace.Checked, out var pfi);
            if (res)
            {
                // Update values as needed

                // Create time
                if (isReplacing && cbCreateTimeKeepExisting.Checked)
                {
                    pfi.CreateTime = oldPakFileInfoCreateTime;
                }
                else
                {
                    //if (rbCreateTimeSourceCreateTime.Checked)
                    //    pfi.CreateTime = File.GetCreationTimeUtc(diskFileName).ToFileTime();
                    if (rbCreateTimeSourceModifiedTime.Checked)
                        pfi.CreateTime = File.GetLastWriteTimeUtc(fn).ToFileTime();
                    if (rbCreateTimePakCreateTime.Checked)
                        pfi.CreateTime = File.GetCreationTimeUtc(pak.GpFilePath).ToFileTime();
                    if (rbCreateTimeUtcNow.Checked)
                        pfi.CreateTime = DateTime.UtcNow.ToFileTime();
                    if (rbCreateTimeSpecifiedTime.Checked)
                        pfi.CreateTime = dtCreateTime.Value.ToFileTime();
                    if (rbCreateTimeSpecifiedValue.Checked)
                        pfi.CreateTime = CreateTimeAsNumber;
                }

                // Modify Time
                if (isReplacing && cbModifyTimeKeepExisting.Checked)
                {
                    pfi.ModifyTime = oldPakFileInfoModifyTime;
                }
                else
                {
                    if (rbCreateTimeSourceCreateTime.Checked)
                        pfi.CreateTime = File.GetCreationTimeUtc(fn).ToFileTime();
                    //if (addDlg.rbModifyTimeSourceModifiedTime.Checked)
                    //    pfi.ModifyTime = File.GetLastWriteTimeUtc(diskFileName).ToFileTime();
                    if (rbModifyTimePakCreateTime.Checked)
                        pfi.ModifyTime = File.GetCreationTimeUtc(pak.GpFilePath).ToFileTime();
                    if (rbModifyTimeUtcNow.Checked)
                        pfi.ModifyTime = DateTime.UtcNow.ToFileTime();
                    if (rbModifyTimeSpecifiedTime.Checked)
                        pfi.ModifyTime = dtModifyTime.Value.ToFileTime();
                    if (rbModifyTimeSpecifiedValue.Checked)
                        pfi.ModifyTime = ModifyTimeAsNumber;
                }

                // MD5
                if (isReplacing && cbMD5KeepExisting.Checked)
                {
                    pfi.Md5 = oldPakFileInfoMd5;
                }
                else
                if (rbMD5Specified.Checked)
                {
                    pfi.Md5 = Md5Value;
                }

                // Dummy 1
                if (isReplacing && cbDummy1KeepExisting.Checked)
                {
                    pfi.Dummy1 = oldPakFileInfoDummy1;
                }
                else
                if (rbDummy1Specified.Checked)
                {
                    pfi.Dummy1 = Dummy1AsNumber;
                }

                // Dummy 2
                if (isReplacing && cbDummy2KeepExisting.Checked)
                {
                    pfi.Dummy2 = oldPakFileInfoDummy2;
                }
                else
                if (rbDummy2Specified.Checked)
                {
                    pfi.Dummy2 = Dummy2AsNumber;
                }

                totalImportedSize += fs.Length;
                filesDone++;
                Thread.Sleep(1);
            }
            else
            {
                errorCount++;
            }

            fs.Dispose();

            if (filesDone % 50 == 0)
                bgwImport.ReportProgress(i * 100 / importFileList.Count);
        }

        etaTimeString = " almost done, please wait while saving ...";
        bgwImport.ReportProgress(99);

        if (pak.IsDirty)
            pak.SaveHeader();

        bgwImport.ReportProgress(100);
        totalSecondsUsed = (long)Math.Round((DateTime.Now - startTime).TotalSeconds);
        etaTimeString = "";

        if (bgwImport.CancellationPending)
            MessageBox.Show("Remaining import cancelled !", "Cancelled", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
    }

    private void bgwImport_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        if (currentSearchDir != "")
        {
            lInfo.Text = "Searching (" + importFileList.Count + " files): " + currentSearchDir;
        }
        else
        {
            pbImport.Minimum = 0;
            pbImport.Maximum = (int)(totalSize / 1024);
            pbImport.Value = (int)(totalImportedSize / 1024);
            lInfo.Text = "Imported " + filesDone + " / " + totalFiles + " files" + etaTimeString;
        }
    }

    private void bgwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        if (AutoRun)
        {
            DialogResult = DialogResult.OK;
            return;
        }

        if (errorCount > 0)
            MessageBox.Show(errorCount + " file(s) failed to import !", "Import errors", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        var n = totalSecondsUsed;
        var s = "\r\nTotal Time Used: ";
        if (n > 3600)
        {
            s += (n / 3600).ToString("D2") + "h ";
            n = n % 3600;
        }

        if (n > 60)
        {
            s += (n / 60).ToString("D2") + "m ";
            n = n % 60;
        }

        if (n > 0) s += n.ToString("D2") + "s ";

        MessageBox.Show("Import finished:\r\n" +
                        totalImportedSize + " / " + totalSize + " bytes " +
                        "(" + totalImportedSize / 1024 / 1024 + " / " + (totalSize / 1024 / 1024) + " MB)" +
                        "\r\n" +
                        filesDone + " / " + totalFiles + " files" + s,
            "Import completed",
            MessageBoxButtons.OK, MessageBoxIcon.Information
        );
        DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Are you sure you want to cancel importing ?", "Cancel Import", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
        {
            btnCancel.Enabled = false;
            btnCancel.Text = "Cancelling";
            bgwImport.CancelAsync();
        }
    }

    private void ImportFolderDlg_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (bgwImport.IsBusy)
        {
            DialogResult = DialogResult.None;
            e.Cancel = true;
            if (bgwImport.CancellationPending == false) btnCancel_Click(null, null);
        }
    }

    private void btnSearchFolder_Click(object sender, EventArgs e)
    {
        sourceFolderDialog.SelectedPath = eDiskFolder.Text;
        if (sourceFolderDialog.ShowDialog() == DialogResult.OK) eDiskFolder.Text = sourceFolderDialog.SelectedPath;
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
        // Auto-correct pak folder location
        ePakFolder.Text = ePakFolder.Text.ToLower();
        ePakFolder.Text.Replace(Path.DirectorySeparatorChar, '/');
        if (ePakFolder.Text != "" && !ePakFolder.Text.EndsWith("/")) ePakFolder.Text += "/";
        while (ePakFolder.Text.StartsWith("/"))
            ePakFolder.Text = ePakFolder.Text.TrimStart('/');

        eDiskFolder.Text.Replace('/', Path.DirectorySeparatorChar);
        if (!eDiskFolder.Text.EndsWith(Path.DirectorySeparatorChar.ToString()))
            eDiskFolder.Text += Path.DirectorySeparatorChar;

        if (!Directory.Exists(eDiskFolder.Text + "."))
        {
            eDiskFolder.Focus();
            return;
        }

        eDiskFolder.Enabled = false;
        ePakFolder.Enabled = false;
        btnStart.Enabled = false;
        btnSearchFolder.Enabled = false;
        btnCancel.Enabled = true;

        cbReserveSpareSpace.Enabled = false;
        gbCreateTime.Enabled = false;
        gbModifyTime.Enabled = false;
        gbMD5.Enabled = false;
        gbDummy1.Enabled = false;
        gbDummy2.Enabled = false;

        btnSetDefaults.Enabled = false;
        btnRevertSaved.Enabled = false;
        btnSaveDefaults.Enabled = false;

        Properties.Settings.Default.LastImportFolder = eDiskFolder.Text;
        Properties.Settings.Default.Save();

        bgwImport.RunWorkerAsync();
    }

    private void ImportFolderDlg_Shown(object sender, EventArgs e)
    {
        // do auto-run stuff when needed
        if (AutoRun)
            btnStart_Click(btnStart, null);
    }

    /// <summary>
    ///     Call this function before ShowDialog() to make this dialog automated
    /// </summary>
    /// <param name="sourceFolder">Source Directory</param>
    /// <param name="targetFolder">Directory Name to use in the pakfile</param>
    public void InitAutoRun(string sourceFolder, string targetFolder)
    {
        eDiskFolder.Text = sourceFolder;
        ePakFolder.Text = targetFolder;
        AutoRun = true;
    }

    private void cbShowAdvanced_CheckedChanged(object sender, EventArgs e)
    {
        ShowHideAdvanced(cbShowAdvanced.Checked);
    }

    private void ShowHideAdvanced(bool showing)
    {
        if (showing)
            ClientSize = new Size(ClientSize.Width, btnSetDefaults.Bottom + 16);
        else
            ClientSize = new Size(ClientSize.Width, cbShowAdvanced.Bottom + 16);
    }

    private void RevertSettings()
    {
        cbShowAdvanced.Checked = Properties.Settings.Default.AddShowAdvanced;

        cbCreateTimeKeepExisting.Checked = Properties.Settings.Default.AddCreateTimeKeepExisting;
        rbCreateTimeSourceCreateTime.Checked = Properties.Settings.Default.AddCreateTimeSourceCreateTime;
        rbCreateTimeSourceModifiedTime.Checked = Properties.Settings.Default.AddCreateTimeSourceModifiedTime;
        rbCreateTimePakCreateTime.Checked = Properties.Settings.Default.AddCreateTimePakCreateTime;
        rbCreateTimeUtcNow.Checked = Properties.Settings.Default.AddCreateTimeUtcNow;
        rbCreateTimeSpecifiedTime.Checked = Properties.Settings.Default.AddCreateTimeSpecifiedTime;
        try
        {
            dtCreateTime.Value = Properties.Settings.Default.AddCreateTime;
        }
        catch
        {
            dtCreateTime.Value = DateTime.UtcNow;
        }
        rbCreateTimeSpecifiedValue.Checked = Properties.Settings.Default.AddCreateTimeSpecifiedValue;
        tCreateAsNumber.Text = Properties.Settings.Default.AddCreateAsNumber;

        tModifyAsNumber.Text = Properties.Settings.Default.AddModifyAsNumber;
        rbModifyTimeSpecifiedValue.Checked = Properties.Settings.Default.AddModifyTimeSpecifiedValue;
        rbModifyTimeSpecifiedTime.Checked = Properties.Settings.Default.AddModifyTimeSpecifiedTime;
        try
        {
            dtModifyTime.Value = Properties.Settings.Default.AddModifyTime;
        }
        catch
        {
            dtModifyTime.Value = DateTime.UtcNow;
        }
        rbModifyTimeUtcNow.Checked = Properties.Settings.Default.AddModifyTimeUtcNow;
        rbModifyTimePakCreateTime.Checked = Properties.Settings.Default.AddModifyTimePakCreateTime;
        cbModifyTimeKeepExisting.Checked = Properties.Settings.Default.AddModifyTimeKeepExisting;
        rbModifyTimeSourceModifiedTime.Checked = Properties.Settings.Default.AddModifyTimeSourceModifiedTime;
        rbModifyTimeSourceCreateTime.Checked = Properties.Settings.Default.AddModifyTimeSourceCreateTime;

        rbMD5Specified.Checked = Properties.Settings.Default.AddMD5Specified;
        rbMD5Recalculate.Checked = Properties.Settings.Default.AddMD5Recalculate;
        cbMD5KeepExisting.Checked = Properties.Settings.Default.AddMD5KeepExisting;
        tHash.Text = Properties.Settings.Default.AddHash;

        tDummy1.Text = Properties.Settings.Default.AddDummy1;
        rbDummy1Specified.Checked = Properties.Settings.Default.AddDummy1Specified;
        rbDummy1Default.Checked = Properties.Settings.Default.AddDummy1Default;
        cbDummy1KeepExisting.Checked = Properties.Settings.Default.AddDummy1KeepExisting;

        tDummy2.Text = Properties.Settings.Default.AddDummy2;
        rbDummy2Specified.Checked = Properties.Settings.Default.AddDummy2Specified;
        rbDummy2Default.Checked = Properties.Settings.Default.AddDummy2Default;
        cbDummy2KeepExisting.Checked = Properties.Settings.Default.AddDummy2KeepExisting;

        cbReserveSpareSpace.Checked = Properties.Settings.Default.AddFileReserveSpace;
    }

    private void SaveSettings()
    {
        Properties.Settings.Default.AddShowAdvanced = cbShowAdvanced.Checked;

        Properties.Settings.Default.AddCreateTimeKeepExisting = cbCreateTimeKeepExisting.Checked;
        Properties.Settings.Default.AddCreateTimeSourceCreateTime = rbCreateTimeSourceCreateTime.Checked;
        Properties.Settings.Default.AddCreateTimeSourceModifiedTime = rbCreateTimeSourceModifiedTime.Checked;
        Properties.Settings.Default.AddCreateTimePakCreateTime = rbCreateTimePakCreateTime.Checked;
        Properties.Settings.Default.AddCreateTimeUtcNow = rbCreateTimeUtcNow.Checked;
        Properties.Settings.Default.AddCreateTimeSpecifiedTime = rbCreateTimeSpecifiedTime.Checked;
        Properties.Settings.Default.AddCreateTime = dtCreateTime.Value;
        Properties.Settings.Default.AddCreateTimeSpecifiedValue = rbCreateTimeSpecifiedValue.Checked;
        Properties.Settings.Default.AddCreateAsNumber = tCreateAsNumber.Text;

        Properties.Settings.Default.AddModifyAsNumber = tModifyAsNumber.Text;
        Properties.Settings.Default.AddModifyTimeSpecifiedValue = rbModifyTimeSpecifiedValue.Checked;
        Properties.Settings.Default.AddModifyTimeSpecifiedTime = rbModifyTimeSpecifiedTime.Checked;
        Properties.Settings.Default.AddModifyTime = dtModifyTime.Value;
        Properties.Settings.Default.AddModifyTimeUtcNow = rbModifyTimeUtcNow.Checked;
        Properties.Settings.Default.AddModifyTimePakCreateTime = rbModifyTimePakCreateTime.Checked;
        Properties.Settings.Default.AddModifyTimeKeepExisting = cbModifyTimeKeepExisting.Checked;
        Properties.Settings.Default.AddModifyTimeSourceModifiedTime = rbModifyTimeSourceModifiedTime.Checked;
        Properties.Settings.Default.AddModifyTimeSourceCreateTime = rbModifyTimeSourceCreateTime.Checked;

        Properties.Settings.Default.AddMD5Specified = rbMD5Specified.Checked;
        Properties.Settings.Default.AddMD5Recalculate = rbMD5Recalculate.Checked;
        Properties.Settings.Default.AddMD5KeepExisting = cbMD5KeepExisting.Checked;
        Properties.Settings.Default.AddHash = tHash.Text;

        Properties.Settings.Default.AddDummy1 = tDummy1.Text;
        Properties.Settings.Default.AddDummy1Specified = rbDummy1Specified.Checked;
        Properties.Settings.Default.AddDummy1Default = rbDummy1Default.Checked;
        Properties.Settings.Default.AddDummy1KeepExisting = cbDummy1KeepExisting.Checked;

        Properties.Settings.Default.AddDummy2 = tDummy2.Text;
        Properties.Settings.Default.AddDummy2Specified = rbDummy2Specified.Checked;
        Properties.Settings.Default.AddDummy2Default = rbDummy2Default.Checked;
        Properties.Settings.Default.AddDummy2KeepExisting = cbDummy2KeepExisting.Checked;

        Properties.Settings.Default.AddFileReserveSpace = cbReserveSpareSpace.Checked;

        Properties.Settings.Default.Save();
    }

    private void Changed()
    {
        btnSetDefaults.Enabled = true;
    }

    private void btnSetDefaults_Click(object sender, EventArgs e)
    {
        cbReserveSpareSpace.Checked = false;
        cbCreateTimeKeepExisting.Checked = true;
        rbCreateTimeSourceCreateTime.Checked = true;
        tCreateAsNumber.Text = "0x0";

        cbModifyTimeKeepExisting.Checked = false;
        rbModifyTimeSourceModifiedTime.Checked = true;
        tModifyAsNumber.Text = "0x0";

        cbMD5KeepExisting.Checked = false;
        rbMD5Recalculate.Checked = true;
        tHash.Text = string.Empty;

        cbDummy1KeepExisting.Checked = true;
        rbDummy1Default.Checked = true;
        tDummy1.Text = "0x0";

        cbDummy2KeepExisting.Checked = true;
        rbDummy2Default.Checked = true;
        tDummy2.Text = "0x0";

        btnSetDefaults.Enabled = false;
    }

    private void btnRevertSaved_Click(object sender, EventArgs e)
    {
        RevertSettings();
    }

    private void btnSaveDefaults_Click(object sender, EventArgs e)
    {
        SaveSettings();
    }

    private void SettingsCheckedChanged(object sender, EventArgs e)
    {
        Changed();
    }
}