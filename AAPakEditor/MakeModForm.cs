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
    public partial class MakeModForm : Form
    {
        public List<string> addFiles = new List<string>();
        public AAPak mainPak;
        static string ModFileFolderName = "aamod/";
        static string SFXInfoFileName = ModFileFolderName + "aamod.exe"; // if present, this needs to be the first file in the pak
        static string ModInfoFileName = ModFileFolderName + "aamod.txt";
        static string ModPNGImageFileName = ModFileFolderName + "aamod.png";
        static string ModJPGImageFileName = ModFileFolderName + "aamod.jpg";
        static string ModNewFilesFileName = ModFileFolderName + "newfiles.txt";
        public string NewCustomImage = string.Empty;
        public bool useOldPNGImage = false;
        public bool useOldJPGImage = false;
        public bool useDefaultImage = false;

        public MakeModForm()
        {
            InitializeComponent();
        }

        private void MakeModForm_Load(object sender, EventArgs e)
        {
            addFiles.Clear();
            foreach (var fi in mainPak.files)
            {
                if (!fi.name.StartsWith(ModFileFolderName))
                    addFiles.Add(fi.name);
            }

            // Load Description
            if (mainPak.FileExists(ModInfoFileName))
            {
                var descStream = mainPak.ExportFileAsStream(ModInfoFileName);
                var desc = AAPak.StreamToString(descStream);
                tDescription.Text = desc;
            }
            else
            {
                var s = "";
                if (addFiles.Count < 50)
                {
                    s = "Updating " + addFiles.Count.ToString() + " file(s):\r\n";
                    foreach (var a in addFiles)
                        s += a + "\r\n";
                }
                else
                {
                    s = "This mod changes " + addFiles.Count.ToString() + " files";
                }
                tDescription.Text += s;
            }

            // Load PNG
            if (mainPak.FileExists(ModPNGImageFileName))
            {
                try
                {
                    var imgStream = mainPak.ExportFileAsStream(ModPNGImageFileName);
                    var img = Image.FromStream(imgStream);
                    modIcon.Image = img;
                    modIcon.BorderStyle = BorderStyle.None;
                    useOldPNGImage = true;
                }
                catch { }
            }
            else
            // Load JPG
            if (mainPak.FileExists(ModJPGImageFileName))
            {
                try
                {
                    var imgStream = mainPak.ExportFileAsStream(ModJPGImageFileName);
                    var img = Image.FromStream(imgStream);
                    modIcon.Image = img;
                    modIcon.BorderStyle = BorderStyle.None;
                    useOldJPGImage = true;
                }
                catch { }
            }
        }

        private void BtnCreateMod_Click(object sender, EventArgs e)
        {
            if (cbCreateSFX.Checked)
                saveFileDlg.FilterIndex = 1;
            else
                saveFileDlg.FilterIndex = 2;
            if (saveFileDlg.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                AAPak modpak = new AAPak(saveFileDlg.FileName, false, true);

                // If you want to use the aamod as a SFX, the .exe needs to be the first file in the pak
                if (cbCreateSFX.Checked)
                {
                    // This AAModSFX resource is loaded from the RELEASE build of the AAMod project, make sure it's compiled as release first if you made changes to it
                    MemoryStream sfxStream = new MemoryStream(Properties.Resources.AAModSFX);
                    if (!modpak.AddFileFromStream(SFXInfoFileName, sfxStream, DateTime.Now, DateTime.Now, false, out _))
                    {
                        MessageBox.Show("Failed to add SFX executable");
                        modpak.ClosePak();
                        return;
                    }
                }

                var modInfoStream = AAPak.StringToStream(tDescription.Text);
                if (!modpak.AddFileFromStream(ModInfoFileName, modInfoStream, DateTime.Now, DateTime.Now, false, out _))
                {
                    MessageBox.Show("Failed to add description");
                    modpak.ClosePak();
                    return;
                }

                if (useDefaultImage)
                {
                    MemoryStream iconStream = new MemoryStream();
                    Properties.Resources.mod_example_icon.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png);
                    if (!modpak.AddFileFromStream(ModPNGImageFileName, iconStream, DateTime.Now, DateTime.Now, false, out _))
                    {
                        MessageBox.Show("Failed to add default icon");
                        modpak.ClosePak();
                        return;
                    }
                }
                else
                if (NewCustomImage != string.Empty)
                {
                    FileStream iconStream = File.Open(NewCustomImage, FileMode.Open, FileAccess.Read);

                    if ( (Path.GetExtension(NewCustomImage).ToLower() == ".png") && (!modpak.AddFileFromStream(ModPNGImageFileName, iconStream, DateTime.Now, DateTime.Now, false, out _)) )
                    {
                        MessageBox.Show("Failed to add PNG icon");
                        modpak.ClosePak();
                        return;
                    }
                    else
                    if ((Path.GetExtension(NewCustomImage).ToLower() == ".jpg") && (!modpak.AddFileFromStream(ModJPGImageFileName, iconStream, DateTime.Now, DateTime.Now, false, out _)))
                    {
                        MessageBox.Show("Failed to add JPG icon");
                        modpak.ClosePak();
                        return;
                    }
                }
                else
                if (useOldPNGImage)
                {
                    var oldpngStream = mainPak.ExportFileAsStream(ModPNGImageFileName);
                    if ((Path.GetExtension(NewCustomImage).ToLower() == ".png") && (!modpak.AddFileFromStream(ModPNGImageFileName, oldpngStream, DateTime.Now, DateTime.Now, false, out _)))
                    {
                        MessageBox.Show("Failed to copy PNG icon");
                        modpak.ClosePak();
                        return;
                    }
                }
                else
                if (useOldJPGImage)
                {
                    var oldjpgStream = mainPak.ExportFileAsStream(ModJPGImageFileName);
                    if ((Path.GetExtension(NewCustomImage).ToLower() == ".png") && (!modpak.AddFileFromStream(ModJPGImageFileName, oldjpgStream, DateTime.Now, DateTime.Now, false, out _)))
                    {
                        MessageBox.Show("Failed to copy PNG icon");
                        modpak.ClosePak();
                        return;
                    }
                }
                else
                {
                    // No image used
                }

                // Copy all files
                foreach(var fi in mainPak.files)
                {
                    if (fi.name.StartsWith(ModFileFolderName))
                        continue;
                    var ms = mainPak.ExportFileAsStream(fi);
                    if (!modpak.AddFileFromStream(fi.name,ms,DateTime.FromFileTime(fi.createTime),DateTime.FromFileTime(fi.modifyTime),false,out _))
                    {
                        MessageBox.Show("Failed to copy \n"+fi.name+"\nAborting !","Copy Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        modpak.ClosePak();
                        return;
                    }
                }
                modpak.ClosePak();

                MessageBox.Show("AAMod create completed !", "AAMod Create", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception x)
            {
                MessageBox.Show("Exception: " + x.Message);
            }
        }

        private void ModIcon_Click(object sender, EventArgs e)
        {
            if (openImageDlg.ShowDialog() == DialogResult.OK)
            {
                NewCustomImage = openImageDlg.FileName;
                modIcon.Load(NewCustomImage);
                useOldPNGImage = false;
                useOldJPGImage = false;
                useDefaultImage = false;
            }
        }

        private void BtnClearIcon_Click(object sender, EventArgs e)
        {
            NewCustomImage = "";
            useOldJPGImage = false;
            useOldPNGImage = false;
            useDefaultImage = false;
            modIcon.Image = null;
        }

        private void ButtonDefaultIcon_Click(object sender, EventArgs e)
        {
            NewCustomImage = "";
            useOldJPGImage = false;
            useOldPNGImage = false;
            useDefaultImage = true;
            var img = Properties.Resources.mod_example_icon;
            modIcon.Image = img;
        }
    }
}
