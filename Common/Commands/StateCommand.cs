using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    [Serializable]
    public class StateCommand : Command
    {
        public StateCommand(string conferenceId):base(conferenceId)
        {
            CommandName = CommandName.State;
        }

        public bool IsPresenter { get; set; }
        public int PresenterWidth { get; set; }
        public int PresenterHeight { get; set; }
    }
}
