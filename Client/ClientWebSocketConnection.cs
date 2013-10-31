using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using DifferenceLib;
using Polymedia.PolyJoin.Common;
using System.Drawing;

namespace Polymedia.PolyJoin.Client
{
    public class ClientWebSocketConnection: ConnectionWrapper
    {
        public event EventHandler<SimpleEventArgs<StateCommand>> StateCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<DiffCommand>> DiffCommandReceived = delegate { };
        public event EventHandler<WebSocketEventArgs<bool>> ConnectionStateChanged = delegate { };
        public event EventHandler<SimpleEventArgs<PaintAddFigureCommand>> PaintAddFigureCommandRecieved = delegate { };
        
        public ClientWebSocketConnection(IWebSocketConnection webSocketConnection)
            : base(webSocketConnection)
        {
            webSocketConnection.ConnectionStateChanged += (sender, args) => ConnectionStateChanged.Invoke(this, args);
        }

        public void QueryState(string conferenceId, int width, int height)
        {
            Command command = new QueryStateCommand(conferenceId, width, height);
            SendCommand(command);
        }

        public void SendDiff(string conferenceId, DiffItem diffItem)
        {
            DiffCommand command = new DiffCommand(conferenceId);
            command.DiffItem = diffItem;
            SendCommand(command);
        }

        public void PaintAddFigureCommand(string conferenceId, List<Point> points, Color color)
        {
            Command command = new PaintAddFigureCommand(conferenceId, points, color);
            SendCommand(command);
        }

        protected override void OnReceivedCommand(Command command)
        {
            //TODO generate events on command received

            command.SenderConnection = this;

            switch (command.CommandName)
            {
                case CommandName.State:
                    Console.WriteLine("Command State");
                    StateCommandReceived.Invoke(this, new SimpleEventArgs<StateCommand>() { Value = (StateCommand)command });
                    break;
                case CommandName.Diff:
                    Console.WriteLine("Command Diff");
                    DiffCommandReceived.Invoke(this, new SimpleEventArgs<DiffCommand>() { Value = (DiffCommand)command });
                    break;
                case CommandName.PaintAddFigure:
                    Console.WriteLine("Command PaintAddFigure");
                    PaintAddFigureCommandRecieved.Invoke(this, new SimpleEventArgs<PaintAddFigureCommand>() { Value = (PaintAddFigureCommand)command });
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }
    }
}
