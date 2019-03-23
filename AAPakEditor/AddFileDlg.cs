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
    public partial class AddFileDialog : Form
    {
        public string suggestedDir = "";

        public AddFileDialog()
        {
            InitializeComponent();
        }

        private void btnSearchFile_Click(object sender, EventArgs e)
        {
            if (openFileDlg.ShowDialog() != DialogResult.OK)
                return;
            eDiskFileName.Text = openFileDlg.FileName;
            ePakFileName.Text = suggestedDir + Path.GetFileName(eDiskFileName.Text).ToLower();
        }

        private void AddFileDialog_Load(object sender, EventArgs e)
        {
            ePakFileName.Text = suggestedDir;
        }
    }
}
