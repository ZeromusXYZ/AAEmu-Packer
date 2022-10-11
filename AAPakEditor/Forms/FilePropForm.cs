using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using AAPacker;

namespace AAPakEditor;

public partial class FilePropForm : Form
{
    private readonly string defaultWarning =
        "Please edit with care.\r\nChanging values to unintended values might completely break the pak-file !";

    public AAPakFileInfo newInfo = new();
    public AAPakFileInfo pfi;

    public FilePropForm()
    {
        InitializeComponent();
    }

    private void FilePropForm_Load(object sender, EventArgs e)
    {
    }

    // source: https://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array#321404
    public static byte[] StringToByteArrayFastest(string hex)
    {
        if (hex.Length % 2 == 1)
            throw new Exception("The binary key cannot have an odd number of digits");

        var arr = new byte[hex.Length >> 1];

        for (var i = 0; i < hex.Length >> 1; ++i)
            arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));

        return arr;
    }

    public static int GetHexVal(char hex)
    {
        int val = hex;
        //For uppercase A-F letters:
        //return val - (val < 58 ? 48 : 55);
        //For lowercase a-f letters:
        //return val - (val < 58 ? 48 : 87);
        //Or the two combined, but a bit slower:
        return val - (val < 58 ? 48 : val < 97 ? 55 : 87);
    }

    public static bool TryFieldParse(string field, out int res)
    {
        var result = false;
        var isNegatice = field.StartsWith("-");
        if (isNegatice)
            field = field.TrimStart('-');
        if (field.StartsWith("+"))
            field = field.TrimStart('+');

        if (field.StartsWith("0x"))
            try
            {
                res = int.Parse(field.Substring(2, field.Length - 2), NumberStyles.HexNumber);
                result = true;
            }
            catch
            {
                res = 0;
            }
        else if (field.StartsWith("$"))
            try
            {
                res = int.Parse(field.Substring(1, field.Length - 1), NumberStyles.HexNumber);
                result = true;
            }
            catch
            {
                res = 0;
            }
        else if (field.EndsWith("h") || field.EndsWith("H"))
            try
            {
                res = int.Parse(field.Substring(0, field.Length - 1), NumberStyles.HexNumber);
                result = true;
            }
            catch
            {
                res = 0;
            }
        else
            try
            {
                res = int.Parse(field);
                result = true;
            }
            catch
            {
                res = 0;
            }

        if (isNegatice)
            res *= -1;
        return result;
    }

    public static bool TryFieldParse(string field, out long res)
    {
        var result = false;
        var isNegatice = field.StartsWith("-");
        if (isNegatice)
            field = field.TrimStart('-');
        if (field.StartsWith("+"))
            field = field.TrimStart('+');

        if (field.StartsWith("0x"))
            try
            {
                res = long.Parse(field.Substring(2, field.Length - 2), NumberStyles.HexNumber);
                result = true;
            }
            catch
            {
                res = 0;
            }
        else if (field.StartsWith("$"))
            try
            {
                res = long.Parse(field.Substring(1, field.Length - 1), NumberStyles.HexNumber);
                result = true;
            }
            catch
            {
                res = 0;
            }
        else if (field.EndsWith("h") || field.EndsWith("H"))
            try
            {
                res = long.Parse(field.Substring(0, field.Length - 1), NumberStyles.HexNumber);
                result = true;
            }
            catch
            {
                res = 0;
            }
        else
            try
            {
                res = long.Parse(field);
                result = true;
            }
            catch
            {
                res = 0;
            }

        if (isNegatice)
            res *= -1;
        return result;
    }

    public static bool TryFieldParseUInt64(string field, out ulong res)
    {
        var result = false;
        if (field.StartsWith("0x"))
            try
            {
                res = ulong.Parse(field.Substring(2, field.Length - 2), NumberStyles.HexNumber);
                result = true;
            }
            catch
            {
                res = 0;
            }
        else if (field.StartsWith("$"))
            try
            {
                res = ulong.Parse(field.Substring(1, field.Length - 1), NumberStyles.HexNumber);
                result = true;
            }
            catch
            {
                res = 0;
            }
        else if (field.EndsWith("h") || field.EndsWith("H"))
            try
            {
                res = ulong.Parse(field.Substring(0, field.Length - 1), NumberStyles.HexNumber);
                result = true;
            }
            catch
            {
                res = 0;
            }
        else
            try
            {
                res = ulong.Parse(field);
                result = true;
            }
            catch
            {
                res = 0;
            }

        return result;
    }

    public void ResetFileInfo()
    {
        if (pfi != null)
        {
            tName.Text = pfi.Name;
            tSize.Text = pfi.Size.ToString();
            tSizeDuplicate.Text = pfi.SizeDuplicate.ToString();
            tPaddingSize.Text = pfi.PaddingSize.ToString();

            tHash.Text = BitConverter.ToString(pfi.Md5).ToUpper().Replace("-", "");

            dtCreate.Value = DateTime.UtcNow;
            dtModified.Value = DateTime.UtcNow;
            try
            {
                dtCreate.Value = DateTime.FromFileTimeUtc(pfi.CreateTime);
            }
            catch
            {
                dtCreate.Enabled = false;
                tCreateAsNumber.Text = pfi.CreateTime.ToString();
            }

            try
            {
                dtModified.Value = DateTime.FromFileTimeUtc(pfi.ModifyTime);
            }
            catch
            {
                dtModified.Enabled = false;
                tModifyAsNumber.Text = pfi.ModifyTime.ToString();
            }

            tOffset.Text = "0x" + pfi.Offset.ToString("X");

            tDummy1.Text = "0x" + pfi.Dummy1.ToString("X");
            tDummy2.Text = "0x" + pfi.Dummy2.ToString("X");

            if (pfi.EntryIndexNumber >= 0)
                lfiIndex.Text = "index: " + pfi.EntryIndexNumber;
            else if (pfi.DeletedIndexNumber >= 0) lfiIndex.Text = "extra-index: " + pfi.DeletedIndexNumber;
        }
        else
        {
            lfiIndex.Text = "<no file selected>";
            tName.Text = "";
            tSize.Text = "";
            tSizeDuplicate.Text = "";
            tHash.Text = "";
            dtCreate.Value = DateTime.Now;
            dtModified.Value = DateTime.Now;
            tOffset.Text = "";
            tDummy1.Text = "";
            tDummy2.Text = "";
        }
    }

    private bool ValidateFields()
    {
        var res = true;
        var warnings = string.Empty;
        newInfo.Name = tName.Text;
        if (newInfo.Name == string.Empty)
        {
            warnings += "Filename cannot be empty\r\n";
            res = false;
        }
        else
        {
            try
            {
                if (Path.GetFileName(newInfo.Name) == string.Empty)
                {
                    warnings += "Filename might be invalid.\r\n";
                    res = false;
                }
            }
            catch
            {
                warnings += "Filename contains invalid characters.\r\n";
            }
        }

        if (TryFieldParse(tSize.Text, out long nsize))
        {
            newInfo.Size = nsize;
        }
        else
        {
            warnings += "Size is not a valid number\r\n";
            res = false;
        }

        if (TryFieldParse(tSizeDuplicate.Text, out long nsizedup))
        {
            newInfo.SizeDuplicate = nsizedup;
        }
        else
        {
            warnings += "Size Duplicate is not a valid number\r\n";
            res = false;
        }

        if (TryFieldParse(tPaddingSize.Text, out int npadding))
        {
            newInfo.PaddingSize = npadding;
        }
        else
        {
            warnings += "Padding Size is not a valid number\r\n";
            res = false;
        }

        if (nsizedup != nsize) warnings += "Size and Size Duplicate have different values !\r\n";
        if ((nsize + npadding) % 512 != 0) warnings += "Size + Padding is not a multiple of 512 !\r\n";
        var nhash = new byte[16];
        if (tHash.Text.Length != 32)
        {
            warnings += "MD5 needs to be a 128 bits hex string (32 hex character) !\r\n";
            res = false;
        }
        else
        {
            try
            {
                nhash = StringToByteArrayFastest(tHash.Text);
            }
            catch
            {
                warnings += "MD5 seems to contains invalid characters !\r\n";
                nhash = new byte[16];
                res = false;
            }
        }

        newInfo.Md5 = nhash;

        try
        {
            dtCreate.Enabled = string.IsNullOrWhiteSpace(tCreateAsNumber.Text);
            if (dtCreate.Enabled)
            {
                newInfo.CreateTime = dtCreate.Value.ToFileTimeUtc();
            }
            else
            {
                if (TryFieldParse(tCreateAsNumber.Text, out long nCreateTime))
                {
                    newInfo.CreateTime = nCreateTime;
                }
                else
                {
                    warnings += "Create Time is not a valid number\r\n";
                    res = false;
                }
            }
        }
        catch
        {
            warnings += "Invalid file create time !\r\n";
            res = false;
        }

        try
        {
            dtModified.Enabled = string.IsNullOrWhiteSpace(tModifyAsNumber.Text);
            if (dtModified.Enabled)
            {
                newInfo.ModifyTime = dtModified.Value.ToFileTimeUtc();
            }
            else
            {
                if (TryFieldParse(tModifyAsNumber.Text, out long nModifiedTime))
                {
                    newInfo.ModifyTime = nModifiedTime;
                }
                else
                {
                    warnings += "Modified Time is not a valid number\r\n";
                    res = false;
                }
            }
        }
        catch
        {
            warnings += "Invalid file modify time !\r\n";
            res = false;
        }

        if (TryFieldParse(tOffset.Text, out long noffset))
        {
            newInfo.Offset = noffset;
        }
        else
        {
            warnings += "Offset is not a valid number\r\n";
            res = false;
        }

        if (TryFieldParse(tDummy1.Text, out long nd1))
        {
            newInfo.Dummy1 = (uint)nd1;
        }
        else
        {
            warnings += "Dummy1 is not a valid number\r\n";
            res = false;
        }

        if (TryFieldParse(tDummy2.Text, out long nd2))
        {
            newInfo.Dummy2 = (uint)nd2;
        }
        else
        {
            warnings += "Dummy2 is not a valid number\r\n";
            res = false;
        }

        if (warnings == string.Empty)
            warnings = defaultWarning;

        tWarnings.Text = warnings;
        return res;
    }

    private bool hasChanged()
    {
        return pfi.Name != newInfo.Name ||
               pfi.Size != newInfo.Size ||
               pfi.SizeDuplicate != newInfo.SizeDuplicate ||
               pfi.PaddingSize != newInfo.PaddingSize ||
               pfi.Offset != newInfo.Offset ||
               BitConverter.ToString(pfi.Md5) != BitConverter.ToString(newInfo.Md5) ||
               pfi.CreateTime != newInfo.CreateTime ||
               pfi.ModifyTime != newInfo.ModifyTime ||
               pfi.Dummy1 != newInfo.Dummy1 ||
               pfi.Dummy2 != newInfo.Dummy2;
    }

    private void tFieldsChanged(object sender, EventArgs e)
    {
        btnSave.Enabled = ValidateFields() && hasChanged();
    }

    private void lCTtoR_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(tCreateAsNumber.Text))
            tCreateAsNumber.Text = newInfo.CreateTime.ToString();
    }

    private void lDTToR_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(tModifyAsNumber.Text))
            tModifyAsNumber.Text = newInfo.ModifyTime.ToString();
    }
}