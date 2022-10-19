using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AAPacker;

/// <summary>
/// AAPak Class used to handle game_pak from the ArcheAge Game
/// </summary>
public class AAPak
{
    /// <summary>
    /// Current Reader that is handling headers
    /// </summary>
    public AAPakFileFormatReader Reader { get; set; }

    public static List<AAPakFileFormatReader> ReaderPool { get; set; } = new() { new AAPakFileFormatReader(true) };

    /// <summary>
    /// Points to this pakFile's header
    /// </summary>
    public AAPakFileHeader Header { get; protected set; }

    /// <summary>
    /// When enabled, failing to open a pakFile will dump the currently used key as a file.
    /// </summary>
    public bool DebugMode { get; set; } = false;

    /// <summary>
    /// List of all unused files, normally these are all named "__unused__"
    /// </summary>
    public List<AAPakFileInfo> ExtraFiles { get; set; } = new();

    /// <summary>
    /// List of all used files
    /// </summary>
    public List<AAPakFileInfo> Files { get; set; } = new();

    /// <summary>
    /// Virtual list of all folder names, use GenerateFolderList() to populate this list (might take a while)
    /// </summary>
    public List<string> Folders { get; set; } = new();

    /// <summary>
    /// Set to true if there have been changes made that require a rewrite of the FAT and/or header
    /// </summary>
    public bool IsDirty { get; set; }

    /// <summary>
    /// Checks if current pakFile information is loaded into memory
    /// </summary>
    public bool IsOpen { get; protected internal set; }

    /// <summary>
    /// Set to true if this is not a pak file, but rather information loaded from somewhere else.
    /// This essentially disabled file read/write inside the pakFile
    /// </summary>
    public bool IsVirtual { get; protected internal set; }

    /// <summary>
    /// Returns the newest file time of the files inside the pakFile
    /// </summary>
    public DateTime NewestFileDate { get; protected internal set; } = DateTime.MinValue;

    /// <summary>
    /// Virtual data to return as a null value for file details, can be used as to initialize a var to pass as a ref
    /// </summary>
    public AAPakFileInfo NullAAPakFileInfo { get; } = new();

    /// <summary>
    /// If set to true, adds the freed space from a delete to the previous file's padding.
    /// If false (default), it "moves" the file into extraFiles for freeing up space, allowing it to be reused instead.
    /// Should only need to be changed if you are writing your own specialized patcher, and only in special cases
    /// </summary>
    public bool PaddingDeleteMode { get; protected set; } = false;

    /// <summary>
    /// Which pak style is being used
    /// </summary>
    public PakFileType PakType { get; set; } = PakFileType.Classic;

    /// <summary>
    /// Flag to enabled automatic MD5 recalculations when adding or replacing a file. Enabled by default.
    /// </summary>
    public bool AutoUpdateMd5WhenAdding { get; set; } = true;

    /// <summary>
    /// Trigger Event for OnProgress
    /// </summary>
    public event AAPakNotify OnProgress ;

    /// <summary>
    /// Defines how many files reading from FAT are skipped between OnProgress events
    /// </summary>
    public int OnProgressFATFileInterval { get; set; } = 10000;

    /// <summary>
    /// Creates and/or opens a game_pak file
    /// </summary>
    /// <param name="filePath">Filename of the pak</param>
    /// <param name="openAsReadOnly">Open pak in readOnly Mode if true. Ignored if createAsNewPak is set</param>
    /// <param name="createAsNewPak">If true, openAsReadOnly is ignored and will create a new pak at filePath location in read/write mode.
    /// Warning: This will overwrite any existing pak at that location !
    /// </param>
    public AAPak(string filePath, bool openAsReadOnly = true, bool createAsNewPak = false)
    {
        // Reader = new AAPakFileFormatReader(true);
        Header = new AAPakFileHeader(this);
        if (filePath != "")
        {
            IsOpen = createAsNewPak ? NewPak(filePath) : OpenPak(filePath, openAsReadOnly);
        }
        else
        {
            IsOpen = false;
        }
    }

    /// <summary>
    /// Internally used PakFile filename
    /// </summary>
    public string GpFilePath { get; protected set; }

    /// <summary>
    /// The Internally used FileStream when a pakFile is open
    /// </summary>
    public FileStream GpFileStream { get; protected set; }

