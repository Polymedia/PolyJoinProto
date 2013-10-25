using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DifferenceLib
{
    [Serializable]
    public class DiffItem
    {
        public Rectangle Rectangle;
        public Bitmap Bitmap;

        public DiffItem(Rectangle rectangle, Bitmap bitmap)
        {
            Rectangle = rectangle;
            Bitmap = bitmap;
        }

        public DiffItem(KeyValuePair<Rectangle, Bitmap> dataItem)
            : this(dataItem.Key, dataItem.Value)
        {}
    }
}
