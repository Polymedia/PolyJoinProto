using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    public class DisconnectCommand: Command
    {
        public DisconnectCommand() : base(string.Empty)
        {
            CommandName = CommandName.Disconnect;
        }
    }
}
