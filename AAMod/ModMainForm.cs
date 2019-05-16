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
        private List<string> FilesToMod = new List<string>();

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
            lInstallLocation.Text = "... opening pak, please wait ...";
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
                    FilesToMod.Add(fi.name);
                }
            }
            if (FilesToMod.Count > 0)
            {
                btnInstall.Enabled = true;
            }

            // Check if these same files are all present in the restore pak
            // If not, disable the uninstall option (likely not installed)
            // TODO: 

        }
    }
}
