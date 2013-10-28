using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DifferenceLib;
using Polymedia.PolyJoin.Common;

namespace Polymedia.PolyJoin.Client
{
    public partial class MainForm : Form
    {
        Bitmap diffFrame = null;

        private Queue _queue = Queue.Synchronized(new Queue());

        private bool _processCommands = new bool();

        private Thread t;

        public MainForm()
        {
            InitializeComponent();

            FormClosed += (sender, ea) => { _processCommands = false; };
        }

        public void Init(int width, int height)
        {
            diffFrame = new Bitmap(width, height);

            _processCommands = false;
            if (t != null)
                while (t.IsAlive)
                    Thread.Sleep(50);

            _processCommands = true;

            t = new Thread(() =>
                {
                    bool draw = false;

                    while (_processCommands)
                    {
                        try
                        {
                            Console.WriteLine("Queue count = " + _queue.Count);

                            if (_queue.Count != 0)
                            {
                                DiffCommand diffCommand = (DiffCommand) _queue.Dequeue();

                                using (Graphics diffFrameGraphics = Graphics.FromImage(diffFrame))
                                {
                                    diffFrameGraphics.DrawImage(
                                        DiffContainer.ByteArrayToImage(diffCommand.DiffItem.ImageBytes),
                                        diffCommand.DiffItem.X, diffCommand.DiffItem.Y,
                                        diffCommand.DiffItem.Width, diffCommand.DiffItem.Height);
                                }

                                draw = true;
                            }
                            else
                            {
                                if (draw)
                                {
                                    var df = new Bitmap(diffFrame, pictureBox2.Width, pictureBox2.Height);
                                    pictureBox2.Image = df;
                                }
                                else
                                {
                                    Thread.Sleep(50);
                                }

                                draw = false;
                            }

                            GC.Collect();
                        }
                        catch
                        {
                        }
                    }
                });
            t.Start();
        }

        public void AddDiffCommand(DiffCommand diffCommand)
        {
            _queue.Enqueue(diffCommand);
        }
    }
}
