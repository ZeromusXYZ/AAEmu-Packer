using System.Security.Cryptography;
using System.Text;
using SubStreamHelper;

namespace AAPacker
{
    /// <summary>
    ///     AAPak Class used to handle game_pak from ArcheAge
    /// </summary>
    public class AAPak
    {
        /// <summary>
        ///     points to this pakfile's header
        /// </summary>
        public AAPakFileHeader _header;

        public bool DebugMode = false;

        /// <summary>
        ///     List of all unused files, normally these are all named "__unused__"
        /// </summary>
        public List<AAPakFileInfo> extraFiles = new();

        /// <summary>
        ///     List of all used files
        /// </summary>
        public List<AAPakFileInfo> files = new();

        /// <summary>
        ///     Virtual list of all folder names, use GenerateFolderList() to populate this list (might take a while)
        /// </summary>
        public List<string> folders = new();

        /// <summary>
        ///     Set to true if there have been changes made that require a rewrite of the FAT and/or header
        /// </summary>
        public bool isDirty;

        /// <summary>
        ///     Checks if current pakfile information is loaded into memory
        /// </summary>
        public bool isOpen;

        /// <summary>
        ///     Set to true if this is not a pak file, but rather information loaded from somewhere else
        /// </summary>
        public bool isVirtual;

        public DateTime NewestFileDate = DateTime.MinValue;

        /// <summary>
        ///     Virtual data to return as a null value for file details, can be used as to initialize a var to pass as a ref
        /// </summary>
        public AAPakFileInfo nullAAPakFileInfo = new();

        /// <summary>
        ///     If set to true, adds the freed space from a delete to the previous file's padding.
        ///     If false (default), it "moves" the file into extraFiles for freeing up space, allowing it to be reused instead.
        ///     Should only need to be changed if you are writing your own specialized patcher, and only in special cases
        /// </summary>
        public bool paddingDeleteMode = false;

        public PakFileType PakType = PakFileType.TypeA;

        /// <summary>
        ///     Creates and/or opens a game_pak file
        /// </summary>
        /// <param name="filePath">Filename of the pak</param>
        /// <param name="openAsReadOnly">Open pak in readOnly Mode if true. Ignored if createAsNewPak is set</param>
        /// <param name="createAsNewPak">
        ///     If true, openAsReadOnly is ignored and will create a new pak at filePath location in
        ///     read/write mode. Warning: This will overwrite any existing pak at that location !
        /// </param>
        public AAPak(string filePath, bool openAsReadOnly = true, bool createAsNewPak = false)
        {
            _header = new AAPakFileHeader(this);
            if (filePath != "")
            {
                var isLoaded = false;

                /*
                var ext = Path.GetExtension(filePath).ToLower();
                if (ext == "csv") 
                {
                    if ((openAsReadOnly == true) && (createAsNewPak == false))
                    {
                        // Open file as CVS data
                        isLoaded = OpenVirtualCSVPak(filePath);
                        return;
                    }
                    // We will only allow opening as a CVS file when it's set to readonly (and not a new file)
                }
                */

                if (createAsNewPak)
                    isLoaded = NewPak(filePath);
                else
                    isLoaded = OpenPak(filePath, openAsReadOnly);
                if (isLoaded)
                    isOpen = ReadHeader();
                else
                    isOpen = false;
            }
            else
            {
                isOpen = false;
            }
        }

        public string _gpFilePath { get; private set; }
        public FileStream _gpFileStream { get; private set; }

        /// <summary>
        ///     Show if this pakfile is opened in read-only mode
        /// </summary>
        public bool readOnly { get; private set; }

        ~AAPak()
        {
            if (isOpen)
                ClosePak();
        }

