using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polymedia.PolyJoin.Common;
using DifferenceLib;

namespace Polymedia.PolyJoin.Server
{
    class ConnectionManager //to singleton
    {
        public static Dictionary<Connection, ServerWebSocketConnection> Connections = new Dictionary<Connection, ServerWebSocketConnection>();

        private static Connection _presenterConnection = null;
        public static ServerWebSocketConnection PresenterConnection = null;

        public static event EventHandler<ConnectionEventArgs<GetStateCommand>> GetStateCommandReceived = delegate { };
        public static event EventHandler<ConnectionEventArgs<DiffCommand>> DiffCommandReceived = delegate { };

        public static void AddConnection(Connection connection)
        {
            if (_presenterConnection == null)
            {
                _presenterConnection = connection;
            }

            ServerWebSocketConnection serverWebSocketConnection = new ServerWebSocketConnection(connection);

            if (_presenterConnection == connection)
                PresenterConnection = serverWebSocketConnection;

            Connections.Add(connection, serverWebSocketConnection);

            serverWebSocketConnection.GetStateCommandReceived += ServerWebSocketConnectionOnGetStateCommandReceived;
            serverWebSocketConnection.DiffCommandReceived += ServerWebSocketConnectionOnDiffCommandReceived;

        }

        public static void RemoveConnection(Connection connection)
        {
            if (_presenterConnection == connection)
            {
                PresenterConnection = null;
                _presenterConnection = null;
            }
                
            Connections.Remove(connection);
        }

        public static void BroadcastSendDiff(DiffContainer diffContainer)
        {
            foreach (var connection in Connections)
            {
               // if (connection.Key != _presenterConnection)
                    connection.Value.SendDiff(diffContainer);
            }
        }

        private static void ServerWebSocketConnectionOnGetStateCommandReceived(object sender, ConnectionEventArgs<GetStateCommand> connectionEventArgs)
        {
            GetStateCommandReceived.Invoke(sender, connectionEventArgs);
        }

        private static void ServerWebSocketConnectionOnDiffCommandReceived(object sender, ConnectionEventArgs<DiffCommand> connectionEventArgs)
        {
            DiffCommandReceived.Invoke(sender, connectionEventArgs);
        }
    }
}
