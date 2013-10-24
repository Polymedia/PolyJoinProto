using System;

namespace DifferenceLib
{
    public class DiffManager:IDiffManager
    {


        object IDiffManager.Compress(DiffContainer diffContainer)
        {
            throw new NotImplementedException();
        }

        DiffContainer IDiffManager.Decompress(object compressedDiffContainer)
        {
            throw new NotImplementedException();
        }
    }
}
