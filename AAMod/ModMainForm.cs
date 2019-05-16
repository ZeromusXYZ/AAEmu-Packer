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
using AAPakEditor;

namespace AAMod
{
    public partial class ModMainForm : Form
    {
        static string GamePakFileName = "game_pak";
        static string ModPakFileName = string.Empty;
        static string RestorePakFileName = string.Empty;
        static string SFXInfoFileName = "aamod/aamod.exe";
        static string ModInfoFileName = "aamod/aamod.txt";
        private string DefaultTitle;
        private string helpText = "Command-Line Arguments:\n\n" +
            "-m <modfile> : use custom mod file\n\n" +
            "-g <gamepak> : use specified game_pak, if not provided a file dialog will pop up to ask the location\n\n" +
            "-r <restorefile> : Use a custom restore file (not recommended)\n\n" +
            "-? : This help message";
        private AAPak gamepak = new AAPak("");
        private AAPak modpak = new AAPak("");
        private AAPak restorepak = new AAPak("");
        private List<AAPakFileInfo> FilesToMod = new List<AAPakFileInfo>();
        private List<AAPakFileInfo> FilesToInstall = new List<AAPakFileInfo>();
        private List<AAPakFileInfo> FilesToUnInstall = new List<AAPakFileInfo>();
        private List<AAPakFileInfo> FilesAddedWithInstall = new List<AAPakFileInfo>();
        private List<string> RestoreNewFilesList = new List<string>();

        public ModMainForm()
        {
            InitializeComponent();
        }

