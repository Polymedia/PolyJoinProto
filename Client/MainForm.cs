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
        Bitmap _diffFrame = null;
        private Queue _queue = Queue.Synchronized(new Queue());
        private bool _processCommands = false;
        private Thread _processCommandsThread;

        private bool _isPresenter;
        private int _presenterWidth;
        private int _presenterHeight;

        public MainForm()
        {
            InitializeComponent();
            FormClosed += (sender, ea) => { _processCommands = false; };
        }

        public void StartProcessingCommands()
        {
            _processCommands = false;
            if (_processCommandsThread != null)
                while (_processCommandsThread.IsAlive)
                    Thread.Sleep(50);

            _processCommands = true;

            _processCommandsThread = new Thread(() =>
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

                                using (Graphics diffFrameGraphics = Graphics.FromImage(_diffFrame))
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
                                    pictureBox.Invoke(new Action(
                                                          () =>
                                                              {
                                                                  pictureBox.Image = _diffFrame;
                                                                  pictureBox.Refresh();
                                                              }
                                                          )
                                        );
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
            _processCommandsThread.Start();
        }

        public void AddDiffCommand(DiffCommand diffCommand)
        {
            _queue.Enqueue(diffCommand);
        }

        public void SetIsPresenter(bool isPresenter, int width, int height)
        {
            _isPresenter = isPresenter;
            _presenterWidth = width;
            _presenterHeight = height;

            _diffFrame = new Bitmap(_presenterWidth, _presenterHeight);

            if(_isPresenter)
                using (Graphics diffFrameGraphics = Graphics.FromImage(_diffFrame))
                {
                    string text = "Presenter";
                    Font font = new Font("Arial", 30);

                    SizeF size = diffFrameGraphics.MeasureString(text, font);

                    diffFrameGraphics.DrawString("Presenter", font, new SolidBrush(Color.Silver), new PointF(_presenterWidth / 2 - size.Width / 2, _presenterHeight / 2 - size.Height / 2));
                    pictureBox.Invoke(new Action(
                                          () =>
                                              {
                                                  pictureBox.Image = _diffFrame;
                                                  pictureBox.Refresh();
                                              }
                                          )
                        );
                }

        }
    }
}
