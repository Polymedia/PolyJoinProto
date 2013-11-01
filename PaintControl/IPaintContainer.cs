using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;

namespace Painter
{
    interface IPaintContainer
    {
        Bitmap Image { get; }
        void AddFigure(string id, List<Point> figurePoints, Color color);
        void AddPointToFigure(Point p, string figureId);
        string RemoveFigure(double x, double y);
        string RemoveFigure(string id);
        Figure GetFigureById(string id);
        double GetLineThickness();
    }
}
