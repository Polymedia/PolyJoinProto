using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    public class PaintDeleteFigureCommand:Command
    {
        public PaintDeleteFigureCommand(string conferenceId, int id):base(conferenceId)
        {
            CommandName = CommandName.PaintDeleteFigure;

            Id = id;
        }

        public int Id;
    }
}
