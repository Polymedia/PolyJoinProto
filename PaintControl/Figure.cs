using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Painter
{
    public class Figure
    {
        public string Id { get; set; }
        public List<Point> Points{get; set;}
        public Color Color { get; set; }
    }
}
