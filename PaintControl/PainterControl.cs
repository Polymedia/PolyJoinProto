using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Threading;
using Polymedia.PolyJoin.Common;

namespace Painter
{
    public partial class PainterControl : UserControl
    {
        bool _mouseButtonDown = false;
        string _currentFigureId = string.Empty;
        IPaintContainer _painter;
        int _imageWidth = 1;
        int _imageHeight = 1;
        DateTime _lastPointTime = DateTime.Now;
        //private FullScreenForm _fullScreenForm;
        private Form _drawingForm;

        public PainterControl()
        {
            InitializeComponent();

            //_fullScreenForm = new FullScreenForm();
            //_fullScreenForm.FullScreenMouseDown += pictureBox_MouseDown;
            //_fullScreenForm.FullScreenMouseMove += pictureBox_MouseMove;
            //_fullScreenForm.FullScreenMouseUp += pictureBox_MouseUp;
            //_fullScreenForm.FullScreenMouseClick += pictureBox_MouseClick;
            //_fullScreenForm.Escaped += (s, e) =>
            //    {
            //        Mode = PaintControlModes.Silent;
            //        if (FullScreenCanceled != null) FullScreenCanceled.Invoke(this, new EventArgs());
            //    };


            Mode = PaintControlModes.Silent;

            Init(_imageWidth, _imageHeight, Color.Black, null);
        }

        public void Init(int width, int height, Color color, IDrawingForm topMostForm)
        {
            _imageWidth = width;
            _imageHeight = height;
            _painter = new PaintContainer(width, height);
            pictureBox.Image = _painter.Image;
            Color = color;
            _drawingForm = (Form)topMostForm;
            if (topMostForm != null)
            {
                topMostForm.DrawingMouseDown += pictureBox_MouseDown;
                topMostForm.DrawingMouseMove += pictureBox_MouseMove;
                topMostForm.DrawingMouseUp += pictureBox_MouseUp;
                topMostForm.DrawingMouseClick += pictureBox_MouseClick;
            }

            AdjustPictureBox();
        }

        public void AddFigure(string id, List<Point> points, Color color)
        {
            _painter.AddFigure(id, points, color);
            FillPictureBox();
        }

        public void RemoveFigure(string id)
        {
            _painter.RemoveFigure(id);
            FillPictureBox();
        }

        public event EventHandler<SimpleEventArgs<Figure>> FigureAdded = delegate { };

        public event EventHandler<SimpleEventArgs<string>> FigureRemoved = delegate { };

        public event EventHandler<SimpleEventArgs<MouseInput>> MouseInputed = delegate { };

        public event EventHandler FullScreenCanceled = delegate { };

        public Color Color { get; set; }

        public Image Image
        {
            set
            {
                pictureBox.BackgroundImage = value;
            }
        }

        public PaintControlModes Mode;

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (Mode == PaintControlModes.Draw /*|| Mode == PaintControlModes.DrawFullScreen*/)
            {
                if (e.Button == MouseButtons.Right)
                    return;

                Console.WriteLine("pictureBox_MouseDown");

                _mouseButtonDown = true;
                _currentFigureId = Guid.NewGuid().ToString();
                _painter.AddFigure(_currentFigureId, new List<Point>(), Color);

                return;
            }

