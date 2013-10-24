using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    [Serializable]
    public enum CommandName
    {
        GetState,
        State,
        Diff
    }

    [Serializable]
    public class Command
    {
        public CommandName CommandName { get; set; }

        [NonSerialized] public ConnectionWrapper SenderConnection;
    }
}
