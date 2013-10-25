using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DifferenceLib;
using Polymedia.PolyJoin.Common;

namespace Polymedia.PolyJoin.Common
{
    [Serializable]
    public class DiffCommand: Command
    {
        public DiffCommand()
        {
            CommandName = CommandName.Diff;
        }

        public DiffItem DiffItem;
    }
}
