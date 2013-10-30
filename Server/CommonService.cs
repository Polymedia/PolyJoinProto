using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DifferenceLib;
using Polymedia.PolyJoin.Common;

namespace Polymedia.PolyJoin.Server
{
    class CommonService
    {
        private static Queue queue = Queue.Synchronized(new Queue());

        private static Dictionary<string, Conference> conferences = new Dictionary<string, Conference>();
        private static Dictionary<ServerWebSocketConnection, string> connections = new Dictionary<ServerWebSocketConnection, string>(); 

        public static void Init()
        {
            ConnectionManager.GetStateCommandReceived += ConnectionManagerOnGetStateCommandReceived;
            ConnectionManager.DiffCommandReceived += ConnectionManagerOnDiffCommandReceived;

            Thread thread = new Thread(() => { 
                while(true)
                    ProceedCommand(); 
            });

            thread.Start();
        }

        private static void ProceedCommand()
        {
            Console.WriteLine("Queue length = " + queue.Count);
            
            if (queue.Count == 0)
                Thread.Sleep(40);
            else
            {
                Command command = (Command)queue.Dequeue();

                if (command is QueryStateCommand)
                {
                    var queryStateCommand = command as QueryStateCommand;

                    Conference conference = null;
                    //Если пришел запрос состояния без id конференции (на создание)
                    if (string.IsNullOrWhiteSpace(queryStateCommand.ConferenceId))
                    {
                        string id = Conference.GenerateId();
                        while (conferences.Keys.Contains(id))
                            id = Conference.GenerateId();
                        
                        conference = Conference.Start(id, (ServerWebSocketConnection) queryStateCommand.SenderConnection,
                                                      queryStateCommand.Width, queryStateCommand.Height);

                        queryStateCommand.SenderConnection.ConnectionStateChanged += (sender, args) =>
                            {
                                DisconnectCommand disconnectCommand = new DisconnectCommand();
                                disconnectCommand.SenderConnection = queryStateCommand.SenderConnection;

                                queue.Enqueue(disconnectCommand);
                            };

                        conferences.Add(conference.Id, conference);
                        connections.Add((ServerWebSocketConnection)queryStateCommand.SenderConnection, conference.Id);
                    }
                    else
                        conferences.TryGetValue(queryStateCommand.ConferenceId, out conference);

                    if (conference != null)
                    {
                        conference.AddConnection((ServerWebSocketConnection)queryStateCommand.SenderConnection);
                        
                        ((ServerWebSocketConnection)queryStateCommand.SenderConnection).SendState(conference.Id, command.SenderConnection == conference.PresenterConnection,
                            (int)conference.Bitmap.Size.Width, (int)conference.Bitmap.Size.Height);

                        //TODO send connections list
                    }
                    else
                    {
                        ((ServerWebSocketConnection)queryStateCommand.SenderConnection).SendState(string.Empty, false, 0, 0);
                        return;
                    }
                    
                    if (command.SenderConnection != conference.PresenterConnection)
                    {
                        //Отсылаем последний вариант картинки
                        var data = DiffContainer.Split(
                            new KeyValuePair<Rectangle, Bitmap>(
                                new Rectangle(0, 0, (int)conference.Bitmap.Size.Width, (int)conference.Bitmap.Size.Height), conference.Bitmap), 50000);

                        foreach (var d in data)
                        {
                            ((ServerWebSocketConnection)queryStateCommand.SenderConnection).SendDiff(conference.Id, new DiffItem(d));
                        }
                    }
                }
                else if (command is DiffCommand)
                {
                    var diffCommand = command as DiffCommand;

                    Conference conference = conferences[diffCommand.ConferenceId];

                    if(conference == null)
                        return;

                    conference.Graphics.DrawImage(DiffContainer.ByteArrayToImage(diffCommand.DiffItem.ImageBytes),
                                                        diffCommand.DiffItem.X, diffCommand.DiffItem.Y,
                                                        diffCommand.DiffItem.Width, diffCommand.DiffItem.Height);

                    foreach (var connection in conference.GetConnectionsCopy())
                    {
                        if (connection == conference.PresenterConnection)
                            continue;
                        connection.SendDiff(conference.Id, diffCommand.DiffItem);
                    }
                }else if (command is DisconnectCommand)
                {
                    var disconnectCommand = command as DisconnectCommand;

                    string conferenceId =
                        connections[((ServerWebSocketConnection)disconnectCommand.SenderConnection)];
                    connections.Remove(((ServerWebSocketConnection)disconnectCommand.SenderConnection));

                    Conference conference = null;
                    conferences.TryGetValue(conferenceId, out conference);

                    if (conference != null)
                    {
                        conference.RemoveConnection(((ServerWebSocketConnection) disconnectCommand.SenderConnection));
                        if (conference.Connections.Count == 0 || conference.PresenterConnection == null)
                            conferences.Remove(conferenceId);
                    }

                    //TODO send connections list
                }
            }
        }

        private static void ConnectionManagerOnGetStateCommandReceived(object sender, SimpleEventArgs<QueryStateCommand> connectionEventArgs)
        {
            queue.Enqueue(connectionEventArgs.Value);
        }

        private static void ConnectionManagerOnDiffCommandReceived(object sender, SimpleEventArgs<DiffCommand> connectionEventArgs)
        {
            queue.Enqueue(connectionEventArgs.Value);
        }
    }

    public class Conference
    {
        public string Id { get; set; }
        public List<ServerWebSocketConnection> Connections = new List<ServerWebSocketConnection>();
        public ServerWebSocketConnection PresenterConnection = null;

        public Bitmap Bitmap;
        public Graphics Graphics;

        public static string GenerateId()
        {
            return (new Random()).Next(111111, 999999).ToString();
        }

        public static Conference Start(string id, ServerWebSocketConnection presenterConnection, int presenterWidth, int presenterHeight)
        {
            Conference conference = new Conference();
            conference.Id = id;
            conference.PresenterConnection = presenterConnection;
            conference.AddConnection(presenterConnection);

            conference.Bitmap = new Bitmap(presenterWidth, presenterHeight);
            conference.Graphics = Graphics.FromImage(conference.Bitmap);

            return conference;
        }

        public void AddConnection(ServerWebSocketConnection connection)
        {
            if (!Connections.Contains(connection))
                Connections.Add(connection);
        }

        public void RemoveConnection(ServerWebSocketConnection connection)
        {
            Connections.Remove(connection);

            if (PresenterConnection == connection)
                PresenterConnection = null;
        }

        public List<ServerWebSocketConnection> GetConnectionsCopy()
        {
            List<ServerWebSocketConnection> result = new List<ServerWebSocketConnection>();

            result = Connections.ToList();

            return result;
        }
    }
}
