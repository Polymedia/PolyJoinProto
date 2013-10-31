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
        int Width { get; }
        int Height {get; }
        Bitmap Image { get; }
        int AddFigure(List<Point> figurePoints, Color color);
        void AddPointToFogure(Point p, int figureId);
        void RemoveFigure(double x, double y);
        double GetLineThickness();
    }
}
