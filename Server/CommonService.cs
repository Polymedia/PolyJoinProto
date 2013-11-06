using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using Common.Commands;
using DifferenceLib;
using Polymedia.PolyJoin.Common;

namespace Server
{
    class CommonService
    {
        private static readonly Queue Queue = Queue.Synchronized(new Queue());

        private static readonly Dictionary<string, Conference> Conferences = new Dictionary<string, Conference>();
        private static readonly Dictionary<ServerWebSocketConnection, string> Connections = new Dictionary<ServerWebSocketConnection, string>(); 

        public static void Init()
        {
            ConnectionManager.GetStateCommandReceived += ConnectionManagerOnGetStateCommandReceived;
            ConnectionManager.DiffCommandReceived += ConnectionManagerOnDiffCommandReceived;
            ConnectionManager.InputCommandReceived += ConnectionManagerOnInputCommandReceived;
            ConnectionManager.PaintAddFigureCommandRecieved+=ConnectionManagerPaintAddFigureCommandRecieved;
            ConnectionManager.PaintDeleteFigureCommandRecieved += ConnectionManagerPaintDelteFigureCommandRecieved;

            var thread = new Thread(() => { 
                while(true)
                    ProceedCommand(); 
            });

            thread.Start();
        }

        private static void ProceedCommand()
        {
            Console.WriteLine("Queue length = " + Queue.Count);
            
            if (Queue.Count == 0)
                Thread.Sleep(40);
            else
            {
                var command = (Command)Queue.Dequeue();

                if (command is QueryStateCommand)
                {
                    var queryStateCommand = command as QueryStateCommand;

                    Conference conference;
                    //Если пришел запрос состояния без id конференции (на создание)
                    if (string.IsNullOrWhiteSpace(queryStateCommand.ConferenceId))
                    {
                        string id = Conference.GenerateId();
                        while (Conferences.Keys.Contains(id))
                            id = Conference.GenerateId();
                        
                        conference = Conference.Start(id, (ServerWebSocketConnection) queryStateCommand.SenderConnection,
                                                      queryStateCommand.Width, queryStateCommand.Height, queryStateCommand.ClientName);

                        Conferences.Add(conference.Id, conference);
                    }
                    else
                        Conferences.TryGetValue(queryStateCommand.ConferenceId, out conference);

                    if (conference != null)
                    {
                        queryStateCommand.SenderConnection.ConnectionStateChangedEvent += (sender, args) =>
                        {
                            if (args.Value) return;
                            var disconnectCommand = new DisconnectCommand
                                {
                                    SenderConnection = queryStateCommand.SenderConnection
                                };
                            Queue.Enqueue(disconnectCommand);
                        };
                        
                        Connections.Add((ServerWebSocketConnection)queryStateCommand.SenderConnection, conference.Id);
                        conference.AddConnection((ServerWebSocketConnection)queryStateCommand.SenderConnection, queryStateCommand.ClientName);

                        ((ServerWebSocketConnection)queryStateCommand.SenderConnection).SendState(conference.Id, ((ServerWebSocketConnection)queryStateCommand.SenderConnection).Id, 
                            command.SenderConnection == conference.PresenterConnection, conference.Bitmap.Size.Width, conference.Bitmap.Size.Height);
                        
                        BroadcastParticipantsCommand(conference);
                    }
                    else
                    {
                        ((ServerWebSocketConnection)queryStateCommand.SenderConnection).SendState(string.Empty, string.Empty, false, 0, 0);
                        return;
                    }
                    
                    if (command.SenderConnection != conference.PresenterConnection)
                    {
                        //Отсылаем последний вариант картинки
                        var data = DiffContainer.Split(
                            new KeyValuePair<Rectangle, Bitmap>(
                                new Rectangle(0, 0, conference.Bitmap.Size.Width, conference.Bitmap.Size.Height), conference.Bitmap), 50000);

                        foreach (var d in data)
                        {
                            ((ServerWebSocketConnection)queryStateCommand.SenderConnection).SendDiff(conference.Id, new DiffItem(d));
                        }
                    }
                }
                else if (command is DiffCommand)
                {
                    var diffCommand = command as DiffCommand;

                    Conference conference;
                    Conferences.TryGetValue(diffCommand.ConferenceId, out conference);

                    if(conference == null)
                        return;

                    conference.Graphics.DrawImage(DiffContainer.ByteArrayToImage(diffCommand.DiffItem.ImageBytes),
                                                        diffCommand.DiffItem.X, diffCommand.DiffItem.Y,
                                                        diffCommand.DiffItem.Width, diffCommand.DiffItem.Height);

                    foreach (var connection in conference.Connections)
                    {
                        if (connection == conference.PresenterConnection)
                            continue;
                        connection.SendDiff(conference.Id, diffCommand.DiffItem);
                    }
                }
                else if (command is DisconnectCommand)
                {
                    var disconnectCommand = command as DisconnectCommand;

                    string conferenceId =
                        Connections[((ServerWebSocketConnection)disconnectCommand.SenderConnection)];
                    Connections.Remove(((ServerWebSocketConnection)disconnectCommand.SenderConnection));

                    Conference conference;
                    Conferences.TryGetValue(conferenceId, out conference);

                    if (conference != null)
                    {
                        conference.RemoveConnection(((ServerWebSocketConnection) disconnectCommand.SenderConnection));

                        BroadcastParticipantsCommand(conference);

                        if (conference.Connections.Count == 0 || conference.PresenterConnection == null)
                            Conferences.Remove(conferenceId);
                    }

                    
                }
                else if (command is PaintAddFigureCommand)
                {
                    var addFigureCommand = command as PaintAddFigureCommand;

                    Conference conference = Conferences[addFigureCommand.ConferenceId];

                    if (conference == null)
                        return;

                    foreach (var connection in conference.Connections)
                    {
                        if (command.SenderConnection != connection)
                            connection.PaintAddFigureCommand(conference.Id, addFigureCommand.FigureId, addFigureCommand.Points, addFigureCommand.Color);
                    }
                }
                else if (command is PaintDeleteFigureCommand)
                {
                    var deleteFigureCommand = command as PaintDeleteFigureCommand;

                    Conference conference = Conferences[deleteFigureCommand.ConferenceId];

                    if (conference == null)
                        return;

                    foreach (var connection in conference.Connections)
                    {
                        if (command.SenderConnection != connection)
                            connection.PaintDeleteFigureCommand(conference.Id, deleteFigureCommand.FigureId);
                    }
                }else if (command is InputCommand)
                {
                    InputCommand inputCommand = command as InputCommand;

                    Conference conference = conferences[inputCommand.ConferenceId];

                    if (conference == null)
                        return;

                    conference.PresenterConnection.SendInput(conference.Id, inputCommand.MouseInput);
                }
            }
        }

