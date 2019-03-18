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
using System.Threading;
using SubStreamHelper;

namespace AAPakEditor
{
    public partial class MainForm : Form
    {
        public AAPak pak;
        private string baseTitle = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MMFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MMFileOpen_Click(object sender, EventArgs e)
        {
            if (openGamePakDialog.ShowDialog() == DialogResult.OK)
            {
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;
                LoadPakFile(openGamePakDialog.FileName);
                Cursor.Current = Cursors.Default;
                Application.UseWaitCursor = false;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            baseTitle = Text;
            // LoadPakFile("C:\\ArcheAge\\Working\\game_pak");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((pak != null) && (pak.isOpen))
                pak.ClosePak();
        }

        private void LoadPakFile(string filename)
        {
            if (pak == null)
            {
                pak = new AAPak("");
            }
            if (pak.isOpen)
                pak.ClosePak();
            var res = pak.OpenPak(filename);
            if (!res)
            {
                Text = baseTitle;
                lFileCount.Text = "no files";
                lbFolders.Items.Clear();
                lbFiles.Items.Clear();
                MessageBox.Show("Failed to open " + openGamePakDialog.FileName);
            }
            else
            {
                Text = baseTitle + " - " + pak._gpFilePath;
                pak.GenerateFolderList();
                lFileCount.Text = pak.files.Count.ToString() + " files in " + pak.folders.Count.ToString() + " folders";
                lbFolders.Items.Clear();
                lbFiles.Items.Clear();
                foreach (string s in pak.folders)
                {
                    lbFolders.Items.Add(s);
                }
            }
        }

        private void lbFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbFiles.Items.Clear();
            if ((pak == null) || (!pak.isOpen))
                return;

            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            var d = (sender as ListBox).SelectedItem.ToString();
            var list = pak.GetFilesInDirectory(d);
            lFiles.Text = list.Count.ToString() + " files in " + d;
            foreach (AAPakFileInfo pfi in list)
            {
                var f = Path.GetFileName(pfi.name);
                lbFiles.Items.Add(f);
            }

            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            if ((lbFolders.SelectedIndex < 0) || (lbFiles.SelectedIndex < 0))
                return;

            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            var d = lbFolders.SelectedItem.ToString();
            if (d != "") d += "/";
            d += lbFiles.SelectedItem.ToString();
            AAPakFileInfo pfi = pak.GetFileByName(d);

            if (pfi.name != "")
            {
                lfiName.Text = pfi.name;
                lfiSize.Text = "Size: " + pfi.size.ToString() + " byte(s)";
                var h = BitConverter.ToString(pfi.md5).ToUpper().Replace("-", "");
                if (h == pak._header.nullHashString)
                {
                    lfiHash.Text = "MD5: Invalid or not calculated !";
                }
                else
                {
                    lfiHash.Text = "MD5: " + BitConverter.ToString(pfi.md5).ToUpper().Replace("-", "");
                }
                lfiCreateTime.Text = "Created: " + DateTime.FromFileTime(pfi.createTime).ToString();
                lfiModifyTime.Text = "Modified: " + DateTime.FromFileTime(pfi.modifyTime).ToString();
                lfiStartOffset.Text = "Start Offset: 0x" + pfi.offset.ToString("X16");
                lfiExtras.Text = "X 0x" + pfi.sizeDuplicate.ToString("X") + "  Z 0x" + pfi.paddingSize.ToString("X") + "  D1 0x" + pfi.dummy1.ToString("X") + "  D2 0x" + pfi.dummy2.ToString("X");
            }

            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        private void MMExportSelectedFile_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            if ((lbFolders.SelectedIndex < 0) || (lbFiles.SelectedIndex < 0))
            {
                MessageBox.Show("No file selected");
                return;
            }
            var d = lbFolders.SelectedItem.ToString();
            if (d != "") d += "/";
            d += lbFiles.SelectedItem.ToString();
            exportFileDialog.FileName = Path.GetFileName(d.Replace('/', Path.DirectorySeparatorChar));

            if (exportFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!ExportFile(d, exportFileDialog.FileName))
                {
                    MessageBox.Show("Failed to export " + d);
                }
            }

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

        public bool ExportFile(string sourceName, string destName)
        {
            AAPakFileInfo pfi = pak.GetFileByName(sourceName);
            return ExportFile(pfi, destName);
        }

        private void MMExportAll_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            if (exportFolderDialog.ShowDialog() != DialogResult.OK)
                return;

            if (MessageBox.Show("Are you sure you want to export all the files ?\r\nAll files in destination will be overwritten !", "Export All", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            ExportAllDlg export = new ExportAllDlg();
            export.pak = this.pak;
            export.TargetDir = exportFolderDialog.SelectedPath;

            export.ShowDialog(this);

            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        private void MMEXtraMD5_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            if ((lbFolders.SelectedIndex < 0) || (lbFiles.SelectedIndex < 0))
            {
                MessageBox.Show("No file selected");
                return;
            }
            var d = lbFolders.SelectedItem.ToString();
            if (d != "") d += "/";
            d += lbFiles.SelectedItem.ToString();

            AAPakFileInfo pfi = pak.GetFileByName(d);
            MessageBox.Show("MD5 Hash updated to " + pak.UpdateMD5(pfi));
        }

        private void MMFileSave_Click(object sender, EventArgs e)
        {
            if (pak.isDirty)
                pak.SaveHeader();
        }

        private void MMFile_DropDownOpening(object sender, EventArgs e)
        {
            MMFileSave.Enabled = (pak != null) && (pak.isOpen) && (pak.isDirty);
        }

        private void MMExport_DropDownOpening(object sender, EventArgs e)
        {
            MMExportSelectedFile.Enabled = (pak != null) && (pak.isOpen) && (lbFolders.SelectedIndex >= 0) && (lbFiles.SelectedIndex >= 0);
            MMExportSelectedFolder.Enabled = (pak != null) && (pak.isOpen) && (lbFolders.SelectedIndex >= 0);
            MMExportAll.Enabled = (pak != null) && (pak.isOpen);
        }

        private void MMExtra_DropDownOpening(object sender, EventArgs e)
        {
            MMExtraMD5.Enabled = (pak != null) && (pak.isOpen) && (lbFolders.SelectedIndex >= 0) && (lbFiles.SelectedIndex >= 0);
            MMExtraExportData.Enabled = (pak != null) && (pak.isOpen);
        }

        private void MMExtraExportData_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            DateTime newest = new DateTime(1600, 1, 1);
            
            List<string> sl = new List<string>();
            string s = "";
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
            foreach (AAPakFileInfo pfi in pak.files)
            {
                DateTime modTime = DateTime.FromFileTime(pfi.modifyTime);
                if (modTime > newest)
                    newest = modTime;

                s = pfi.name;
                s += ";" + pfi.size.ToString();
                s += ";" + pfi.offset.ToString();
                s += ";" + BitConverter.ToString(pfi.md5).Replace("-","").ToUpper();
                s += ";" + DateTime.FromFileTime(pfi.createTime).ToString();
                s += ";" + modTime.ToString();
                s += ";" + pfi.sizeDuplicate.ToString();
                s += ";" + pfi.paddingSize.ToString();
                s += ";" + pfi.dummy1.ToString();
                s += ";" + pfi.dummy2.ToString();
                sl.Add(s);

            }

            exportFileDialog.FileName = "game_pak_files_"+ newest.ToString("yyyyMMdd")+".csv";

            if (exportFileDialog.ShowDialog() != DialogResult.OK)
                return;

            File.WriteAllLines(exportFileDialog.FileName, sl);
        }
    }
}