        private bool HandleArgs()
        {
            bool showHelp = false;

            var args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i++)
            {
                var larg = args[i].ToLower();
                string nextArg = "";
                if (i < args.Length-1)
                {
                    nextArg = args[i + 1];
                }


                switch(larg)
                {
                    case "-m":
                        // Mod source
                        ModPakFileName = nextArg;
                        i++;
                        break;
                    case "-g":
                        // Game_Pak
                        GamePakFileName = nextArg;
                        i++;
                        break;
                    case "-r":
                        // restore pak
                        RestorePakFileName = nextArg;
                        i++;
                        break;
                    case "-?":
                    case "-h":
                    case "-help":
                    case "/?":
                    case "/h":
                    case "/help":
                        // Help
                        showHelp = true;
                        break;
                }

            }
            if (showHelp)
            {
                Hide();
                MessageBox.Show(helpText, "AAMod - Command-Line Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            // if nothing specified, try self
            if (ModPakFileName == string.Empty)
            {
                ModPakFileName = Application.ExecutablePath;
                Text = DefaultTitle + " - Self-Extractor - " + Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            }
            else
            {
                Text = DefaultTitle + " - " + ModPakFileName;
            }

            if (!File.Exists(ModPakFileName))
            {
                MessageBox.Show("Mod-file not found\n" + ModPakFileName, "aamod Open Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (!modpak.OpenPak(ModPakFileName,true))
            {
                MessageBox.Show("Failed to open mod-file for reading\n" + ModPakFileName, "aamod Open Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            while (!File.Exists(GamePakFileName))
            {
                if (openGamePakDlg.ShowDialog() == DialogResult.OK)
                {
                    GamePakFileName = openGamePakDlg.FileName;
                }
                else
                {
                    return false;
                }
            }
            Refresh();
            lInstallLocation.Text = "Loading, please wait ...";
            lInstallLocation.Refresh();
            if (gamepak.OpenPak(GamePakFileName, false))
            {
                lInstallLocation.Text = GamePakFileName;
            }
            else
            {
                MessageBox.Show("Failed to open ArcheAge game pak for writing\n" + GamePakFileName, "game_pak Open Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (RestorePakFileName == string.Empty)
            {
                RestorePakFileName = Path.GetDirectoryName(GamePakFileName).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + ".aamod"+ Path.DirectorySeparatorChar + "restore_pak" ;
            }

            if (File.Exists(RestorePakFileName))
            {
                if (!restorepak.OpenPak(RestorePakFileName, false))
                {
                    MessageBox.Show("Failed to open mod uninstall file for writing\n" + RestorePakFileName, "restore_pak Open Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(RestorePakFileName));
                }
                catch
                {
                    MessageBox.Show("Failed to create mod uninstall directory for \n" + RestorePakFileName, "restore_pak Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                // create a new one if none exists yet
                if (!restorepak.NewPak(RestorePakFileName))
                {
                    MessageBox.Show("Failed to create mod uninstall file\n" + RestorePakFileName, "restore_pak Create Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            RestoreNewFilesList.Clear();
            AAPakFileInfo rnfl = restorepak.nullAAPakFileInfo;
            if (restorepak.GetFileByName("aamod/newfiles.txt",ref rnfl))
            {
                var rf = restorepak.ExportFileAsStream(rnfl);
                var s = StreamToString(rf);
                RestoreNewFilesList.AddRange(s.Split('\n').ToArray());
            }

            // TODO: continue work here

            return true;
        }

        private void ModMainForm_Load(object sender, EventArgs e)
        {
            DefaultTitle = Text;
        }

        private void ModMainForm_Shown(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            Cursor = Cursors.WaitCursor;
            // returns false if errors in initialization
            if (!HandleArgs())
            {
                Close();
                return;
            }

            if (modpak.FileExists(ModInfoFileName))
            {
                var infoStream = modpak.ExportFileAsStream(ModInfoFileName);
                var tw = StreamToString(infoStream);
                tDescription.Text = tw;
            }
            else
            {
                tDescription.Text = "No description provided for this mod file.";
            }
            tDescription.SelectionLength = 0;

            if (modpak.FileExists("aamod/aamod.png"))
            {
                var picStream = modpak.ExportFileAsStream("aamod/aamod.png");
                var img = Image.FromStream(picStream);
                modIcon.Image = img;
            }
            else
            if (modpak.FileExists("aamod/aamod.jpg"))
            {
                var picStream = modpak.ExportFileAsStream("aamod/aamod.jpg");
                var img = Image.FromStream(picStream);
                modIcon.Image = img;
            }

            ValidateInstallOptions();

            Cursor = Cursors.Default;
            UseWaitCursor = false;
        }

        static private string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static Stream StringToStream(string src)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(src);
            return new MemoryStream(byteArray);
        }

        private void ValidateInstallOptions()
        {
            // TODO: Enabled buttons depending on the state of game_pak and restore_pak compared to the aamod pak
            // Get file list from mod pak
            FilesToMod.Clear();
            foreach(var fi in modpak.files)
            {
                if (fi.name.StartsWith("aamod/"))
                {
                    // Don't include own mod files
                }
                else
                {
                    // TODO: compare to gamepak to check if installed or not
                    FilesToMod.Add(fi);
                }
            }

            FilesToInstall.Clear();
            FilesAddedWithInstall.Clear();
            foreach (var fi in FilesToMod)
            {
                AAPakFileInfo gfi = gamepak.nullAAPakFileInfo ;
                if (gamepak.GetFileByName(fi.name, ref gfi))
                {
                    if ( (fi.size != gfi.size) || (fi.md5 != gfi.md5) )
                    {
                        FilesToInstall.Add(fi);
                    }
                }
                else
                {
                    FilesToInstall.Add(fi);
                    FilesAddedWithInstall.Add(fi);
                }
            }

            FilesToUnInstall.Clear();
            foreach (var fi in FilesToMod)
            {
                if (restorepak.FileExists(fi.name))
                    FilesToUnInstall.Add(fi);
            }

            if (FilesToInstall.Count > 0)
            {
                btnInstall.Enabled = true;
                string s = string.Empty;
                if (FilesToMod.Count != 1)
                    s += "Mod " + FilesToMod.Count.ToString() + " file(s)";
                else
                    s += "Mod " + FilesToMod.Count.ToString() + " file";
                if (FilesAddedWithInstall.Count > 0)
                    s += ", with " + FilesAddedWithInstall.Count.ToString() + " new";
                lInstallInfo.Text = s;
            }

            // Check if these same files are all present in the restore pak
            // If not, disable the uninstall option (likely not installed)
            // TODO: 

        }

        private void BtnInstall_Click(object sender, EventArgs e)
        {
            btnInstall.Enabled = false;
            pb.Minimum = 0;
            pb.Maximum = (FilesToMod.Count * 2) - FilesAddedWithInstall.Count + 1 ;
            pb.Step = 1;
            lInstallInfo.Text = "Creating restore files";
            foreach(var fi in FilesToInstall)
            {
                // If file exists in gamepak, make a backup
                if (gamepak.FileExists(fi.name))
                {
                    var expStream = gamepak.ExportFileAsStream(fi.name);
                    expStream.Position = 0;
                    if (restorepak.AddFileFromStream(fi.name, expStream, DateTime.FromFileTime(fi.createTime), DateTime.FromFileTime(fi.modifyTime), false, out AAPakFileInfo newFile))
                    {
                        pb.PerformStep();
                    }
                }

                pb.PerformStep();
                pb.Refresh();
                System.Threading.Thread.Sleep(500);
            }

            // Add newly added files to the newfiles.txt
            foreach (var fi in FilesAddedWithInstall)
            {
                if (RestoreNewFilesList.IndexOf(fi.name) >= 0)
                    RestoreNewFilesList.Add(fi.name);
            }
            var ms = StringToStream(string.Join("\n",RestoreNewFilesList.ToArray()));

            restorepak.AddFileFromStream("aamod/newfiles.txt", ms, DateTime.Now, DateTime.Now, false, out var pfi);
            
            MessageBox.Show("Mod has been installed", "Install", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ValidateInstallOptions();
        }
    }
}