        /// <summary>
        ///     Opens a pak file, can only be used if no other file is currently loaded
        /// </summary>
        /// <param name="filePath">Filename of the pakfile to open</param>
        /// <param name="openAsReadOnly">Set to true to open the pak in read-only mode</param>
        /// <returns>Returns true on success, or false if something failed</returns>
        public bool OpenPak(string filePath, bool openAsReadOnly)
        {
            // Fail if already open
            if (isOpen)
                return false;

            // Check if it exists
            if (!File.Exists(filePath)) return false;

            isVirtual = false;

            var ext = Path.GetExtension(filePath).ToLower();
            if (ext == ".csv")
            {
                openAsReadOnly = true;
                readOnly = true;
                // Open file as CVS data
                return OpenVirtualCSVPak(filePath);
            }

            try
            {
                // Open stream
                if (openAsReadOnly)
                    _gpFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                else
                    _gpFileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                _gpFilePath = filePath;
                isDirty = false;
                isOpen = true;
                readOnly = openAsReadOnly;
                return ReadHeader();
            }
            catch
            {
                _gpFilePath = null;
                isOpen = false;
                readOnly = true;
                return false;
            }
        }

        /// <summary>
        ///     Creates a new pakfile with name filename, will overwrite a existing file if it exists
        /// </summary>
        /// <param name="filePath">Filename of the new pakfile</param>
        /// <returns>Returns true on success, or false if something went wrong, or if you still have a pakfile open</returns>
        public bool NewPak(string filePath)
        {
            // Fail if already open
            if (isOpen)
                return false;
            isVirtual = false;
            try
            {
                // Create new file stream
                _gpFileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                _gpFilePath = filePath;
                readOnly = false;
                isOpen = true;
                isDirty = true;
                SaveHeader(); // Save blank data
                return ReadHeader(); // read blank data to confirm
            }
            catch
            {
                _gpFilePath = null;
                isOpen = false;
                readOnly = true;
                return false;
            }
        }


        public bool OpenVirtualCSVPak(string csvfilePath)
        {
            // Fail if already open
            if (isOpen)
                return false;

            // Check if it exists
            if (!File.Exists(csvfilePath)) return false;
            isVirtual = true;
            _gpFileStream = null; // Not used on virtual paks
            try
            {
                // Open stream
                _gpFilePath = csvfilePath;
                isDirty = false;
                isOpen = true;
                readOnly = true;
                PakType = PakFileType.CSV;
                return ReadCSVData();
            }
            catch
            {
                isOpen = false;
                readOnly = true;
                return false;
            }
        }

        /// <summary>
        ///     Closes the currently opened pakfile (if open)
        /// </summary>
        public void ClosePak()
        {
            if (!isOpen)
                return;
            if (isDirty && readOnly == false)
                SaveHeader();
            if (_gpFileStream != null)
                _gpFileStream.Close();
            _gpFileStream = null;
            _gpFilePath = null;
            isOpen = false;
            _header.SetDefaultKey();
        }

        /// <summary>
        ///     Encrypts and saves Header and File Information Table back to the pak.
        ///     This is also automatically called on ClosePak() if changes where made.
        ///     Warning: Failing to save might corrupt your pak if files were added or deleted !
        /// </summary>
        public void SaveHeader()
        {
            _header.WriteToFAT();
            _gpFileStream.Position = _header.FirstFileInfoOffset;
            _header.FAT.Position = 0;
            _header.FAT.CopyTo(_gpFileStream);
            _gpFileStream.SetLength(_gpFileStream.Position);

            isDirty = false;
        }

        /// <summary>
        ///     Read Pak Header and FAT
        /// </summary>
        /// <returns>Returns true if the read information makes a valid pakfile</returns>
        protected bool ReadHeader()
        {
            NewestFileDate = DateTime.MinValue;
            files.Clear();
            extraFiles.Clear();
            folders.Clear();

            // Read the last 512 bytes as raw header data
            _gpFileStream.Seek(-AAPakFileHeader.Size, SeekOrigin.End);

            // Mark correct location as header offset location
            _gpFileStream.Read(_header.RawData, 0,
                AAPakFileHeader.Size); // We don't need to read the entire thing, just the first 32 bytes contain data
            // _gpFileStream.Read(_header.rawData, 0, _header.Size);

            _header.DecryptHeaderData();

            if (_header.IsValid)
            {
                // Only allow editing for TypeA
                // if (PakType != PakFileType.PakTypeA) readOnly = true;
                _header.LoadRawFAT();
                _header.ReadFileTable();
            }
            else
            {
                _header.FAT.SetLength(0);
            }

            return _header.IsValid;
        }

