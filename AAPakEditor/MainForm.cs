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
        private string currentFileViewFolder = "";

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
                LoadPakFile(openGamePakDialog.FileName, openGamePakDialog.ReadOnlyChecked);
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

        private void GenerateFolderViews()
        {
            lFileCount.Text = "Analyzing folder structure ... ";
            lFileCount.Refresh();

            pak.GenerateFolderList();

            lFileCount.Text = "Loading ... ";
            lFileCount.Refresh();

            lbFolders.Items.Clear();
            lbFiles.Items.Clear();
            tvFolders.Nodes.Clear();
            TreeNode rootNode = tvFolders.Nodes.Add("", "root");
            TreeNode foundNode = null;
            var c = 0;
            foreach (string s in pak.folders)
            {
                lbFolders.Items.Add(s);
                c++;
                if ((c % 250) == 0)
                {
                    lFileCount.Text = "Loading folders ... " + c.ToString() + " / " + pak.folders.Count.ToString();
                    lFileCount.Refresh();
                    //Thread.Sleep(1);
                }


                if (s != "")
                {
                    string[] dirwalk = s.Split('/');
                    string dd = "";
                    TreeNode lastNode = rootNode;
                    foreach (string ds in dirwalk)
                    {
                        if (dd != "")
                            dd += "/";
                        dd += ds;

                        foundNode = null;
                        foreach (TreeNode n in lastNode.Nodes)
                        {
                            if (n.Name == dd)
                            {
                                foundNode = n;
                                break;
                            }
                        }

                        //TreeNode[] nsearch = lastNode.Nodes.Find(ds, false);
                        // if (nsearch.Length <= 0)
                        if (foundNode == null)
                        {
                            // No node for this yet, make one
                            lastNode = lastNode.Nodes.Add(dd, ds);
                        }
                        else
                        {
                            lastNode = foundNode;
                        }
                    }
                }


            }
            rootNode.Expand();
            lFileCount.Text = pak.files.Count.ToString() + " files in " + pak.folders.Count.ToString() + " folders";
        }


        private void LoadPakFile(string filename, bool openAsReadOnly)
        {
            if (pak == null)
            {
                pak = new AAPak("",true);
            }
            if (pak.isOpen)
            {
                lFileCount.Text = "Closing pak ... ";
                lFileCount.Refresh();
                pak.ClosePak();
            }

            if (openAsReadOnly == false)
            {
                MessageBox.Show("!!! Warning !!!\r\n" +
                    "You are opening the pak in read/write mode !\r\n" +
                    "\r\n" +
                    "This program comes with absolutly NO warranty.\r\n" +
                    "It is possible that this program will inreversably damage your game files while editing.\r\n" +
                    "Please be sure that you have a backup available of the pak file you are editing.\r\n" +
                    "That being said, I did my best as to avoid possible damage (other than oderered by the user) caused by malfunctions.\r\n" +
                    "\r\n" +
                    "Enjoy, and edit responsibly.\r\n"+
                    "~ZeromusXYZ",
                    "Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

            lFileCount.Text = "Opening Pak ... ";
            lFileCount.Refresh();
            var res = pak.OpenPak(filename,openAsReadOnly);
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

                GenerateFolderViews();
            }

        }

        private void lbFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbFiles.Items.Clear();
            if ((pak == null) || (!pak.isOpen))
                return;

            var d = (sender as ListBox).SelectedItem.ToString();
            PopulateFilesList(d);
        }

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            if (lbFiles.SelectedIndex < 0)
                return;

            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            var d = currentFileViewFolder ;
            if (d != "") d += "/";
            d += lbFiles.SelectedItem.ToString();
            ref AAPakFileInfo pfi = ref pak.nullAAPakFileInfo;

            //if (pfi.name != "")
            if (pak.GetFileByName(d, ref pfi))
            {
                lfiName.Text = pfi.name;
                lfiSize.Text = "Size: " + pfi.size.ToString() + " byte(s)";
                if (pfi.paddingSize > 0)
                    lfiSize.Text += "  + " + pfi.paddingSize + " padding";

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
                lfiExtras.Text = "D1 0x" + pfi.dummy1.ToString("X") + "  D2 0x" + pfi.dummy2.ToString("X");
            }

            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        private void MMExportSelectedFile_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            if (lbFiles.SelectedIndex < 0)
            {
                MessageBox.Show("No file selected");
                return;
            }
            var d = currentFileViewFolder ;
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
            ref AAPakFileInfo pfi = ref pak.nullAAPakFileInfo;
            if (pak.GetFileByName(sourceName, ref pfi))
            {
                return ExportFile(pfi, destName);
            }
            else
            {
                return false;
            }
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

            if (lbFiles.SelectedIndex < 0)
            {
                MessageBox.Show("No file selected");
                return;
            }
            var d = currentFileViewFolder ;
            if (d != "") d += "/";
            d += lbFiles.SelectedItem.ToString();

            ref AAPakFileInfo pfi = ref pak.nullAAPakFileInfo;
            if (pak.GetFileByName(d, ref pfi))
            {
                MessageBox.Show("MD5 Hash updated to " + pak.UpdateMD5(pfi));
            }
            else
            {
                MessageBox.Show("ERROR: No file");
            }
       
        }

        private void MMFileSave_Click(object sender, EventArgs e)
        {
            if ((!pak.readOnly) && (pak.isDirty))
                pak.SaveHeader();
        }

        private void MMFile_DropDownOpening(object sender, EventArgs e)
        {
            MMFileSave.Enabled = (pak != null) && (pak.isOpen) && (!pak.readOnly) && (pak.isDirty);
        }

        private void MMExport_DropDownOpening(object sender, EventArgs e)
        {
            MMExportSelectedFile.Enabled = (pak != null) && (pak.isOpen) && (lbFiles.SelectedIndex >= 0);
            MMExportSelectedFolder.Enabled = (pak != null) && (pak.isOpen) && false ;
            MMExportAll.Enabled = (pak != null) && (pak.isOpen);
        }

        private void MMExtra_DropDownOpening(object sender, EventArgs e)
        {
            MMExtraMD5.Enabled = (pak != null) && (pak.isOpen) && (lbFiles.SelectedIndex >= 0);
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

        private void MMEdit_DropDownOpening(object sender, EventArgs e)
        {
            MMEditAddFile.Enabled = ((pak != null) && (pak.isOpen) && (pak.readOnly == false));
            MMEditImportFiles.Enabled = ((pak != null) && (pak.isOpen) && (pak.readOnly == false));
            MMEditDeleteSelected.Enabled = ((pak != null) && (pak.isOpen) && (pak.readOnly == false) && (lbFiles.SelectedIndex >= 0));
            MMEditReplace.Enabled = ((pak != null) && (pak.isOpen) && (pak.readOnly == false) && (lbFiles.SelectedIndex >= 0));
        }

        private void MMEditReplace_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen) || (lbFiles.SelectedIndex < 0))
                return;

            if (pak.readOnly)
            {
                MessageBox.Show("Pak is opened in Read-Only mode, cannot add/replace files.");
                return;
            }

            var filename = currentFileViewFolder ;
            if (filename != "") filename += "/";
            filename += lbFiles.SelectedItem.ToString();

            ref AAPakFileInfo pfi = ref pak.nullAAPakFileInfo;

            if (!pak.GetFileByName(filename, ref pfi))
                return;
            /*
            if (pfi.Equals(pak.nullAAPakFileInfo))
                return;
            */

            var maxSize = pfi.size + pfi.paddingSize;

            importFileDialog.FileName = lbFiles.SelectedItem.ToString();
            if (importFileDialog.ShowDialog() != DialogResult.OK)
                return;

            // DateTime createTime = File.GetCreationTime(importFileDialog.FileName);
            DateTime modifyTime = File.GetLastWriteTime(importFileDialog.FileName);

            try
            {
                FileStream fs = new FileStream(importFileDialog.FileName, FileMode.Open, FileAccess.Read);
                if (fs.Length > (maxSize))
                {
                    fs.Dispose();
                    MessageBox.Show(string.Format("File is too big!\r\n{0}\r\nCan only be replaced with a file with the \r\nmaximum size of {1} bytes",filename,maxSize));
                }
                fs.Position = 0;
                if (pak.ReplaceFile(ref pfi, fs, modifyTime) == false)
                {
                    MessageBox.Show(string.Format("Failed to replace file !\r\n{0}\r\nPak or File might damaged !!", filename));
                }
                fs.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }

        }

        private void tvFolders_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }

        private void PopulateFilesList(string withdir)
        {
            currentFileViewFolder = withdir;

            lbFiles.Items.Clear();
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            var list = pak.GetFilesInDirectory(currentFileViewFolder);
            lFiles.Text = list.Count.ToString() + " files in " + currentFileViewFolder;
            List<string> sl = new List<string>();
            foreach (AAPakFileInfo pfi in list)
            {
                var f = Path.GetFileName(pfi.name);
                // lbFiles.Items.Add(f);
                sl.Add(f);
            }
            sl.Sort();
            lbFiles.Items.AddRange(sl.ToArray());
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;

        }

        private void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;


            if ((e == null) || (e.Node == null))
                return;

            PopulateFilesList(e.Node.Name);
            e.Node.Expand();
        }

        private void MMExtraDebugTest_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            pak._header.WriteToFAT();
            FileStream fs = new FileStream("FAT-write.bin", FileMode.Create);
            pak._header.FAT.Position = 0;
            pak._header.FAT.CopyTo(fs);
            fs.Dispose();

            MessageBox.Show("FAT Location: " + pak._header.FirstFileInfoOffset.ToString());
        }

        private void MMEditDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen) || (lbFiles.SelectedIndex < 0))
                return;

            if (pak.readOnly)
            {
                MessageBox.Show("Pak is opened in Read-Only mode, cannot delete files.");
                return;
            }

            var filename = currentFileViewFolder;
            if (filename != "") filename += "/";
            filename += lbFiles.SelectedItem.ToString();

            ref AAPakFileInfo pfi = ref pak.nullAAPakFileInfo;

            if (!pak.GetFileByName(filename, ref pfi))
                return;

            if (MessageBox.Show("Are you sure you want to delete this file ?\r\n" + filename, "Delete", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            if (pak.DeleteFile(pfi))
            {
                MessageBox.Show("Reference to " + filename + " has been removed from the pak.");
            }

            if (lbFiles.Items.Count <= 1)
            {
                // If this was the last file listed in the directory listing, we will need to re-populate the folder views to update this change
                GenerateFolderViews();
            }
            lbFiles.Items.Clear();

        }

        private void MMEditAddFile_Click(object sender, EventArgs e)
        {
            // open add dialog
            AddFileDialog addDlg = new AddFileDialog();
            if (currentFileViewFolder != "")
            addDlg.suggestedDir = currentFileViewFolder + '/' ;
            if (addDlg.ShowDialog(this) == DialogResult.OK)
            {
                string diskfn = addDlg.eDiskFileName.Text.ToLower();
                string pakfn = addDlg.ePakFileName.Text;
                addDlg.Dispose();

                if (File.Exists(diskfn))
                {
                    DateTime cTime = File.GetCreationTime(diskfn);
                    DateTime mTime = File.GetLastWriteTime(diskfn);
                    AAPakFileInfo pfi = pak.nullAAPakFileInfo;
                    FileStream fs = new FileStream(diskfn, FileMode.Open,FileAccess.Read);
                    var res = pak.AddFileFromStream(pakfn, fs, cTime, mTime, true, out pfi);
                    fs.Dispose();
                    if (res == true)
                    {
                        MessageBox.Show("File: " + diskfn + "\r\nadded as: " + pfi.name);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add file: " + diskfn);
                    }
                }
                else
                {
                    MessageBox.Show("File not found: " + diskfn);
                }

            }
            else
            {
                addDlg.Dispose();
            }

        }
    }
}
