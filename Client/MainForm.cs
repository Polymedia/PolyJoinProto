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
using Painter;

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
                _clientWebSocketConnection.ConnectionStateChangedEvent += ClientWebSocketConnectionOnConnectionStateChanged;
                _clientWebSocketConnection.StateCommandReceived += ClientWebSocketConnectionOnStateCommandReceived;
                _clientWebSocketConnection.DiffCommandReceived += ClientWebSocketConnectionOnDiffCommandReceived;
                _clientWebSocketConnection.ParticipantsCommandReceived += ClientWebSocketConnectionOnParticipantsCommandReceived;
                _clientWebSocketConnection.PaintAddFigureCommandRecieved += ClientWebSocketConnectionPaintAddFigureCommandRecieved;
                _clientWebSocketConnection.PaintDeleteFigureCommandRecieved += ClientWebSocketConnectionPaintDeleteFigureCommandRecieved;
            }
            private get { return _clientWebSocketConnection; }
        }

        public float ScreenshotScale { get; set; }
        public int ScreenshotTimeout { get; set; }

        public string ConferenceId { get; set; }

        private bool _isPresenter;
        private int _presenterWidth;
        private int _presenterHeight;
        private string _id;

        private Color _paintColor;
        private Bitmap _diffFrame = null;

        private PainterControl _paintControl;

        public MainForm()
        {
            InitializeComponent();

            dataGridView.AutoGenerateColumns = false;
            dataGridView.SelectionChanged += (sender, ea) => dataGridView.ClearSelection();
            dataGridView.CellPainting += (sender, args) =>
                {
                    if (dataGridView.Columns["ColorColumn"] != null && dataGridView.Columns["ColorColumn"].Index == args.ColumnIndex && args.RowIndex >= 0)
                    {
                        using (
                            Brush gridBrush = new SolidBrush(dataGridView.GridColor),
                                  backColorBrush = new SolidBrush(args.CellStyle.BackColor))
                        {
                            using (Pen gridLinePen = new Pen(gridBrush))
                            {
                                args.Graphics.FillRectangle(backColorBrush, args.CellBounds);

                                args.Graphics.DrawLine(gridLinePen, args.CellBounds.Left,
                                                       args.CellBounds.Bottom - 1, args.CellBounds.Right - 1,
                                                       args.CellBounds.Bottom - 1);
                                args.Graphics.DrawLine(gridLinePen, args.CellBounds.Right - 1,
                                                       args.CellBounds.Top, args.CellBounds.Right - 1,
                                                       args.CellBounds.Bottom);

                                args.Graphics.FillEllipse(new SolidBrush(Color.FromArgb((int)args.Value)), args.CellBounds.Location.X + args.CellBounds.Width / 2 - 5, args.CellBounds.Location.Y + args.CellBounds.Height / 2 - 5, 10, 10);

                                args.Handled = true;
                            }
                        }
                    }
                };

            conferenceIdValueLabel.DoubleClick += (sender, args) => { Clipboard.SetText(conferenceIdValueLabel.Text); };
            
            VisibleChanged += (sender, ea) =>
                {
                    if (!Visible)
                    {
                        _runProcessCommandsThread = false;
                        _runDiffDetectThread = false;
                        _isPresenter = false;
                        _presenterWidth = 0;
                        _presenterHeight = 0;
                        _id = string.Empty;

                        conferenceIdValueLabel.Text = string.Empty;
                        _paintControl.BackgroundImage = null;
                        roleValueLabel.Text = string.Empty;

                        
                        dataGridView.DataSource = null;
                    }
                };

            FormClosing += (sender, ea) =>
            {
                Hide();
                ea.Cancel = true;
                if (_clientWebSocketConnection != null)
                    _clientWebSocketConnection.ConnectionStateChangedEvent -=
                        ClientWebSocketConnectionOnConnectionStateChanged;
            };
        }

        private void ClientWebSocketConnectionOnParticipantsCommandReceived(object sender, SimpleEventArgs<ParticipantsCommand> simpleEventArgs)
        {
            _queue.Enqueue(simpleEventArgs.Value);
        }

        private void InitPaintControl(int width, int height)
        {
            _paintControl = new PainterControl(_presenterWidth, _presenterHeight, Color.Black);
            tableLayoutPanel.Controls.Add(_paintControl, 1, 1);
            _paintControl.Dock = DockStyle.Fill;

            _paintControl.FigureAdded += (s, e) =>
            {
                ClientWebSocketConnection.PaintAddFigureCommand(ConferenceId, e.Value.Id, e.Value.Points, e.Value.Color);
            };

            _paintControl.FigureRemoved += (s, e) =>
            {
                ClientWebSocketConnection.PaintDeleteFigureCommand(ConferenceId, e.Value);
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
                        _id = stateCommand.ParticipantId;

                        if (_isPresenter)
                            StartDiffDetectThread();

                        StartProcessCommandsThread();

                        conferenceIdValueLabel.Text = ConferenceId;

                        _diffFrame = new Bitmap(_presenterWidth, _presenterHeight);

                        roleValueLabel.Text = _isPresenter ? "Presenter" : "Viewer";

                        InitPaintControl(_presenterWidth, _presenterHeight);
                    }
                }));
        }

        private void ClientWebSocketConnectionOnConnectionStateChanged(object sender, SimpleEventArgs<bool> webSocketEventArgs)
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

        private void ClientWebSocketConnectionPaintAddFigureCommandRecieved(object sender, SimpleEventArgs<PaintAddFigureCommand> e)
        {
            Invoke(new Action(() => 
            {
                _paintControl.AddFigure(e.Value.FigureId, e.Value.Points, e.Value.Color);
            }));
        }

        private void ClientWebSocketConnectionPaintDeleteFigureCommandRecieved(object sender, SimpleEventArgs<PaintDeleteFigureCommand> e)
        {
            Invoke(new Action(() =>
            {
                _paintControl.RemoveFigure(e.Value.FigureId);
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
                            Command command = (Command)_queue.Dequeue();

                            if (command is DiffCommand)
                            {
                                DiffCommand diffCommand = command  as DiffCommand;
                                ProcessDiffCommand(diffCommand);
                                draw = true;
                            }else if (command is ParticipantsCommand)
                            {
                                ParticipantsCommand participantsCommand = command as ParticipantsCommand;
                                ProcessParticipantsCommand(participantsCommand);
                            }
                        }
                        else
                        {
                            if (draw)
                            {
                                _paintControl.Invoke(new Action(
                                                      () =>
                                                      {
                                                          _paintControl.Image = _diffFrame;
                                                          _paintControl.Refresh();
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

        private void ProcessDiffCommand(DiffCommand diffCommand)
        {
            using (Graphics diffFrameGraphics = Graphics.FromImage(_diffFrame))
            {

                diffFrameGraphics.DrawImage(
                    DiffContainer.ByteArrayToImage(diffCommand.DiffItem.ImageBytes),
                    diffCommand.DiffItem.X, diffCommand.DiffItem.Y,
                    diffCommand.DiffItem.Width, diffCommand.DiffItem.Height);
            }
        }

        private void ProcessParticipantsCommand(ParticipantsCommand participantsCommand)
        {
            Invoke(new Action(() =>
                {
                    dataGridView.DataSource = participantsCommand.Participants;
                    foreach (DataGridViewRow  row in dataGridView.Rows)
                        if (row.Cells["IdColumn"].Value.Equals(_id))
                            foreach (DataGridViewCell cell in row.Cells)
                                cell.Style.BackColor = Color.Gainsboro;

                    var me = participantsCommand.Participants.Where(p => p.Id == _id).First();
                    _paintColor = Color.FromArgb(me.BrushArgb);
                    _paintControl.Color = _paintColor;
                }));
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
