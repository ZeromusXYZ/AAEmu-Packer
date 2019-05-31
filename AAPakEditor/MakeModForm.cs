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
using Vestris.ResourceLib;

namespace AAPakEditor
{
    public partial class MakeModForm : Form
    {
        public List<string> addFiles = new List<string>();
        public AAPak mainPak;
        public static string ModFileFolderName = "aamod/";
        public static string SFXInfoFileName = ModFileFolderName + "aamod.exe"; // if present, this needs to be the first file in the pak
        public static string ModInfoFileName = ModFileFolderName + "aamod.txt";
        public static string ModPNGImageFileName = ModFileFolderName + "aamod.png";
        public static string ModJPGImageFileName = ModFileFolderName + "aamod.jpg";
        public static string ModNewFilesFileName = ModFileFolderName + "newfiles.txt";
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

        public Icon ImageToIcon(Image imgTest)
        {
            Bitmap bitmap = new Bitmap(imgTest);
            Icon icoTest;
            IntPtr iPtr = bitmap.GetHicon();
            icoTest = (Icon)Icon.FromHandle(iPtr).Clone();
            return icoTest;
        }

        // From: https://stackoverflow.com/a/11448060/368354
        public MemoryStream SaveAsIconMemoryStream(Bitmap SourceBitmap)
        {
            MemoryStream FS = new MemoryStream();
            //FileStream FS = new FileStream(FilePath, FileMode.Create);
            // ICO header
            FS.WriteByte(0); FS.WriteByte(0);
            FS.WriteByte(1); FS.WriteByte(0);
            FS.WriteByte(1); FS.WriteByte(0);

            // Image size
            // Set to 0 for 256 px width/height
            FS.WriteByte(0);
            FS.WriteByte(0);
            // Palette
            FS.WriteByte(0);
            // Reserved
            FS.WriteByte(0);
            // Number of color planes
            FS.WriteByte(1); FS.WriteByte(0);
            // Bits per pixel
            FS.WriteByte(32); FS.WriteByte(0);

            // Data size, will be written after the data
            FS.WriteByte(0);
            FS.WriteByte(0);
            FS.WriteByte(0);
            FS.WriteByte(0);

            // Offset to image data, fixed at 22
            FS.WriteByte(22);
            FS.WriteByte(0);
            FS.WriteByte(0);
            FS.WriteByte(0);

            // Writing actual data
            SourceBitmap.Save(FS, System.Drawing.Imaging.ImageFormat.Png);

            // Getting data length (file length minus header)
            long Len = FS.Length - 22;

            // Write it in the correct place
            FS.Seek(14, SeekOrigin.Begin);
            FS.WriteByte((byte)Len);
            FS.WriteByte((byte)(Len >> 8));

            //FS.Close();
            return FS;
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
                Bitmap customIconImage = null;

                // First check image we will use (if any)
                if (useDefaultImage)
                {
                    MemoryStream iconStream = new MemoryStream();
                    Properties.Resources.mod_example_icon.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png);
                    /*
                    if (!modpak.AddFileFromStream(ModPNGImageFileName, iconStream, DateTime.Now, DateTime.Now, false, out _))
                    {
                        MessageBox.Show("Failed to add default icon");
                        modpak.ClosePak();
                        return;
                    }
                    */
                    customIconImage = new Bitmap(Properties.Resources.mod_example_icon.GetThumbnailImage(128, 128, null, new IntPtr()));
                }
                else
                if (NewCustomImage != string.Empty)
                {
                    FileStream iconStream = File.Open(NewCustomImage, FileMode.Open, FileAccess.Read);
                    /*
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
                    */
                    Image img = Image.FromStream(iconStream);
                    customIconImage = new Bitmap(img.GetThumbnailImage(128, 128, null, new IntPtr()));
                }
                else
                if (useOldPNGImage)
                {
                    var oldpngStream = mainPak.ExportFileAsStream(ModPNGImageFileName);
                    /*
                    if ((Path.GetExtension(NewCustomImage).ToLower() == ".png") && (!modpak.AddFileFromStream(ModPNGImageFileName, oldpngStream, DateTime.Now, DateTime.Now, false, out _)))
                    {
                        MessageBox.Show("Failed to copy PNG icon");
                        modpak.ClosePak();
                        return;
                    }
                    */
                    Image img = Image.FromStream(oldpngStream);
                    customIconImage = new Bitmap(img.GetThumbnailImage(128, 128, null, new IntPtr()));
                }
                else
                if (useOldJPGImage)
                {
                    var oldjpgStream = mainPak.ExportFileAsStream(ModJPGImageFileName);
                    /*
                    if ((Path.GetExtension(NewCustomImage).ToLower() == ".png") && (!modpak.AddFileFromStream(ModJPGImageFileName, oldjpgStream, DateTime.Now, DateTime.Now, false, out _)))
                    {
                        MessageBox.Show("Failed to copy PNG icon");
                        modpak.ClosePak();
                        return;
                    }
                    */
                    Image img = Image.FromStream(oldjpgStream);
                    customIconImage = new Bitmap(img.GetThumbnailImage(128, 128, null, new IntPtr()));
                }
                else
                {
                    // No image used
                }