    /// <summary>
    /// Show if this pakFile is opened in read-only mode
    /// </summary>
    public bool ReadOnly { get; protected set; }

    /// <summary>
    /// Returns the text message of the last internally caught exception (like file open errors)
    /// </summary>
    public string LastError { get; set; }

    /// <summary>
    /// Make sure the pakFile gets closed when destroying the object
    /// </summary>
    ~AAPak()
    {
        if (IsOpen)
            ClosePak();
    }

    /// <summary>
    /// Opens a pak file, can only be used if no other file is currently loaded
    /// </summary>
    /// <param name="filePath">Filename of the pakFile to open</param>
    /// <param name="openAsReadOnly">Set to true to open the pak in read-only mode</param>
    /// <returns>Returns true on success, or false if something failed</returns>
    public bool OpenPak(string filePath, bool openAsReadOnly)
    {
        TriggerProgress(AAPakLoadingProgressType.OpeningFile, 0, 100);

        // Fail if already open
        if (IsOpen)
            return false;

        // Check if it exists
        if (!File.Exists(filePath)) return false;

        IsVirtual = false;
            
        TriggerProgress(AAPakLoadingProgressType.OpeningFile, 25, 100);
        bool res;

        var ext = Path.GetExtension(filePath).ToLower();
        if (ext == ".csv")
        {
            ReadOnly = true;
            // Open file as CVS data
            res = OpenVirtualCsvPak(filePath);
            TriggerProgress(AAPakLoadingProgressType.OpeningFile, 100, 100);
            return res;
        }

        try
        {
            // Open stream
            GpFileStream = new FileStream(filePath, FileMode.Open,
                openAsReadOnly ? FileAccess.Read : FileAccess.ReadWrite);
            GpFilePath = filePath;
            IsDirty = false;
            IsOpen = true;
            ReadOnly = openAsReadOnly;
            res = ReadHeader();
        }
        catch (Exception ex)
        {
            GpFilePath = null;
            IsOpen = false;
            ReadOnly = true;
            LastError = ex.Message;
            res = false;
        }
        TriggerProgress(AAPakLoadingProgressType.OpeningFile, 100, 100);
        return res;
    }

    /// <summary>
    /// Creates a new pakFile with Name filename, will overwrite a existing file if it exists
    /// </summary>
    /// <param name="filePath">Filename of the new pakFile</param>
    /// <returns>Returns true on success, or false if something went wrong, or if you still have a pakFile open</returns>
    public bool NewPak(string filePath)
    {
        TriggerProgress(AAPakLoadingProgressType.OpeningFile, 0, 100);
        // Fail if already open
        if (IsOpen)
            return false;

        TriggerProgress(AAPakLoadingProgressType.OpeningFile, 25, 100);

        IsVirtual = false;

        bool res;
        try
        {
            // Create new file stream
            GpFileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            GpFilePath = filePath;
            ReadOnly = false;
            IsOpen = true;
            IsDirty = true;
            SaveHeader(); // Save blank data
            res = ReadHeader(); // read blank data to confirm
        }
        catch (Exception ex)
        {
            GpFilePath = null;
            IsOpen = false;
            ReadOnly = true;
            LastError = ex.Message;
            res = false;
        }
        TriggerProgress(AAPakLoadingProgressType.OpeningFile, 100, 100);
        return res;
    }

    /// <summary>
    /// Opens a CSV file as a virtual pak file
    /// </summary>
    /// <param name="csvFilePath"></param>
    /// <returns></returns>
    public bool OpenVirtualCsvPak(string csvFilePath)
    {
        // Fail if already open
        if (IsOpen)
            return false;

        // Check if it exists
        if (!File.Exists(csvFilePath)) return false;

        IsVirtual = true;
        GpFileStream = null; // Not used on virtual pakFiles
        try
        {
            // Open stream
            GpFilePath = csvFilePath;
            IsDirty = false;
            IsOpen = true;
            ReadOnly = true;
            PakType = PakFileType.Csv;
            return ReadCsvData();
        }
        catch (Exception ex)
        {
            IsOpen = false;
            ReadOnly = true;
            LastError = ex.Message;
            return false;
        }
    }

