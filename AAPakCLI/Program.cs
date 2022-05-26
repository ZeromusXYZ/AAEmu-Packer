using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AAPakCLI.Properties;
using AAPacker;

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
            if (pak.isOpen)
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
                    Console.WriteLine("[ERROR] Custom  game_pak.key  does not seem valid for {0}", filename);
                else
                    Console.WriteLine("[ERROR] Failed to open {0}", filename);
            }

            if (pak.PakType != PakFileType.TypeA) Console.WriteLine("[PAK] PakFileType = {0}", pak.PakType.ToString());

            if (pak.files.Count <= 0 && pak.extraFiles.Count <= 0)
            {
                Console.WriteLine("[PAK] contains no files");
            }
            else
            {
                if (pak.files.Count > 0)
                    Console.WriteLine("[PAK] contains {0} file(s)", pak.files.Count);
                if (pak.extraFiles.Count > 0)
                    Console.WriteLine("[PAK] contains {0} extra/deleted file(s)", pak.extraFiles.Count);
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
            foreach (var pfi in pak.files)
            {
                var modTime = DateTime.FromFileTimeUtc(pfi.modifyTime);
                if (modTime > newest)
                    newest = modTime;

                s = pfi.name;
                s += ";" + pfi.size;
                s += ";" + pfi.offset;
                s += ";" + BitConverter.ToString(pfi.md5).Replace("-", "").ToUpper();
                s += ";" + AAPak.DateTimeToDateTimeStr(
                    DateTime.FromFileTimeUtc(pfi
                        .createTime)); // DateTimeToDateTimeStr DateTime.FromFileTimeUtc(pfi.createTime).ToString("yyyy-MM-dd HH:mm:ss");
                s += ";" + AAPak.DateTimeToDateTimeStr(modTime); // .ToString("yyyy-MM-dd HH:mm:ss");
                s += ";" + pfi.sizeDuplicate;
                s += ";" + pfi.paddingSize;
                s += ";" + pfi.dummy1;
                s += ";" + pfi.dummy2;
                sl.Add(s);
            }

            File.WriteAllLines(filename, sl);
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
                    if (pak == null || !pak.isOpen) cmdErrors += "[ERROR] Failed to open for r/w: " + arg1 + "\r\n";
                }
                else if (arg == "+c")
                {
                    i++; // take one arg

                    if (pak != null)
                        pak.ClosePak();
                    // Create and a new pakfile
                    pak = new AAPak(arg1, false, true);
                    if (pak == null || !pak.isOpen)
                    {
                        cmdErrors += "[ERROR] Failed to created file: " + arg1 + "\r\n";
                        continue;
                    }

                    pak.ClosePak();
                    // Re-open it in read/write mode
                    LoadPakFile(ref pak, arg1, false, false, true);

                    if (pak == null || !pak.isOpen)
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
                    if (pak == null || !pak.isOpen || pak.readOnly)
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
                    if (pak == null || !pak.isOpen || pak.readOnly)
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
                    if (pak == null || !pak.isOpen || pak.readOnly)
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
                    if (pak == null || !pak.isOpen || pak.readOnly)
                    {
                        cmdErrors +=
                            "[ERROR] Pak file needs to be opened in read/write mode to be able to add a file !\r\n";
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
                    if (pak == null || !pak.isOpen || pak.readOnly)
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
                            for (var n = pak.files.Count - 1; n >= 0; n--)
                                //foreach(AAPakFileInfo pfi in pak.files)
                            {
                                var pfi = pak.files[n];
                                if (pfi.name.ToLower().StartsWith(delDir))
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
                    if (pak == null || !pak.isOpen)
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
                else if (arg == "-csv" || arg == "+csv")
                {
                    i++; // take one arg
                    if (pak == null || !pak.isOpen)
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
                    if (pak == null || !pak.isOpen)
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
                            Console.WriteLine("Current File: " + pak._gpFilePath);
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
                    if (pak == null || !pak.isOpen) cmdErrors += "[ERROR] Failed to open: " + arg + "\r\n";
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
                HandleCommandLine();
            }
            catch (Exception x)
            {
                Console.WriteLine("[EXCEPTION] {0}\r\n", x.Message);
            }

            try
            {
                if (pak != null && pak.isOpen)
                {
                    if (pak.isDirty)
                        Console.WriteLine("[PAK] Saving pak ... {0}", pak._gpFilePath);
                    else
                        Console.WriteLine("[PAK] Closing pak ... {0}", pak._gpFilePath);
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