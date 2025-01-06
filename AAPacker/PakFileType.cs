namespace AAPacker;

/// <summary>
/// How the pak file is opened
/// </summary>
public enum PakFileType
{
    /// <summary>
    /// Loaded from using a PakFileFormat definition
    /// </summary>
    Reader,
    /// <summary>
    /// Loaded using default settings
    /// </summary>
    Classic,
    /// <summary>
    /// Loaded from CSV file
    /// </summary>
    Csv,
}