using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DifferenceLib
{
    [Serializable]
    public class DiffItem
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] ImageBytes { get; set; }
        
        public DiffItem(Rectangle rectangle, Bitmap bitmap, byte quality)
        {
            X = rectangle.X;
            Y = rectangle.Y;
            Width = rectangle.Width;
            Height = rectangle.Height;
            //ImageBytes = DiffContainer.ImageToByteArray(bitmap);
            ImageBytes = DiffContainer.ImageToByte2(bitmap, quality);
        }

        public DiffItem(KeyValuePair<Rectangle, Bitmap> dataItem, byte quality)
            : this(dataItem.Key, dataItem.Value, quality)
        {}

       
    }
}
