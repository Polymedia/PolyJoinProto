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
        Diff,
        Disconnect,
        PaintAddFigure,
        PaintDeleteFigure
    }

    [Serializable]
    public abstract class Command
    {
        public CommandName CommandName { get; set; }
        public string ConferenceId { get; set; }

        public Command(string conferenceId)
        {
            ConferenceId = conferenceId;
        }

        [NonSerialized] public ConnectionWrapper SenderConnection;
    }
}
