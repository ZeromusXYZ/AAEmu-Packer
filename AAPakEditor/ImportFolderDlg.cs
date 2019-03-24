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

    public partial class ImportFolderDlg : Form
    {

        public string TargetDir = "";
        public AAPak pak;
        public long totalImportedSize = 0 ;
        public long totalSize = 0;
        public int filesDone = 0;
        public int totalFiles = 0;
        private List<string> importFileList = new List<string>();
        private string masterRoot = "" ;
        private string currentSearchDir = "";
        private string etaTimeString = "";
        private int updateCounter = 0;
        private int errorCount = 0;
        private long totalSecondsUsed = 0;

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

            DirectoryInfo thisDir = new DirectoryInfo(path);
            FileInfo[] files = thisDir.GetFiles();
            foreach (FileInfo fi in files)
            {
                // Don't add hidden files, or the pak we are adding, or anything that is called game_pak
                bool canAdd = (((fi.Attributes & FileAttributes.Hidden) == 0) && (!fi.Name.StartsWith(".")) && ((path + fi.Name) != pak._gpFilePath) && (fi.Name != "game_pak"));

                // if file it larger than 512MB, ask if we really want to add it, it's likely a mistake
                if (canAdd && (fi.Length > 0x20000000))
                {
                    var mbSize = fi.Length / 0x100000 ;
                    canAdd = (MessageBox.Show("You are about to add a very large file, are you sure you want to include this file ?\r\n\r\n" + path + fi.Name + "\r\n" + fi.Length.ToString() + " bytes (" + mbSize.ToString() + " MB)", "Big File", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes);
                }

                if (canAdd)
                {
                    importFileList.Add(path.Substring(masterRoot.Length) + fi.Name);
                    totalSize += fi.Length;

                    updateCounter++;
                    if ((updateCounter % 100) == 0)
                        bgwImport.ReportProgress(0);
                }
            }

            DirectoryInfo[] dirs = thisDir.GetDirectories();
            foreach (DirectoryInfo di in dirs)
            {
                AddDirectory(path + di.Name + Path.DirectorySeparatorChar);
            }
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

            DateTime startTime = DateTime.Now  ;
            long diffTime = 0 ;
            long exptectedEndTime = diffTime + 30 ; // Start with 30s minimum
            long eta = 0;
            
            errorCount = 0;
            totalSecondsUsed = 0;
            for (int i = 0;i<importFileList.Count;i++)
            {
                if (bgwImport.CancellationPending)
                    break;

                diffTime = (long)Math.Round( (DateTime.Now - startTime).TotalSeconds );
                // Only update expected time every 50 files
                if ( ((i % 50) == 0) && (totalImportedSize > 0) && (totalSize > 0) )
                {
                    exptectedEndTime = (diffTime * totalSize) / totalImportedSize;
                    eta = exptectedEndTime - diffTime;
                }
                if ((diffTime > 15) && (eta > 0))
                {
                    // Only start adding eta when we are at least 15 seconds busy

                    if (eta < 10)
                    {
                        etaTimeString = ", a few seconds remaining";
                    }
                    else
                    if (eta < 100) // one and a half minute
                    {
                        etaTimeString = ", about " + eta.ToString() + " seconds remaining";
                    }
                    else
                    if (eta < 7200) // 2 hours
                    {
                        etaTimeString = ", about " + (eta / 60).ToString() + " minutes remaining";
                    }
                    else
                    {
                        etaTimeString = ", " + (eta / 3600).ToString() + " hours remaining";
                    }
                }

                var fn = masterRoot + importFileList[i];
                var pfn = importFileList[i].Replace(Path.DirectorySeparatorChar, '/');
                var fi = new FileInfo(fn);
                FileStream fs = new FileStream(fn,FileMode.Open,FileAccess.Read);
                AAPakFileInfo pfi = pak.nullAAPakFileInfo;
                var res = pak.AddFileFromStream(pakRoot + pfn,fs,fi.CreationTime,fi.LastWriteTime,false,out pfi);
                if (res)
                {
                    totalImportedSize += fs.Length;
                    filesDone++;
                    System.Threading.Thread.Sleep(5);
                }
                else
                {
                    errorCount++;
                }
                fs.Dispose();

                if ((filesDone % 50) == 0)
                    bgwImport.ReportProgress(((i * 100) / importFileList.Count));
            }
            etaTimeString = " almost done ...";
            bgwImport.ReportProgress(99);

            if (pak.isDirty)
                pak.SaveHeader();

            bgwImport.ReportProgress(100);
            totalSecondsUsed = (long)Math.Round((DateTime.Now - startTime).TotalSeconds);
            etaTimeString = "";

            if (bgwImport.CancellationPending)
            {
                MessageBox.Show("Remaining import cancelled !","Cancelled",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }

        }

        private void bgwImport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (currentSearchDir != "")
            {
                lInfo.Text = "Searching ("+ importFileList.Count.ToString() +" files): " + currentSearchDir ;
            }
            else
            {
                pbImport.Minimum = 0;
                pbImport.Maximum = (int)(totalSize / 1024);
                pbImport.Value = (int)(totalImportedSize / 1024);
                lInfo.Text = "Imported " + filesDone.ToString() + " / " + totalFiles.ToString() + " files" + etaTimeString;
            }
        }

        private void bgwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (errorCount > 0)
            {
                MessageBox.Show(errorCount.ToString() + " file(s) failed to import !", "Import errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            var n = totalSecondsUsed;
            var s = "\r\nTotal Time Used: ";
            if (n > 3600)
            {
                s += (n / (3600)).ToString("D2")+"h ";
                n = (n % 3600);
            }
            if (n > 60)
            {
                s += (n / (60)).ToString("D2") + "m ";
                n = (n % 60);
            }
            if (n > 0)
            {
                s += n.ToString("D2") + "s ";
            }

            MessageBox.Show("Import finished:\r\n" + 
                totalImportedSize.ToString() + " / " + totalSize.ToString() + " bytes "+
                "(" + (totalImportedSize / 1024 / 1024).ToString() + " / " + (totalSize / 1024 / 1024).ToString() + " MB)"+
                "\r\n"+
                filesDone + " / " + totalFiles+" files"+s,
                "Import completed",
                MessageBoxButtons.OK,MessageBoxIcon.Information
                );
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel importing ?","Cancel Import",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
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
                this.DialogResult = DialogResult.None;
                e.Cancel = true;
                if (bgwImport.CancellationPending == false)
                {
                    btnCancel_Click(null, null);
                }
            }

        }

        private void btnSearchFolder_Click(object sender, EventArgs e)
        {
            sourceFolderDialog.SelectedPath = eDiskFolder.Text;
            if (sourceFolderDialog.ShowDialog() == DialogResult.OK)
            {
                eDiskFolder.Text = sourceFolderDialog.SelectedPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Auto-correct pak folder location
            ePakFolder.Text = ePakFolder.Text.ToLower();
            ePakFolder.Text.Replace(Path.DirectorySeparatorChar, '/');
            if ((ePakFolder.Text != "") && (!ePakFolder.Text.EndsWith("/")) )
            {
                ePakFolder.Text += "/";
            }
            while (ePakFolder.Text.StartsWith("/"))
                ePakFolder.Text = ePakFolder.Text.TrimStart('/');

            eDiskFolder.Text.Replace('/',Path.DirectorySeparatorChar);
            if (!eDiskFolder.Text.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                eDiskFolder.Text += Path.DirectorySeparatorChar;
            }

            if (!Directory.Exists(eDiskFolder.Text+"."))
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
    }
}
