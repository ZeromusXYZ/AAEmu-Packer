using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AAPakCLI.Properties;
using AAPacker;
using System.Globalization;
using AAPakEditor.Helpers;
using Newtonsoft.Json;

namespace AAPakCLI
{
    internal class Program
    {
        private static AAPak pak;
        private static readonly bool useCustomKey = false;

        private static void LoadPakFile(ref AAPak pak, string filename, bool openAsReadOnly, 
            bool showWriteWarning = true, bool quickLoad = false)
        {
            if (pak == null) pak = new AAPak("");
            if (pak.IsOpen)
            {
                Console.WriteLine("[PAK] Closing pak ... ");
                pak.ClosePak();
            }

            Console.WriteLine("[PAK] Opening Pak ... {0}", filename);
            /* Currently disabled custom key support
            try
            {
                LoadPakKeys(Path.GetDirectoryName(filename).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
            }
            catch
            {
                MessageBox.Show("Error loading custom keys");
            }
            */

            var res = pak.OpenPak(filename, openAsReadOnly);
            if (!res)
            {
                if (useCustomKey)
                    Console.WriteLine("[ERROR] Reader  game_pak.key  does not seem valid for {0}", filename);
                else
                    Console.WriteLine("[ERROR] Failed to open {0}", filename);
            }

            if (pak.PakType != PakFileType.Classic) Console.WriteLine("[PAK] PakFileType = {0}", pak.PakType.ToString());

            if (pak.Files.Count <= 0 && pak.ExtraFiles.Count <= 0)
            {
                Console.WriteLine("[PAK] contains no files");
            }
            else
            {
                if (pak.Files.Count > 0)
                    Console.WriteLine("[PAK] contains {0} file(s)", pak.Files.Count);
                if (pak.ExtraFiles.Count > 0)
                    Console.WriteLine("[PAK] contains {0} extra/deleted file(s)", pak.ExtraFiles.Count);
            }
        }

        private static int AddDirectory(ref AAPak pak, string sourceDir, string targetDir)
        {
            var filesAdded = 0;
            // Make sure slashes are correct
            sourceDir = sourceDir.Replace('/', Path.DirectorySeparatorChar);
            if (sourceDir.Last() != Path.DirectorySeparatorChar)
                sourceDir += Path.DirectorySeparatorChar;
            if (targetDir != string.Empty)
            {
                targetDir = targetDir.Replace(Path.DirectorySeparatorChar, '/');
                if (targetDir.Last() != '/')
                    targetDir += '/';
            }

            var dirs = Directory.GetDirectories(sourceDir);
            foreach (var dir in dirs)
            {
                var dn = Path.GetFileName(dir);
                filesAdded += AddDirectory(ref pak, sourceDir + dn, targetDir + dn);
            }

            var files = Directory.GetFiles(sourceDir);
            foreach (var file in files)
            {
                var fn = Path.GetFileName(file);
                if (pak.AddFileFromFile(sourceDir + fn, targetDir + fn, false))
                    filesAdded++;
            }

            return filesAdded;
        }

        private static (int, long, int) ExtractFolder(ref AAPak pak, string sourceDir, string targetDir)
        {
            var filesDone = 0;
            long totalExportedSize = 0;
            var errorCount = 0;

            foreach (var pfi in pak.Files)
            {
                if (sourceDir != "")
                    if (pfi.Name.Length <= sourceDir.Length || pfi.Name.Substring(0, sourceDir.Length) != sourceDir)
                        continue;

                var destName = targetDir.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                var exportedFileName = pfi.Name.Substring(sourceDir.Length);
                destName += exportedFileName.Replace('/', Path.DirectorySeparatorChar);

                // Check if target directory exists
                var destFolder = string.Empty;
                try
                {
                    destFolder = Path.GetDirectoryName(destName);
                }
                catch
                {
                    // Fallback for stuff with invalid chars
                    destFolder = targetDir + Path.DirectorySeparatorChar;
                }

                if (!Directory.Exists(destFolder))
                    Directory.CreateDirectory(destFolder);

                // Export the file
                if (ExportFile(pfi, destName))
                {
                    filesDone++;
                    totalExportedSize += pfi.Size;
                }
                else
                {
                    errorCount++;
                }
            }

            return (filesDone, totalExportedSize, errorCount);
        }

