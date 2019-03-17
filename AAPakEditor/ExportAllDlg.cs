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

        public ExportAllDlg()
        {
            InitializeComponent();
        }

        private void ExportAllDlg_Load(object sender, EventArgs e)
        {
            bgwExport.RunWorkerAsync();
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
            foreach (AAPakFileInfo pfi in pak.files)
            {
                TotalSize += pfi.size;
            }

            filesDone = 0;

            foreach (AAPakFileInfo pfi in pak.files)
            {
                var destName = TargetDir + Path.DirectorySeparatorChar;
                destName += pfi.name.Replace('/', Path.DirectorySeparatorChar);

                // Check if target directory exists
                var destFolder = Path.GetDirectoryName(destName);
                if (!Directory.Exists(destFolder))
                    Directory.CreateDirectory(destFolder);

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

        }

        private void bgwExport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbExport.Minimum = 0;
            pbExport.Maximum = (int)(TotalSize / 1024);
            pbExport.Value = (int)(TotalExportedSize / 1024);
            lInfo.Text = "Exported " + filesDone.ToString() + " / " + pak.files.Count.ToString() + " files";
        }

        private void bgwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done exporting " + TotalExportedSize.ToString() + " bytes (" + (TotalExportedSize / 1024 / 1024).ToString() + " MB)");
            DialogResult = DialogResult.OK;
            //Close();
        }
    }
}