                if ((cbCreateSFX.Checked) && (customIconImage != null))
                {
                    // Create some temp files
                    string tempIconFile = Path.GetTempFileName();// .GetTempPath() + "aamod_tmp.ico";
                    string tempEXEFile = Path.GetTempFileName();// .GetTempPath() + "aamod_tmp.exe";
                    try
                    {
                        // Very hacky way of adding/editing a icon to the sfx exe to add

                        // Create 64x64 "thumbnail" as icon
                        var pu = GraphicsUnit.Pixel;
                        var cImg = customIconImage.Clone(customIconImage.GetBounds(ref pu),System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        cImg.SetResolution(72, 72);
                        var tempImg = cImg.GetThumbnailImage(64, 64, null, new IntPtr());
                        // tempImg.Save(Path.GetTempPath() + "tmp_aamod.bmp");

                        // Create and Save temporary ico file
                        // Icon ic = ImageToIcon(tempImg);
                        var tempBitmap = new Bitmap(tempImg,new Size(64,64));

                        MemoryStream iconStream = SaveAsIconMemoryStream(tempBitmap);
                        // MemoryStream iconStream = new MemoryStream();
                        // ic.Save(iconStream);
                        FileStream iconStreamFile = File.Create(tempIconFile);
                        iconStream.Position = 0;
                        iconStream.CopyTo(iconStreamFile);
                        iconStreamFile.Close();

                        // Save temporary exe file
                        MemoryStream exeStream = new MemoryStream(Properties.Resources.AAModSFX);
                        FileStream fs = File.Create(tempEXEFile);
                        exeStream.CopyTo(fs);
                        fs.Close();
                        exeStream.Close();


                        // Create IconFile from temporary ico file
                        IconFile icf = new IconFile(tempIconFile);

                        // Delete old Icon resources
                        ResourceInfo vi = new ResourceInfo();
                        vi.Load(tempEXEFile);
                        foreach (ResourceId id in vi.ResourceTypes)
                        {
                            if (id.ResourceType != Kernel32.ResourceTypes.RT_GROUP_ICON)
                                continue;
                            foreach (Resource resource in vi.Resources[id])
                            {
                                resource.DeleteFrom(tempEXEFile);
                            }
                        }


                        // Add to IconDirectory
                        IconDirectoryResource iconDirectoryResource = new IconDirectoryResource(icf);
                        IconResource icr = new IconResource();
                        // Save to temporary exe
                        iconDirectoryResource.SaveTo(tempEXEFile);

                        // Read temporary exe back into stream
                        fs = File.OpenRead(tempEXEFile);
                        exeStream = new MemoryStream();
                        fs.CopyTo(exeStream);

                        // Add modified exe to modpak
                        if (!modpak.AddFileFromStream(SFXInfoFileName, exeStream, DateTime.Now, DateTime.Now, true, out _))
                        {
                            MessageBox.Show("Failed to add modified SFX executable");
                            modpak.ClosePak();
                            return;
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Exception editing icon:\n" + x.Message);
                        modpak.ClosePak();
                        return;
                    }
                    try
                    {
                        File.Delete(tempEXEFile);
                    }
                    catch { }
                    try
                    {
                        File.Delete(tempIconFile);
                    }
                    catch { }
                }
                else
                // If you want to use the aamod as a SFX, the .exe needs to be the first file in the pak
                if (cbCreateSFX.Checked)
                {
                    // This AAModSFX resource is loaded from the RELEASE build of the AAMod project, make sure it's compiled as release first if you made changes to it
                    MemoryStream sfxStream = new MemoryStream(Properties.Resources.AAModSFX);
                    // We will be possibly be editing the icon, so it's a good idea to have some spare space here
                    if (!modpak.AddFileFromStream(SFXInfoFileName, sfxStream, DateTime.Now, DateTime.Now, true, out _))
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

                if (customIconImage != null)
                {
                    MemoryStream imgStream = new MemoryStream();
                    customIconImage.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png);
                    if (!modpak.AddFileFromStream(ModPNGImageFileName, imgStream, DateTime.Now, DateTime.Now, false, out _))
                    {
                        MessageBox.Show("Failed to add icon image");
                        modpak.ClosePak();
                        return;
                    }

                }


                // Copy all files
                foreach (var fi in mainPak.files)
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
