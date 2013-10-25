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

        public static event EventHandler<SimpleEventArgs<GetStateCommand>> GetStateCommandReceived = delegate { };
        public static event EventHandler<SimpleEventArgs<DiffCommand>> DiffCommandReceived = delegate { };

        public static void AddConnection(Connection connection)
        {
            if (Connections.ContainsKey(connection)) return;
            
            if (_presenterConnection == null)
            {
                _presenterConnection = connection;
            }

            ServerWebSocketConnection serverWebSocketConnection = new ServerWebSocketConnection(connection);

            if (_presenterConnection == connection)
                PresenterConnection = serverWebSocketConnection;

            lock (Connections)
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

            lock (Connections)    
                Connections.Remove(connection);
        }

        public static void BroadcastSendDiff(DiffContainer diffContainer)
        {
            var connections = new Dictionary<Connection, ServerWebSocketConnection>().ToList();

            lock (Connections)
                connections = Connections.ToList();

            foreach (var connection in connections)
            {
               // if (connection.Key != _presenterConnection)
                    connection.Value.SendDiff(diffContainer);
            }
        }

        private static void ServerWebSocketConnectionOnGetStateCommandReceived(object sender, SimpleEventArgs<GetStateCommand> connectionEventArgs)
        {
            GetStateCommandReceived.Invoke(sender, connectionEventArgs);
        }

        private static void ServerWebSocketConnectionOnDiffCommandReceived(object sender, SimpleEventArgs<DiffCommand> connectionEventArgs)
        {
            DiffCommandReceived.Invoke(sender, connectionEventArgs);
        }
    }
}
