using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DifferenceLib
{
    [Serializable]
    public class DiffContainer
    {
        public long Elapsed { get; set; }

        public Dictionary<Rectangle, Bitmap> Data = new Dictionary<Rectangle, Bitmap>();
        static ImageCodecInfo _jgpEncoder;
        public static byte Quality;

        public static void Init(byte jpegQuality)
        {
            Quality = jpegQuality;
            _jgpEncoder = GetEncoder(ImageFormat.Jpeg);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static KeyValuePair<Rectangle, Bitmap> FromBytes(byte[] data)
        {
            var result = new KeyValuePair<Rectangle, Bitmap>();

            try
            {
                var imageBytes = new byte[data.Length - 16];

                Array.Copy(data, 16, imageBytes, 0, data.Length - 16);

                int x = BitConverter.ToInt32(data, 0);
                int y = BitConverter.ToInt32(data, 4);
                int width = BitConverter.ToInt32(data, 8);
                int height = BitConverter.ToInt32(data, 12);

                Bitmap bmp;
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    PngBitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                    BitmapSource bitmapSource = decoder.Frames[0];
                    bmp = GetBitmap(bitmapSource);
                }

                return new KeyValuePair<Rectangle, Bitmap>(new Rectangle(x, y, width, height), bmp);
            }
            catch
            {

            }
            return result;
        }

        private static Bitmap GetBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(
                source.PixelWidth,
                source.PixelHeight,
                PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(
                new Rectangle(System.Drawing.Point.Empty, bmp.Size),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppPArgb);
            source.CopyPixels(
                Int32Rect.Empty,
                data.Scan0,
                data.Height * data.Stride,
                data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        public static Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                //PngBitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                //BitmapSource bitmapSource = decoder.Frames[0];
                //return GetBitmap(bitmapSource);

                Image image = Image.FromStream(new MemoryStream(byteArray));
                return image;
            }
        }

        public static byte[] ImageToByteArray(Image imageIn, byte quality)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var encoderParameters = new EncoderParameters(1);
                Encoder myEncoder = Encoder.Quality;
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, Int64.Parse(quality.ToString()));
                encoderParameters.Param[0] = myEncoderParameter;

                imageIn.Save(ms, _jgpEncoder, encoderParameters);

                //imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static byte[] ImageToByte2(Image img, byte quality)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                var encoderParameters = new EncoderParameters(1);
                Encoder myEncoder = Encoder.Quality;
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, Int64.Parse(quality.ToString()));
                encoderParameters.Param[0] = myEncoderParameter;

                img.Save(stream, _jgpEncoder, encoderParameters);

                //img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Close();
                byteArray = stream.ToArray();
            }

            return byteArray;
        }





        public static Dictionary<Rectangle, Bitmap> Split(KeyValuePair<Rectangle, Bitmap> originalImage, int maxSize, byte quality)
        {
            var result = new Dictionary<Rectangle, Bitmap>();

            if (ImageToByteArray(originalImage.Value, quality).Length <= maxSize)
            {
                result.Add(originalImage.Key, originalImage.Value);
                return result;
            }

            Bitmap firstBitmap;
            Bitmap secondBitmap;

            Rectangle r1;
            Rectangle r2;

            if (originalImage.Value.Width > originalImage.Value.Height)
            {
                //split vertically

                AForge.Imaging.Filters.Crop crop1 = new AForge.Imaging.Filters.Crop(new Rectangle(0, 0, originalImage.Value.Width / 2, originalImage.Value.Height));
                AForge.Imaging.Filters.Crop crop2 = new AForge.Imaging.Filters.Crop(new Rectangle(originalImage.Value.Width / 2, 0, originalImage.Value.Width / 2, originalImage.Value.Height));

                firstBitmap = crop1.Apply(originalImage.Value);
                secondBitmap = crop2.Apply(originalImage.Value);

                //firstBitmap = new Bitmap(originalImage.Value.Width/2, originalImage.Value.Height);
                //secondBitmap = new Bitmap(originalImage.Value.Width/2, originalImage.Value.Height);

                //Graphics g1 = Graphics.FromImage(firstBitmap);
                //g1.DrawImage(originalImage.Value, 0, 0,
                //             new Rectangle(0, 0, originalImage.Value.Width/2, originalImage.Value.Height),
                //             GraphicsUnit.Pixel);

                //Graphics g2 = Graphics.FromImage(secondBitmap);
                //g2.DrawImage(originalImage.Value, 0, 0,
                //             new Rectangle(originalImage.Value.Width / 2, 0, originalImage.Value.Width / 2, originalImage.Value.Height),
                //             GraphicsUnit.Pixel);

                r1 = new Rectangle(originalImage.Key.X, originalImage.Key.Y, originalImage.Key.Width / 2,
                                   originalImage.Key.Height);

                r2 = new Rectangle(originalImage.Key.X + originalImage.Key.Width / 2, originalImage.Key.Y, originalImage.Key.Width / 2,
                                   originalImage.Key.Height);
            }
            else
            {
                //split horizontally


                AForge.Imaging.Filters.Crop crop1 = new AForge.Imaging.Filters.Crop(new Rectangle(0, 0, originalImage.Value.Width, originalImage.Value.Height / 2));
                AForge.Imaging.Filters.Crop crop2 = new AForge.Imaging.Filters.Crop(new Rectangle(0, originalImage.Value.Height / 2, originalImage.Value.Width, originalImage.Value.Height / 2));

                firstBitmap = crop1.Apply(originalImage.Value);
                secondBitmap = crop2.Apply(originalImage.Value);


                //firstBitmap = new Bitmap(originalImage.Value.Width, originalImage.Value.Height/2);
                //secondBitmap = new Bitmap(originalImage.Value.Width, originalImage.Value.Height/2);

                //Graphics g1 = Graphics.FromImage(firstBitmap);
                //g1.DrawImage(originalImage.Value, 0, 0,
                //             new Rectangle(0, 0, originalImage.Value.Width, originalImage.Value.Height/2),
                //             GraphicsUnit.Pixel);

                //Graphics g2 = Graphics.FromImage(secondBitmap);
                //g2.DrawImage(originalImage.Value, 0, 0,
                //             new Rectangle(0, originalImage.Value.Height/2, originalImage.Value.Width, originalImage.Value.Height/2),
                //             GraphicsUnit.Pixel);

                r1 = new Rectangle(originalImage.Key.X, originalImage.Key.Y, originalImage.Key.Width,
                                   originalImage.Key.Height / 2);

                r2 = new Rectangle(originalImage.Key.X, originalImage.Key.Y + originalImage.Key.Height / 2, originalImage.Key.Width,
                                   originalImage.Key.Height / 2);
            }


            return Split(new KeyValuePair<Rectangle, Bitmap>(r1, firstBitmap), maxSize, quality).Concat(
                Split(new KeyValuePair<Rectangle, Bitmap>(r2, secondBitmap), maxSize, quality)).ToDictionary(p => p.Key, p => p.Value);
        }

        public static Dictionary<Rectangle, Bitmap> Split(Dictionary<Rectangle, Bitmap> originalImagesDict, int maxSize, byte quality)
        {
            if (originalImagesDict != null && originalImagesDict.Count > 0)
                return
                    originalImagesDict.Select(p => Split(p, maxSize, quality)).AsParallel()
                                      .Aggregate((d1, d2) => d1.Concat(d2).ToDictionary(p => p.Key, p => p.Value));
            else return originalImagesDict;
        }
    }
}
