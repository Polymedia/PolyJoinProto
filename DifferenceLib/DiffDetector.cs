using AForge.Imaging;
using AForge.Imaging.Filters;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace DifferenceLib
{
    public class DiffDetector
    {
        private Bitmap oldImage;

        //private Stopwatch sw = new Stopwatch();

        Image<Bgr, Byte> Previous_Frame = null;

        public DiffContainer GetDiffs(Bitmap newFrame, int CompressRate = 1)
        {
            //sw.Restart();

            Image<Bgr, Byte> Frame = new Image<Bgr, byte>(newFrame);

            if (oldImage == null || oldImage.Height != newFrame.Height) oldImage = new Bitmap(newFrame.Width, newFrame.Height);
            Previous_Frame = new Image<Bgr, byte>(oldImage);
            Image<Bgr, Byte> Difference;

            Difference = Previous_Frame.AbsDiff(Frame);

            Image<Gray, Byte> gray = Difference.Convert<Gray, Byte>().PyrDown().PyrUp();

            DiffContainer container = new DiffContainer();
            List<MovedObject> movedObjects = new List<MovedObject>();

            using (MemStorage storage = new MemStorage())
                for (Contour<System.Drawing.Point> contours = gray.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, RETR_TYPE.CV_RETR_EXTERNAL, storage);
                        contours != null; contours = contours.HNext)
                {
                    Contour<Point> contour = contours.ApproxPoly(contours.Perimeter * 0.001);
                    if (contour.Area > 100)
                      //  if (contour.Total > 5)
                        {
                            Rectangle rect = contour.BoundingRectangle;

                            rect.X -= 1;
                            rect.Y -= 1;
                            rect.Width += 2;
                            rect.Height += 2;

                                var part = Frame.GetSubRect(rect);

                                //var j = ImageResizer.ImageBuilder.Current.Build(part.ToBitmap(),
                                //    new ImageResizer.ResizeSettings(
                                //        "maxwidth=" + part.Width / CompressRate +
                                //        "&maxheight=" + part.Height +
                                //        "&format=jpg&quality=20"
                                //        ));

                                var j = part.ToBitmap();

                                container.Data.Add(rect, j);


                        }
                }


            UpdateOldFrame(container);
            //sw.Stop();
            //container.Elapsed = sw.ElapsedMilliseconds;

            return container;
        }


        private void UpdateOldFrame(DiffContainer diffs)
        {
            Graphics g = Graphics.FromImage(oldImage);

            foreach (var p in diffs.Data)
            {
                g.DrawImage(p.Value, p.Key);
            }

            Previous_Frame = new Image<Bgr, byte>(oldImage);
        }




        UnmanagedImage prevousFrame = null;
        BlobCounter bc = new BlobCounter();
        public DiffContainer GetDiffsAforge(Bitmap newFrame)
        {
            if (prevousFrame == null) prevousFrame = UnmanagedImage.FromManagedImage(new Bitmap(@"d:\1.png"));

            List<Bitmap> bitmaps = new List<Bitmap>();
            ThresholdedDifference filter = new ThresholdedDifference(20);

            filter.OverlayImage = newFrame;

            var res = filter.Apply(prevousFrame);

            bc.ProcessImage(res);

            Rectangle[] rects = bc.GetObjectsRectangles();

          

            foreach (Rectangle rect in rects)
            {
                if (rect.Width > 4 || rect.Height > 4)
                {
                    Crop crop = new Crop(rect);

                    bitmaps.Add(crop.Apply(prevousFrame).ToManagedImage());
                }
                //    Drawing.Rectangle(prevousFrame, rect, Color.Red);
            }


            //Parallel.ForEach(rects, rect =>
            //{
            //    if (rect.Width > 2 && rect.Height > 2)
            //    {
            //        Crop crop = new Crop(rect);

            //        bitmaps.Add(crop.Apply(prevousFrame));
            //    }
            //    //  Drawing.Rectangle(prevousFrame, rect, Color.Red);
            //}
            //);

            return new DiffContainer();// { temp = prevousFrame.ToManagedImage() };

        }
    }

   
}
