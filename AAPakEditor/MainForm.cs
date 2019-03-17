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
using SubStreamHelper;

namespace AAPakEditor
{
    public partial class MainForm : Form
    {
        AAPak pak;
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
                lfiStartOffset.Text = "Start Offset: 0x" + pfi.offset.ToString("X8");
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

        public bool ExportFile(string sourceName, string destName)
        {
            try
            {
                // Save file stream
                AAPakFileInfo pfi = pak.GetFileByName(sourceName);
                var filePakStream = pak.ExportFileAsStream(pfi);
                FileStream fs = new FileStream(destName, FileMode.Create);
                filePakStream.Position = 0;

                filePakStream.CopyTo(fs);

                filePakStream.Dispose();
                fs.Dispose();

                // Update file details
                File.SetCreationTime(exportFileDialog.FileName, DateTime.FromFileTime(pfi.createTime));
                File.SetLastWriteTime(exportFileDialog.FileName, DateTime.FromFileTime(pfi.modifyTime));
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}