        public static bool ExportFile(AAPakFileInfo pfi, string destName)
        {
            try
            {
                var destFolder = Path.GetDirectoryName(destName);
                if (!Directory.Exists(destFolder))
                    Directory.CreateDirectory(destFolder);

                // Save file stream
                var filePakStream = pak.ExportFileAsStream(pfi);
                var fs = new FileStream(destName, FileMode.Create);
                filePakStream.Position = 0;

                filePakStream.CopyTo(fs);

                filePakStream.Dispose();
                fs.Dispose();

                // Update file details
                File.SetCreationTime(destName, DateTime.FromFileTimeUtc(pfi.CreateTime));
                File.SetLastWriteTime(destName, DateTime.FromFileTimeUtc(pfi.ModifyTime));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[EXCEPTION] {ex.Message}");
                return false;
            }

            return true;
        }

        private static void CreateCSVFile(ref AAPak pak, string filename = "")
        {
            if (filename == string.Empty) return;

            var newest = new DateTime(1600, 1, 1);

            var sl = new List<string>();
            var s = "";
            s = "name";
            s += ";size";
            s += ";offset";
            s += ";md5";
            s += ";createTime";
            s += ";modifyTime";
            s += ";sizeDuplicate";
            s += ";paddingSize";
            s += ";dummy1";
            s += ";dummy2";
            sl.Add(s);
            foreach (var pfi in pak.Files)
            {
                var modTime = DateTime.FromFileTimeUtc(pfi.ModifyTime);
                if (modTime > newest)
                    newest = modTime;

                s = pfi.Name;
                s += ";" + pfi.Size;
                s += ";" + pfi.Offset;
                s += ";" + BitConverter.ToString(pfi.Md5).Replace("-", "").ToUpper();
                s += ";" + AAPak.DateTimeToDateTimeStr(
                    DateTime.FromFileTimeUtc(pfi
                        .CreateTime)); // DateTimeToDateTimeStr DateTime.FromFileTimeUtc(pfi.createTime).ToString("yyyy-MM-dd HH:mm:ss");
                s += ";" + AAPak.DateTimeToDateTimeStr(modTime); // .ToString("yyyy-MM-dd HH:mm:ss");
                s += ";" + pfi.SizeDuplicate;
                s += ";" + pfi.PaddingSize;
                s += ";" + pfi.Dummy1;
                s += ";" + pfi.Dummy2;
                sl.Add(s);
            }

            File.WriteAllLines(filename, sl);
        }

        private static void LoadCustomReaders(string userFolder)
        {
            AAPak.ReaderPool.Clear();

            var jsonSettings = new JsonSerializerSettings
            {
                Culture = CultureInfo.InvariantCulture,
                Formatting = Formatting.Indented,
            };
            jsonSettings.Converters.Add(new ByteArrayHexConverter());

            var readerSettingsFileName = Path.Combine(userFolder, "readers.json");
            try
            {
                if (File.Exists(readerSettingsFileName))
                {
                    var data = JsonConvert.DeserializeObject<List<AAPakFileFormatReader>>(File.ReadAllText(readerSettingsFileName), jsonSettings);
                    if (data?.Count > 0)
                        foreach (var r in data)
                            AAPak.ReaderPool.Add(r);
                }
            }
            catch
            {
                // Ignore
            }

            // Add only default in case of errors
            if (AAPak.ReaderPool.Count <= 0)
            {
                AAPak.ReaderPool.Add(new AAPakFileFormatReader(true));
                // Write default file to user's settings
                try
                {
                    Directory.CreateDirectory(userFolder);
                    File.WriteAllText(readerSettingsFileName, JsonConvert.SerializeObject(AAPak.ReaderPool, jsonSettings));
                }
                catch
                {
                    // Ignore
                }
            }
        }

