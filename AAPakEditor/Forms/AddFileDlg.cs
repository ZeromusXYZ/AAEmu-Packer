using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using AAPacker;

namespace AAPakEditor.Forms;

public partial class AddFileDialog : Form
{
    public string SuggestedDir { get; set; } = string.Empty;
    public string SuggestedFile { get; set; } = string.Empty;
    public AAPak Pak { get; set; }
    public long CreateTimeAsNumber { get; set; }
    public long ModifyTimeAsNumber { get; set; }
    public byte[] Md5Value { get; set; } = new byte[16];
    public uint Dummy1AsNumber { get; set; }
    public uint Dummy2AsNumber { get; set; }

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

    private bool TryParseLong(string text, out long value)
    {
        value = 0;
        if (text.StartsWith("0x") &&
            (long.TryParse(text.Substring(2),
                NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hexVal)))
        {
            value = hexVal;
            return true;
        }
        
        if (long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var val))
        {
            value = val;
            return true;
        }

        return false;
    }

    private void tCreateAsNumber_TextChanged(object sender, EventArgs e)
    {
        if (TryParseLong(tCreateAsNumber.Text, out var val))
        {
            CreateTimeAsNumber = val;
            tCreateAsNumber.ForeColor = SystemColors.WindowText;
        }
        else
        {
            CreateTimeAsNumber = 0;
            tCreateAsNumber.ForeColor = Color.Red;
        }
    }

    private void tModifyAsNumber_TextChanged(object sender, EventArgs e)
    {
        if (TryParseLong(tModifyAsNumber.Text, out var val))
        {
            ModifyTimeAsNumber = val;
            tModifyAsNumber.ForeColor = SystemColors.WindowText;
        }
        else
        {
            ModifyTimeAsNumber = 0;
            tModifyAsNumber.ForeColor = Color.Red;
        }
    }

    private void tHash_TextChanged(object sender, EventArgs e)
    {
        byte[] nHash ;
        if (tHash.Text.Length != 32)
        {
            nHash = new byte[16];
            tHash.ForeColor = Color.Red;
        }
        else
        {
            try
            {
                nHash = FilePropForm.StringToByteArrayFastest(tHash.Text);
                tHash.ForeColor = SystemColors.WindowText;
            }
            catch
            {
                nHash = new byte[16];
                tHash.ForeColor = Color.Red;
            }
        }
        Md5Value = nHash;
    }

    private void tDummy1_TextChanged(object sender, EventArgs e)
    {
        if (TryParseLong(tDummy1.Text, out var val) && (val >= uint.MinValue) && (val <= uint.MaxValue))
        {
            Dummy1AsNumber = (uint)val;
            tDummy1.ForeColor = SystemColors.WindowText;
        }
        else
        {
            Dummy1AsNumber = 0;
            tDummy1.ForeColor = Color.Red;
        }
    }

    private void tDummy2_TextChanged(object sender, EventArgs e)
    {
        if (TryParseLong(tDummy2.Text, out var val) && (val >= uint.MinValue) && (val <= uint.MaxValue))
        {
            Dummy2AsNumber = (uint)val;
            tDummy2.ForeColor = SystemColors.WindowText;
        }
        else
        {
            Dummy2AsNumber = 0;
            tDummy2.ForeColor = Color.Red;
        }
    }
}