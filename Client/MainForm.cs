using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Common.Commands;
using DifferenceLib;
using Polymedia.PolyJoin.Client;
using Polymedia.PolyJoin.Common;
using Painter;
using System.Diagnostics;
using System.IO;

namespace Client
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
                _clientWebSocketConnection.InputCommandReceived += ClientWebSocketConnectionOnInputCommandReceived;
                _clientWebSocketConnection.PaintAddFigureCommandRecieved += ClientWebSocketConnectionPaintAddFigureCommandRecieved;
                _clientWebSocketConnection.PaintDeleteFigureCommandRecieved += ClientWebSocketConnectionPaintDeleteFigureCommandRecieved;
                _clientWebSocketConnection.RequestControlCommandReceived += ClientWebSocketConnectionOnRequestControlCommandReceived;
                _clientWebSocketConnection.ControlAccessCommandReceived += ClientWebSocketConnectionOnControlAccessCommandReceived;
            }
            private get { return _clientWebSocketConnection; }
        }

        public float ScreenshotScale { get; set; }
        public int ScreenshotTimeout { get; set; }
        public byte JpegQuality { get; set; }

        public string ConferenceId { get; set; }
        public string ClientName { get; set; }

        private bool _isPresenter;
        private int _presenterWidth;
        private int _presenterHeight;
        private string _id;

        private Color _paintColor;
        private Bitmap _diffFrame = null;

        private PainterControl _paintControl;

        private TopMostForm _topMostForm;

        public MainForm()
        {
            InitializeComponent();

            InitPaintControl();

            _topMostForm = new TopMostForm();
            _topMostForm.Done += (sender, ea) =>
                {
                    Focus();
                    silentRadioButton.Checked = true;
                };

            dataGridView.AutoGenerateColumns = false;
            dataGridView.SelectionChanged += (sender, ea) => dataGridView.ClearSelection();
            dataGridView.CellDoubleClick += (sender, args) =>
                {
                    if (!_isPresenter) return;

                    var clickedParticipantRowIndex = args.RowIndex;
                    var participants = (IList<Participant>) dataGridView.DataSource;
                    if (participants == null || clickedParticipantRowIndex < 0 ||
                        clickedParticipantRowIndex >= participants.Count) return;

                    var clickedParticipant = participants[clickedParticipantRowIndex];
                    ClientWebSocketConnection.ControlAccessCommand(ConferenceId, _id, clickedParticipant.Id, true);
                };
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
                        _topMostForm.Hide();

                        _runProcessCommandsThread = false;
                        _runDiffDetectThread = false;
                        _isPresenter = false;
                        _presenterWidth = 0;
                        _presenterHeight = 0;
                        _id = string.Empty;

                        conferenceIdValueLabel.Text = string.Empty;
                        _paintControl.Image = null;
                        roleValueLabel.Text = string.Empty;

                        _paintControl.Mode = PaintControlModes.Silent;
                        silentRadioButton.Checked = true;

                        //modeGroupBox.Enabled = false;

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

            silentRadioButton.CheckedChanged += RadioButtonOnCheckedChanged;
            drawRadioButton.CheckedChanged += RadioButtonOnCheckedChanged;
            inputRadioButton.CheckedChanged += RadioButtonOnCheckedChanged;
        }

        private void InitPaintControl()
        {
            _paintControl = new PainterControl();
            tableLayoutPanel.Controls.Add(_paintControl, 1, 1);
            _paintControl.Dock = DockStyle.Fill;

            _paintControl.FigureAdded += (s, e) =>
            {
                ClientWebSocketConnection.PaintAddFigureCommand(ConferenceId, e.Value.Id, e.Value.Points,
                                                                e.Value.Color);
            };

            _paintControl.FigureRemoved += (s, e) =>
            {
                ClientWebSocketConnection.PaintDeleteFigureCommand(ConferenceId, e.Value);
            };

            _paintControl.MouseInputed += (sender, args) =>
            {
                ClientWebSocketConnection.SendInput(ConferenceId, args.Value);
            };

            _paintControl.FullScreenCanceled += (s, e) =>
                {
                    silentRadioButton.Checked = true;
                };
        }

        private void RadioButtonOnCheckedChanged(object sender, EventArgs eventArgs)
        {
            if (sender == inputRadioButton)
            {
                if (inputRadioButton.Checked)
                {
                    if (_isPresenter)
                        return;

                    ClientWebSocketConnection.RequestControlCommand(ConferenceId, _id, true);
                }
                else
                {
                    ClientWebSocketConnection.RequestControlCommand(ConferenceId, _id, false);
                    ClientWebSocketConnection.ControlAccessCommand(ConferenceId, "0", _id, false); //Хак (передаем ноль, так как в данной реализации может быть только один презентер и этот ноль переопределиться на сервере в нормальный id presenter'а)
                }
            }

            if (silentRadioButton.Checked) _paintControl.Mode = PaintControlModes.Silent;
            if (drawRadioButton.Checked)
            {
                if (_isPresenter)
                    _topMostForm.SetClickThrough(false);
                _paintControl.Mode = PaintControlModes.Draw;
            }
            if (inputRadioButton.Checked) _paintControl.Mode = PaintControlModes.Input;
        }

        private void ClientWebSocketConnectionOnParticipantsCommandReceived(object sender, SimpleEventArgs<ParticipantsCommand> simpleEventArgs)
        {
            _queue.Enqueue(simpleEventArgs.Value);
        }

        private void ClientWebSocketConnectionOnInputCommandReceived(object sender, SimpleEventArgs<InputCommand> simpleEventArgs)
        {
            _queue.Enqueue(simpleEventArgs.Value);
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

                        _paintControl.Mode = PaintControlModes.Silent;
                        silentRadioButton.Checked = true;
                        
                        if (_isPresenter)
                        {
                            silentRadioButton.Visible = true;
                            drawRadioButton.Visible = true;
                            inputRadioButton.Visible = false;
                        }
                        else
                        {
                            silentRadioButton.Visible = true;
                            drawRadioButton.Visible = true;
                            inputRadioButton.Visible = true;
                        }

                        if (_isPresenter)
                        {
                            StartDiffDetectThread();
                            _paintControl.Init(_presenterWidth, _presenterHeight, Color.Black, _topMostForm);
                            _topMostForm.Show();
                        }
                        else
                            _paintControl.Init(_presenterWidth, _presenterHeight, Color.Black, null);

                        conferenceIdValueLabel.Text = ConferenceId;

                        _diffFrame = new Bitmap(_presenterWidth, _presenterHeight);

                        roleValueLabel.Text = _isPresenter ? "Presenter" : "Viewer";

                        StartProcessCommandsThread();
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

                        ClientWebSocketConnection.QueryState(ConferenceId, (int)(Screen.PrimaryScreen.WorkingArea.Width * ScreenshotScale), (int)(Screen.PrimaryScreen.WorkingArea.Height * ScreenshotScale), ClientName);
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
            _queue.Enqueue(e.Value);
        }

        private void ClientWebSocketConnectionPaintDeleteFigureCommandRecieved(object sender, SimpleEventArgs<PaintDeleteFigureCommand> e)
        {
            _queue.Enqueue(e.Value);
        }

        private void ClientWebSocketConnectionOnRequestControlCommandReceived(object sender, SimpleEventArgs<RequestControlCommand> e)
        {
            _queue.Enqueue(e.Value);
        }

        private void ClientWebSocketConnectionOnControlAccessCommandReceived(object sender, SimpleEventArgs<ControlAccessCommand> e)
        {
            _queue.Enqueue(e.Value);
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
                            }
                            else if (command is ParticipantsCommand)
                            {
                                ParticipantsCommand participantsCommand = command as ParticipantsCommand;
                                ProcessParticipantsCommand(participantsCommand);
                            }
                            else if (command is InputCommand)
                            {
                                InputCommand inputCommand = command as InputCommand;
                                ProcessInputCommand(inputCommand);
                            }
                            else if (command is PaintAddFigureCommand)
                            {
                                PaintAddFigureCommand paintAddFigureCommand = command as PaintAddFigureCommand;
                                ProcessPaintAddFigureCommand(paintAddFigureCommand);
                            }
                            else if (command is PaintDeleteFigureCommand)
                            {
                                PaintDeleteFigureCommand paintDeleteFigureCommand =
                                    command as PaintDeleteFigureCommand;
                                ProcessPaintDeleteFigureCommand(paintDeleteFigureCommand);
                            }
                            else if (command is RequestControlCommand)
                            {
                                var requestControlCommand = command as RequestControlCommand;
                                ProcessRequestControlCommand(requestControlCommand);
                            }
                            else if (command is ControlAccessCommand)
                            {
                                var controllAccessCommand = command as ControlAccessCommand;
                                ProcessControlAccessCommand(controllAccessCommand);
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
                                Thread.Sleep(50);
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

        private int GetRowIndex(string clientId)
        {
            var participants = dataGridView.DataSource as IList<Participant>;
            if (participants == null)
                return -1;
            var requestedControlParticipant =
                participants.FirstOrDefault(p => p.Id.Equals(clientId));
            if (requestedControlParticipant == null)
                return -1;
            var requestedControlParticipantIndex = participants.IndexOf(requestedControlParticipant);
            return requestedControlParticipantIndex;
        }

        private Color GetRowBackColor(bool controllIsAllowed)
        {
            return controllIsAllowed ? Color.Aqua : Color.White;
        }

        private void ProcessRequestControlCommand(RequestControlCommand requestControlCommand)
        {
            var rowIndex = GetRowIndex(requestControlCommand.ClientId);
            if (rowIndex < 0)
                return;

            dataGridView.Rows[rowIndex].DefaultCellStyle.BackColor = GetRowBackColor(requestControlCommand.IsAllowed);
        }

        private void ProcessControlAccessCommand(ControlAccessCommand controlAccessCommand)
        {
            var rowIndex = GetRowIndex(controlAccessCommand.ClientId);
            if (rowIndex < 0)
                return;

            if (controlAccessCommand.ClientId.Equals(_id) && !(bool)dataGridView["IsInputContollerColumn", rowIndex].Value  && controlAccessCommand.IsAllowed)
            {
                Invoke((Action) (() =>
                    {
                        //Хак, чтобы события правильно отрабатывали (в данном случае нужно только обновление gui без event'а)
                        inputRadioButton.CheckedChanged -= RadioButtonOnCheckedChanged;
                        inputRadioButton.Checked = true;
                        inputRadioButton.CheckedChanged += RadioButtonOnCheckedChanged;
                    }));
            }
            if (controlAccessCommand.ClientId.Equals(_id) && (bool)dataGridView["IsInputContollerColumn", rowIndex].Value && !controlAccessCommand.IsAllowed)
            {
                Invoke((Action)(() =>
                {
                    //Хак, чтобы события правильно отрабатывали (в данном случае нужно только обновление gui без event'а)
                    inputRadioButton.CheckedChanged -= RadioButtonOnCheckedChanged;
                    inputRadioButton.Checked = false;
                    inputRadioButton.CheckedChanged += RadioButtonOnCheckedChanged;
                }));
            }
            dataGridView["IsInputContollerColumn", rowIndex].Value = controlAccessCommand.IsAllowed;
            dataGridView.Rows[rowIndex].DefaultCellStyle.BackColor = GetRowBackColor(false);
        }

        private void ProcessDiffCommand(DiffCommand diffCommand)
        {
            //_diffFrame = new Bitmap(_diffFrame.Width, _diffFrame.Height);
            using (Graphics diffFrameGraphics = Graphics.FromImage(_diffFrame))
            {
                diffFrameGraphics.DrawImage(
                    DiffContainer.ByteArrayToImage(diffCommand.DiffItem.ImageBytes),
                    diffCommand.DiffItem.X, diffCommand.DiffItem.Y,
                    diffCommand.DiffItem.Width, diffCommand.DiffItem.Height);

                //diffFrameGraphics.DrawRectangle(Pens.Red,new Rectangle(
                //    diffCommand.DiffItem.X,
                //    diffCommand.DiffItem.Y,
                //    diffCommand.DiffItem.Width,
                //    diffCommand.DiffItem.Height));
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

        private void ProcessInputCommand(InputCommand inputCommand)
        {
            if (inputCommand.MouseInput != null)
            {
                MouseInput mouseInput = inputCommand.MouseInput;
                int x = (int) (mouseInput.X/ScreenshotScale);
                int y = (int) (mouseInput.Y/ScreenshotScale);
                switch (mouseInput.MouseInputType)
                {
                    case MouseInput.MouseInputEnum.Move:
                        MouseAPI.Move(x, y);
                        break;
                    case MouseInput.MouseInputEnum.Down:
                        if (mouseInput.LeftButton)
                            MouseAPI.LeftButtonDown(x, y);
                        else
                            MouseAPI.RightButtonDown(x, y);
                        break;
                    case MouseInput.MouseInputEnum.Up:
                        if (mouseInput.LeftButton)
                            MouseAPI.LeftButtonUp(x, y);
                        else
                            MouseAPI.RightButtonUp(x, y);
                        break;
                    case MouseInput.MouseInputEnum.Click:
                        if (mouseInput.LeftButton)
                            MouseAPI.LeftButtonClick(x, y);
                        else
                            MouseAPI.RightButtonClick(x, y);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ProcessPaintAddFigureCommand(PaintAddFigureCommand paintAddFigureCommand)
        {
            Invoke(new Action(() =>
                {
                    _paintControl.AddFigure(paintAddFigureCommand.FigureId, paintAddFigureCommand.Points,
                                            paintAddFigureCommand.Color);
                }));
        }

        private void ProcessPaintDeleteFigureCommand(PaintDeleteFigureCommand paintDeleteFigureCommand)
        {
            Invoke(new Action(() =>
                {
                    _paintControl.RemoveFigure(paintDeleteFigureCommand.FigureId);
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

                IDiffDetector _diffDetector = new CustomDiffDetector(JpegQuality);
                
                //IDiffDetector _diffDetector = new DiffDetector();
                //IDiffDetector _diffDetector = new DiffDetectorOpenCvSharp();


                while (_runDiffDetectThread)
                {
                    try
                    {
                        Bitmap screenShot = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width,
                                                       Screen.PrimaryScreen.WorkingArea.Height);

                        using (Graphics screenShotGraphics = Graphics.FromImage(screenShot))
                        {
                            screenShotGraphics.CopyFromScreen(0, 0, 0, 0,
                                                              new Size(
                                                                  Screen.PrimaryScreen.WorkingArea.Width,
                                                                  Screen.PrimaryScreen.WorkingArea.Height));

                            if (Math.Abs(ScreenshotScale - 1) > 0.01)
                                screenShot = new Bitmap(screenShot,
                                                        (int)
                                                        (Screen.PrimaryScreen.WorkingArea.Width * ScreenshotScale),
                                                        (int)
                                                        (Screen.PrimaryScreen.WorkingArea.Height * ScreenshotScale));
                        }

                        DiffContainer diffContainer = _diffDetector.GetDiffs(screenShot);

                        diffContainer.Data = DiffContainer.Split(diffContainer.Data, 44000);

                        foreach (var s in diffContainer.Data)
                            ClientWebSocketConnection.SendDiff(ConferenceId, new DiffItem(s));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("_diffDetectThread : " + ex.Message );
                    }
                    //Thread.Sleep(ScreenshotTimeout);
                }
            });
            _diffDetectThread.Start();
        }
    }
}
