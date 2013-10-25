using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DifferenceLib;

namespace Polymedia.PolyJoin.Client
{
    public partial class MainForm : Form
    {
        Bitmap oldFrame = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

        Bitmap diffFrame;

        Graphics diffFrameGraphics;

        private int _compr = 1;
        
        public MainForm()
        {
            InitializeComponent();
        }

        public void DrawDiff(DiffItem diffItem)
        {
            diffFrame = new Bitmap((int)(Screen.PrimaryScreen.Bounds.Width / _compr), (int)(Screen.PrimaryScreen.Bounds.Height / _compr));
            diffFrameGraphics = Graphics.FromImage(diffFrame);

            //using (Graphics oldFrameGraphics = Graphics.FromImage(oldFrame))
            //{
                //foreach (var p in diffContainer.Data)
                //{
                    diffFrameGraphics.DrawImage(diffItem.Bitmap, diffItem.Rectangle);

                    //oldFrameGraphics.DrawImage(p.Value, p.Key);
                //}
            //}

                var df = new Bitmap(diffFrame, pictureBox2.Width, pictureBox2.Height);
            //var of = new Bitmap(oldFrame, pictureBox2.Width, pictureBox2.Height);
            

            //pictureBox1.Image = df;

            pictureBox2.Image = df;
        }
    }
}
