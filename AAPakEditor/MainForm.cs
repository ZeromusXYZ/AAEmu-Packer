using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SubStream;

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
                if (pak.isOpen)
                    pak.ClosePak();
                Text = baseTitle;
                if (!pak.OpenPak(openGamePakDialog.FileName))
                {
                    MessageBox.Show("Failed to open " + openGamePakDialog.FileName);
                }
                else
                {
                    Text = baseTitle + " - " + pak._gpFilePath;
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            baseTitle = Text;
            pak = new AAPak("C:\\ArcheAge\\Working\\game_pak");
            if (pak.isOpen)
                Text = baseTitle + " - " + pak._gpFilePath;

        }
    }
}
