using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAPakEditor;

namespace AAPakCLI
{
    class Program
    {
        static AAPak pak;
        static bool useCustomKey = false;

        static private void LoadPakFile(string filename, bool openAsReadOnly, bool showWriteWarning = true, bool quickLoad = false)
        {
            if (pak == null)
            {
                pak = new AAPak("", true);
            }
            if (pak.isOpen)
            {
                Console.WriteLine("[PAK] Closing pak ... ");
                pak.ClosePak();
            }

            Console.WriteLine("[PAK] Opening Pak ... {0}",filename);
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

            if (pak.PakType != PakFileType.TypeA)
            {
                Console.WriteLine("[PAK] PakFileType = {0}", pak.PakType.ToString());
            }

            if ((pak.files.Count <= 0) && (pak.extraFiles.Count <= 0))
                Console.WriteLine("[PAK] contains no files");
            else
            {
                if (pak.files.Count > 0)
                    Console.WriteLine("[PAK] contains {0} file(s)", pak.files.Count);
                if (pak.extraFiles.Count > 0)
                    Console.WriteLine("[PAK] contains {0} extra/deleted file(s)", pak.extraFiles.Count);
            }
        }

        static private int AddDirectory(string sourceDir, string targetDir)
        {
            int filesAdded = 0;
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
                filesAdded += AddDirectory(sourceDir + dir, targetDir + dir);

            var files = Directory.GetFiles(sourceDir);
            foreach(var file in files)
            {
                if (pak.AddFileFromFile(sourceDir + file, targetDir + file, false))
                    filesAdded++;
            }

            return filesAdded;
        }

