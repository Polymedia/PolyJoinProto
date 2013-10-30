using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    [Serializable]
    public class QueryStateCommand : Command
    {
        public QueryStateCommand(string conferenceId, int width, int height):base(conferenceId)
        {
            CommandName = CommandName.GetState;

            Width = width;
            Height = height;
        }

        public int Width;
        public int Height;
    }
}
