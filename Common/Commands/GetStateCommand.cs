using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    [Serializable]
    public class GetStateCommand : Command
    {
        public GetStateCommand()
        {
            CommandName = CommandName.GetState;
        }
    }
}
