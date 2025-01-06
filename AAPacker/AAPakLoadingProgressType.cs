using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAPacker;

/// <summary>
/// Pak progress event
/// </summary>
/// <param name="sender"></param>
/// <param name="progressType"></param>
/// <param name="step"></param>
/// <param name="maximum"></param>
public delegate void AAPakNotify(AAPak sender, AAPakLoadingProgressType progressType, int step, int maximum);

/// <summary>
/// Type of progress updates
/// </summary>
public enum AAPakLoadingProgressType
{
    /// <summary>
    /// Opening the pak file
    /// </summary>
    OpeningFile,
    /// <summary>
    /// Reading the pak file header
    /// </summary>
    ReadingHeader,
    /// <summary>
    /// Writing the pak file header
    /// </summary>
    WritingHeader,
    /// <summary>
    /// Reading the FAT data
    /// </summary>
    ReadingFAT,
    /// <summary>
    /// Writing the FAT data
    /// </summary>
    WritingFAT,
    /// <summary>
    /// Closing the pak file
    /// </summary>
    ClosingFile,
    /// <summary>
    /// Generating virtual directories
    /// </summary>
    GeneratingDirectories,
}