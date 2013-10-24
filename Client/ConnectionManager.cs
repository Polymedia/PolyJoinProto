using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Client
{
    public class ConnectionManager
    {
        private static Connection _connection = null;

        public static Connection Connect()
        {
            if (_connection == null)
            {
                _connection = new Connection("ws://localhost:9080/PolyJoin");
                _connection.Start();
            }
            return _connection;
        }
    }
}
