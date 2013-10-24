using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using DifferenceLib;
using Polymedia.PolyJoin.Common;

namespace Polymedia.PolyJoin.Client
{
    public class ClientWebSocketConnection: ConnectionWrapper
    {
        public event EventHandler<ConnectionEventArgs<StateCommand>> StateCommandReceived = delegate { };
        public event EventHandler<ConnectionEventArgs<DiffCommand>> DiffCommandReceived = delegate { }; 
        
        public ClientWebSocketConnection(IWebSocketConnection webSocketConnection)
            : base(webSocketConnection)
        {
        }

        public void QueryState()
        {
            Command command = new GetStateCommand();
            SendCommand(command);
        }

        public void SendDiff(DiffContainer container)
        {
            DiffCommand command = new DiffCommand();
            command.Container = container;
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
                    StateCommandReceived.Invoke(this, new ConnectionEventArgs<StateCommand>() { Value = (StateCommand)command });
                    break;
                case CommandName.Diff:
                    Console.WriteLine("Command Diff");
                    DiffCommandReceived.Invoke(this, new ConnectionEventArgs<DiffCommand>() { Value = (DiffCommand)command });
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }
    }
}
