﻿using System;
using System.Collections.Generic;
using Common.Commands;
using Polymedia.PolyJoin.Common;

namespace Server
{
    class ConnectionManager //to singleton
    {
        public static Dictionary<Connection, ServerWebSocketConnection> Connections = new Dictionary<Connection, ServerWebSocketConnection>();

        public static event EventHandler<SimpleEventArgs<QueryStateCommand>> GetStateCommandReceived = delegate { };
        public static event EventHandler<SimpleEventArgs<DiffCommand>> DiffCommandReceived = delegate { };
        public static event EventHandler<SimpleEventArgs<InputCommand>> InputCommandReceived = delegate { };
        public static event EventHandler<SimpleEventArgs<PaintAddFigureCommand>> PaintAddFigureCommandRecieved = delegate { };
        public static event EventHandler<SimpleEventArgs<PaintDeleteFigureCommand>> PaintDeleteFigureCommandRecieved = delegate { };
        public static event EventHandler<SimpleEventArgs<ControlAccessCommand>> ControllAccessCommandReceived = delegate
            { };

        public static event EventHandler<SimpleEventArgs<RequestControlCommand>> RequestControlCommandReceived =
            delegate { };

        public static void AddConnection(Connection connection)
        {
            if (Connections.ContainsKey(connection)) return;
            
            ServerWebSocketConnection serverWebSocketConnection = new ServerWebSocketConnection(connection);

            lock (Connections)
                Connections.Add(connection, serverWebSocketConnection);

            serverWebSocketConnection.GetStateCommandReceived += ServerWebSocketConnectionOnGetStateCommandReceived;
            serverWebSocketConnection.DiffCommandReceived += ServerWebSocketConnectionOnDiffCommandReceived;
            serverWebSocketConnection.InputCommandReceived +=ServerWebSocketConnectionOnInputCommandReceived;
            serverWebSocketConnection.PaintAddFigureCommandRecieved+=ServerWebSocketConnectionPaintAddFigureCommandRecieved;
            serverWebSocketConnection.PaintDeleteFigureCommandRecieved += ServerWebSocketConnectionPaintDeleteFigureCommandRecieved;
            serverWebSocketConnection.ControlAccessCommandReceived += ServerWebSocketConnectionOnControlAccessCommandReceived;
            serverWebSocketConnection.RequestControllCommandReceived += ServerWebSocketConnectionOnRequestControllCommandReceived;
        }

        private static void ServerWebSocketConnectionOnRequestControllCommandReceived(object sender, SimpleEventArgs<RequestControlCommand> simpleEventArgs)
        {
            RequestControlCommandReceived.Invoke(sender, simpleEventArgs);
        }

        private static void ServerWebSocketConnectionOnControlAccessCommandReceived(object sender, SimpleEventArgs<ControlAccessCommand> simpleEventArgs)
        {
            ControllAccessCommandReceived.Invoke(sender, simpleEventArgs);
        }

        private static void ServerWebSocketConnectionOnInputCommandReceived(object sender, SimpleEventArgs<InputCommand> simpleEventArgs)
        {
            InputCommandReceived.Invoke(sender, simpleEventArgs);
        }

        private static void ServerWebSocketConnectionPaintAddFigureCommandRecieved(object sender, SimpleEventArgs<PaintAddFigureCommand> e)
        {
            PaintAddFigureCommandRecieved.Invoke(sender, e);
        }

        private static void ServerWebSocketConnectionPaintDeleteFigureCommandRecieved(object sender, SimpleEventArgs<PaintDeleteFigureCommand> e)
        {
            PaintDeleteFigureCommandRecieved.Invoke(sender, e);
        }

        public static void RemoveConnection(Connection connection)
        {
            lock (Connections)    
                Connections.Remove(connection);
        }

        //public static void BroadcastSendDiff(DiffItem diffItem)
        //{
        //    var connections = new Dictionary<Connection, ServerWebSocketConnection>().ToList();

        //    lock (Connections)
        //        connections = Connections.ToList();

        //    foreach (var connection in connections)
        //    {
        //        if (connection.Key != _presenterConnection)
        //            connection.Value.SendDiff(diffItem);
        //    }
        //}

        private static void ServerWebSocketConnectionOnGetStateCommandReceived(object sender, SimpleEventArgs<QueryStateCommand> connectionEventArgs)
        {
            GetStateCommandReceived.Invoke(sender, connectionEventArgs);
        }

        private static void ServerWebSocketConnectionOnDiffCommandReceived(object sender, SimpleEventArgs<DiffCommand> connectionEventArgs)
        {
            DiffCommandReceived.Invoke(sender, connectionEventArgs);
        }
    }
}
