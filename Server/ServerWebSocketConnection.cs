﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DifferenceLib;
using Polymedia.PolyJoin.Common;
using System.Drawing;

namespace Polymedia.PolyJoin.Server
{
    public class ServerWebSocketConnection: ConnectionWrapper
    {
        public event EventHandler<SimpleEventArgs<QueryStateCommand>> GetStateCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<DiffCommand>> DiffCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<InputCommand>> InputCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<PaintAddFigureCommand>> PaintAddFigureCommandRecieved = delegate { };
        public event EventHandler<SimpleEventArgs<PaintDeleteFigureCommand>> PaintDeleteFigureCommandRecieved = delegate { };

        public string Id = Guid.NewGuid().ToString();
        public string Name = string.Empty;
        public int BrushArgb = 0;

        public ServerWebSocketConnection(IWebSocketConnection webSocketConnection) : base(webSocketConnection)
        {
        }

        public void SendState(string conferenceId, string id, bool isPresenter, int presenterWidth, int presenterHeight)
        {
            StateCommand command = new StateCommand(conferenceId);
            command.ParticipantId = id;
            command.IsPresenter = isPresenter;
            command.PresenterWidth = presenterWidth;
            command.PresenterHeight = presenterHeight;
            SendCommand(command);
        }

        public void SendDiff(string conferenceId, DiffItem diffItem)
        {
            DiffCommand command = new DiffCommand(conferenceId);
            command.DiffItem = diffItem;
            SendCommand(command);
        }

        public void SendParticipants(string conferenceId, List<Participant> participants)
        {
            ParticipantsCommand command = new ParticipantsCommand(conferenceId);
            command.Participants = participants;
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
                    GetStateCommandReceived.Invoke(this, new SimpleEventArgs<QueryStateCommand>() { Value = (QueryStateCommand)command });
                    break;
                case CommandName.Diff:
                    Console.WriteLine("Command Diff");
                    DiffCommandReceived.Invoke(this, new SimpleEventArgs<DiffCommand>() { Value = (DiffCommand)command });
                    break;
                case CommandName.Input:
                    Console.WriteLine("Command Input");
                    InputCommandReceived.Invoke(this, new SimpleEventArgs<InputCommand>() { Value = (InputCommand)command });
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
