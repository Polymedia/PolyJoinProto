using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DifferenceLib;
using Polymedia.PolyJoin.Common;

namespace Polymedia.PolyJoin.Server
{
    class ServerWebSocketConnection: ConnectionWrapper
    {
        public event EventHandler<SimpleEventArgs<GetStateCommand>> GetStateCommandReceived = delegate { };
        public event EventHandler<SimpleEventArgs<DiffCommand>> DiffCommandReceived = delegate { }; 
        
        public ServerWebSocketConnection(IWebSocketConnection webSocketConnection) : base(webSocketConnection)
        {
        }

        public void SendState(int conferenceId, bool isPresenter)
        {
            StateCommand command = new StateCommand();
            command.ConferenceId = conferenceId;
            command.IsPresenter = isPresenter;
            SendCommand(command);
        }

        public void SendDiff(DiffItem diffItem)
        {
            DiffCommand command = new DiffCommand();
            command.DiffItem = diffItem;
            SendCommand(command);
        }

        protected override void OnReceivedCommand(Command command)
        {
            command.SenderConnection = this;

            switch (command.CommandName)
            {
                case CommandName.GetState:
                    Console.WriteLine("Command GetState");
                    GetStateCommandReceived.Invoke(this, new SimpleEventArgs<GetStateCommand>() { Value = (GetStateCommand)command });
                    break;
                case CommandName.Diff:
                    Console.WriteLine("Command Diff");
                    DiffCommandReceived.Invoke(this, new SimpleEventArgs<DiffCommand>() { Value = (DiffCommand)command });
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }
    }
}