        static private bool HandleCommandLine()
        {
            bool closeWhenDone = false;

            string cmdErrors = string.Empty;
            var args = Environment.GetCommandLineArgs();
            if (args.Length <= 1)
            {
                Console.WriteLine("[INFO] AAPakCLI the Command-Line pak editor");
                Console.WriteLine("[INFO] Run this tool with -? command-line to get a list of available arguments");
                return true;
            }
            for (int i = 1; i < args.Length; i++)
            {
                var arg = args[i];
                var arg1 = "";
                var arg2 = "";
                if (i + 1 < args.Length)
                    arg1 = args[i + 1];
                if (i + 2 < args.Length)
                    arg2 = args[i + 2];

                if ((arg == "-o") || (arg == "+o"))
                {
                    i++; // take one arg
                    if (pak != null)
                        pak.ClosePak();
                    LoadPakFile(arg1, false, false, true);
                    if ((pak == null) || (!pak.isOpen))
                    {
                        cmdErrors += "[ERROR] Failed to open for r/w: " + arg1 + "\r\n";
                    }
                }
                else

                if (arg == "+c")
                {
                    i++; // take one arg

                    if (pak != null)
                        pak.ClosePak();
                    // Create and a new pakfile
                    pak = new AAPak(arg1, false, true);
                    if ((pak == null) || (!pak.isOpen))
                    {
                        cmdErrors += "[ERROR] Failed to created file: " + arg1 + "\r\n";
                        continue;
                    }
                    pak.ClosePak();
                    // Re-open it in read/write mode
                    LoadPakFile(arg1, false, false, true);

                    if ((pak == null) || (!pak.isOpen))
                    {
                        cmdErrors += "[ERROR] Failed to re-open created file: " + arg1 + "\r\n";
                    }
                }
                else

                /* Currently disable SFX support for cdommand-line
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
                    if ((pak == null) || (!pak.isOpen) || (pak.readOnly))
                    {
                        cmdErrors += "[ERROR] Pak file needs to be opened in read/write mode to be able to add a file !\r\n";
                    }
                    else
                    {
                        if (!pak.AddFileFromFile(arg1, arg2, false))
                        {
                            cmdErrors += "[ERROR] Failed to add file:\r\n" + arg1 + "\r\n=>" + arg2 + "\r\n";
                        }
                    }
                }
                else

                if (arg == "-f")
                {
                    i++; // take one arg
                    if ((pak == null) || (!pak.isOpen) || (pak.readOnly))
                    {
                        cmdErrors += "[ERROR] Pak file needs to be opened in read/write mode to be able to delete a file !\r\n";
                    }
                    else
                    {
                        if (!pak.DeleteFile(arg1))
                        {
                            // Technically, this could never fail as it only can return false if it's in read-only
                            cmdErrors += "[ERROR] Failed to delete file:\r\n" + arg1;
                        }
                    }
                }
                else

                if ((arg == "-s") || (arg == "+s"))
                {
                    if ((pak == null) || (!pak.isOpen) || (pak.readOnly))
                    {
                        cmdErrors += "[ERROR] Pak file needs to be opened in read/write mode to be able save it !\r\n";
                    }
                    else
                    {
                        Console.WriteLine("[PAK] Saving pak file ...");
                        pak.SaveHeader();
                    }
                }
                else

                if (arg == "+d")
                {
                    i += 2; // take two args
                    if ((pak == null) || (!pak.isOpen) || (pak.readOnly))
                    {
                        cmdErrors += "[ERROR] Pak file needs to be opened in read/write mode to be able to add a file !\r\n";
                    }
                    else
                    {
                        Console.WriteLine("[PAK] Adding directory {0} => {1}",arg1,arg2);
                        try
                        {
                            var filesAdded = AddDirectory(arg1, arg2);
                            Console.WriteLine("[PAK] Added {0} file(s)", filesAdded);
                        }
                        catch (Exception x)
                        {
                            cmdErrors += "[EXCEPTION] " + x.Message + " \r\nPossible file corruption !";
                        }
                    }
                }
                else

                if ((arg == "-x") || (arg == "+x"))
                {
                    if ((pak == null) || (!pak.isOpen))
                    {
                        cmdErrors += "[ERROR] Pak file needs to be opened before you can close it !\r\n";
                    }
                    else
                    {
                        pak.ClosePak();
                    }
                    if (arg == "+x")
                        closeWhenDone = true;
                }
                else

                if ((arg == "-m") || (arg == "+m"))
                {
                    i++;
                    Console.WriteLine("[INFO] {0} ",arg1);
                    Console.Write("Press ENTER to continue ...");
                    _ = Console.ReadLine();
                }
                else

                if (arg == "-csv")
                {
                    i++; // take one arg
                    if ((pak == null) || (!pak.isOpen))
                    {
                        cmdErrors += "[ERROR] Pak file needs to be opened to be able generate a CSV file !\r\n";
                    }
                    else
                    {
                        Console.WriteLine("[NYI] Export CSV currently not implemented");
                        //CreateCSVFile(arg1);
                    }
                }
                else

                if ((arg == "-h") || (arg == "--h") || (arg == "--help") || (arg == "-help") || (arg == "-?") || (arg == "--?") || (arg == "/?") || (arg == "/help"))
                {

                    Console.WriteLine("[HELP] AAPakCLI the Command-Line pak editor");
                    Console.WriteLine(Properties.Resources.cmdhelp);
                    closeWhenDone = true;
                }
                else

                if (File.Exists(arg))
                {
                    // Open file in read-only mode if nothing is specified and it's a valid filename
                    if (pak != null)
                        pak.ClosePak();
                    LoadPakFile(arg, true, true, true);
                    if ((pak == null) || (!pak.isOpen))
                    {
                        cmdErrors += "[ERROR] Failed to open: " + arg + "\r\n";
                    }
                }
                else
                {
                    cmdErrors += "[ERROR] Unknown command or filename: " + arg + "\r\n";
                }
            }

            if (cmdErrors != string.Empty)
            {
                Console.WriteLine(cmdErrors);
            }
            
            return closeWhenDone;
        }

        static void Main(string[] args)
        {
            HandleCommandLine();
            try
            {
                if ((pak != null) && (pak.isOpen))
                {
                    if (pak.isDirty)
                        Console.WriteLine("[PAK] Saving pak ... {0}", pak._gpFilePath);
                    else
                        Console.WriteLine("[PAK] Closing pak ... {0}",pak._gpFilePath);
                    pak.ClosePak();
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("[EXCEPTION] Error closing or saving pak file, possibly data corruption !\r\n{0}", x.Message);
            }
        }
    }
}
