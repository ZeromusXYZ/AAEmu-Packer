namespace AAPacker
{
    public enum AAPakFileHeaderElement
    {
        AnyByte,
        NullByte,
        Header,
        FilesCount,
        ExtraFilesCount
    }
    
    public class AAPakFileFormatReader
    {
        /// <summary>
        ///     Default AES128 key used by XLGames for ArcheAge as encryption key for header and fileInfo data
        /// </summary>
        public static readonly byte[] XlGamesKey = { 0x32, 0x1F, 0x2A, 0xEE, 0xAA, 0x58, 0x4A, 0xB4, 0x9A, 0x6C, 0x9E, 0x09, 0xD5, 0x9E, 0x9C, 0x6F };

        public string ReaderName = "Default";
        public byte[] HeaderEncryptionKey = XlGamesKey; 
        public byte[] HeaderBytes = { 0x57, 0x49, 0x42, 0x4F }; // W I B O = 0x57 0x49 0x42 0x4F

        public List<AAPakFileHeaderElement> ReadOrder = new()
        {
            AAPakFileHeaderElement.Header,
            AAPakFileHeaderElement.NullByte,
            AAPakFileHeaderElement.NullByte,
            AAPakFileHeaderElement.NullByte,
            AAPakFileHeaderElement.NullByte,
            AAPakFileHeaderElement.FilesCount,
            AAPakFileHeaderElement.ExtraFilesCount,
            AAPakFileHeaderElement.NullByte,
            AAPakFileHeaderElement.NullByte,
            AAPakFileHeaderElement.NullByte,
            AAPakFileHeaderElement.NullByte
        };

    }
}