        private static void ConnectionManagerOnInputCommandReceived(object sender, SimpleEventArgs<InputCommand> simpleEventArgs)
        {
            queue.Enqueue(simpleEventArgs.Value);
        }

        private static void ConnectionManagerOnGetStateCommandReceived(object sender, SimpleEventArgs<QueryStateCommand> connectionEventArgs)
        {
            Queue.Enqueue(connectionEventArgs.Value);
        }

        private static void ConnectionManagerOnDiffCommandReceived(object sender, SimpleEventArgs<DiffCommand> connectionEventArgs)
        {
            Queue.Enqueue(connectionEventArgs.Value);
        }

        private static void ConnectionManagerPaintAddFigureCommandRecieved(object sender, SimpleEventArgs<PaintAddFigureCommand> e)
        {
            Queue.Enqueue(e.Value);
        }

        private static void ConnectionManagerPaintDelteFigureCommandRecieved(object sender, SimpleEventArgs<PaintDeleteFigureCommand> e)
        {
            Queue.Enqueue(e.Value);
        }

        private static void BroadcastParticipantsCommand(Conference conference)
        {
            var participants = conference.Connections.Select(connection => new Participant
                {
                    Id = connection.Id, Name = connection.ClientName, BrushArgb = connection.BrushArgb, IsPresenter = connection == conference.PresenterConnection
                }).ToList();

            foreach (var connection in conference.Connections)
                connection.SendParticipants(conference.Id, participants);
        }
    }

    public class Conference
    {
        public string Id { get; set; }
        public List<ServerWebSocketConnection> Connections = new List<ServerWebSocketConnection>();
        public ServerWebSocketConnection PresenterConnection = null;

        public Bitmap Bitmap;
        public Graphics Graphics;

        private int _lastConnectionNum;

        public static Color[] Colors = new[]{Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Violet};

        public static string GenerateId()
        {
            return (new Random()).Next(111111, 999999).ToString(CultureInfo.InvariantCulture);
        }

        public static Conference Start(string id, ServerWebSocketConnection presenterConnection, int presenterWidth, int presenterHeight, string clientName)
        {
            var conference = new Conference {Id = id, PresenterConnection = presenterConnection};
            conference.AddConnection(presenterConnection, clientName);

            conference.Bitmap = new Bitmap(presenterWidth, presenterHeight);
            conference.Graphics = Graphics.FromImage(conference.Bitmap);

            return conference;
        }

        public int GenerateConnectionNumber()
        {
            return ++_lastConnectionNum;
        }

        public void AddConnection(ServerWebSocketConnection connection, string clientName)
        {
            if (!Connections.Contains(connection))
            {
                int num = GenerateConnectionNumber();
                connection.BrushArgb = Colors[(num - 1) % Colors.Length].ToArgb();
                connection.ClientName = clientName;
                Connections.Add(connection);
            }
        }

        public void RemoveConnection(ServerWebSocketConnection connection)
        {
            Connections.Remove(connection);

            if (PresenterConnection == connection)
                PresenterConnection = null;
        }
    }
}
