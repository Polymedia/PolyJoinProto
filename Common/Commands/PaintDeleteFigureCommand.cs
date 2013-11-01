using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    [Serializable]
    public class PaintDeleteFigureCommand : Command
    {
        public PaintDeleteFigureCommand(string conferenceId, string figureId)
            : base(conferenceId)
        {
            CommandName = CommandName.PaintDeleteFigure;

            FigureId = figureId;
        }

        public string FigureId;
    }
}