        private static bool HandleCommandLine()
        {
            var closeWhenDone = false;

            var cmdErrors = string.Empty;
            var args = Environment.GetCommandLineArgs();
            if (args.Length <= 1)
            {
                Console.WriteLine("[INFO] AAPakCLI the Command-Line pak editor");
                Console.WriteLine("[INFO] Run this tool with -? command-line to get a list of available arguments");
                return true;
            }

            for (var i = 1; i < args.Length; i++)
            {
                var arg = args[i];
                var arg1 = "";
                var arg2 = "";
                if (i + 1 < args.Length)
                    arg1 = args[i + 1];
                if (i + 2 < args.Length)
                    arg2 = args[i + 2];

                if (arg == "-o" || arg == "+o")
                {
                    i++; // take one arg
                    if (pak != null)
                        pak.ClosePak();
                    LoadPakFile(ref pak, arg1, false, false, true);
                    if (pak == null || !pak.IsOpen) cmdErrors += "[ERROR] Failed to open for r/w: " + arg1 + "\r\n";
                }
                else if (arg == "+c")
                {
                    i++; // take one arg

                    if (pak != null)
                        pak.ClosePak();
                    // Create and a new pakfile
                    pak = new AAPak(arg1, false, true);
                    if (pak == null || !pak.IsOpen)
                    {
                        cmdErrors += "[ERROR] Failed to created file: " + arg1 + "\r\n";
                        continue;
                    }

                    pak.ClosePak();
                    // Re-open it in read/write mode
                    LoadPakFile(ref pak, arg1, false, false, true);

                    if (pak == null || !pak.IsOpen)
                        cmdErrors += "[ERROR] Failed to re-open created file: " + arg1 + "\r\n";
                    else
                        Console.WriteLine("[PAK] Created pak file {0}", arg1);
                }
                else
                    /* Currently disable SFX support for command-line
                    if (arg == "+sfx")
                    {
                        i++;
                        if ((pak == null) || (!pak.isOpen) || (pak.readOnly))
                        {
                            cmdErrors += "Pak file needs to be opened in read/write mode to be able to add a mod installer !\r\n";
                        }
                        else
                        {
                            // add MODSFX
                            MemoryStream sfxStream = new MemoryStream(Properties.Resources.AAModSFX);
                            // We will be possibly be editing the icon, so it's a good idea to have some spare space here
                            if (!pak.AddFileFromStream(MakeModForm.SFXInfoFileName, sfxStream, DateTime.Now, DateTime.Now, true, out _))
                            {
                                cmdErrors += "Failed to add SFX executable\r\n";
                            }
    
                            if (File.Exists(arg1))
                            {
                                if (!pak.AddFileFromFile(arg1, MakeModForm.ModInfoFileName, false))
                                {
                                    cmdErrors += "Failed to add SFX description file: \r\n" + arg1;
                                }
                            }
                            else
                            {
                                // Consider the provided arg as a name
                                MemoryStream modDescStream = new MemoryStream();
                                var descBytes = Encoding.UTF8.GetBytes(arg1);
                                modDescStream.Write(descBytes, 0, descBytes.Length);
                                modDescStream.Position = 0;
    
                                if (!pak.AddFileFromStream(MakeModForm.ModInfoFileName, modDescStream, DateTime.Now, DateTime.Now, false, out _))
                                {
                                    cmdErrors += "Failed to add SFX description text: \r\n" + arg1;
                                }
    
                            }
                        }
                    }
                    else
                    */

                if (arg == "+f")
                {
                    i += 2; // take two args
                    if (pak == null || !pak.IsOpen || pak.ReadOnly)
                    {
                        cmdErrors +=
                            "[ERROR] Pak file needs to be opened in read/write mode to be able to add a file !\r\n";
                    }
                    else
                    {
                        if (!pak.AddFileFromFile(arg1, arg2, false))
                            cmdErrors += "[ERROR] Failed to add file: " + arg1 + " => " + arg2 + "\r\n";
                        else
                            Console.WriteLine("[PAK] Added file {0} => {1}", arg1, arg2);
                    }
                }
                else if (arg == "-f")
                {
                    i++; // take one arg
                    if (pak == null || !pak.IsOpen || pak.ReadOnly)
                    {
                        cmdErrors +=
                            "[ERROR] Pak file needs to be opened in read/write mode to be able to delete a file !\r\n";
                    }
                    else
                    {
                        if (!pak.DeleteFile(arg1))
                            // Technically, this could never fail as it only can return false if it's in read-only
                            cmdErrors += "[ERROR] Failed to delete file:\r\n" + arg1;
                        else
                            Console.WriteLine("[PAK] Deleted file {0}", arg1);
                    }
                }
                else if (arg == "-s" || arg == "+s")
                {
                    if (pak == null || !pak.IsOpen || pak.ReadOnly)
                    {
                        cmdErrors +=
                            "[ERROR] Pak file needs to be opened in read/write mode to be able save it !\r\n";
                    }
                    else
                    {
                        Console.WriteLine("[PAK] Saving pak file ...");
                        pak.SaveHeader();
                    }
                }
                else if (arg == "+d")
                {
                    i += 2; // take two args
                    if (pak == null || !pak.IsOpen || pak.ReadOnly)
                    {
                        cmdErrors += "[ERROR] Pak file needs to be opened in read/write mode to be able to add a file !\r\n";
                    }
                    else
                    {
                        Console.WriteLine("[PAK] Adding directory {0} => {1}", arg1, arg2);
                        try
                        {
                            var filesAdded = AddDirectory(ref pak, arg1, arg2);
                            Console.WriteLine("[PAK] Added {0} file(s)", filesAdded);
                        }
                        catch (Exception x)
                        {
                            cmdErrors += "[EXCEPTION] " + x.Message + " \r\nPossible file corruption !";
                        }
                    }
                }
                else if (arg == "-d")
                {
                    i += 1; // takes one arg
                    if (pak == null || !pak.IsOpen || pak.ReadOnly)
                    {
                        cmdErrors +=
                            "[ERROR] Pak file needs to be opened in read/write mode to be able to add a file !\r\n";
                    }
                    else
                    {
                        Console.WriteLine("[PAK] Deleting directory {0}", arg1);
                        try
                        {
                            var filesDeleted = 0;
                            var delDir = arg1.ToLower();
                            if (delDir.Last() != '/')
                                delDir += '/';
                            for (var n = pak.Files.Count - 1; n >= 0; n--)
                                //foreach(AAPakFileInfo pfi in pak.files)
                            {
                                var pfi = pak.Files[n];
                                if (pfi.Name.ToLower().StartsWith(delDir))
                                    if (pak.DeleteFile(pfi))
                                        filesDeleted++;
                            }

                            Console.WriteLine("[PAK] Deleted {0} file(s)", filesDeleted);
                        }
                        catch (Exception x)
                        {
                            cmdErrors += "[EXCEPTION] " + x.Message + " \r\nPossible file corruption !";
                        }
                    }
                }
                else if (arg == "-x" || arg == "+x")
                {
                    if (pak == null || !pak.IsOpen)
                        cmdErrors += "[ERROR] Pak file needs to be opened before you can close it !\r\n";
                    else
                        pak.ClosePak();
                    if (arg == "+x")
                        closeWhenDone = true;
                }
                else if (arg == "-m" || arg == "+m")
                {
                    i++;
                    Console.WriteLine("[INFO] {0} ", arg1);
                    Console.Write("Press ENTER to continue ...");
                    _ = Console.ReadLine();
                }
                else if (arg == "-u" || arg == "+u")
                {
                    i += 2;
                    if (pak == null || !pak.IsOpen)
                        cmdErrors += "Pak file needs to be opened to be able to add extract a folder!\r\n";
                    else
                    {
                        try
                        {
                            var (fileExtracted, sizeExtracted, errors) = ExtractFolder(ref pak, arg1, arg2);
                            if (errors > 0)
                                cmdErrors += $"[ERROR] Possible errors while unpacking directory {arg1} => {arg2}";
                            if (fileExtracted > 0)
                            {
                                Console.WriteLine($"[PAK] Extracted {fileExtracted} file(s) from {arg1} => {arg2} for {sizeExtracted} bytes");
                            }
                            else
                            {
                                Console.WriteLine($"[PAK] No files extract from {arg1}");
                            }

                        }
                        catch (Exception x)
                        {
                            cmdErrors += $"[EXCEPTION] {x.Message} \r\nPossible file corruption !";
                        }
                    }
                }
                else if (arg == "-l" || arg == "+l")
                {
                    i += 2;
                    if (pak == null || !pak.IsOpen)
                        cmdErrors += "[ERROR] Pak file needs to be opened to be able to add extract a file!\r\n";
                    else
                    {
                        try
                        {
                            if (pak.GetFileByName(arg1, out var pfi))
                            {
                                if (ExportFile(pfi, arg2))
                                {
                                    Console.WriteLine($"[PAK] Extracted file from {arg1} => {arg2} for {pfi.Size} bytes");
                                }
                                else
                                {
                                    cmdErrors += $"[ERROR] Error while unpacking file {arg1} => {arg2}";
                                }
                            }
                            else
                            {
                                cmdErrors += $"[ERROR] File does not exist {arg1}";
                            }
                        }
                        catch (Exception x)
                        {
                            cmdErrors += $"[EXCEPTION] {x.Message} \r\nPossible file corruption !";
                        }
                    }
                }
                else if (arg == "-csv" || arg == "+csv")
                {
                    i++; // take one arg
                    if (pak == null || !pak.IsOpen)
                    {
                        cmdErrors += "[ERROR] Pak file needs to be opened to be able generate a CSV file !\r\n";
                    }
                    else
                    {
                        if (arg1 == string.Empty)
                            Console.WriteLine("[ERROR] you need to provide a filename to export to");
                        else
                            CreateCSVFile(ref pak, arg1);
                    }
                }
                else if (arg == "-patchbycompare" || arg == "+patchbycompare" || arg == "-pbc" || arg == "+pbc")
                {
                    i += 2; // take two args
                    if (pak == null || !pak.IsOpen)
                    {
                        cmdErrors +=
                            "[ERROR] A Pak file needs to be opened before you can create a patch by compare !\r\n";
                    }
                    else
                    {
                        if (arg1 == string.Empty)
                        {
                            Console.WriteLine(
                                "[ERROR] you need to provide a older pak or csv filename to compare with.");
                        }
                        else if (arg2 == string.Empty)
                        {
                            Console.WriteLine(
                                "[ERROR] you need to provide a patch filename to write the updated files to.");
                        }
                        else
                        {
                            Console.WriteLine("Current File: " + pak.GpFilePath);
                            Console.WriteLine("Compare to old file: " + arg1);
                            Console.WriteLine("Write changes to: " + arg2);
                            Console.WriteLine("[NYI] This function is not yet implemented");
                        }
                    }
                }
                else if (arg == "-h" || arg == "--h" || arg == "--help" || arg == "-help" || arg == "-?" ||
                         arg == "--?" || arg == "/?" || arg == "/help")
                {
                    Console.WriteLine("[HELP] AAPakCLI the Command-Line pak editor");
                    Console.WriteLine(Resources.cmdhelp);
                    closeWhenDone = true;
                }
                else if (File.Exists(arg))
                {
                    // Open file in read-only mode if nothing is specified and it's a valid filename
                    if (pak != null)
                        pak.ClosePak();
                    LoadPakFile(ref pak, arg, true, true, true);
                    if (pak == null || !pak.IsOpen) cmdErrors += "[ERROR] Failed to open: " + arg + "\r\n";
                }
                else
                {
                    cmdErrors += "[ERROR] Unknown command or filename: " + arg + "\r\n";
                }
            }

            if (cmdErrors != string.Empty) Console.WriteLine(cmdErrors);

            return closeWhenDone;
        }

        private static void Main(string[] args)
        {
            try
            {
                LoadCustomReaders(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ZeromusXYZ", "AAPakEditor"));

                HandleCommandLine();
            }
            catch (Exception x)
            {
                Console.WriteLine("[EXCEPTION] {0}\r\n", x.Message);
            }

            try
            {
                if (pak != null && pak.IsOpen)
                {
                    if (pak.IsDirty)
                        Console.WriteLine("[PAK] Saving pak ... {0}", pak.GpFilePath);
                    else
                        Console.WriteLine("[PAK] Closing pak ... {0}", pak.GpFilePath);
                    pak.ClosePak();
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(
                    "[EXCEPTION] Error closing or saving pak file, possibly data corruption !\r\n{0}",
                    x.Message);
            }
        }
    }
}