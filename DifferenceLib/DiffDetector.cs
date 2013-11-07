using System.Drawing.Imaging;
using AForge.Imaging;
using AForge.Imaging.Filters;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using OpenCvSharp;
using System.Linq;

namespace DifferenceLib
{
    public interface IDiffDetector
    {
        DiffContainer GetDiffs(Bitmap newFrame, int compressRate);
        DiffContainer GetDiffs(Bitmap newFrame);
    }

    public class DiffDetector : IDiffDetector
    {
        private Bitmap _oldImage;


        //private Stopwatch sw = new Stopwatch();

        Image<Bgr, Byte> Previous_Frame = null;

        public DiffContainer GetDiffs(Bitmap newFrame, int compressRate)
        {
            //sw.Restart();

            Image<Bgr, Byte> Frame = new Image<Bgr, byte>(newFrame);

            if (_oldImage == null || _oldImage.Height != newFrame.Height) _oldImage = new Bitmap(newFrame.Width, newFrame.Height);
            Previous_Frame = new Image<Bgr, byte>(_oldImage);
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
            Graphics g = Graphics.FromImage(_oldImage);

            foreach (var p in diffs.Data)
            {
                g.DrawImage(p.Value, p.Key);
            }

            Previous_Frame = new Image<Bgr, byte>(_oldImage);
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
        private byte _jpegQuality;

        public CustomDiffDetector()
        {

        }

        public CustomDiffDetector(byte quality)
        {
            _jpegQuality = quality;
        }

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
            var inc = 4;

            unsafe
            {
                var identical = true;
                for (int j = 0; j < newFrame.Height - inc; j += inc)
                {
                    var needBreak = false;
                    for (int i = 0; i < newFrame.Width - inc; i += inc)
                    {
                        if (
                            *
                            ((int*)
                             ((int)bitmapData.Scan0 + bitmapData.Stride * j + bitmapData.Stride / bitmapData.Width * i)) !=
                            *
                            ((int*)
                             ((int)oldBitmapData.Scan0 + oldBitmapData.Stride * j + oldBitmapData.Stride / oldBitmapData.Width * i)))
                        {
                            startY = j;
                            identical = false;
                            needBreak = true;
                            break;
                        }
                    }
                    if (needBreak)
                        break;
                }

                if (identical)
                {
                    newFrame.UnlockBits(bitmapData);
                    _oldImage.UnlockBits(oldBitmapData);
                    return diffContainer;
                }

                for (int j = newFrame.Height - 1; j >= startY + inc; j -= inc)
                {
                    var needBreak = false;
                    for (int i = 0; i < newFrame.Width - inc; i += inc)
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

                for (int i = 0; i < newFrame.Width - inc; i += inc)
                {
                    var needBreak = false;
                    for (int j = startY; j <= endY - inc; j += inc)
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

                for (int i = newFrame.Width - 1; i >= startX + inc; i -= inc)
                {
                    var needBreak = false;
                    for (int j = startY; j <= endY - inc; j += inc)
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
            else
                startY = 0;
            if (endY + 10 < newFrame.Height)
                endY += 10;
            else
                endY = newFrame.Height - 1;
            if (startX - 10 >= 0)
                startX -= 10;
            else
                startX = 0;
            if (endX + 10 < newFrame.Width)
                endX += 10;
            else
                endX = newFrame.Width - 1;


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

    public class DiffDetectorOpenCvSharp : IDiffDetector
    {
        private Bitmap _oldImage;

        IplImage Previous_Frame = null;



        public DiffContainer GetDiffs(Bitmap newFrame, int compressRate)
        {
            IplImage Frame = BitmapConverter.ToIplImage(newFrame);

            if (_oldImage == null || _oldImage.Height != newFrame.Height) _oldImage = new Bitmap(newFrame.Width, newFrame.Height);
            IplImage Previous_Frame = BitmapConverter.ToIplImage(_oldImage);

            IplImage Difference = new IplImage(Previous_Frame.Width, Previous_Frame.Height, BitDepth.U8, 4);

            Cv.AbsDiff(Frame, Previous_Frame, Difference);

            IplImage gray = new IplImage(Previous_Frame.Width, Previous_Frame.Height, BitDepth.U8, 1);

            Cv.CvtColor(Difference, gray, ColorConversion.RgbToGray);

            DiffContainer container = new DiffContainer();

            CvSeq<CvPoint> contours;
            CvSeq<CvPoint> contour;

            using (CvMemStorage storage = new CvMemStorage())
                for (int i = gray.FindContours(storage, out contours, CvContour.SizeOf, ContourRetrieval.External, ContourChain.ApproxSimple); contours != null; contours = contours.HNext)
                {

                    contour = Cv.ApproxPoly(contours, CvContour.SizeOf, storage, ApproxPolyMethod.DP, 0.2);

                    if (contour.ContourPerimeter() > 100
                        &&contour.ContourPerimeter()<10000
                        )
                    {
                        var r = contour.BoundingRect();


                            var im = Frame.GetSubImage(r);

                        

                            Rectangle rect = new Rectangle(r.X, r.Y, r.Width, r.Height);


                            rect.X -= 1;
                            rect.Y -= 1;
                            rect.Width += 2;
                            rect.Height += 2;


                            var j = BitmapConverter.ToBitmap(im);
                            container.Data.Add(rect, j);

                    }

                }
            UpdateOldFrame(container);


            return container;
        }

        public DiffContainer GetDiffs(Bitmap newFrame)
        {
            return GetDiffs(newFrame, 1);
        }

        private void UpdateOldFrame(DiffContainer diffs)
        {
            Graphics g = Graphics.FromImage(_oldImage);

            foreach (var p in diffs.Data)
            {
                g.DrawImage(p.Value, p.Key);
            }

            Previous_Frame = BitmapConverter.ToIplImage(_oldImage);
        }



    }
}
