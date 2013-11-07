using DifferenceLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IDiffDetector detecttor = new DiffDetectorOpenCvSharp();

            Bitmap b = new Bitmap(@"c:\1.jpg");

            var c = detecttor.GetDiffs(b,0);

            Graphics g = Graphics.FromImage(b);

            foreach (var item in c.Data)
            {
                g.DrawRectangle(new Pen(Brushes.Red), item.Key);    
            }
            

            pictureBox1.Image = b;

        }
    }
}
