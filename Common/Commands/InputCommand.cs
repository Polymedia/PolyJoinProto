using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    [Serializable]
    public class InputCommand: Command
    {
        public InputCommand(string conferenceId) : base(conferenceId)
        {
            CommandName = CommandName.Input;
        }

        public int MouseX { get; set; }
        public int MouseY { get; set; }
    }
}
