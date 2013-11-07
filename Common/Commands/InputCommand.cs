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

        public MouseInput MouseInput { get; set; }
    }

    [Serializable]
    public class MouseInput
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool LeftButton { get; set; }
        public bool RightButton { get; set; }

        public MouseInputEnum MouseInputType { get; set; }

        public enum MouseInputEnum
        {
            Move,
            Click,
            Up,
            Down
        }
    }
}
