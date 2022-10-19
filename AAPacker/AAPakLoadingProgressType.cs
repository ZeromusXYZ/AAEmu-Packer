using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAPacker;

public delegate void AAPakNotify(AAPak sender, AAPakLoadingProgressType progressType, int step, int maximum);

public enum AAPakLoadingProgressType
{
    OpeningFile,
    ReadingHeader,
    WritingHeader,
    ReadingFAT,
    WritingFAT,
    ClosingFile,
    GeneratingDirectories,
}