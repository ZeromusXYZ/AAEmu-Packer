using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace AAPakEditor
{
    public partial class ExportAllDlg : Form
    {

        public string TargetDir = "";
        public AAPak pak;
        public long TotalExportedSize = 0 ;
        public long TotalSize = 0;
        public int filesDone = 0;
        public int TotalFileCountToExport = 0;
        public string masterRoot = "";

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
                FileStream fs = new FileStream(destName, FileMode.Create);
                filePakStream.Position = 0;

                filePakStream.CopyTo(fs);

                filePakStream.Dispose();
                fs.Dispose();

                // Update file details
                File.SetCreationTime(destName, DateTime.FromFileTime(pfi.createTime));
                File.SetLastWriteTime(destName, DateTime.FromFileTime(pfi.modifyTime));
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
            foreach (AAPakFileInfo pfi in pak.files)
            {
                if (bgwExport.CancellationPending)
                    return;

                if (masterRoot != "")
                {
                    if ((pfi.name.Length <= masterRoot.Length) || (pfi.name.Substring(0, masterRoot.Length) != masterRoot))
                        continue;
                }

                TotalSize += pfi.size;
                TotalFileCountToExport++;
            }

            filesDone = 0;

            foreach (AAPakFileInfo pfi in pak.files)
            {
                if (bgwExport.CancellationPending)
                    break;

                if (masterRoot != "")
                {
                    if ((pfi.name.Length <= masterRoot.Length) || (pfi.name.Substring(0, masterRoot.Length) != masterRoot))
                        continue;
                }

                var destName = TargetDir + Path.DirectorySeparatorChar;
                var exportedFileName = pfi.name.Substring(masterRoot.Length);
                destName += exportedFileName.Replace('/', Path.DirectorySeparatorChar);

                // Check if target directory exists
                var destFolder = Path.GetDirectoryName(destName);
                if (!Directory.Exists(destFolder))
                    Directory.CreateDirectory(destFolder);

                if (bgwExport.CancellationPending)
                    break;

                // Export the file
                if (ExportFile(pfi, destName))
                {
                    TotalExportedSize += pfi.size;
                    filesDone++;

                    // We don't actually use this progress %, but let's put it in there anyway
                    var p = TotalExportedSize * 100 / TotalSize;
                    bgwExport.ReportProgress((int)p);
                }
            }

            if (bgwExport.CancellationPending)
            {
                MessageBox.Show("Remaining export cancelled !");
            }

        }

        private void bgwExport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbExport.Minimum = 0;
            pbExport.Maximum = (int)(TotalSize / 1024);
            pbExport.Value = (int)(TotalExportedSize / 1024);
            lInfo.Text = "Exported " + filesDone.ToString() + " / " + TotalFileCountToExport.ToString() + " files";
        }

        private void bgwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done exporting " + TotalExportedSize.ToString() + " bytes (" + (TotalExportedSize / 1024 / 1024).ToString() + " MB)","Export completed");
            DialogResult = DialogResult.OK;
            //Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel exporting ?","Cancel Export",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
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
                this.DialogResult = DialogResult.None;
                e.Cancel = true;
                if (bgwExport.CancellationPending == false)
                {
                    btnCancel_Click(null, null);
                }
            }

        }
    }
}