            if (Mode == PaintControlModes.Input)
            {
                FireMouseInputed(e, MouseInput.MouseInputEnum.Down);
                return;
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            
            if (Mode == PaintControlModes.Draw/* || Mode==PaintControlModes.DrawFullScreen*/)
            {
                _mouseButtonDown = false;

                if (_painter.GetFigureById(_currentFigureId) != null && FigureAdded != null)
                    FigureAdded.Invoke(this, new SimpleEventArgs<Figure>(_painter.GetFigureById(_currentFigureId)));

                Console.WriteLine("pictureBox_MouseUp");

                _currentFigureId = string.Empty;
                FillPictureBox();

                return;
            }

            if (Mode == PaintControlModes.Input)
            {
                FireMouseInputed(e, MouseInput.MouseInputEnum.Up);
                return;
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mode == PaintControlModes.Draw)
            {
                if (_mouseButtonDown)
                {
                    int x = e.X;
                    int y = e.Y;

                    if (sender == pictureBox)
                    {
                        x = (int)(e.X * (float)_imageWidth / pictureBox.Width);
                        y = (int)(e.Y * (float)_imageHeight / pictureBox.Height);
                    }

                    if ((DateTime.Now - _lastPointTime).TotalMilliseconds > 50)
                    {
                        _lastPointTime = DateTime.Now;
                        _painter.AddPointToFigure(new Point(x, y), _currentFigureId);
                        FillPictureBox();
                    }
                }
                return;
            }

            if (Mode == PaintControlModes.Input)
            {
                FireMouseInputed(e, MouseInput.MouseInputEnum.Move);
                return;
            }
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {

            if (Mode == PaintControlModes.Draw)
            {
                if (e.Button == MouseButtons.Right)
                {
                    int x = e.X;
                    int y = e.Y;

                    if (sender == pictureBox)
                    {
                        x = (int)(e.X * (float)_imageWidth / pictureBox.Width);
                        y = (int)(e.Y * (float)_imageHeight / pictureBox.Height);
                    }

                    string removedFigureId = _painter.RemoveFigure(x, y);

                    if (!removedFigureId.Equals(string.Empty) && FigureRemoved != null)
                    {
                        FigureRemoved.Invoke(this, new SimpleEventArgs<string>(removedFigureId));
                    }
                    Console.WriteLine("pictureBox_MouseClick");
                    FillPictureBox();
                }
                return;
            }

            if (Mode == PaintControlModes.Input)
            {
                FireMouseInputed(e, MouseInput.MouseInputEnum.Click);
                return;
            }
        }

        private void PainterControl_SizeChanged(object sender, EventArgs e)
        {
            AdjustPictureBox();
        }

        public void AdjustPictureBox()
        {
            double orignalImageAspect = (double)_imageWidth / (double)_imageHeight;
            double controlAspect = (double)Width / (double)Height;
            if (orignalImageAspect > controlAspect)
            {
                var newWidth = Width;
                var newHeight = _imageHeight * newWidth / _imageWidth;
                var newLocation = new Point(0, (Height - newHeight) / 2);

                pictureBox.Location = newLocation;
                pictureBox.Width = newWidth;
                pictureBox.Height = newHeight;
            }
            else
            {
                var newHeight = Height;
                var newWidth = _imageWidth * newHeight / _imageHeight; ;
                var newLocation = new Point((Width - newWidth) / 2, 0);

                pictureBox.Location = newLocation;
                pictureBox.Width = newWidth;
                pictureBox.Height = newHeight;
            }
            FillPictureBox();
        }

        private void FillPictureBox()
        {
            try
            {
                //if (Mode == PaintControlModes.DrawFullScreen)
                if(_drawingForm != null)
                {
                    Bitmap newImage = new Bitmap(_drawingForm.Width, _drawingForm.Height);
                    using (Graphics g = Graphics.FromImage(newImage))
                    {
                        g.DrawImage(_painter.Image, 0, 0, _drawingForm.Width, _drawingForm.Height);
                        ((IDrawingForm) _drawingForm).Image = newImage;
                    }
                }
                //else
                {
                    Bitmap newImage = new Bitmap(pictureBox.Width, pictureBox.Height);
                    using (Graphics g = Graphics.FromImage(newImage))
                    {
                        g.DrawImage(_painter.Image, 0, 0, pictureBox.Width, pictureBox.Height);
                        pictureBox.Image = newImage;
                    }
                }
            }
            catch
            {
            }
        }

        private void FireMouseInputed(MouseEventArgs args, MouseInput.MouseInputEnum mouseInputEnum)
        {
            MouseInput mouseInput = new MouseInput()
            {
                MouseInputType = mouseInputEnum,
                LeftButton = args.Button == MouseButtons.Left,
                RightButton = args.Button == MouseButtons.Right,
                X = (int)(_imageWidth/ (float)pictureBox.Width * args.X),
                Y = (int)(_imageHeight / (float)pictureBox.Height * args.Y)
            };

            MouseInputed.Invoke(this, new SimpleEventArgs<MouseInput>(mouseInput));
        }
    }

    public enum PaintControlModes
    {
        Silent,
        Draw,
        Input,
        //DrawFullScreen
    }
}
