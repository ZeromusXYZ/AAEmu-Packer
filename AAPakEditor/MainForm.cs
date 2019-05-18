using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using SubStreamHelper;

namespace AAPakEditor
{
    public partial class MainForm : Form
    {
        public AAPak pak;
        private string urlGitHub = "https://github.com/ZeromusXYZ/AAEmu-Packer";
        private string urlDiscord = "https://discord.gg/GhVfDtK";
        private string baseTitle = "";
        private string currentFileViewFolder = "";
        private bool useDBKey = false;
        private bool useCustomKey = false;
        private byte[] dbKey = new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] customKey = new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public MainForm()
        {
            InitializeComponent();
        }

        private void UpdateMM()
        {
            MMFileSave.Enabled = (pak != null) && (pak.isOpen) && (!pak.readOnly) && (pak.isDirty);
            MMFileClose.Enabled = (pak != null) && (pak.isOpen);

            MMEditAddFile.Enabled = ((pak != null) && (pak.isOpen) && (pak.readOnly == false));
            MMEditImportFiles.Enabled = ((pak != null) && (pak.isOpen) && (pak.readOnly == false));
            MMEditDeleteSelected.Enabled = ((pak != null) && (pak.isOpen) && (pak.readOnly == false) && (lbFiles.SelectedIndex >= 0));
            MMEditReplace.Enabled = ((pak != null) && (pak.isOpen) && (pak.readOnly == false) && (lbFiles.SelectedIndex >= 0));
            MMEdit.Visible = (pak != null) && (pak.isOpen) && (!pak.readOnly);

            MMExportSelectedFile.Enabled = (pak != null) && (pak.isOpen) && (lbFiles.SelectedIndex >= 0);
            MMExportSelectedFolder.Enabled = (pak != null) && (pak.isOpen) && (currentFileViewFolder != "");
            MMExportAll.Enabled = (pak != null) && (pak.isOpen);
            MMExportDB.Enabled = (pak != null) && (pak.isOpen) && (lbFiles.SelectedIndex >= 0) && (useDBKey) && (Path.GetExtension(lbFiles.SelectedItem.ToString()) == ".sqlite3");
            MMExportDB.Visible = (pak != null) && (pak.isOpen) && (useDBKey);
            MMExportS2.Visible = MMExportDB.Visible;
            MMExport.Visible = (pak != null) && (pak.isOpen);

            MMExtraMD5.Enabled = (pak != null) && (pak.isOpen) && (pak.readOnly == false) && (lbFiles.SelectedIndex >= 0);
            MMExtraExportData.Enabled = (pak != null) && (pak.isOpen);
            MMExtra.Visible = (pak != null) && (pak.isOpen);

            if ((pak != null) && (pak.isOpen))
            {
                if (pak.isDirty)
                {
                    Text = baseTitle + " - *" + pak._gpFilePath;
                }
                else
                {
                    Text = baseTitle + " - " + pak._gpFilePath;
                }
            }
            else
            {
                Text = baseTitle;
            }
        }


        private void MMFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MMFileOpen_Click(object sender, EventArgs e)
        {
            openGamePakDialog.CheckFileExists = false;
            openGamePakDialog.ShowReadOnly = true;
            openGamePakDialog.Title = "Open EXISTING Pak file";
            if (openGamePakDialog.ShowDialog() == DialogResult.OK)
            {
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;
                LoadPakFile(openGamePakDialog.FileName, openGamePakDialog.ReadOnlyChecked);
                Cursor.Current = Cursors.Default;
                Application.UseWaitCursor = false;
            }
            UpdateMM();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            baseTitle = Text;
            var AppVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string v = "Version ";
            v += AppVer.Major.ToString();
            v += "." + AppVer.Minor.ToString();
            if ((AppVer.Build > 0) || (AppVer.MinorRevision > 0))
                v += "." + AppVer.Build.ToString();
            if (AppVer.MinorRevision > 0)
                v += "." + AppVer.MinorRevision.ToString();
            MMVersion.Text = v ;
            UpdateMM();
            // LoadPakFile("C:\\ArcheAge\\Working\\game_pak");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((pak != null) && (pak.isOpen))
            {
                pak.ClosePak();
                pak = null;
            }
            UpdateMM();
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
            UpdateMM();
        }

