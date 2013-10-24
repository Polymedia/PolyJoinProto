using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    [Serializable]
    public class StateCommand: Command
    {
        public StateCommand()
        {
            CommandName = CommandName.State;
        }
        
        public int ConferenceId { get; set; }
        public bool IsPresenter { get; set; }
    }
}