        public static byte[] StringToByteArray(string hex)
        {
            var NumberChars = hex.Length;
            var bytes = new byte[NumberChars / 2];
            for (var i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string DateTimeToDateTimeStr(DateTime aTime)
        {
            var res = "";
            try
            {
                res = aTime.ToString("yyyyMMdd-HHmmss");
            }
            catch
            {
                res = "00000000-000000";
            }

            return res;
        }

        /// <summary>
        ///     Creates a file time from a given specialized string
        /// </summary>
        /// <param name="encodedString"></param>
        /// <returns>FILETIME as UTC</returns>
        public static long DateTimeStrToFILETIME(string encodedString)
        {
            long res = 0;

            var yyyy = 0;
            var mm = 0;
            var dd = 0;
            var hh = 0;
            var nn = 0;
            var ss = 0;

            try
            {
                if (!int.TryParse(encodedString.Substring(0, 4), out yyyy)) yyyy = 0;
                if (!int.TryParse(encodedString.Substring(5, 2), out mm)) mm = 0;
                if (!int.TryParse(encodedString.Substring(8, 2), out dd)) dd = 0;
                if (!int.TryParse(encodedString.Substring(11, 2), out hh)) hh = 0;
                if (!int.TryParse(encodedString.Substring(14, 2), out nn)) nn = 0;
                if (!int.TryParse(encodedString.Substring(17, 2), out ss)) ss = 0;

                res = new DateTime(yyyy, mm, dd, hh, nn, ss).ToFileTimeUtc();
            }
            catch
            {
                res = 0;
            }

            return res;
        }

        protected bool ReadCSVData()
        {
            files.Clear();
            extraFiles.Clear();
            folders.Clear();

            var lines = File.ReadAllLines(_gpFilePath);

            if (lines.Length >= 1)
            {
                var csvHead = "";
                csvHead = "name";
                csvHead += ";size";
                csvHead += ";offset";
                csvHead += ";md5";
                csvHead += ";createTime";
                csvHead += ";modifyTime";
                csvHead += ";sizeDuplicate";
                csvHead += ";paddingSize";
                csvHead += ";dummy1";
                csvHead += ";dummy2";

                if (lines[0].ToLower() != csvHead)
                    _header.IsValid = true;
                else
                    _header.IsValid = false;
            }
            else
            {
                _header.IsValid = false;
            }

            if (_header.IsValid)
                for (var i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var fields = line.Split(';');
                    if (fields.Length == 10)
                        try
                        {
                            var fni = new AAPakFileInfo();

                            // Looks like it's valid, read it
                            fni.name = fields[0];
                            fni.size = long.Parse(fields[1]);
                            fni.offset = long.Parse(fields[2]);
                            fni.md5 = StringToByteArray(fields[3]);
                            fni.createTime = DateTimeStrToFILETIME(fields[4]);
                            fni.modifyTime = DateTimeStrToFILETIME(fields[5]);
                            fni.sizeDuplicate = long.Parse(fields[6]);
                            fni.paddingSize = int.Parse(fields[7]);
                            fni.dummy1 = uint.Parse(fields[8]);
                            fni.dummy2 = uint.Parse(fields[9]);

                            // TODO: check if this reads correctly
                            files.Add(fni);
                        }
                        catch
                        {
                            _header.IsValid = false;
                            return false;
                        }
                }

            return _header.IsValid;
        }


        /// <summary>
        ///     Populate the folders string list with virual folder names derived from the files found inside the pak
        /// </summary>
        /// <param name="sortTheList">Set to false if you don't want the resulting folders list to be sorted (not recommended)</param>
        public void GenerateFolderList(bool sortTheList = true)
        {
            // There is no actual directory info stored in the pak file, so we just generate it based on filenames
            folders.Clear();
            if (!isOpen || !_header.IsValid) return;
            foreach (var pfi in files)
            {
                if (pfi.name == string.Empty)
                    continue;
                try
                {
                    // Horror function, I know :p
                    var n = Path.GetDirectoryName(pfi.name.ToLower().Replace('/', Path.DirectorySeparatorChar))
                        .Replace(Path.DirectorySeparatorChar, '/');
                    var pos = folders.IndexOf(n);
                    if (pos >= 0)
                        continue;
                    folders.Add(n);
                }
                catch
                {
                }
            }

            if (sortTheList)
                folders.Sort();
        }

        /// <summary>
        ///     Get a list of files inside a given "directory".
        /// </summary>
        /// <param name="dirname">Directory name to search in</param>
        /// <returns>Returns a new List of all found files</returns>
        public List<AAPakFileInfo> GetFilesInDirectory(string dirname)
        {
            var res = new List<AAPakFileInfo>();
            dirname = dirname.ToLower();
            foreach (var pfi in files)
            {
                // extract dir name
                var n = string.Empty;
                try
                {
                    n = Path.GetDirectoryName(pfi.name.ToLower().Replace('/', Path.DirectorySeparatorChar))
                        .Replace(Path.DirectorySeparatorChar, '/');
                }
                catch
                {
                    n = string.Empty;
                }

                if (n == dirname)
                    res.Add(pfi);
            }

            return res;
        }

        /// <summary>
        ///     Find a file information inside the pak by it's filename
        /// </summary>
        /// <param name="filename">filename inside the pak of the requested file</param>
        /// <param name="fileInfo">Returns the AAPakFile info of the requested file or nullAAPakFileInfo if it does not exist</param>
        /// <returns>Returns true if the file was found</returns>
        public bool GetFileByName(string filename, ref AAPakFileInfo fileInfo)
        {
            foreach (var pfi in files)
                if (pfi.name == filename)
                {
                    fileInfo = pfi;
                    return true;
                }

            fileInfo = nullAAPakFileInfo; // return null file if it fails
            return false;
        }

        public bool GetFileByIndex(int fileIndex, ref AAPakFileInfo fileInfo)
        {
            foreach (var pfi in files)
                if (pfi.entryIndexNumber == fileIndex)
                {
                    fileInfo = pfi;
                    return true;
                }

            fileInfo = nullAAPakFileInfo; // return null file if it fails
            return false;
        }

        /// <summary>
        ///     Check if file exists within the pak
        /// </summary>
        /// <param name="filename">filename of the file to check</param>
        /// <returns>Returns true if the file was found</returns>
        public bool FileExists(string filename)
        {
            foreach (var pfi in files)
                if (pfi.name == filename)
                    return true;
            return false;
        }

        /// <summary>
        ///     Exports a given file as a Stream
        /// </summary>
        /// <param name="file">AAPakFileInfo of the file to be exported</param>
        /// <returns>Returns a SubStream of file within the pak</returns>
        public Stream ExportFileAsStream(AAPakFileInfo file)
        {
            return new SubStream(_gpFileStream, file.offset, file.size);
        }

        /// <summary>
        ///     Exports a given file as stream
        /// </summary>
        /// <param name="fileName">filename inside the pak of the file to be exported</param>
        /// <returns>Returns a SubStream of file within the pak</returns>
        public Stream ExportFileAsStream(string fileName)
        {
            var file = nullAAPakFileInfo;
            if (GetFileByName(fileName, ref file))
                return new SubStream(_gpFileStream, file.offset, file.size);
            return new MemoryStream();
        }

        /// <summary>
        ///     Calculates and set the MD5 Hash of a given file
        /// </summary>
        /// <param name="file">AAPakFileInfo of the file to be updated</param>
        /// <returns>Returns the new hash as a hex string (with removed dashes)</returns>
        public string UpdateMD5(AAPakFileInfo file)
        {
            var hash = MD5.Create();
            var fs = ExportFileAsStream(file);
            var newHash = hash.ComputeHash(fs);
            hash.Dispose();
            if (!file.md5.SequenceEqual(newHash))
            {
                // Only update if different
                newHash.CopyTo(file.md5, 0);
                isDirty = true;
            }

            return BitConverter.ToString(file.md5).Replace("-", ""); // Return the (updated) md5 as a string
        }

        /// <summary>
        ///     Manually set a new MD5 value for a file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="newHash"></param>
        /// <returns>Returns true if a new value was set</returns>
        public bool SetMD5(AAPakFileInfo file, byte[] newHash)
        {
            if (file == null || newHash == null || newHash.Length != 16)
                return false;
            newHash.CopyTo(file.md5, 0);
            isDirty = true;
            return true;
        }


        /// <summary>
        ///     Try to find a file inside the pakfile base on a offset position inside the pakfile.
        ///     Note: this only checks inside the used files and does not account for "deleted" files
        /// </summary>
        /// <param name="offset">Offset to check against</param>
        /// <param name="fileInfo">Returns the found file's info, or nullAAPakFileInfo if nothing was found</param>
        /// <returns>Returns true if the location was found to be inside a valid file</returns>
        public bool FindFileByOffset(long offset, ref AAPakFileInfo fileInfo)
        {
            foreach (var pfi in files)
                if (offset >= pfi.offset && offset <= pfi.offset + pfi.size + pfi.paddingSize)
                {
                    fileInfo = pfi;
                    return true;
                }

            fileInfo = nullAAPakFileInfo;
            return false;
        }

        /// <summary>
        ///     Replaces a file's data with new data from a stream, can only be used if the current file location has enough space
        ///     to hold the new data
        /// </summary>
        /// <param name="pfi">Fileinfo of the file to replace</param>
        /// <param name="sourceStream">Stream to replace the data with</param>
        /// <param name="modifyTime">Time to be used as a modified time stamp</param>
        /// <returns>Returns true on success</returns>
        public bool ReplaceFile(ref AAPakFileInfo pfi, Stream sourceStream, DateTime modifyTime)
        {
            // Overwrite a existing file in the pak

            if (readOnly)
                return false;

            // Fail if the new file is too big
            if (sourceStream.Length > pfi.size + pfi.paddingSize)
                return false;

            // Save endpos for easy calculation later
            var endPos = pfi.offset + pfi.size + pfi.paddingSize;

            try
            {
                // Copy new data over the old data
                _gpFileStream.Position = pfi.offset;
                sourceStream.Position = 0;
                sourceStream.CopyTo(_gpFileStream);
            }
            catch
            {
                return false;
            }

            // Update File Size in File Table
            pfi.size = sourceStream.Length;
            pfi.sizeDuplicate = pfi.size;
            // Calculate new Padding size
            pfi.paddingSize = (int)(endPos - pfi.size - pfi.offset);
            // Recalculate the MD5 hash
            UpdateMD5(pfi); // TODO: optimize this to calculate WHILE we are copying the stream
            pfi.modifyTime = modifyTime.ToFileTimeUtc();

            if (PakType == PakFileType.TypeB)
                pfi.dummy1 = 0x80000000;

            // Mark File Table as dirty
            isDirty = true;

            return true;
        }

        /// <summary>
        ///     Delete a file from pak. Behaves differenly depending on the paddingDeleteMode setting
        /// </summary>
        /// <param name="pfi">AAPakFileInfo of the file that is to be deleted</param>
        /// <returns>Returns true on success</returns>
        public bool DeleteFile(AAPakFileInfo pfi)
        {
            // When we detele a file from the pak, we remove the entry from the FileTable and expand the previous file's padding to take up the space
            if (readOnly)
                return false;

            if (paddingDeleteMode)
            {
                var prevPfi = nullAAPakFileInfo;
                if (FindFileByOffset(pfi.offset - 1, ref prevPfi))
                    // If we have a previous file, expand it's padding area with the free space from this file
                    prevPfi.paddingSize += (int)pfi.size + pfi.paddingSize;
                files.Remove(pfi);
            }
            else
            {
                // "move" offset and size data to extrafiles
                var eFile = new AAPakFileInfo();
                eFile.name = "__unused__";
                eFile.offset = pfi.offset;
                eFile.size = pfi.size + pfi.paddingSize;
                eFile.sizeDuplicate = eFile.size;
                eFile.paddingSize = 0;
                eFile.md5 = new byte[16];
                if (PakType == PakFileType.TypeB)
                    eFile.dummy1 = 0x80000000;

                extraFiles.Add(eFile);

                files.Remove(pfi);
            }

            isDirty = true;
            return true;
        }

        /// <summary>
        ///     Delete a file from pak. Behaves differenly depending on the paddingDeleteMode setting
        /// </summary>
        /// <param name="filename">Filename of the file to delete from the pakfile</param>
        /// <returns>Returns true on success or if the file didn't exist</returns>
        public bool DeleteFile(string filename)
        {
            if (readOnly)
                return false;

            var pfi = nullAAPakFileInfo;
            if (GetFileByName(filename, ref pfi))
                return DeleteFile(pfi);
            return true;
        }

        /// <summary>
        ///     Adds a new file into the pak
        /// </summary>
        /// <param name="filename">Filename of the file inside the pakfile</param>
        /// <param name="sourceStream">Source Stream containing the file data</param>
        /// <param name="CreateTime">Time to use as initial file creation timestamp</param>
        /// <param name="ModifyTime">Time to use as last modified timestamp</param>
        /// <param name="autoSpareSpace">
        ///     When set, tries to pre-allocate extra free space at the end of the file, this will be 25%
        ///     of the filesize if used. If a "deleted file" is used, this parameter is ignored
        /// </param>
        /// <param name="pfi">Returns the fileinfo of the newly created file</param>
        /// <returns>Returns true on success</returns>
        public bool AddAsNewFile(string filename, Stream sourceStream, DateTime CreateTime, DateTime ModifyTime,
            bool autoSpareSpace, out AAPakFileInfo pfi)
        {
            // When we have a new file, or previous space wasn't enough, we will add it where the file table starts, and move the file table
            if (readOnly)
            {
                pfi = nullAAPakFileInfo;
                return false;
            }

            var addedAtTheEnd = true;

            var newFile = new AAPakFileInfo();
            newFile.name = filename;
            newFile.offset = _header.FirstFileInfoOffset;
            newFile.size = sourceStream.Length;
            newFile.sizeDuplicate = newFile.size;
            newFile.createTime = CreateTime.ToFileTimeUtc();
            newFile.modifyTime = ModifyTime.ToFileTimeUtc();
            newFile.paddingSize = 0;
            newFile.md5 = new byte[16];
            if (PakType == PakFileType.TypeB)
                newFile.dummy1 = 0x80000000;

            // check if we have "unused" space in extraFiles that we can use
            for (var i = 0; i < extraFiles.Count; i++)
                if (newFile.size <= extraFiles[i].size)
                {
                    // Copy the spare file's properties and remove it from extraFiles
                    newFile.offset = extraFiles[i].offset;
                    newFile.paddingSize = (int)(extraFiles[i].size - newFile.size); // This should already be aligned
                    addedAtTheEnd = false;
                    extraFiles.Remove(extraFiles[i]);
                    break;
                }

            if (addedAtTheEnd)
            {
                // Only need to calculate padding if we are adding at the end
                var dif = newFile.size % 0x200;
                if (dif > 0) newFile.paddingSize = (int)(0x200 - dif);
                if (autoSpareSpace)
                {
                    // If autoSpareSpace is used to add files, we will reserve some extra space as padding
                    // Add 25% by default
                    var spareSpace = newFile.size / 4;
                    spareSpace -= spareSpace % 0x200; // Align the spare space
                    newFile.paddingSize += (int)spareSpace;
                }
            }

            // Add to files list
            files.Add(newFile);

            isDirty = true;

            // Add File Data
            _gpFileStream.Position = newFile.offset;
            sourceStream.Position = 0;
            sourceStream.CopyTo(_gpFileStream);

            if (addedAtTheEnd) _header.FirstFileInfoOffset = newFile.offset + newFile.size + newFile.paddingSize;

            UpdateMD5(newFile); // TODO: optimize this to calculate WHILE we are copying the stream

            // Set output
            pfi = newFile;
            return true;
        }

        /// <summary>
        ///     Adds or replace a given file with name filename with data from sourceStream
        /// </summary>
        /// <param name="filename">The filename used inside the pak</param>
        /// <param name="sourceStream">Source Stream of file to be added</param>
        /// <param name="CreateTime">Time to use as original file creation time</param>
        /// <param name="ModifyTime">Time to use as last modified time</param>
        /// <param name="autoSpareSpace">Enable adding 25% of the sourceStream size as padding when not replacing a file</param>
        /// <param name="pfi">AAPakFileInfo of the newly added or modified file</param>
        /// <returns>Returns true on success</returns>
        public bool AddFileFromStream(string filename, Stream sourceStream, DateTime CreateTime, DateTime ModifyTime,
            bool autoSpareSpace, out AAPakFileInfo pfi)
        {
            pfi = nullAAPakFileInfo;
            if (readOnly) return false;

            var addAsNew = true;
            // Try to find the existing file
            if (GetFileByName(filename, ref pfi))
            {
                var reservedSizeMax = pfi.size + pfi.paddingSize;
                addAsNew = sourceStream.Length > reservedSizeMax;
                // Bugfix: If we have inssuficient space, make sure to delete the old file first as well
                if (addAsNew) DeleteFile(pfi);
            }

            if (addAsNew)
                return AddAsNewFile(filename, sourceStream, CreateTime, ModifyTime, autoSpareSpace, out pfi);
            return ReplaceFile(ref pfi, sourceStream, ModifyTime);
        }

        /// <summary>
        ///     Adds a file into the pakfile with a given name
        /// </summary>
        /// <param name="sourceFileName">Filename of the source file to be added</param>
        /// <param name="asFileName">Filename inside the pakfile to use</param>
        /// <param name="autoSpareSpace">
        ///     When set, tries to pre-allocate extra free space at the end of the file, this will be 25%
        ///     of the filesize if used. If a "deleted file" is used, this parameter is ignored
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
        ///     Convert a stream into a string
        /// </summary>
        /// <param name="stream">Source stream</param>
        /// <returns>String value of the data isnide the stream</returns>
        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        ///     Convert a string into a MemoryStream
        /// </summary>
        /// <param name="src">Source string</param>
        /// <returns>A new MemoryStream that holds the source string's data</returns>
        public static Stream StringToStream(string src)
        {
            var byteArray = Encoding.UTF8.GetBytes(src);
            return new MemoryStream(byteArray);
        }
        
        /// <summary>
        ///     If you want to use custom keys on your pak file, use this function to change the key that is used for
        ///     encryption/decryption of the FAT and header data
        /// </summary>
        /// <param name="newKey"></param>
        public void SetCustomKey(byte[] newKey)
        {
            _header.SetCustomKey(newKey);
        }
        
        /// <summary>
        ///     Reverts back to the original encryption key, this function is also automatically called when closing a file
        /// </summary>
        public void SetDefaultKey()
        {
            _header.SetDefaultKey();
        }
    }
}