        private void LoadPakKeys(string fromDirectory)
        {
            useDBKey = false;
            useCustomKey = false;
            string fn ;

            // DB key
            fn = fromDirectory + "game_db.key";
            if (File.Exists(fn))
            {
                FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
                if (fs.Length != 16)
                {
                    fs.Dispose();
                    return;
                }
                fs.Read(dbKey, 0, 16);
                fs.Dispose();
                useDBKey = true;
            }

            // PAK-Header Key
            fn = fromDirectory + "game_pak.key";
            if (File.Exists(fn))
            {
                FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
                if (fs.Length != 16)
                {
                    fs.Dispose();
                    return;
                }
                fs.Read(customKey, 0, 16);
                fs.Dispose();
                useCustomKey = true;
                pak._header.SetCustomKey(customKey);
            }
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

            lFileCount.Text = "Opening Pak ... ";
            lFileCount.Refresh();
            try
            {
                LoadPakKeys(Path.GetDirectoryName(filename).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
            }
            catch
            {
                MessageBox.Show("Error loading custom keys");
            }
            var res = pak.OpenPak(filename,openAsReadOnly);
            if (!res)
            {
                Text = baseTitle;
                lFileCount.Text = "no files";
                lbFolders.Items.Clear();
                lbFiles.Items.Clear();
                UpdateMM();
                if (useCustomKey)
                    MessageBox.Show("Custom  game_pak.key  does not seem valid for " + openGamePakDialog.FileName,"OpenPak Key Error");
                else
                    MessageBox.Show("Failed to open " + openGamePakDialog.FileName,"OpenPak Error");
            }
            else
            {
                Text = baseTitle + " - " + pak._gpFilePath;

                GenerateFolderViews();

                // Only show this waring if this is not a new pak file
                if ((openAsReadOnly == false) && (pak.files.Count > 0))
                {
                    MessageBox.Show("!!! Warning !!!\r\n" +
                        "You have opened this pak in read/write mode !\r\n" +
                        "\r\n" +
                        "This program comes with absolutly NO warranty.\r\n" +
                        "It is possible that this program will inreversably damage your game files while editing.\r\n" +
                        "Please be sure that you have a backup available of the pak file you are editing.\r\n" +
                        "That being said, I did my best as to avoid possible damage (other than oderered by the user) caused by malfunctions.\r\n" +
                        "\r\n" +
                        "Enjoy, and edit responsibly.\r\n" +
                        "~ ZeromusXYZ",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        private void lbFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbFiles.Items.Clear();
            if ((pak == null) || (!pak.isOpen))
                return;

            var d = (sender as ListBox).SelectedItem.ToString();
            PopulateFilesList(d);
            UpdateMM();
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

            UpdateMM();

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

            exportFolderDialog.Description = "Select a folder to where to export all files to";
            exportFolderDialog.SelectedPath = Path.GetDirectoryName(pak._gpFilePath);
            if (exportFolderDialog.ShowDialog() != DialogResult.OK)
                return;

            if (MessageBox.Show("Are you sure you want to export all the files ?\r\nAll files in destination will be overwritten !", "Export All", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            ExportAllDlg export = new ExportAllDlg();
            export.pak = this.pak;
            export.TargetDir = exportFolderDialog.SelectedPath;
            export.Text = "Export All Files";
            export.masterRoot = "";

            try
            {
                export.ShowDialog(this);
            }
            catch (Exception x)
            {
                MessageBox.Show("ERROR: " + x.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
            UpdateMM();
       
        }

        private void MMFileSave_Click(object sender, EventArgs e)
        {
            if ((!pak.readOnly) && (pak.isDirty))
            {
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;
                pak.SaveHeader();
                Cursor.Current = Cursors.Default;
                Application.UseWaitCursor = false;
            }
            UpdateMM();
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

            exportFileDialog.FileName = Path.GetFileName(pak._gpFilePath) + "_files_"+ newest.ToString("yyyyMMdd")+".csv";

            if (exportFileDialog.ShowDialog() != DialogResult.OK)
                return;

            File.WriteAllLines(exportFileDialog.FileName, sl);
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
            UpdateMM();

        }

        private void PopulateFilesList(string withdir)
        {
            currentFileViewFolder = withdir;

            lbFiles.Items.Clear();
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            var list = pak.GetFilesInDirectory(currentFileViewFolder);
            if (currentFileViewFolder == "")
                lFiles.Text = list.Count.ToString() + " files in <root>";
            else
                lFiles.Text = list.Count.ToString() + " files in \"" + currentFileViewFolder + "\"";
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
            UpdateMM();
        }

        private void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;


            if ((e == null) || (e.Node == null))
                return;

            PopulateFilesList(e.Node.Name);
            e.Node.Expand();
            UpdateMM();
        }

        private void MMExtraDebugTest_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            UpdateMM();
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
            PopulateFilesList(currentFileViewFolder);

            UpdateMM();
        }

        private void MMEditAddFile_Click(object sender, EventArgs e)
        {
            // open add dialog
            AddFileDialog addDlg = new AddFileDialog();
            if (currentFileViewFolder != "")
            addDlg.suggestedDir = currentFileViewFolder + '/' ;
            if (addDlg.ShowDialog(this) == DialogResult.OK)
            {
                string diskfn = addDlg.eDiskFileName.Text;
                string pakfn = addDlg.ePakFileName.Text.ToLower().Replace("\\","/"); // You just know people will put this wrong, so pre-emptive replace
                addDlg.Dispose();

                // virual directory to the new file
                var newpakfilepath = Path.GetDirectoryName(pakfn.Replace("/","\\")).Replace("\\","/");

                if (File.Exists(diskfn))
                {
                    var sourceIsDBFile = (Path.GetExtension(diskfn).ToLower() == ".sqlite3");

                    bool doEncrypt = false;
                    if (sourceIsDBFile && useDBKey)
                    {
                        if (MessageBox.Show("Import of DB detected, do you want to encrypt before importing using the provided key ?", "Import as DB", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            doEncrypt = true;
                    }


                    DateTime cTime = File.GetCreationTime(diskfn);
                    DateTime mTime = File.GetLastWriteTime(diskfn);
                    AAPakFileInfo pfi = pak.nullAAPakFileInfo;
                    Stream fs;
                    FileStream fStream = new FileStream(diskfn, FileMode.Open,FileAccess.Read);
                    MemoryStream mStream = new MemoryStream();

                    if (doEncrypt)
                    {
                        while (fStream.Position < fStream.Length)
                        {
                            var rest = (int)(fStream.Length - fStream.Position);
                            // reading blocks doesn't end too well, comment for now :p
                            /*
                            if (rest > 4096)
                                rest = 4096;
                            */
                            byte[] buf = new byte[rest];
                            byte[] decBuf = new byte[rest];
                            fStream.Read(buf, 0, rest);
                            decBuf = AAPakFileHeader.EncryptAES(buf, dbKey, true);
                            mStream.Write(decBuf, 0, decBuf.Length);
                        }
                        fStream.Dispose();
                        mStream.Position = 0;
                        fs = mStream;
                    }
                    else
                    {
                        mStream.Dispose();
                        fs = fStream;
                    }

                    var res = pak.AddFileFromStream(pakfn, fs, cTime, mTime, true, out pfi);
                    fs.Dispose();
                    if (res == true)
                    {
                        MessageBox.Show("File:\r\n" + diskfn + "\r\n\r\nadded as:\r\n" + pfi.name);

                        if (pak.folders.IndexOf(newpakfilepath) < 0)
                        {
                            // We added to a new folder
                            GenerateFolderViews();
                        }

                        lbFolders.SelectedIndex = lbFolders.Items.IndexOf(newpakfilepath);
                        tvFolders.SelectedNode = tvFolders.Nodes.Find(newpakfilepath, true)[0];

                        PopulateFilesList(newpakfilepath);
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

            PopulateFilesList(currentFileViewFolder);
            UpdateMM();

        }

        private void MMFileClose_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            lbFiles.Items.Clear();
            lbFolders.Items.Clear();
            tvFolders.Nodes.Clear();
            lFiles.Text = "";
            lFileCount.Text = "Closing pak";
            lFileCount.Refresh();
            pak.ClosePak();
            lFileCount.Text = "Pak Closed";
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
            UpdateMM();
        }

        private void MMFileNew_Click(object sender, EventArgs e)
        {
            openGamePakDialog.CheckFileExists = false;
            openGamePakDialog.ShowReadOnly = false;
            openGamePakDialog.Title = "Create NEW Pak file";
            if (openGamePakDialog.ShowDialog() != DialogResult.OK)
                return;

            if (File.Exists(openGamePakDialog.FileName))
            {
                if (MessageBox.Show("Overwrite and erase the existing pak file ?\r\n" + openGamePakDialog.FileName, "Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            if (pak != null)
            {
                pak.ClosePak();
            }
            // Create and a new pakfile
            pak = new AAPak(openGamePakDialog.FileName, false, true);
            pak.ClosePak();
            // Re-open it in read/write mode
            LoadPakFile(openGamePakDialog.FileName, false);

            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
            UpdateMM();
        }

        private void MMExportSelectedFolder_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            exportFolderDialog.Description = "Select a folder to where to export \r\n" + currentFileViewFolder;
            exportFolderDialog.SelectedPath = Path.GetDirectoryName(pak._gpFilePath);
            if (exportFolderDialog.ShowDialog() != DialogResult.OK)
                return;

            if (MessageBox.Show("Are you sure you want to export \""+ currentFileViewFolder +"\" and all of it's sub-folders ?\r\nAll files in destination will be overwritten !", "Export Folder", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            ExportAllDlg export = new ExportAllDlg();
            export.pak = this.pak;
            export.TargetDir = exportFolderDialog.SelectedPath;
            export.masterRoot = currentFileViewFolder + "/" ;
            export.Text = "Export "+currentFileViewFolder ;

            try
            {
                export.ShowDialog(this);
            }
            catch (Exception x)
            {
                MessageBox.Show("ERROR: " + x.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
            UpdateMM();
        }

        private void MMEditImportFiles_Click(object sender, EventArgs e)
        {
            ImportFolderDlg importFolder = new ImportFolderDlg();
            importFolder.ePakFolder.Text = currentFileViewFolder;
            importFolder.eDiskFolder.Text = Path.GetDirectoryName(pak._gpFilePath);
            importFolder.pak = this.pak;
            try
            {
                if (importFolder.ShowDialog() == DialogResult.OK)
                {
                    GenerateFolderViews();
                    PopulateFilesList(currentFileViewFolder);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("ERROR: " + x.Message+" \r\n\r\nDo not forget to save your file to prevent further corruption !", 
                    "Exception", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
            importFolder.Dispose();
            UpdateMM();
        }

        private void MMVersionSourceCode_Click(object sender, EventArgs e)
        {
            Process.Start(urlGitHub);
        }

        private void MMExportDB_Click(object sender, EventArgs e)
        {
            if ((pak == null) || (!pak.isOpen))
                return;

            var d = currentFileViewFolder;
            if (d != "") d += "/";
            d += lbFiles.SelectedItem.ToString();
            exportFileDialog.FileName = Path.GetFileName(d.Replace('/', Path.DirectorySeparatorChar));

            if (exportFileDialog.ShowDialog() != DialogResult.OK)
                return;

            var pf = pak.ExportFileAsStream(d);
            MemoryStream ms = new MemoryStream();
            pf.CopyTo(ms);
            pf.Dispose();

            ms.Position = 0;
            

            FileStream fs = new FileStream(exportFileDialog.FileName, FileMode.Create);
            while (ms.Position < ms.Length)
            {
                var rest = (int)(ms.Length - ms.Position);
                // reading blocks doesn't end to well, comment for now :p
                /*
                if (rest > 4096)
                    rest = 4096;
                */
                byte[] buf = new byte[rest];
                byte[] decBuf = new byte[rest];
                ms.Read(buf, 0, rest);
                decBuf = AAPakFileHeader.EncryptAES(buf, dbKey, false);
                fs.Write(decBuf, 0, decBuf.Length);
            }
            fs.Dispose();
            ms.Dispose();
            MessageBox.Show("Debug: Done");
            UpdateMM();
        }

        private void VisitDiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(urlDiscord);
        }

        private void MMExtraMakeMod_Click(object sender, EventArgs e)
        {
            // Basically, what we'll make here, is just a simple pak file, but the first file added is going to be our modding tool's executable
            // Because of the way that XL's pak file format works, it's not encrypting or compressing the files.
            // Adding a executable as first file, and renaming that pak to .exe, would effectively create a executeable with the rest of the
            // Pak's data attached to it. If you also mark that executeable with a special tag that the executable knows to ignore, you can effectively
            // make a "self-extracting pak file" in the same way that the old Self Extractiong Zip files work, except, that this time, the executeable
            // is INSIDE it's own pak and not just in front of it, so you don't even need to account for the offset where the data starts.
            // Pretty sure this was originally made by design, neat ^_^

            using (var MakeModDlg = new MakeModForm())
            {
                MakeModDlg.mainPak = pak;
                MakeModDlg.ShowDialog();
            }
            
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            for(int i = 1; i < args.Length;i++)
            {
                var arg = args[i];
                if (File.Exists(arg))
                {
                    LoadPakFile(arg, true);
                }
            }
        }
    }
}
