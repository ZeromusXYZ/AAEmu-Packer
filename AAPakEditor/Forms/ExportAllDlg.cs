using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using AAPacker;

namespace AAPakEditor;

public partial class ExportAllDlg : Form
{
    public int filesDone;
    public string masterRoot = "";
    public AAPak pak;

    public string TargetDir = "";
    public long TotalExportedSize;
    public int TotalFileCountToExport;
    public long TotalSize;

    public ExportAllDlg()
    {
        InitializeComponent();
    }

    private void ExportAllDlg_Load(object sender, EventArgs e)
    {
        bgwExport.RunWorkerAsync();
        btnCancel.Enabled = true;
        btnCancel.Text = "Cancel";
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
            File.SetCreationTime(destName, DateTime.FromFileTimeUtc(pfi.CreateTime));
            File.SetLastWriteTime(destName, DateTime.FromFileTimeUtc(pfi.ModifyTime));
        }
        catch
        {
            return false;
        }

        return true;
    }

    private void bgwExport_DoWork(object sender, DoWorkEventArgs e)
    {
        // Calculate Total Size
        TotalSize = 0;
        TotalExportedSize = 0;
        TotalFileCountToExport = 0;
        foreach (var pfi in pak.Files)
        {
            if (bgwExport.CancellationPending)
                return;

            if (masterRoot != "")
                if (pfi.Name.Length <= masterRoot.Length || pfi.Name.Substring(0, masterRoot.Length) != masterRoot)
                    continue;

            TotalSize += pfi.Size;
            TotalFileCountToExport++;
        }

        filesDone = 0;

        foreach (var pfi in pak.Files)
        {
            if (bgwExport.CancellationPending)
                break;

            if (masterRoot != "")
                if (pfi.Name.Length <= masterRoot.Length || pfi.Name.Substring(0, masterRoot.Length) != masterRoot)
                    continue;

            var destName = TargetDir + Path.DirectorySeparatorChar;
            var exportedFileName = pfi.Name.Substring(masterRoot.Length);
            destName += exportedFileName.Replace('/', Path.DirectorySeparatorChar);

            // Check if target directory exists
            var destFolder = string.Empty;
            try
            {
                destFolder = Path.GetDirectoryName(destName);
            }
            catch
            {
                // Fallback for stuff with invalid chars
                destFolder = TargetDir + Path.DirectorySeparatorChar;
            }

            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            if (bgwExport.CancellationPending)
                break;

            // Export the file
            if (ExportFile(pfi, destName))
            {
                TotalExportedSize += pfi.Size;
                filesDone++;

                // We don't actually use this progress %, but let's put it in there anyway
                var p = TotalExportedSize * 100 / TotalSize;
                bgwExport.ReportProgress((int)p);
            }
        }

        if (bgwExport.CancellationPending) MessageBox.Show("Remaining export cancelled !");
    }

    private void bgwExport_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        pbExport.Minimum = 0;
        pbExport.Maximum = (int)(TotalSize / 1024);
        pbExport.Value = (int)(TotalExportedSize / 1024);
        lInfo.Text = "Exported " + filesDone + " / " + TotalFileCountToExport + " files";
    }

    private void bgwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        MessageBox.Show("Done exporting " + TotalExportedSize + " bytes (" + (TotalExportedSize / 1024 / 1024) + " MB)",
            "Export completed");
        DialogResult = DialogResult.OK;
        //Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Are you sure you want to cancel exporting ?", "Cancel Export", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
        {
            btnCancel.Enabled = false;
            btnCancel.Text = "Cancelling";
            bgwExport.CancelAsync();
        }
    }

    private void ExportAllDlg_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (bgwExport.IsBusy)
        {
            DialogResult = DialogResult.None;
            e.Cancel = true;
            if (bgwExport.CancellationPending == false) btnCancel_Click(null, null);
        }
    }
}