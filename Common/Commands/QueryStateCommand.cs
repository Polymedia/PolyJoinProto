using System;
using Polymedia.PolyJoin.Common;

namespace Common.Commands
{
    [Serializable]
    public class QueryStateCommand : Command
    {
        public QueryStateCommand(string conferenceId, int width, int height, string clientName):base(conferenceId)
        {
            CommandName = CommandName.GetState;

            Width = width;
            Height = height;
            ClientName = clientName;
        }

        public int Width;
        public int Height;
        public string ClientName;
    }
}
