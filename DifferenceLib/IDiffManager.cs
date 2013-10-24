using System;

namespace DifferenceLib
{
    interface IDiffManager
    {
        object Compress(DiffContainer diffContainer);

        DiffContainer Decompress(object compressedDiffContainer);



    }
}
