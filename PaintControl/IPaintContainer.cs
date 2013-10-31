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
        int AddFigure(List<Point> figurePoints, Color color);
        void AddPointToFogure(Point p, int figureId);
        int RemoveFigure(double x, double y);
        Figure GetFigureById(int id);
        double GetLineThickness();
    }
}
