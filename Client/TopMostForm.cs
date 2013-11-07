using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class TopMostForm : Form, Painter.IDrawingForm
    {
        #region WinAPI 

        private enum GWL
        {
            ExStyle = -20
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        #endregion

        private int initialWindowLong = 0;

        public event EventHandler<EventArgs> Done = delegate { };
        public event EventHandler<MouseEventArgs> DrawingMouseDown = delegate { };
        public event EventHandler<MouseEventArgs> DrawingMouseUp = delegate { };
        public event EventHandler<MouseEventArgs> DrawingMouseMove = delegate { };
        public event EventHandler<MouseEventArgs> DrawingMouseClick = delegate { };

        public TopMostForm()
        {
            InitializeComponent();

            Width = SystemInformation.VirtualScreen.Width;
            Height = SystemInformation.VirtualScreen.Height;

            Location = new Point(0, 0);

            TopMost = true;

            initialWindowLong = GetWindowLong(Handle, GWL.ExStyle);

            SetClickThrough(true);

            KeyDown += (sender, args) =>
                {
                    if(args.KeyCode == Keys.Escape)
                        SetClickThrough(true);
                    Done.Invoke(this, new EventArgs());
                };

            pictureBox.MouseDown += (sender, ea) =>
                {
                    DrawingMouseDown.Invoke(sender, ea);
                };
            pictureBox.MouseUp += (sender, ea) =>
                {
                    DrawingMouseUp.Invoke(sender, ea);
                };
            pictureBox.MouseMove += (sender, ea) =>
                {
                    DrawingMouseMove.Invoke(sender, ea);
                };
            pictureBox.MouseClick += (sender, ea) =>
                {
                    DrawingMouseClick.Invoke(sender, ea);
                };
        }

        public void SetClickThrough(bool clickThrough)
        {
            TopMost = true;
            if(clickThrough)
                SetWindowLong(Handle, GWL.ExStyle, initialWindowLong | 0x80000 | 0x20);
            else
                SetWindowLong(Handle, GWL.ExStyle, initialWindowLong);
        }

        public Image Image
        {
            get
            {
                return pictureBox.BackgroundImage;
            }
            set
            {
                pictureBox.BackgroundImage = value;
            }
        }
    }
}
