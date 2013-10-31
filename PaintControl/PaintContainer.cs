using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Painter
{
    public class PaintContainer : IPaintContainer
    {
        private int _imageWidth;
        private int _imageHeight;
        private Dictionary<int, Figure> _figuresDictionary;
        private Bitmap _image;
        int _lastFigureId = 0;

        public PaintContainer(int width, int height)
        {
            Init(width, height);
        }

        private void Init(int width, int height)
        {
            _imageWidth = width;
            _imageHeight = height;
            _figuresDictionary = new Dictionary<int, Figure>();

            _image = new Bitmap(_imageWidth, _imageHeight);
        }

        public int Width
        {
            get { return _imageWidth; }
            private set 
            { 
                _imageWidth = value;
                Init(value, Height);
            }
        }

        public int Height
        {
            get { return _imageHeight; }
            private set 
            { 
                _imageHeight = value;
                Init(Width, value);
            }
        }

        public Bitmap Image
        {
            get { return _image; }
            set 
            {
                _image = value;
            }
        }

        public int AddFigure(List<Point> figurePoints, Color color)
        {
            var f = new Figure
            {
                Points = figurePoints,
                Color = color
            };

            _lastFigureId++;
            _figuresDictionary.Add(_lastFigureId, f);

            RenderFigure(f);
            return _lastFigureId;
        }

        public void AddPointToFogure(Point p, int figureId)
        {
            var f = _figuresDictionary[figureId];
            if (f.Points.Count == 0)
            {
                f.Points.Add(p);

            }
            else
            {
                var oldLastPoint = f.Points.LastOrDefault();

                f.Points.Add(p);

                Graphics g = Graphics.FromImage(_image);
                Pen pen = new Pen(f.Color, (float)GetLineThickness());
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                g.DrawLine(pen, oldLastPoint, p);
            }
        }

        public void RemoveFigure(double x, double y)
        {
            int figureId = -1;
            foreach (var f in _figuresDictionary)
            {
                for (int i = 0; i < f.Value.Points.Count - 1; i++)
                {
                    if (PointCloseToLineSegment(x, y, f.Value.Points[i].X, f.Value.Points[i].Y, f.Value.Points[i + 1].X, f.Value.Points[i + 1].Y))
                    {
                        _figuresDictionary.Remove(f.Key);
                        _image = new Bitmap(_imageWidth, _imageHeight);
                        RenderFigures();
                        return;
                    }
                }
            }
        }

        private bool PointCloseToLineSegment(double pointX, double pointY, double x1, double y1, double x2, double y2)
        {
            var t = GetLineThickness()*2;
            if ((pointX > x1 - t && pointX < x2 + t || pointX > x2 - t && pointX < x1 + t) && (pointY > y1 - t && pointY < y2 + t || pointY > y2 - t && pointY < y1 + t))
            {
                if (x1 == x2 && y1 == y2)
                    return Math.Sqrt((pointX - x1) * (pointX - x1) + (pointY - y1) * (pointY - y1)) < t;
                else if (x1 == x2)
                    return Math.Abs(pointX - x1) < t;
                else if (y1 == y2)
                    return Math.Abs(pointY - y1) < t;
                else
                    return Math.Abs((pointX - x1) / (x2 - x1) - (pointY - y1) / (y2 - y1)) < t;
            }
            return false;
        }

        public double GetLineThickness()
        {
            return Width / 100;
        }

        private void RenderFigure(Figure f)
        {
            Pen p = new Pen(f.Color, (float)GetLineThickness());
            p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            p.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            Graphics g = Graphics.FromImage(_image);
            var points = f.Points.ToArray();
            if (points.Count() == 1)
                g.DrawLine(p,
                    new Point(points.First().X, points.First().Y),
                    new Point(points.First().X, points.First().Y));
            else
            {
                for (int i = 0; i < points.Length - 1; i++)
                {
                    g.DrawLine(p,
                        new Point(points[i].X, points[i].Y),
                        new Point(points[i + 1].X, points[i + 1].Y));
                }
            }
        }
        
        private void RenderFigures()
        {
            foreach (var f in _figuresDictionary)
            {
                RenderFigure(f.Value);
            }
        }

    }
}
