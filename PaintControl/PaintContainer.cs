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
        private List<Figure> _figures;
        private Bitmap _image;
        
        public PaintContainer(int width, int height)
        {
            Init(width, height);
        }

        private void Init(int width, int height)
        {
            _imageWidth = width;
            _imageHeight = height;
            _figures = new List<Figure>();

            _image = new Bitmap(_imageWidth, _imageHeight);
        }

        public Bitmap Image
        {
            get { return _image; }
            set 
            {
                _image = value;
            }
        }

        public void AddFigure(string id, List<Point> figurePoints, Color color)
        {
            var f = new Figure
            {
                Id=id,
                Points = figurePoints,
                Color = color
            };

            _figures.Add(f);

            RenderFigure(f);
        }

        public void AddPointToFigure(Point p, string figureId)
        {
            var figures = _figures.Where(F => F.Id == figureId).ToList();

            if (figures.Count == 0)
                return;

            var f = figures.First();

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

        public string RemoveFigure(double x, double y)
        {
            foreach (var f in _figures)
            {
                for (int i = 0; i < f.Points.Count - 1; i++)
                {
                    if (PointCloseToSegment(x, y, f.Points[i].X, f.Points[i].Y, f.Points[i + 1].X, f.Points[i + 1].Y))
                    {
                        var id = f.Id;
                        return RemoveFigure(id);
                    }
                }
            }
            return string.Empty;
        }

        public string RemoveFigure(string id)
        {
            var figures = _figures.Where(f => f.Id==id).ToList();
            if (figures.Count()>0)
            {
                foreach(var f in figures)
                _figures.Remove(f);
                _image = new Bitmap(_imageWidth, _imageHeight);
                RenderFigures();
                return id;
            }
            return string.Empty;
        }

        public Figure GetFigureById(string id)
        {
            var figs = _figures.Where(f => f.Id.Equals(id)).ToList();
            if (figs.Count()>0)
            {
                return figs.First();
            } return null;
        }

        private bool PointCloseToSegment(double pointX, double pointY, double x1, double y1, double x2, double y2)
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
            return _imageWidth / 100;
        }

        private void RenderFigure(Figure f)
        {
            Pen p = new Pen(f.Color, (float)GetLineThickness());
            p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            p.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            using (Graphics g = Graphics.FromImage(_image))
            {
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
        }
        
        private void RenderFigures()
        {
            foreach (var f in _figures)
            {
                RenderFigure(f);
            }
        }

    }
}