    /// <summary>
    /// Closes the currently opened pakFile (if open)
    /// </summary>
    public void ClosePak()
    {
        TriggerProgress(AAPakLoadingProgressType.ClosingFile, 0, 100);

        if (!IsOpen)
            return;

        if (IsDirty && ReadOnly == false)
            SaveHeader();

        GpFileStream?.Close();
        GpFileStream = null;
        GpFilePath = null;
        IsOpen = false;
        PakType = PakFileType.Classic;
        Reader = null;
        Header.SetDefaultKey();
        LastError = string.Empty;

        TriggerProgress(AAPakLoadingProgressType.ClosingFile, 100, 100);
    }

    /// <summary>
    /// Encrypts and saves the Header and File Information Table back to the pak.
    /// This is also automatically called on ClosePak() if changes where made.
    /// Warning: Failing to save will corrupt your pak if files were added or deleted!
    /// </summary>
    public bool SaveHeader()
    {
        try
        {
            TriggerProgress(AAPakLoadingProgressType.WritingHeader, 0, 100);
                
            Header.WriteToFAT();

            TriggerProgress(AAPakLoadingProgressType.WritingHeader, 50, 100);

            GpFileStream.Position = Header.FirstFileInfoOffset;
            Header.FAT.Position = 0;
            Header.FAT.CopyTo(GpFileStream);
            GpFileStream.SetLength(GpFileStream.Position);

            IsDirty = false;
                
            TriggerProgress(AAPakLoadingProgressType.WritingHeader, 100, 100);
        }
        catch (Exception e)
        {
            LastError = e.Message;
        }
        return !IsDirty;
    }

    /// <summary>
    /// Read Pak Header and FAT
    /// </summary>
    /// <returns>Returns true if the read information makes a valid pakFile</returns>
    protected bool ReadHeader()
    {
        TriggerProgress(AAPakLoadingProgressType.ReadingHeader, 0, 100);

        NewestFileDate = DateTime.MinValue;
        Files.Clear();
        ExtraFiles.Clear();
        Folders.Clear();

        // Read the last 512 bytes as raw header data
        GpFileStream.Seek(-AAPakFileHeader.Size, SeekOrigin.End);

        TriggerProgress(AAPakLoadingProgressType.ReadingHeader, 10, 100);

        // Mark correct location as header Offset location
        // We don't need to read the entire thing, just the first 32 bytes contain data
        var amountRead = GpFileStream.Read(Header.RawData, 0, AAPakFileHeader.Size);
            
        // If it failed to even read the first 32 bytes, then don't even bother
        if (amountRead < 32)
            return false;

        TriggerProgress(AAPakLoadingProgressType.ReadingHeader, 25, 100);

        Header.DecryptHeaderData();

        TriggerProgress(AAPakLoadingProgressType.ReadingHeader, 50, 100);

        if (Header.IsValid)
        {
            // Only allow editing for Classic
            // if (PakType != PakFileType.PakTypeA) readOnly = true;
            Header.LoadRawFAT();
            Header.ReadFileTable();
        }
        else
        {
            Header.FAT.SetLength(0);
        }

        TriggerProgress(AAPakLoadingProgressType.ReadingHeader, 100, 100);

        return Header.IsValid;
    }

