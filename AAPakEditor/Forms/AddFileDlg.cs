using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AAPacker;

namespace AAPakEditor.Forms;

public partial class AddFileDialog : Form
{
    public string SuggestedDir { get; set; } = string.Empty;
    public string SuggestedFile { get; set; } = string.Empty;
    public AAPak Pak { get; set; }

    public AddFileDialog()
    {
        InitializeComponent();
    }

    private void btnSearchFile_Click(object sender, EventArgs e)
    {
        if (openFileDlg.ShowDialog() != DialogResult.OK)
            return;
        eDiskFileName.Text = openFileDlg.FileName;
        ePakFileName.Text = SuggestedDir + Path.GetFileName(eDiskFileName.Text).ToLower();
    }

    private void AddFileDialog_Load(object sender, EventArgs e)
    {
        if (SuggestedFile != string.Empty)
            ePakFileName.Text = SuggestedFile;
        else
            ePakFileName.Text = SuggestedDir;

        ShowHideAdvanced(false);
        //cbShowAdvanced.Checked = false;
    }

    private void cbShowAdvanced_CheckedChanged(object sender, EventArgs e)
    {
        ShowHideAdvanced(cbShowAdvanced.Checked);
    }

    private void ShowHideAdvanced(bool showing)
    {
        if (showing)
            ClientSize = new Size(ClientSize.Width, gbDummy1.Bottom + 16);
        else
            ClientSize = new Size(ClientSize.Width, cbShowAdvanced.Bottom + 16);
    }

    private void ePakFileName_TextChanged(object sender, EventArgs e)
    {
        cbReserveSpareSpace.Enabled = !Pak?.FileExists(ePakFileName.Text) ?? true;
    }
}