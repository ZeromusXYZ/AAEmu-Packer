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

        RevertSettings();

        ShowHideAdvanced(false);
    }

    private void RevertSettings()
    {
        cbShowAdvanced.Checked = Properties.Settings.Default.AddShowAdvanced;

        cbCreateTimeKeepExisting.Checked = Properties.Settings.Default.AddCreateTimeKeepExisting;
        rbCreateTimeSourceCreateTime.Checked = Properties.Settings.Default.AddCreateTimeSourceCreateTime;
        rbCreateTimeSourceModifiedTime.Checked = Properties.Settings.Default.AddCreateTimeSourceModifiedTime;
        rbCreateTimePakCreateTime.Checked = Properties.Settings.Default.AddCreateTimePakCreateTime;
        rbCreateTimeUtcNow.Checked = Properties.Settings.Default.AddCreateTimeUtcNow;
        rbCreateTimeSpecifiedTime.Checked = Properties.Settings.Default.AddCreateTimeSpecifiedTime;
        try
        {
            dtCreateTime.Value = Properties.Settings.Default.AddCreateTime;
        }
        catch
        {
            dtCreateTime.Value = DateTime.UtcNow;
        }
        rbCreateTimeSpecifiedValue.Checked = Properties.Settings.Default.AddCreateTimeSpecifiedValue;
        tCreateAsNumber.Text = Properties.Settings.Default.AddCreateAsNumber;

        tModifyAsNumber.Text = Properties.Settings.Default.AddModifyAsNumber;
        rbModifyTimeSpecifiedValue.Checked = Properties.Settings.Default.AddModifyTimeSpecifiedValue;
        rbModifyTimeSpecifiedTime.Checked = Properties.Settings.Default.AddModifyTimeSpecifiedTime;
        try
        {
            dtModifyTime.Value = Properties.Settings.Default.AddModifyTime;
        }
        catch
        {
            dtModifyTime.Value = DateTime.UtcNow;
        }
        rbModifyTimeUtcNow.Checked = Properties.Settings.Default.AddModifyTimeUtcNow;
        rbModifyTimePakCreateTime.Checked = Properties.Settings.Default.AddModifyTimePakCreateTime;
        cbModifyTimeKeepExisting.Checked = Properties.Settings.Default.AddModifyTimeKeepExisting;
        rbModifyTimeSourceModifiedTime.Checked = Properties.Settings.Default.AddModifyTimeSourceModifiedTime;
        rbModifyTimeSourceCreateTime.Checked = Properties.Settings.Default.AddModifyTimeSourceCreateTime;

        rbMD5Specified.Checked = Properties.Settings.Default.AddMD5Specified;
        rbMD5Recalculate.Checked = Properties.Settings.Default.AddMD5Recalculate;
        cbMD5KeepExisting.Checked = Properties.Settings.Default.AddMD5KeepExisting;
        tHash.Text = Properties.Settings.Default.AddHash;

        tDummy1.Text = Properties.Settings.Default.AddDummy1;
        rbDummy1Specified.Checked = Properties.Settings.Default.AddDummy1Specified;
        rbDummy1Default.Checked = Properties.Settings.Default.AddDummy1Default;
        cbDummy1KeepExisting.Checked = Properties.Settings.Default.AddDummy1KeepExisting;

        tDummy2.Text = Properties.Settings.Default.AddDummy2;
        rbDummy2Specified.Checked = Properties.Settings.Default.AddDummy2Specified;
        rbDummy2Default.Checked = Properties.Settings.Default.AddDummy2Default;
        cbDummy2KeepExisting.Checked = Properties.Settings.Default.AddDummy2KeepExisting;

        cbReserveSpareSpace.Checked = Properties.Settings.Default.AddFileReserveSpace;
    }

    private void SaveSettings()
    {
        Properties.Settings.Default.AddShowAdvanced = cbShowAdvanced.Checked;

        Properties.Settings.Default.AddCreateTimeKeepExisting = cbCreateTimeKeepExisting.Checked;
        Properties.Settings.Default.AddCreateTimeSourceCreateTime = rbCreateTimeSourceCreateTime.Checked;
        Properties.Settings.Default.AddCreateTimeSourceModifiedTime = rbCreateTimeSourceModifiedTime.Checked;
        Properties.Settings.Default.AddCreateTimePakCreateTime = rbCreateTimePakCreateTime.Checked;
        Properties.Settings.Default.AddCreateTimeUtcNow = rbCreateTimeUtcNow.Checked;
        Properties.Settings.Default.AddCreateTimeSpecifiedTime = rbCreateTimeSpecifiedTime.Checked;
        Properties.Settings.Default.AddCreateTime = dtCreateTime.Value;
        Properties.Settings.Default.AddCreateTimeSpecifiedValue = rbCreateTimeSpecifiedValue.Checked;
        Properties.Settings.Default.AddCreateAsNumber = tCreateAsNumber.Text;

        Properties.Settings.Default.AddModifyAsNumber = tModifyAsNumber.Text;
        Properties.Settings.Default.AddModifyTimeSpecifiedValue = rbModifyTimeSpecifiedValue.Checked;
        Properties.Settings.Default.AddModifyTimeSpecifiedTime = rbModifyTimeSpecifiedTime.Checked;
        Properties.Settings.Default.AddModifyTime = dtModifyTime.Value;
        Properties.Settings.Default.AddModifyTimeUtcNow = rbModifyTimeUtcNow.Checked;
        Properties.Settings.Default.AddModifyTimePakCreateTime = rbModifyTimePakCreateTime.Checked;
        Properties.Settings.Default.AddModifyTimeKeepExisting = cbModifyTimeKeepExisting.Checked;
        Properties.Settings.Default.AddModifyTimeSourceModifiedTime = rbModifyTimeSourceModifiedTime.Checked;
        Properties.Settings.Default.AddModifyTimeSourceCreateTime = rbModifyTimeSourceCreateTime.Checked;

        Properties.Settings.Default.AddMD5Specified = rbMD5Specified.Checked;
        Properties.Settings.Default.AddMD5Recalculate = rbMD5Recalculate.Checked;
        Properties.Settings.Default.AddMD5KeepExisting = cbMD5KeepExisting.Checked;
        Properties.Settings.Default.AddHash = tHash.Text;

        Properties.Settings.Default.AddDummy1 = tDummy1.Text;
        Properties.Settings.Default.AddDummy1Specified = rbDummy1Specified.Checked;
        Properties.Settings.Default.AddDummy1Default = rbDummy1Default.Checked;
        Properties.Settings.Default.AddDummy1KeepExisting = cbDummy1KeepExisting.Checked;

        Properties.Settings.Default.AddDummy2 = tDummy2.Text;
        Properties.Settings.Default.AddDummy2Specified = rbDummy2Specified.Checked;
        Properties.Settings.Default.AddDummy2Default = rbDummy2Default.Checked;
        Properties.Settings.Default.AddDummy2KeepExisting = cbDummy2KeepExisting.Checked;

        Properties.Settings.Default.AddFileReserveSpace = cbReserveSpareSpace.Checked;

        Properties.Settings.Default.Save();
    }


    private void cbShowAdvanced_CheckedChanged(object sender, EventArgs e)
    {
        ShowHideAdvanced(cbShowAdvanced.Checked);
    }

    private void ShowHideAdvanced(bool showing)
    {
        if (showing)
            ClientSize = new Size(ClientSize.Width, btnSetDefaults.Bottom + 16);
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
        Changed();
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
        Changed();
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
        Changed();
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
        Changed();
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
        Changed();
    }

    private void Changed()
    {
        btnSetDefaults.Enabled = true;

    }

    private void btnSetDefaults_Click(object sender, EventArgs e)
    {
        cbReserveSpareSpace.Checked = false;
        cbCreateTimeKeepExisting.Checked = true;
        rbCreateTimeSourceCreateTime.Checked = true;
        tCreateAsNumber.Text = "0x0";

        cbModifyTimeKeepExisting.Checked = false;
        rbModifyTimeSourceModifiedTime.Checked = true;
        tModifyAsNumber.Text = "0x0";

        cbMD5KeepExisting.Checked = false;
        rbMD5Recalculate.Checked = true;
        tHash.Text = string.Empty;

        cbDummy1KeepExisting.Checked = true;
        rbDummy1Default.Checked = true;
        tDummy1.Text = "0x0";

        cbDummy2KeepExisting.Checked = true;
        rbDummy2Default.Checked = true;
        tDummy2.Text = "0x0";

        btnSetDefaults.Enabled = false;
    }

    private void SettingsCheckedChanged(object sender, EventArgs e)
    {
        Changed();
    }

    private void btnRevertSaved_Click(object sender, EventArgs e)
    {
        RevertSettings();
    }

    private void btnSaveDefaults_Click(object sender, EventArgs e)
    {
        SaveSettings();
    }
}