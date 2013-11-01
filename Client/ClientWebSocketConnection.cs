using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DifferenceLib;
using Polymedia.PolyJoin.Common;
using System.Drawing;

namespace Polymedia.PolyJoin.Client
{
    public class ClientWebSocketConnection: ConnectionWrapper
    {
        public event EventHandler<SimpleEventArgs<ParticipantsCommand>> ParticipantsCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<StateCommand>> StateCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<DiffCommand>> DiffCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<PaintAddFigureCommand>> PaintAddFigureCommandRecieved = delegate { };
        public event EventHandler<SimpleEventArgs<PaintDeleteFigureCommand>> PaintDeleteFigureCommandRecieved = delegate { };
        
        public ClientWebSocketConnection(IWebSocketConnection webSocketConnection)
            : base(webSocketConnection)
        {
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

        public void SendInput(string conferenceId, int mouseX, int mouseY)
        {
            InputCommand command = new InputCommand(conferenceId);
            command.MouseX = mouseX;
            command.MouseY = mouseY;
            SendCommand(command);
        }
        
        public void PaintAddFigureCommand(string conferenceId, string figureId, List<Point> points, Color color)
        {
            Command command = new PaintAddFigureCommand(conferenceId, figureId, points, color);
            SendCommand(command);
        }

        public void PaintDeleteFigureCommand(string conferenceId, string figureId)
        {
            Command command = new PaintDeleteFigureCommand(conferenceId, figureId);
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
                case CommandName.Participants:
                    Console.WriteLine("Command Participants");
                    ParticipantsCommandReceived.Invoke(this, new SimpleEventArgs<ParticipantsCommand>() { Value = (ParticipantsCommand)command });
                    break;
                case CommandName.PaintAddFigure:
                    Console.WriteLine("Command PaintAddFigure");
                    PaintAddFigureCommandRecieved.Invoke(this, new SimpleEventArgs<PaintAddFigureCommand>() { Value = (PaintAddFigureCommand)command });
                    break;
                case CommandName.PaintDeleteFigure:
                    Console.WriteLine("Command PaintDeleteFigure");
                    PaintDeleteFigureCommandRecieved.Invoke(this, new SimpleEventArgs<PaintDeleteFigureCommand>() { Value = (PaintDeleteFigureCommand)command });
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }
    }
}
