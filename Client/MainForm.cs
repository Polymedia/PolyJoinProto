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
using Common;
using DifferenceLib;
using Polymedia.PolyJoin.Common;

namespace Polymedia.PolyJoin.Client
{
    public partial class MainForm : Form
    {
        private ClientWebSocketConnection _clientWebSocketConnection = null;

        private bool _runDiffDetectThread = false;
        private Thread _diffDetectThread = null;

        private Queue _queue = Queue.Synchronized(new Queue());

        private bool _runProcessCommandsThread = false;
        private Thread _processCommandsThread = null;

        public ClientWebSocketConnection ClientWebSocketConnection
        {
            set
            {
                _clientWebSocketConnection = value;
                _clientWebSocketConnection.ConnectionStateChanged += ClientWebSocketConnectionOnConnectionStateChanged;
                _clientWebSocketConnection.StateCommandReceived += ClientWebSocketConnectionOnStateCommandReceived;
                _clientWebSocketConnection.DiffCommandReceived += ClientWebSocketConnectionOnDiffCommandReceived;
            }
            private get { return _clientWebSocketConnection; }
        }

        public float ScreenshotScale { get; set; }
        public int ScreenshotTimeout { get; set; }

        public string ConferenceId { get; set; }

        private bool _isPresenter;
        private int _presenterWidth;
        private int _presenterHeight;

        private Bitmap _diffFrame = null;

        public MainForm()
        {
            InitializeComponent();

            conferenceIdValueLabel.DoubleClick += (sender, args) => { Clipboard.SetText(conferenceIdValueLabel.Text); };
            
            FormClosed += (sender, ea) =>
                {
                    _runProcessCommandsThread = false;
                    _runDiffDetectThread = false;
                    _isPresenter = false;
                    _presenterWidth = 0;
                    _presenterHeight = 0;

                    conferenceIdValueLabel.Text = string.Empty;
                    pictureBox.Image = null;
                    roleValueLabel.Text = string.Empty;
                };

            FormClosing += (sender, ea) =>
            {
                Hide();
                ea.Cancel = true;
                if (_clientWebSocketConnection != null)
                    _clientWebSocketConnection.ConnectionStateChanged -=
                        ClientWebSocketConnectionOnConnectionStateChanged;
            };
        }

        private void ClientWebSocketConnectionOnDiffCommandReceived(object sender, SimpleEventArgs<DiffCommand> simpleEventArgs)
        {
            _queue.Enqueue(simpleEventArgs.Value);
        }

        private void ClientWebSocketConnectionOnStateCommandReceived(object sender, SimpleEventArgs<StateCommand> simpleEventArgs)
        {
            Invoke(new Action(() =>
                {
                    StateCommand stateCommand = simpleEventArgs.Value;
                    if (string.IsNullOrEmpty(stateCommand.ConferenceId))
                    {
                        MessageBox.Show(this, "Unable create or join conference! Retry later!");
                        Close();
                    }
                    else
                    {
                        ConferenceId = stateCommand.ConferenceId;
                        _isPresenter = stateCommand.IsPresenter;
                        _presenterWidth = stateCommand.PresenterWidth;
                        _presenterHeight = stateCommand.PresenterHeight;

                        if (_isPresenter)
                            StartDiffDetectThread();

                        StartProcessCommandsThread();

                        conferenceIdValueLabel.Text = ConferenceId;

                        _diffFrame = new Bitmap(_presenterWidth, _presenterHeight);

                        roleValueLabel.Text = _isPresenter ? "Presenter" : "Viewer";
                    }
                }));
        }

        private void ClientWebSocketConnectionOnConnectionStateChanged(object sender, WebSocketEventArgs<bool> webSocketEventArgs)
        {
            Invoke(new Action(() =>
                {
                    if (webSocketEventArgs.Value)
                    {
                        connectionStateValueLabel.Text = "Connected";

                        ClientWebSocketConnection.QueryState(ConferenceId, (int)(Screen.PrimaryScreen.Bounds.Width * ScreenshotScale), (int)(Screen.PrimaryScreen.Bounds.Height * ScreenshotScale));
                    }
                    else
                    {
                        connectionStateValueLabel.Text = "Connecting...";
                        _runDiffDetectThread = false;
                        _runProcessCommandsThread = false;
                    }
                }));
        }

        public void StartProcessCommandsThread()
        {
            _runProcessCommandsThread = false;
            if (_processCommandsThread != null)
                while (_processCommandsThread.IsAlive)
                    Thread.Sleep(100);

            _runProcessCommandsThread = true;

            _processCommandsThread = new Thread(() =>
            {
                bool draw = false;

                while (_runProcessCommandsThread)
                {
                    try
                    {
                        Console.WriteLine("Queue count = " + _queue.Count);

                        if (_queue.Count != 0)
                        {
                            DiffCommand diffCommand = (DiffCommand)_queue.Dequeue();
                                //_diffFrame = new Bitmap(_presenterWidth, _presenterHeight);

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
                                                      })
                                    );
                            }
                            else
                            {
                                Thread.Sleep(100);
                            }

                            draw = false;
                        }

                        GC.Collect();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("_processCommandsThread : " + ex.Message);
                    }
                }
            });
            _processCommandsThread.Start();
        }

        public void StartDiffDetectThread()
        {
            _runDiffDetectThread = false;

            if (_diffDetectThread != null)
                while (_diffDetectThread.IsAlive)
                    Thread.Sleep(100);

            _runDiffDetectThread = true;

            _diffDetectThread = new Thread(() =>
            {
                IDiffDetector _diffDetector = new CustomDiffDetector();
                while (_runDiffDetectThread)
                {
                    try
                    {
                        Bitmap screenShot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                                       Screen.PrimaryScreen.Bounds.Height);

                        using (Graphics screenShotGraphics = Graphics.FromImage(screenShot))
                        {
                            screenShotGraphics.CopyFromScreen(0, 0, 0, 0,
                                                              new Size(
                                                                  Screen.PrimaryScreen.Bounds.Width,
                                                                  Screen.PrimaryScreen.Bounds.Height));

                            if (Math.Abs(ScreenshotScale - 1) > 0.01)
                                screenShot = new Bitmap(screenShot,
                                                        (int)
                                                        (Screen.PrimaryScreen.Bounds.Width * ScreenshotScale),
                                                        (int)
                                                        (Screen.PrimaryScreen.Bounds.Height * ScreenshotScale));
                        }
                        DiffContainer diffContainer = _diffDetector.GetDiffs(screenShot);

                        diffContainer.Data = DiffContainer.Split(diffContainer.Data, 40000);

                        foreach (var s in diffContainer.Data)
                            ClientWebSocketConnection.SendDiff(ConferenceId, new DiffItem(s));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("_diffDetectThread : " + ex.Message );
                    }
                    Thread.Sleep(ScreenshotTimeout);
                }
            });
            _diffDetectThread.Start();
        }
    }
}
