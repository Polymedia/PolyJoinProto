using System;
using System.Collections.Generic;
using Common.Commands;
using DifferenceLib;
using Polymedia.PolyJoin.Common;
using System.Drawing;

namespace Server
{
    public class ServerWebSocketConnection: ConnectionWrapper
    {
        public event EventHandler<SimpleEventArgs<QueryStateCommand>> GetStateCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<DiffCommand>> DiffCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<InputCommand>> InputCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<PaintAddFigureCommand>> PaintAddFigureCommandRecieved = delegate { };
        public event EventHandler<SimpleEventArgs<PaintDeleteFigureCommand>> PaintDeleteFigureCommandRecieved = delegate { };
        public event EventHandler<SimpleEventArgs<ControlAccessCommand>> ControlAccessCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<RequestControlCommand>> RequestControllCommandReceived = delegate { }; 

        public string Id = Guid.NewGuid().ToString();
        public string ClientName = string.Empty;
        public bool IsInputController = false;
        public int BrushArgb = 0;
        


        public ServerWebSocketConnection(IWebSocketConnection webSocketConnection) : base(webSocketConnection)
        {
        }

        public void ChangeControlAccess(string conferenceId, string presenterId, string clientId, bool isAllowed)
        {
            var command = new ControlAccessCommand(conferenceId, presenterId, clientId, isAllowed);
            SendCommand(command);
        }

        public void RequestControl(string conferenceId, string clientId, bool isAllowed)
        {
            var command = new RequestControlCommand(conferenceId, clientId, isAllowed);
            SendCommand(command);
        }

        public void SendState(string conferenceId, string id, bool isPresenter, int presenterWidth, int presenterHeight)
        {
            var command = new StateCommand(conferenceId)
                {
                    ParticipantId = id,
                    IsPresenter = isPresenter,
                    PresenterWidth = presenterWidth,
                    PresenterHeight = presenterHeight
                };
            SendCommand(command);
        }

        public void SendDiff(string conferenceId, DiffItem diffItem)
        {
            var command = new DiffCommand(conferenceId) {DiffItem = diffItem};
            SendCommand(command);
        }

        public void SendParticipants(string conferenceId, List<Participant> participants)
        {
            var command = new ParticipantsCommand(conferenceId) {Participants = participants};
            SendCommand(command);
        }

        public void SendInput(string conferenceId, MouseInput mouseInput)
        {
            Command command = new InputCommand(conferenceId)
                {
                    MouseInput = mouseInput
                };
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
            command.SenderConnection = this;

            switch (command.CommandName)
            {
                case CommandName.GetState:
                    Console.WriteLine("Command GetState");
                    GetStateCommandReceived.Invoke(this, new SimpleEventArgs<QueryStateCommand> { Value = (QueryStateCommand)command });
                    break;
                case CommandName.Diff:
                    Console.WriteLine("Command Diff");
                    DiffCommandReceived.Invoke(this, new SimpleEventArgs<DiffCommand> { Value = (DiffCommand)command });
                    break;
                case CommandName.Input:
                    Console.WriteLine("Command Input");
                    InputCommandReceived.Invoke(this, new SimpleEventArgs<InputCommand> { Value = (InputCommand)command });
                    break;
                case CommandName.PaintAddFigure:
                    Console.WriteLine("Command PaintAddFigure");
                    PaintAddFigureCommandRecieved.Invoke(this, new SimpleEventArgs<PaintAddFigureCommand> { Value = (PaintAddFigureCommand)command });
                    break;
                case CommandName.PaintDeleteFigure:
                    Console.WriteLine("Command PaintDeleteFigure");
                    PaintDeleteFigureCommandRecieved.Invoke(this, new SimpleEventArgs<PaintDeleteFigureCommand> { Value = (PaintDeleteFigureCommand)command });
                    break;
                case CommandName.ControlAccess:
                    Console.WriteLine("Command ControllAccess");
                    ControlAccessCommandReceived.Invoke(this, new SimpleEventArgs<ControlAccessCommand> { Value = (ControlAccessCommand)command });
                    break;
                case CommandName.RequestControl:
                    Console.WriteLine("Command RequestControl");
                    RequestControllCommandReceived.Invoke(this, new SimpleEventArgs<RequestControlCommand> { Value = (RequestControlCommand)command });
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }

        
    }
}