    /// <summary>
    /// Helper function for converting Hex string to Byte Array
    /// </summary>
    /// <param name="hex">Input Hex string, must be a multiple of 2 long</param>
    /// <returns>Byte Array</returns>
    public static byte[] StringToByteArray(string hex)
    {
        var numberChars = hex.Length;
        var bytes = new byte[numberChars / 2];
        for (var i = 0; i < numberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }

    /// <summary>
    /// Converts DateTime to a fixed layout string for use with CSV virtual pak files
    /// </summary>
    /// <param name="dateTime">Input DateTime</param>
    /// <returns>String as a yyyyMMdd-HHmmss format</returns>
    public static string DateTimeToDateTimeStr(DateTime dateTime)
    {
        try
        {
            return dateTime.ToString("yyyyMMdd-HHmmss");
        }
        catch
        {
            return "00000000-000000";
        }
    }

    /// <summary>
    /// Creates a file time from a given specialized string
    /// </summary>
    /// <param name="encodedString"></param>
    /// <returns>FileTime as UTC</returns>
    public static long DateTimeStrToFileTime(string encodedString)
    {
        try
        {
            if (!int.TryParse(encodedString.Substring(0, 4), out var yyyy)) yyyy = 0;
            if (!int.TryParse(encodedString.Substring(5, 2), out var mm)) mm = 0;
            if (!int.TryParse(encodedString.Substring(8, 2), out var dd)) dd = 0;
            if (!int.TryParse(encodedString.Substring(11, 2), out var hh)) hh = 0;
            if (!int.TryParse(encodedString.Substring(14, 2), out var nn)) nn = 0;
            if (!int.TryParse(encodedString.Substring(17, 2), out var ss)) ss = 0;

            return new DateTime(yyyy, mm, dd, hh, nn, ss).ToFileTimeUtc();
        }
        catch
        {
            return 0u;
        }
    }

    /// <summary>
    /// Reads the virtual pak file using CSV data
    /// </summary>
    /// <returns></returns>
    protected bool ReadCsvData()
    {
        TriggerProgress(AAPakLoadingProgressType.ReadingFAT, 0, 100);
        Files.Clear();
        ExtraFiles.Clear();
        Folders.Clear();

        var lines = File.ReadAllLines(GpFilePath);
        TriggerProgress(AAPakLoadingProgressType.ReadingFAT, 0, lines.Length);

        if (lines.Length >= 1)
        {
            var csvHead = "";
            csvHead += "Name";
            csvHead += ";Size";
            csvHead += ";Offset";
            csvHead += ";Md5";
            csvHead += ";CreateTime";
            csvHead += ";ModifyTime";
            csvHead += ";SizeDuplicate";
            csvHead += ";PaddingSize";
            csvHead += ";Dummy1";
            csvHead += ";Dummy2";

            Header.IsValid = lines[0].ToLower() != csvHead;
        }
        else
        {
            Header.IsValid = false;
        }
        TriggerProgress(AAPakLoadingProgressType.ReadingFAT, 1, lines.Length);

        if (!Header.IsValid) return Header.IsValid;

        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            var fields = line.Split(';');
            if (fields.Length != 10) continue;
            try
            {
                var fni = new AAPakFileInfo();

                // Looks like it's valid, read it
                fni.Name = fields[0];
                fni.Size = long.Parse(fields[1]);
                fni.Offset = long.Parse(fields[2]);
                fni.Md5 = StringToByteArray(fields[3]);
                fni.CreateTime = DateTimeStrToFileTime(fields[4]);
                fni.ModifyTime = DateTimeStrToFileTime(fields[5]);
                fni.SizeDuplicate = long.Parse(fields[6]);
                fni.PaddingSize = int.Parse(fields[7]);
                fni.Dummy1 = uint.Parse(fields[8]);
                fni.Dummy2 = uint.Parse(fields[9]);

                // TODO: check if this reads correctly
                Files.Add(fni);

                if ((i % OnProgressFATFileInterval) == 0)
                    TriggerProgress(AAPakLoadingProgressType.ReadingFAT, i, lines.Length);
            }
            catch (Exception ex)
            {
                Header.IsValid = false;
                LastError = ex.Message;
                return false;
            }
        }

        TriggerProgress(AAPakLoadingProgressType.ReadingFAT, lines.Length, lines.Length);

        return Header.IsValid;
    }


