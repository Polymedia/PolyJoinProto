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
        public PaintAddFigureCommand(string conferenceId, string figureId, List<Point> points, Color color)
            : base(conferenceId)
        {
            CommandName = CommandName.PaintAddFigure;

            FigureId = figureId;
            Points = points;
            Color = color;
        }

        public string FigureId;
        public List<Point> Points;
        public Color Color;
    }
}
