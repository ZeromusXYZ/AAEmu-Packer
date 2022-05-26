using System;
using System.IO;
using System.Windows.Forms;

namespace AAPakEditor;

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