using System;
using Polymedia.PolyJoin.Common;

namespace Common.Commands
{
    [Serializable]
    public class RequestControlCommand : Command
    {
        private readonly string _clientId;
        private readonly bool _isAllowed;

        public RequestControlCommand(string conferenceId, string clientId, bool isAllowed)
            : base(conferenceId)
        {
            _clientId = clientId;
            _isAllowed = isAllowed;
            CommandName = CommandName.RequestControl;
        }

        public string ClientId
        {
            get { return _clientId; }
        }

        public bool IsAllowed
        {
            get { return _isAllowed; }
        }
    }
}
