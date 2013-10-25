using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polymedia.PolyJoin.Common;

namespace Polymedia.PolyJoin.Server
{
    class CommonService
    {
        private static int ConferenceId = 123;
        
        public static void Init()
        {
            ConnectionManager.GetStateCommandReceived += ConnectionManagerOnGetStateCommandReceived;
            ConnectionManager.DiffCommandReceived += ConnectionManagerOnDiffCommandReceived;
        }

        private static void ConnectionManagerOnGetStateCommandReceived(object sender, SimpleEventArgs<GetStateCommand> connectionEventArgs)
        {
            ServerWebSocketConnection serverWebSocketConnection = sender as ServerWebSocketConnection;
            serverWebSocketConnection.SendState(ConferenceId, serverWebSocketConnection == ConnectionManager.PresenterConnection);
        }

        private static void ConnectionManagerOnDiffCommandReceived(object sender, SimpleEventArgs<DiffCommand> connectionEventArgs)
        {
            ConnectionManager.BroadcastSendDiff(connectionEventArgs.Value.DiffItem);
        }
    }
}
