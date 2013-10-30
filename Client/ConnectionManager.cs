using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Client
{
    public class ConnectionManager
    {
        private Connection _connection = null;

        public string ServerIp { get; set; }

        public Connection GetConnection()
        {
            if (_connection == null)
                _connection = new Connection("ws://" + ServerIp + ":9080/PolyJoin");
            return _connection;
        }

        public Connection Connect()
        {
            if (_connection == null)
                _connection = new Connection("ws://" + ServerIp + ":9080/PolyJoin");
            _connection.Start();
            return _connection;
        }

        public void Disconnect()
        {
            if (_connection != null)
                _connection.Stop();
            _connection = null;
        }
    }
}
