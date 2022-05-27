using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                         path + fi.Name != pak._gpFilePath && fi.Name != "game_pak";

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
            var fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
            var pfi = pak.NullAAPakFileInfo;
            var res = pak.AddFileFromStream(pakRoot + pfn, fs, fi.CreationTime, fi.LastWriteTime, false, out pfi);
            if (res)
            {
                totalImportedSize += fs.Length;
                filesDone++;
                Thread.Sleep(5);
            }
            else
            {
                errorCount++;
            }

            fs.Dispose();

            if (filesDone % 50 == 0)
                bgwImport.ReportProgress(i * 100 / importFileList.Count);
        }

        etaTimeString = " almost done ...";
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
}