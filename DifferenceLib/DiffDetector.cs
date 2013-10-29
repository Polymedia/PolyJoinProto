using System.Drawing.Imaging;
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
    public interface IDiffDetector
    {
        DiffContainer GetDiffs(Bitmap newFrame, int compressRate);
        DiffContainer GetDiffs(Bitmap newFrame);
    }

    public class DiffDetector : IDiffDetector
    {
        private Bitmap oldImage;

        //private Stopwatch sw = new Stopwatch();

        Image<Bgr, Byte> Previous_Frame = null;

        public DiffContainer GetDiffs(Bitmap newFrame, int compressRate)
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

        public DiffContainer GetDiffs(Bitmap newFrame)
        {
            return GetDiffs(newFrame, 1);
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

    public class CustomDiffDetector : IDiffDetector
    {
        private Bitmap _oldImage;

        public DiffContainer GetDiffs(Bitmap newFrame, int compressRate)
        {
             if (_oldImage == null || _oldImage.Height != newFrame.Height) 
                 _oldImage = new Bitmap(newFrame.Width, newFrame.Height);

            var diffContainer = new DiffContainer();

            var bitmapData = newFrame.LockBits(new Rectangle(0, 0, newFrame.Width, newFrame.Height), ImageLockMode.ReadOnly,
                              PixelFormat.Format32bppArgb);

            var oldBitmapData = _oldImage.LockBits(new Rectangle(0, 0, _oldImage.Width, _oldImage.Height),
                                                   ImageLockMode.ReadOnly,
                                                   PixelFormat.Format32bppArgb);
                  
            var startY = 0;
            var startX = 0;
            var endY = newFrame.Height - 1;
            var endX = newFrame.Width - 1;

            unsafe
            {

                for (int i = 0; i < newFrame.Width; ++i)
                {
                    var needBreak = false;
                    for (int j = 0; j < newFrame.Height; ++j)
                    {
                        if (
                            *
                            ((int*)
                             ((int) bitmapData.Scan0 + bitmapData.Stride*j + bitmapData.Stride/bitmapData.Width*i)) !=
                            *
                            ((int*)
                             ((int)oldBitmapData.Scan0 + oldBitmapData.Stride * j + oldBitmapData.Stride / oldBitmapData.Width * i)))
                        {
                            startY = j;
                            needBreak = true;
                            break;
                        }
                    }
                    if (needBreak)
                        break;
                }

                for (int i = 0; i < newFrame.Width; ++i)
                {
                    var needBreak = false;
                    for (int j = newFrame.Height - 1; j >= 0; --j)
                    {
                        if (
                            *
                            ((int*)
                             ((int)bitmapData.Scan0 + bitmapData.Stride * j + bitmapData.Stride / bitmapData.Width * i)) !=
                            *
                            ((int*)
                             ((int)oldBitmapData.Scan0 + oldBitmapData.Stride * j + oldBitmapData.Stride / oldBitmapData.Width * i)))
                        {
                            endY = j;
                            needBreak = true;
                            break;
                        }
                    }
                    if (needBreak)
                        break;
                }

                for (int j = 0; j < newFrame.Height; ++j)
                {
                    var needBreak = false;
                    for (int i = 0; i < newFrame.Width; ++i)
                    {
                        if (
                            *
                            ((int*)
                             ((int)bitmapData.Scan0 + bitmapData.Stride * j + bitmapData.Stride / bitmapData.Width * i)) !=
                            *
                            ((int*)
                             ((int)oldBitmapData.Scan0 + oldBitmapData.Stride * j + oldBitmapData.Stride / oldBitmapData.Width * i)))
                        {
                            startX = i;
                            needBreak = true;
                            break;
                        }
                    }
                    if (needBreak)
                        break;
                }

                for (int j = 0; j < newFrame.Height; ++j)
                {
                    var needBreak = false;
                    for (int i = newFrame.Width - 1; i >= 0; --i)
                    {
                        if (
                            *
                            ((int*)
                             ((int)bitmapData.Scan0 + bitmapData.Stride * j + bitmapData.Stride / bitmapData.Width * i)) !=
                            *
                            ((int*)
                             ((int)oldBitmapData.Scan0 + oldBitmapData.Stride * j + oldBitmapData.Stride / oldBitmapData.Width * i)))
                        {
                            endX = i;
                            needBreak = true;
                            break;
                        }
                    }
                    if (needBreak)
                        break;
                }
            }

            if (startY - 10 >= 0)
                startY -= 10;
            if (endY + 10 < newFrame.Height)
                endY += 10;
            if (startX - 10 >= 0)
                startX -= 10;
            if (endX + 10 < newFrame.Width)
                endX += 10;

            newFrame.UnlockBits(bitmapData);
            _oldImage.UnlockBits(oldBitmapData);

            var diffBitmap = newFrame.Clone(new Rectangle(startX, startY, endX - startX + 1, endY - startY + 1), newFrame.PixelFormat);

            diffContainer.Data.Add(new Rectangle(startX, startY, diffBitmap.Width, diffBitmap.Height), diffBitmap);
            _oldImage = newFrame;

            return diffContainer;
        }

        public DiffContainer GetDiffs(Bitmap newFrame)
        {
            return GetDiffs(newFrame, 1);
        }
    }
}
