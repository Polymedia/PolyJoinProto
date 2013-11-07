using System;
using Polymedia.PolyJoin.Common;

namespace Common.Commands
{
    [Serializable]
    public class ControlAccessCommand : Command
    {
        private readonly string _clientId;
        private readonly bool _isAllowed;
        private string _presenterId;

        public ControlAccessCommand(string conferenceId, string presenterId, string clientId, bool isAllowed) : base(conferenceId)
        {
            CommandName = CommandName.ControlAccess;

            _clientId = clientId;
            _isAllowed = isAllowed;
            _presenterId = presenterId;
        }

        public string ClientId
        {
            get { return _clientId; }
        }

        public string PresenterId
        {
            get { return _presenterId; }
            set { _presenterId = value; }
        }

        public bool IsAllowed
        {
            get { return _isAllowed; }
        }
    }
}
