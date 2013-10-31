using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    [Serializable]
    public class PaintAddFigureCommand : Command
    {
        public PaintAddFigureCommand(string conferenceId, List<Point> points, Color color)
            : base(conferenceId)
        {
            CommandName = CommandName.PaintAddFigure;

            Points = points;
            Color = color;
        }

        public List<Point> Points;
        public Color Color;
    }
}
