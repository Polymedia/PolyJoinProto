using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Painter
{
    public interface IDrawingForm
    {
         event EventHandler<MouseEventArgs> DrawingMouseDown;
         event EventHandler<MouseEventArgs> DrawingMouseUp;
         event EventHandler<MouseEventArgs> DrawingMouseMove;
         event EventHandler<MouseEventArgs> DrawingMouseClick;

        Image Image { get; set; }
    }
}
