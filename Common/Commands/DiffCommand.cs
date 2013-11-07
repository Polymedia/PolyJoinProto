using System;
using DifferenceLib;
using Polymedia.PolyJoin.Common;

namespace Common.Commands
{
    [Serializable]
    public class DiffCommand: Command
    {
        public DiffCommand(string conferenceId) : base(conferenceId)
        {
            CommandName = CommandName.Diff;
        }

        public DiffItem DiffItem;
    }
}
