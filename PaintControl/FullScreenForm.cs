using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Painter
{
    public partial class FullScreenForm : Form
    {
        private FullScreeenControlForm _controlForm;

        public event MouseEventHandler FullScreenMouseDown;
        public event MouseEventHandler FullScreenMouseMove;
        public event MouseEventHandler FullScreenMouseUp;
        public event MouseEventHandler FullScreenMouseClick;
        public event EventHandler Escaped;
        
        public FullScreenForm()
        {
            InitializeComponent();

            Width = SystemInformation.VirtualScreen.Width;
            Height = SystemInformation.VirtualScreen.Height;

            _controlForm = new FullScreeenControlForm();
            _controlForm.Width = SystemInformation.VirtualScreen.Width;
            _controlForm.Height = SystemInformation.VirtualScreen.Height;

            _controlForm.MouseDown += (sender, ea) =>
                {
                    if (FullScreenMouseDown != null)
                        FullScreenMouseDown.Invoke(sender, ea);
                };

            _controlForm.MouseMove += (sender, ea) =>
                {
                    if (FullScreenMouseMove != null)
                        FullScreenMouseMove.Invoke(sender, ea);
                };

            _controlForm.MouseUp += (sender, ea) =>
                {
                    if (FullScreenMouseUp != null)
                        FullScreenMouseUp.Invoke(sender, ea);
                };

            _controlForm.MouseClick += (sender, ea) =>
                {
                    if (FullScreenMouseClick != null)
                        FullScreenMouseClick.Invoke(sender, ea);
                };

            _controlForm.Escaped += (sender, ea) =>
                {
                    this.Hide();
                    if (Escaped != null) Escaped.Invoke(sender, new EventArgs());
                };
        }

        public Image Image
        {
            get
            {
                return pictureBox1.Image;
            }
            set 
            { 
                pictureBox1.Image = value;
            }
        }

        private void FullScreenForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                _controlForm.Show();
                _controlForm.Location = SystemInformation.VirtualScreen.Location;
                _controlForm.BringToFront();
            }
        }
    }
}
