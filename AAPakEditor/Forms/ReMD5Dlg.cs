using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AAPacker;

namespace AAPakEditor;

public partial class ReMD5Dlg : Form
{
    public bool allFiles = false;
    private string lastFile = string.Empty;

    public AAPak pak;
    private int toUpdate;
    private int updated;

    public ReMD5Dlg()
    {
        InitializeComponent();
    }

    private void ReMD5Dlg_Load(object sender, EventArgs e)
    {
        if (allFiles)
            Text += " (force all)";
        bgwRehash.RunWorkerAsync();
        btnCancel.Enabled = true;
        btnCancel.Text = "Cancel";
        lInfo.Text = "Scanning " + pak.Files.Count + " files ...";
        toUpdate = 0;

        // Count files
        foreach (var pfi in pak.Files)
        {
            if (bgwRehash.CancellationPending)
                break;

            if (allFiles || pfi.Md5.SequenceEqual(AAPakFileHeader.NullHash))
                toUpdate++;
        }

        lInfo.Text = "Updating " + toUpdate + " files ...";
    }

    private void bgwRehash_DoWork(object sender, DoWorkEventArgs e)
    {
        updated = 0;
        Thread.Sleep(1000);

        // Actually update
        foreach (var pfi in pak.Files)
        {
            if (bgwRehash.CancellationPending)
                break;

            if (allFiles || pfi.Md5.SequenceEqual(AAPakFileHeader.NullHash))
            {
                updated++;
                pak.UpdateMd5(pfi);

                var p = updated * 100 / toUpdate;
                if (updated < toUpdate && p >= 100)
                    p = 99;
                lastFile = pfi.Name;
                bgwRehash.ReportProgress(p);
            }
        }

        bgwRehash.ReportProgress(100);
        Thread.Sleep(1000);

        if (bgwRehash.CancellationPending) MessageBox.Show("Remaining rehash cancelled !");
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        bgwRehash.CancelAsync();
        btnCancel.Enabled = false;
    }

    private void bgwRehash_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        if (e.ProgressPercentage < 100 && lastFile != string.Empty)
            lInfo.Text = updated + " / " + toUpdate + "  " + lastFile;
        else
            lInfo.Text = "Updated " + updated + " / " + toUpdate + " files !";
        pbRehash.Minimum = 0;
        pbRehash.Maximum = toUpdate;
        pbRehash.Value = updated;
        if (e.ProgressPercentage >= 100) btnCancel.Enabled = false;
    }

    private void bgwRehash_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        if (updated > 0)
            MessageBox.Show("Update completed !");
        else
            MessageBox.Show("Everything was already up to date !");
        Close();
    }
}