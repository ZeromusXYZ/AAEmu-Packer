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
        static string DefaultGamePak = "game_pak";
        static string DefaultModPak = "mod.aamod";
        static string DefaultRestorePak = "mod.aarestore";
        private string DefaultTitle;
        private AAPak gamepak;
        private AAPak modpak;
        private AAPak restorepak;

        public ModMainForm()
        {
            InitializeComponent();
        }

        private void ModMainForm_Load(object sender, EventArgs e)
        {
            DefaultTitle = Text;
            // Try to open self as a mod first
            modpak = new AAPak(Application.ExecutablePath);
            if (!modpak.isOpen)
            {
                // External mod file
                if (File.Exists(DefaultModPak))
                {
                    modpak = new AAPak(DefaultGamePak, true);
                    Text = DefaultTitle + " - " + modpak._gpFilePath;
                }
                else
                {
                    modpak.ClosePak();
                    Text = DefaultTitle;
                }
            }
            else
            {
                // Self-containing installer
                Text = DefaultTitle + " - Self-Extractor";
            }
            if (File.Exists(DefaultGamePak))
                gamepak = new AAPak(DefaultGamePak, false);
            if (File.Exists(DefaultRestorePak))
                restorepak = new AAPak(DefaultRestorePak, true);
        }

        private bool OpenModProject(string aamodFileName, string gamepakFileName)
        {
            return true;
        }
    }
}
