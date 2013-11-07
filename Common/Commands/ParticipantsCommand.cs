using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    [Serializable]
    public class ParticipantsCommand: Command
    {
        public ParticipantsCommand(string conferenceId) : base(conferenceId)
        {
            CommandName = CommandName.Participants;
            Participants = new List<Participant>();
        }

        public List<Participant> Participants { get; set; }
    }

    [Serializable]
    public class Participant
    {
        public string Id { get; set; }
        public int BrushArgb { get; set; }
        public string Name { get; set; }
        public bool IsPresenter { get; set; }
        public bool IsInputController { get; set; }
    }
}
