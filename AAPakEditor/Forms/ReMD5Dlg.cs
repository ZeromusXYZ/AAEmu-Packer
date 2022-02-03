using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAPakEditor
{
    public partial class ReMD5Dlg : Form
    {

        public AAPak pak;
        public bool allFiles = false;
        private int toUpdate = 0;
        private int updated = 0;
        private string lastFile = string.Empty;

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
            lInfo.Text = "Scanning " + pak.files.Count.ToString() + " files ...";
            toUpdate = 0;

            // Count files
            foreach (var pfi in pak.files)
            {
                if (bgwRehash.CancellationPending)
                    break;

                if (allFiles || pfi.md5.SequenceEqual(AAPakFileHeader.nullHash))
                    toUpdate++;
            }

            lInfo.Text = "Updating " + toUpdate.ToString() + " files ...";
        }

        private void bgwRehash_DoWork(object sender, DoWorkEventArgs e)
        {
            updated = 0;
            System.Threading.Thread.Sleep(1000);

            // Actually update
            foreach (var pfi in pak.files)
            {
                if (bgwRehash.CancellationPending)
                    break;

                if (allFiles || pfi.md5.SequenceEqual(AAPakFileHeader.nullHash))
                {
                    updated++;
                    pak.UpdateMD5(pfi);

                    var p = updated * 100 / toUpdate;
                    if ((updated < toUpdate) && (p >= 100))
                        p = 99;
                    lastFile = pfi.name;
                    bgwRehash.ReportProgress((int)p);
                }
            }

            bgwRehash.ReportProgress(100);
            System.Threading.Thread.Sleep(1000);

            if (bgwRehash.CancellationPending)
            {
                MessageBox.Show("Remaining rehash cancelled !");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bgwRehash.CancelAsync();
            btnCancel.Enabled = false;
        }

        private void bgwRehash_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ((e.ProgressPercentage < 100) && (lastFile != string.Empty))
                lInfo.Text = updated.ToString() + " / " + toUpdate.ToString() + "  " + lastFile;
            else
                lInfo.Text = "Updated " + updated.ToString() + " / " + toUpdate.ToString() + " files !";
            pbRehash.Minimum = 0;
            pbRehash.Maximum = toUpdate;
            pbRehash.Value = updated;
            if (e.ProgressPercentage >= 100)
            {
                btnCancel.Enabled = false;
            }
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
}
