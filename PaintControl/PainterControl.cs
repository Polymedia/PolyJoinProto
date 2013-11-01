﻿using System;
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
        int _imageWidth;
        int _imageHeight;
        DateTime _lastPointTime = DateTime.Now;

        public PainterControl(int width,int height, Color color)
        {
            InitializeComponent();
            _imageWidth = width;
            _imageHeight = height;
            _painter = new PaintContainer(width, height);
            pictureBox.Image = _painter.Image;
            Color = color;
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

        public event EventHandler<SimpleEventArgs<Figure>> FigureAdded;

        public event EventHandler<SimpleEventArgs<string>> FigureRemoved;

        public Color Color { get; set; }

        public Image Image
        {
            set
            {
                pictureBox.BackgroundImage = value;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return; 
             
            _mouseButtonDown = true;
            _currentFigureId = Guid.NewGuid().ToString();
            _painter.AddFigure(_currentFigureId, new List<Point>(), Color);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseButtonDown = false;

            if (_painter.GetFigureById(_currentFigureId)!=null && FigureAdded != null)
                FigureAdded.Invoke(this, new SimpleEventArgs<Figure>(_painter.GetFigureById(_currentFigureId)));

            _currentFigureId = string.Empty ;
            FillPictureBox();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseButtonDown)
            {
                int x = e.X * _imageWidth / pictureBox.Width;
                int y = e.Y * _imageHeight / pictureBox.Height;
                
                if ((DateTime.Now - _lastPointTime).TotalMilliseconds > 50)
                {
                    _lastPointTime = DateTime.Now;
                    _painter.AddPointToFigure(new Point(x, y), _currentFigureId);
                    FillPictureBox();
                }
            } 
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int x = e.X * _imageWidth / pictureBox.Width;
                int y = e.Y * _imageHeight / pictureBox.Height;
                string removedFigureId = _painter.RemoveFigure(x, y);

                if (!removedFigureId.Equals(string.Empty) && FigureRemoved != null)
                {
                    FigureRemoved.Invoke(this, new SimpleEventArgs<string>(removedFigureId));
                }

                FillPictureBox();
            }
        }

        private void PainterControl_SizeChanged(object sender, EventArgs e)
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
            Bitmap newImage = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImage(_painter.Image, 0, 0, pictureBox.Width, pictureBox.Height);
            pictureBox.Image = newImage;
        }
    }
}