    /// <summary>
    /// Populate the folders string list with virtual folder names derived from the files found inside the pak
    /// </summary>
    /// <param name="sortTheList">Set to false if you don't want the resulting folders list to be sorted (not recommended)</param>
    public void GenerateFolderList(bool sortTheList = true)
    {
        TriggerProgress(AAPakLoadingProgressType.GeneratingDirectories, 0, Files.Count);

        // There is no actual directory info stored in the pak file, so we just generate it based on fileNames
        Folders.Clear();
        if (!IsOpen || !Header.IsValid) return;
        var c = 0;
        foreach (var pfi in Files)
        {
            if (pfi.Name == string.Empty)
                continue;
            try
            {
                // Horror function, I know :p
                var n = Path.GetDirectoryName(pfi.Name.ToLower().Replace('/', Path.DirectorySeparatorChar))
                    ?.Replace(Path.DirectorySeparatorChar, '/');
                var pos = Folders.IndexOf(n);

                c++;
                if ((c % OnProgressFATFileInterval) == 0)
                    TriggerProgress(AAPakLoadingProgressType.GeneratingDirectories, c, Files.Count);

                if (pos >= 0)
                    continue;
                Folders.Add(n);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }

        if (Files.Count > 2)
            TriggerProgress(AAPakLoadingProgressType.GeneratingDirectories, Files.Count - 1, Files.Count);
            
        if (sortTheList)
            Folders.Sort();

        TriggerProgress(AAPakLoadingProgressType.GeneratingDirectories, Files.Count, Files.Count);
    }

    /// <summary>
    /// Get a list of files inside a given "directory".
    /// </summary>
    /// <param name="dirname">Directory Name to search in</param>
    /// <returns>Returns a new List of all found files</returns>
    public List<AAPakFileInfo> GetFilesInDirectory(string dirname)
    {
        var res = new List<AAPakFileInfo>();
        dirname = dirname.ToLower();
        foreach (var pfi in Files)
        {
            // extract dir Name
            string n;
            try
            {
                n = Path.GetDirectoryName(pfi.Name.ToLower().Replace('/', Path.DirectorySeparatorChar))
                    ?.Replace(Path.DirectorySeparatorChar, '/');
            }
            catch (Exception ex)
            {
                n = string.Empty;
                LastError = ex.Message;
            }

            if (n == dirname)
                res.Add(pfi);
        }

        return res;
    }

    /// <summary>
    /// Find a file information inside the pak by it's filename
    /// </summary>
    /// <param name="filename">filename inside the pak of the requested file</param>
    /// <param name="fileInfo">Returns the AAPakFile info of the requested file or nullAAPakFileInfo if it does not exist</param>
    /// <returns>Returns true if the file was found</returns>
    public bool GetFileByName(string filename, out AAPakFileInfo fileInfo)
    {
        fileInfo = Files.FirstOrDefault(pfi => pfi.Name == filename) ?? NullAAPakFileInfo;
        return fileInfo != null;
        /*
        foreach (var pfi in Files.Where(pfi => pfi.Name == filename))
        {
            fileInfo = pfi;
            return true;
        }
        fileInfo = NullAAPakFileInfo; // return null file if it fails
        return false;
        */
    }

    /// <summary>
    /// Find file info by using the pak's file index
    /// </summary>
    /// <param name="fileIndex"></param>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public bool GetFileByIndex(int fileIndex, out AAPakFileInfo fileInfo)
    {
        fileInfo = Files.FirstOrDefault(pfi => pfi.EntryIndexNumber == fileIndex) ?? NullAAPakFileInfo;
        return fileInfo != null;
        /*
        foreach (var pfi in Files.Where(pfi => pfi.EntryIndexNumber == fileIndex))
        {
            fileInfo = pfi;
            return true;
        }

        fileInfo = NullAAPakFileInfo; // return null file if it fails
        return false;
        */
    }

    /// <summary>
    /// Check if file exists within the pak
    /// </summary>
    /// <param name="filename">filename of the file to check</param>
    /// <returns>Returns true if the file was found</returns>
    public bool FileExists(string filename)
    {
        return Files.Select(pfi => pfi.Name == filename).FirstOrDefault();
        /*
        foreach (var pfi in Files)
            if (pfi.Name == filename)
                return true;
        return false;
        */
    }

    /// <summary>
    /// Exports a given file as a Stream
    /// </summary>
    /// <param name="file">AAPakFileInfo of the file to be exported</param>
    /// <returns>Returns a PackerSubStream of file within the pak</returns>
    public Stream ExportFileAsStream(AAPakFileInfo file)
    {
        return new PackerSubStream(GpFileStream, file.Offset, file.Size);
    }

    /// <summary>
    /// Exports a given file as stream
    /// </summary>
    /// <param name="fileName">filename inside the pak of the file to be exported</param>
    /// <returns>Returns a PackerSubStream of file within the pak, or a empty MemoryStream if the file was not found</returns>
    public Stream ExportFileAsStream(string fileName)
    {
        if (GetFileByName(fileName, out var file))
            return new PackerSubStream(GpFileStream, file.Offset, file.Size);
        return new MemoryStream();
    }

    /// <summary>
    /// Calculates and set the MD5 Hash of a given file
    /// </summary>
    /// <param name="file">AAPakFileInfo of the file to be updated</param>
    /// <returns>Returns the new hash as a hex string (with removed dashes)</returns>
    public string UpdateMd5(AAPakFileInfo file)
    {
        var hash = MD5.Create();
        var fs = ExportFileAsStream(file);
        var newHash = hash.ComputeHash(fs);
        hash.Dispose();
        if (!file.Md5.SequenceEqual(newHash))
        {
            // Only update if different
            newHash.CopyTo(file.Md5, 0);
            IsDirty = true;
        }

        return BitConverter.ToString(file.Md5).Replace("-", ""); // Return the (updated) Md5 as a string
    }

    /// <summary>
    /// Manually set a new MD5 value for a file
    /// </summary>
    /// <param name="file"></param>
    /// <param name="newHash"></param>
    /// <returns>Returns true if a new value was set</returns>
    public bool SetMd5(AAPakFileInfo file, byte[] newHash)
    {
        if ((file == null) || (newHash == null) || (newHash.Length != 16))
            return false;
        newHash.CopyTo(file.Md5, 0);
        IsDirty = true;
        return true;
    }


    /// <summary>
    /// Try to find a file inside the pak file based on a Offset position inside the pak file.
    /// Note: this only checks inside the used files and does not account for "deleted" files
    /// </summary>
    /// <param name="Offset">Offset to check against</param>
    /// <param name="fileInfo">Returns the found file's info, or nullAAPakFileInfo if nothing was found</param>
    /// <returns>Returns true if the location was found to be inside a valid file</returns>
    public bool FindFileByOffset(long offset, out AAPakFileInfo fileInfo)
    {
        foreach (var pfi in Files.Where(pfi =>
                     (offset >= pfi.Offset) && (offset <= pfi.Offset + pfi.Size + pfi.PaddingSize)))
        {
            fileInfo = pfi;
            return true;
        }

        fileInfo = NullAAPakFileInfo;
        return false;
    }

    /// <summary>
    /// Replaces a file's data with new data from a stream, can only be used if the current file location has enough space
    /// to hold the new data
    /// </summary>
    /// <param name="pfi">FileInfo of the file to replace</param>
    /// <param name="sourceStream">Stream to replace the data with</param>
    /// <param name="modifyTime">Time to be used as a modified time stamp</param>
    /// <returns>Returns true on success</returns>
    public bool ReplaceFile(ref AAPakFileInfo pfi, Stream sourceStream, DateTime modifyTime)
    {
        // Overwrite a existing file in the pak

        if (ReadOnly)
            return false;

        // Fail if the new file is too big
        if (sourceStream.Length > pfi.Size + pfi.PaddingSize)
            return false;

        // Save endPos for easy calculation later
        var endPosition = pfi.Offset + pfi.Size + pfi.PaddingSize;

        try
        {
            // Copy new data over the old data
            GpFileStream.Position = pfi.Offset;
            sourceStream.Position = 0;
            sourceStream.CopyTo(GpFileStream);
        }
        catch (Exception ex)
        {
            LastError = ex.Message;
            return false;
        }

        // Update File Size in File Table
        pfi.Size = sourceStream.Length;
        pfi.SizeDuplicate = pfi.Size;
        // Calculate new Padding Size
        pfi.PaddingSize = (int)(endPosition - pfi.Size - pfi.Offset);

        // Recalculate the MD5 hash
        // TODO: optimize this to calculate WHILE we are copying the stream
        if (AutoUpdateMd5WhenAdding)
            UpdateMd5(pfi);

        pfi.ModifyTime = modifyTime.ToFileTimeUtc();

        pfi.Dummy1 = Reader?.DefaultDummy1 ?? 0;
        pfi.Dummy2 = Reader?.DefaultDummy2 ?? 0;

        // Mark File Table as dirty
        IsDirty = true;

        return true;
    }

    /// <summary>
    /// Delete a file from pak. Behaves differently depending on the paddingDeleteMode setting
    /// </summary>
    /// <param name="pfi">AAPakFileInfo of the file that is to be deleted</param>
    /// <returns>Returns true on success</returns>
    public bool DeleteFile(AAPakFileInfo pfi)
    {
        // When we delete a file from the pak, we remove the entry from the FileTable and expand the previous file's padding to take up the space
        if (ReadOnly)
            return false;

        if (PaddingDeleteMode)
        {
            if (FindFileByOffset(pfi.Offset - 1, out var prevPfi))
                // If we have a previous file, expand it's padding area with the free space from this file
                prevPfi.PaddingSize += (int)pfi.Size + pfi.PaddingSize;
            Files.Remove(pfi);
        }
        else
        {
            // "move" Offset and Size data to extraFiles
            var eFile = new AAPakFileInfo();
            eFile.Name = "__unused__";
            eFile.Offset = pfi.Offset;
            eFile.Size = pfi.Size + pfi.PaddingSize;
            eFile.SizeDuplicate = eFile.Size;
            eFile.PaddingSize = 0;
            eFile.Md5 = new byte[16];
            eFile.Dummy1 = Reader?.DefaultDummy1 ?? 0;
            eFile.Dummy2 = Reader?.DefaultDummy2 ?? 0;

            ExtraFiles.Add(eFile);

            Files.Remove(pfi);
        }

        IsDirty = true;
        return true;
    }

    /// <summary>
    /// Delete a file from pak. Behaves differently depending on the paddingDeleteMode setting
    /// </summary>
    /// <param name="filename">Filename of the file to delete from the pakFile</param>
    /// <returns>Returns true on success or if the file didn't exist</returns>
    public bool DeleteFile(string filename)
    {
        if (ReadOnly)
            return false;

        return !GetFileByName(filename, out var pfi) || DeleteFile(pfi);
    }

    /// <summary>
    /// Adds a new file into the pak
    /// </summary>
    /// <param name="filename">Filename of the file inside the pakFile</param>
    /// <param name="sourceStream">Source Stream containing the file data</param>
    /// <param name="createTime">Time to use as initial file creation timestamp</param>
    /// <param name="modifyTime">Time to use as last modified timestamp</param>
    /// <param name="autoSpareSpace"> When set, tries to pre-allocate extra free space at the end of the file, this will be 25%
    /// of the fileSize if used. If a "deleted file" is used, this parameter is ignored
    /// </param>
    /// <param name="pfi">Returns the fileInfo of the newly created file</param>
    /// <returns>Returns true on success</returns>
    public bool AddAsNewFile(string filename, Stream sourceStream, DateTime createTime, DateTime modifyTime, bool autoSpareSpace, out AAPakFileInfo pfi)
    {
        // When we have a new file, or previous space wasn't enough, we will add it where the file table starts, and move the file table
        if (ReadOnly)
        {
            pfi = NullAAPakFileInfo;
            return false;
        }

        var addedAtTheEnd = true;

        var newFile = new AAPakFileInfo();
        newFile.Name = filename;
        newFile.Offset = Header.FirstFileInfoOffset;
        newFile.Size = sourceStream.Length;
        newFile.SizeDuplicate = newFile.Size;
        newFile.CreateTime = createTime.ToFileTimeUtc();
        newFile.ModifyTime = modifyTime.ToFileTimeUtc();
        newFile.PaddingSize = 0;
        newFile.Md5 = new byte[16];
        newFile.Dummy1 = Reader?.DefaultDummy1 ?? 0;
        newFile.Dummy2 = Reader?.DefaultDummy2 ?? 0;

        // check if we have "unused" space in extraFiles that we can use
        for (var i = 0; i < ExtraFiles.Count; i++)
            if (newFile.Size <= ExtraFiles[i].Size)
            {
                // Copy the spare file's properties and remove it from extraFiles
                newFile.Offset = ExtraFiles[i].Offset;
                newFile.PaddingSize = (int)(ExtraFiles[i].Size - newFile.Size); // This should already be aligned
                addedAtTheEnd = false;
                ExtraFiles.Remove(ExtraFiles[i]);
                break;
            }

        if (addedAtTheEnd)
        {
            // Only need to calculate padding if we are adding at the end
            var dif = newFile.Size % 0x200;
            if (dif > 0) newFile.PaddingSize = (int)(0x200 - dif);
            if (autoSpareSpace)
            {
                // If autoSpareSpace is used to add files, we will reserve some extra space as padding
                // Add 25% by default
                var spareSpace = newFile.Size / 4;
                spareSpace -= spareSpace % 0x200; // Align the spare space
                newFile.PaddingSize += (int)spareSpace;
            }
        }

        // Add to files list
        Files.Add(newFile);

        IsDirty = true;

        // Add File Data
        GpFileStream.Position = newFile.Offset;
        sourceStream.Position = 0;
        sourceStream.CopyTo(GpFileStream);

        if (addedAtTheEnd) 
            Header.FirstFileInfoOffset = newFile.Offset + newFile.Size + newFile.PaddingSize;

        // TODO: optimize this to calculate WHILE we are copying the stream
        if (AutoUpdateMd5WhenAdding)
            UpdateMd5(newFile);

        // Set output
        pfi = newFile;
        return true;
    }

    /// <summary>
    /// Adds or replace a given file with Name filename with data from sourceStream
    /// </summary>
    /// <param name="filename">The filename used inside the pak</param>
    /// <param name="sourceStream">Source Stream of file to be added</param>
    /// <param name="createTime">Time to use as original file creation time</param>
    /// <param name="modifyTime">Time to use as last modified time</param>
    /// <param name="autoSpareSpace">Enable adding 25% of the sourceStream Size as padding when not replacing a file</param>
    /// <param name="pfi">AAPakFileInfo of the newly added or modified file</param>
    /// <returns>Returns true on success</returns>
    public bool AddFileFromStream(string filename, Stream sourceStream, DateTime createTime, DateTime modifyTime, bool autoSpareSpace, out AAPakFileInfo pfi)
    {
        pfi = NullAAPakFileInfo;
        if (ReadOnly) return false;

        var addAsNew = true;
        // Try to find the existing file
        if (GetFileByName(filename, out pfi))
        {
            var reservedSizeMax = pfi.Size + pfi.PaddingSize;
            addAsNew = sourceStream.Length > reservedSizeMax;
            // Bug-fix: If we have insufficient space, make sure to delete the old file first as well
            if (addAsNew)
                DeleteFile(pfi);
        }

        return addAsNew
            ? AddAsNewFile(filename, sourceStream, createTime, modifyTime, autoSpareSpace, out pfi)
            : ReplaceFile(ref pfi, sourceStream, modifyTime);
    }

    /// <summary>
    /// Adds a file into the pakFile with a given Name
    /// </summary>
    /// <param name="sourceFileName">Filename of the source file to be added</param>
    /// <param name="asFileName">Filename inside the pakFile to use</param>
    /// <param name="autoSpareSpace">When set, tries to pre-allocate extra free space at the end of the file, this will be 25%
    /// of the fileSize if used. If a "deleted file" is used, this parameter is ignored
    /// </param>
    /// <returns>Returns true on success</returns>
    public bool AddFileFromFile(string sourceFileName, string asFileName, bool autoSpareSpace)
    {
        if (!File.Exists(sourceFileName))
            return false;
        var createTime = File.GetCreationTime(sourceFileName);
        var modTime = File.GetLastWriteTime(sourceFileName);
        var fs = File.OpenRead(sourceFileName);
        var res = AddFileFromStream(asFileName, fs, createTime, modTime, autoSpareSpace, out _);
        fs.Dispose();
        return res;
    }

    /// <summary>
    /// Convert a stream into a string
    /// </summary>
    /// <param name="stream">Source stream</param>
    /// <returns>String value of the data inside the stream</returns>
    public static string StreamToString(Stream stream)
    {
        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Convert a string into a MemoryStream
    /// </summary>
    /// <param name="src">Source string</param>
    /// <returns>A new MemoryStream that holds the source string's data</returns>
    public static Stream StringToStream(string src)
    {
        var byteArray = Encoding.UTF8.GetBytes(src);
        return new MemoryStream(byteArray);
    }

    /// <summary>
    /// If you want to use custom keys on your pak file, use this function to change the key that is used for
    /// encryption/decryption of the FAT and header data
    /// </summary>
    /// <param name="newKey"></param>
    public void SetCustomKey(byte[] newKey)
    {
        Header.SetCustomKey(newKey);
    }

    /// <summary>
    ///     Reverts back to the original encryption key, this function is also automatically called when closing a file
    /// </summary>
    public void SetDefaultKey()
    {
        Header.SetDefaultKey();
    }

    public void TriggerProgress(AAPakLoadingProgressType progressType, int step, int maximum)
    {
        OnProgress?.Invoke(this, progressType, step, maximum);
    }
}